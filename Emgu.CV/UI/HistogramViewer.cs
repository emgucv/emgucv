using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Emgu.CV.UI
{
    public partial class HistogramViewer : Form
    {
        public HistogramViewer()
        {
            InitializeComponent();
        }

        public HistogramCtrl HistogramCtrl
        {
            get
            {
                return histogramCtrl1;
            }
        }
    }
}