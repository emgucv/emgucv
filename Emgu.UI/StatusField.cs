using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Emgu.UI
{
    public partial class StatusField : UserControl
    {
        private int _status;

        public StatusField()
        {
            _status = 0;
            InitializeComponent();
        }
        
        public int Status
        {
            get { return _status; }
            set 
            { 
                _status = value;
                label1.Text = String.Format("{0}% completed", _status);
                Progressbar1.Value = _status;
            }
        }

        public void Clear()
        {
            _status = 0;
            label1.Text = "unknown";
            Progressbar1.Value = 0;
        }
    }
}
