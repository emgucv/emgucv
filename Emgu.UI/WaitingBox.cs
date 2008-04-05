using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Emgu.UI
{
    public partial class WaitDialog : Form
    {
        private bool _continute;
        private Thread _t;

        public new void Close()
        {
            StopProgress();
            base.Close();
        }

        public WaitDialog()
        {
            InitializeComponent();
          
        }

        public void StartProgress()
        {   
            // if there are any current progress, stop it first
            StopProgress();
            
            _continute = true;
            long t1 = DateTime.Now.Ticks;

            _t = new Thread(
                   delegate()
                   {
                       while (_continute)
                       {
                           System.Threading.Thread.Sleep(200);
                           if (this.components != null)
                               Invoke((MethodInvoker)
                               delegate()
                               {
                                   progressBar1.Value = (int)((DateTime.Now.Ticks - t1) >> 18) % 100;
                               });
                       }
                   }
               );
        
            _t.Start();
        }

        public void StopProgress()
        {
            _continute = false;
            if (_t != null)
                _t.Join();
        }
    }
}