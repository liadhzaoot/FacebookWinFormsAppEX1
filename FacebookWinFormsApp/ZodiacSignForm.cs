﻿using FacebookWrapper.ObjectModel;
using Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    internal partial class ZodiacSignForm : Form
    {
        internal delegate void BackButtonEventHandler();

        internal event BackButtonEventHandler BackButtonClicked;

        private ZodiacSignAdapter m_ZodiacMatch;

        private User m_LoggedInUser;

        internal ZodiacSignForm()
        {
            InitializeComponent();
            AppManager.GetInstance.LoginEvent += switchUser;
        }

        private void switchUser()
        {
            m_LoggedInUser = AppManager.GetInstance.LoggedInUser;
            m_ZodiacMatch = new ZodiacSignAdapter(m_LoggedInUser.Birthday);
            try
            {
                pictureBox1.LoadAsync(m_ZodiacMatch.PictureUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load picture of zodiac sign.");
                throw ex;
            }

        }

        private void findButton_Click(object sender, EventArgs e)
        {
            m_ZodiacMatch.UpdateBestMatchedSign();

            try
            {
                pictureBox2.LoadAsync(m_ZodiacMatch.BestMatchedSign.PictureUrl);
            }
            catch (Exception)
            {
                MessageBox.Show("Could not load picture of zodiac sign.");
            }

            findButton.Visible = false;
            pictureBox2.Visible = true;
            fateQuotePicture.Visible = true;
            shareButton.Visible = true;
            MessageBox.Show("Congratulations! A match was found!");
        }

        private void goBackButton_Click(object sender, EventArgs e)
        {
            AppManager.GetInstance.Back();
            findButton.Visible = true;
            pictureBox2.Visible = false;
            fateQuotePicture.Visible = false;
            shareButton.Visible = false;
        }

        private void shareButton_Click(object sender, EventArgs e)
        {
            try
            {
                string textForPost = string.Format(
                    "Looking for {0} {1} {2} Anyone?",
                    m_ZodiacMatch.BestMatchedSign.Name,
                    m_LoggedInUser.InterestedIn,
                    Environment.NewLine);
                m_LoggedInUser.PostStatus(textForPost);
                MessageBox.Show(string.Format("Status Posted! {0}{1}", Environment.NewLine, textForPost));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not share post.");
            }
        }

        private void ZodiacSignForm_Load(object sender, EventArgs e)
        {

        }
    }
}
