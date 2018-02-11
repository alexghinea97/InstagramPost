using InstaSharp;
using InstaSharp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstagramPost
{
    public partial class Index : Form
    {
        public Index()
        {
            InitializeComponent();
        }

        private void Index_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //load file opem
            string imagePath = string.Empty;
            openFileDialog1.ShowDialog();
            if (openFileDialog1.CheckFileExists)
            {
                imagePath = openFileDialog1.FileName;
            }
            //upload to instagram
            var uploader = new InstagramUploader("username", ConvertToSecureString("password"));
            uploader.InvalidLoginEvent += InvalidLoginEvent;
            uploader.ErrorEvent += ErrorEvent;
            uploader.OnCompleteEvent += OnCompleteEvent;
            uploader.OnLoginEvent += OnLoginEvent;
            uploader.SuccessfulLoginEvent += SuccessfulLoginEvent;
            uploader.OnMediaConfigureStarted += OnMediaConfigureStarted;
            uploader.OnMediaUploadStartedEvent += OnMediaUploadStartedEvent;
            uploader.OnMediaUploadeComplete += OnmediaUploadCompleteEvent;
            uploader.UploadImage(imagePath, "#UploadedFromPC", true, true);
            txtDebug.AppendText("Your DeviceID is " + InstaSharp.Properties.Settings.Default.deviceId + Environment.NewLine);
        }

        public SecureString ConvertToSecureString(string strPassword)
        {
            var secureStr = new SecureString();
            if (strPassword.Length > 0)
            {
                foreach (var c in strPassword.ToCharArray()) secureStr.AppendChar(c);
            }
            return secureStr;
        }

        public void OnMediaUploadStartedEvent(object sender, EventArgs e)
        {
            txtDebug.AppendText("Attempting to upload image" + Environment.NewLine);
        }

        public void OnmediaUploadCompleteEvent(object sender, EventArgs e)
        {
            txtDebug.AppendText("The image was uploaded, but has not been configured yet." + Environment.NewLine);
        }

        public void OnMediaConfigureStarted(object sender, EventArgs e)
        {
            txtDebug.AppendText("The image has started to be configured" + Environment.NewLine);
        }

        public void SuccessfulLoginEvent(object sender, EventArgs e)
        {
            txtDebug.AppendText("Logged in! " + ((LoggedInUserResponse)e).FullName + Environment.NewLine);
        }

        public void OnLoginEvent(object sender, EventArgs e)
        {
            txtDebug.AppendText("Event fired for login: " + ((NormalResponse)e).Message + Environment.NewLine);
        }

        public void OnCompleteEvent(object sender, EventArgs e)
        {
            txtDebug.AppendText("Image posted to Instagram, here are all the urls" + Environment.NewLine);
            foreach (var image in ((UploadResponse)e).Images)
            {
                txtDebug.AppendText("Url: " + image.Url + Environment.NewLine);
                txtDebug.AppendText("Width: " + image.Width + Environment.NewLine);
                txtDebug.AppendText("Height: " + image.Height + Environment.NewLine);
            }
        }

        public void ErrorEvent(object sender, EventArgs e)
        {
            txtDebug.AppendText("Error " + ((ErrorResponse)e).Message + Environment.NewLine);
        }

        public void InvalidLoginEvent(object sender, EventArgs e)
        {
            txtDebug.AppendText("Error while logging " + ((ErrorResponse)e).Message + Environment.NewLine);
        }
    }
}
