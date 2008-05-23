using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Emgu.CV.UI
{
    /// <summary>
    /// A dialog to display the property of an image
    /// </summary>
    public partial class PropertyDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get the image property panel
        /// </summary>
        public ImageProperty ImagePropertyPanel
        {
            get
            {
                return imageProperty1;
            }
        }
    }
}