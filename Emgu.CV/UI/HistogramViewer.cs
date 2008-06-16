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
    /// A view for histogram
    /// </summary>
    public partial class HistogramViewer : Form
    {
        /// <summary>
        /// Constructor for histogram viewer
        /// </summary>
        public HistogramViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get the histogram control of this viewer
        /// </summary>
        public HistogramCtrl HistogramCtrl
        {
            get
            {
                return histogramCtrl1;
            }
        }
    }
}