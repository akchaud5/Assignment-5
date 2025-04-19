using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;

namespace OnlineStoreApp.Controls
{
    public partial class Controls_CaptchaControl : System.Web.UI.UserControl
    {
        // Public property to check if captcha is valid
        public bool IsValid
        {
            get
            {
                if (String.IsNullOrEmpty(txtCaptcha.Text))
                    return false;

                string captchaText = (string)Session["CaptchaText"];
                return txtCaptcha.Text.ToUpper() == captchaText;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateCaptcha();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }

        private void GenerateCaptcha()
        {
            // Generate random text
            string captchaText = GenerateRandomText(6);
            
            // Store in session
            Session["CaptchaText"] = captchaText.ToUpper();

            // Generate image
            byte[] imageBytes = GenerateCaptchaImage(captchaText);

            // Set image source
            string base64String = Convert.ToBase64String(imageBytes);
            imgCaptcha.Src = "data:image/jpeg;base64," + base64String;

            // Clear textbox
            txtCaptcha.Text = "";
        }

        private string GenerateRandomText(int length)
        {
            const string validChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            Random random = new Random();
            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }

            return new string(chars);
        }

        private byte[] GenerateCaptchaImage(string text)
        {
            // Simple CAPTCHA image generation
            using (Bitmap bitmap = new Bitmap(200, 80))
            using (Graphics g = Graphics.FromImage(bitmap))
            using (MemoryStream ms = new MemoryStream())
            {
                // Set up graphics
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Add noise
                Random random = new Random();
                for (int i = 0; i < 100; i++)
                {
                    int x1 = random.Next(bitmap.Width);
                    int y1 = random.Next(bitmap.Height);
                    int x2 = random.Next(bitmap.Width);
                    int y2 = random.Next(bitmap.Height);

                    g.DrawLine(new Pen(Color.FromArgb(random.Next(0, 100), random.Next(0, 100), random.Next(0, 100))), x1, y1, x2, y2);
                }

                // Draw text
                using (Font font = new Font("Arial", 24, FontStyle.Bold))
                {
                    g.DrawString(text, font, Brushes.DarkBlue, 30, 20);
                }

                // Add more noise
                for (int i = 0; i < 50; i++)
                {
                    int x = random.Next(bitmap.Width);
                    int y = random.Next(bitmap.Height);
                    int size = random.Next(1, 4);
                    g.FillEllipse(Brushes.Gray, x, y, size, size);
                }

                // Save to memory stream
                bitmap.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}