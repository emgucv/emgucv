using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Emgu.CV.UI
{
    public partial class ImageProperty : UserControl
    {
        public ImageProperty()
        {
            InitializeComponent();
        }

        public int Width
        {
            set
            {
                widthTextbox.Text = value.ToString();
            }
        }

        public int Height
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

        public Point2D<int> MousePosition
        {
            set
            {
                mousePositionTextbox.Text = String.Format("[{0}, {1}]", value.X, value.Y);
            }
        }
    }
}
