using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;

namespace Emgu.CV.UI
{
    public partial class ImageProperty : UserControl
    {
        public ImageProperty()
        {
            InitializeComponent();
        }

        public int ImageWidth
        {
            set
            {
                widthTextbox.Text = value.ToString();
            }
        }

        public int ImageHeight
        {
            set
            {
                heightTextBox.Text = value.ToString();
            }
        }

        public String ColorType
        {
            set
            {
                colorTypeTexbox.Text = value;
            }
        }

        public Point2D<int> MousePositionOnImage
        {
            set
            {
                mousePositionTextbox.Text = String.Format("[{0}, {1}]", value.X, value.Y);
            }
        }

        public ColorType ColorIntensity
        {
            set
            {
                colorIntensityTextbox.Text = String.Format("[{0}]",
                    value == null ? "" : String.Join(",", System.Array.ConvertAll<double, String>(value.Coordinate, System.Convert.ToString)));
            }
        }

        public System.Type ColorDepth
        {
            set
            {
                colorDepthTextBox.Text = value.Name;
            }
        }

        public String OperationStackText
        {
            set
            {
                operationStackTextBox.Text = value;
            }
        }
    }
}
