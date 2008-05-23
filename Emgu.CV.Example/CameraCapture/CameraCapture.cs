using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using System.Threading;

namespace CameraCapture
{
    public partial class CameraCapture : Form
    {
        private Capture _capture;

        private Thread _captureThread;

        private bool _flipHorizontal;
        private bool _flipVertical;

        public CameraCapture()
        {
            InitializeComponent();
        }

        private void captureButtonClick(object sender, EventArgs e)
        {
            if (_captureThread == null) // if currently there is no capture thread running
            {
                if (_capture == null)
                {
                    try
                    {
                        _capture = new Capture();
                    }
                    catch (Emgu.PrioritizedException excpt)
                    {
                        excpt.Alert(true);
                        return;
                    }
                }
                _captureThread = new Thread(
                    delegate()
                    {
                        while (true)
                        {
                            Image<Bgr, Byte> frame = _capture.QueryFrame();

                            //determine if any fliping is required
                            if (_flipVertical || _flipHorizontal)
                            {
                                Emgu.CV.CvEnum.FLIP flipType;
                                if (_flipHorizontal && _flipVertical)
                                    flipType = Emgu.CV.CvEnum.FLIP.VERTICAL | Emgu.CV.CvEnum.FLIP.HORIZONTAL;
                                else if (_flipVertical)
                                    flipType = Emgu.CV.CvEnum.FLIP.VERTICAL;
                                else
                                    flipType = Emgu.CV.CvEnum.FLIP.HORIZONTAL;

                                frame = frame.Flip(flipType);
                            }
                        
                            Image<Gray, Byte> grayFrame = frame.Convert<Gray, Byte>();
                            Image<Gray, Byte> smallGrayFrame = grayFrame.PyrDown();
                            Image<Gray, Byte> smoothedGrayFrame = smallGrayFrame.PyrUp();
                            Image<Gray, Byte> cannyFrame = smoothedGrayFrame.Canny(new Gray(100), new Gray(60));

                            captureImageBox.Image = frame;
                            grayscaleImageBox.Image = grayFrame;
                            smoothedGrayscaleImageBox.Image = smoothedGrayFrame;
                            cannyImageBox.Image = cannyFrame;

                        }
                    }
                    );

                captureButton.Text = "Stop";

                _captureThread.Start();

            }
            else // if currently capturing
            {
                if (_captureThread != null)
                {
                    _captureThread.Abort(); 
                    _captureThread = null;
                }
                
                captureButton.Text = "Start Capture";
            }
        }

        private void ReleaseData()
        {
            if (_captureThread != null)
                _captureThread.Abort();

            if (_capture != null)
                _capture.Dispose();
        }

        private void FlipHorizontalButtonClick(object sender, EventArgs e)
        {
            _flipHorizontal = !_flipHorizontal;
        }

        private void FlipVerticalButtonClick(object sender, EventArgs e)
        {
            _flipVertical = !_flipVertical;
        }
    }
}