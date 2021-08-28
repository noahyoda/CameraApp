using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Size = System.Drawing.Size;
using Point = System.Drawing.Point;

namespace Camera
{
    public partial class Form1 : Form
    {
        VideoCapture capture;
        Mat frame;
        Bitmap image;
        private Thread camera1;
        bool isCameraRunning = false;
        PictureBox picBox;
        public Form1()
        {
            InitializeComponent();
            this.Shown += CreateButtonDelegate;
            this.Shown += CreatePictureBoxDelegate;
        }

        private void CaptureCamera()
        {
            camera1 = new Thread(new ThreadStart(CaptureCameraCallback));
            camera1.Start();
        }

        private void CaptureCameraCallback()
        {
            frame = new Mat();
            capture = new VideoCapture(0);
            capture.Open(0);
            if (capture.IsOpened())
            {
                while (isCameraRunning)
                {
                    capture.Read(frame);
                    image = BitmapConverter.ToBitmap(frame);

                    if(picBox.Image != null)
                    {
                        picBox.Image.Dispose();
                    }
                    picBox.Image = image;
                }
            }
        }

        private void button1_Clicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (clickedButton.Text.Equals("Start"))
            {
                CaptureCamera();
                clickedButton.Text = "Stop";
                isCameraRunning = true;
            } else
            {
                capture.Release();
                clickedButton.Text = "Start";
                isCameraRunning = false;
            }
        }

        public void CreatePictureBoxDelegate(object sender, EventArgs e)
        {
            PictureBox newBox = new PictureBox();
            newBox.ClientSize = new Size(Width, Height - 100);
            picBox = newBox;
        }

        public void CreateButtonDelegate(object sender, EventArgs e)
        {
            Button capture = MakeButton("Capture", new Point(50, 350), new Size(300, 80));
            Button startBtn = MakeButton("Start", new Point(450, 350), new Size(300, 80));
            startBtn.Click += new System.EventHandler(this.button1_Clicked);

        }

        public Button MakeButton(string t, Point p, Size s)
        {
            Button newButton = new Button();
            this.Controls.Add(newButton);
            newButton.Text = t;
            newButton.Location = p;
            newButton.Size = s;
            return newButton;
        }

    }
}
