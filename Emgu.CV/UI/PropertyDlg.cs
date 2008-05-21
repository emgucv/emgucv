using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Emgu.CV.UI
{
    public partial class PropertyDlg : Form
    {
        public PropertyDlg()
        {
            InitializeComponent();
        }

        public ImageProperty ImagePropertyPanel
        {
            get
            {
                return imageProperty1;
            }
        }
    }
}