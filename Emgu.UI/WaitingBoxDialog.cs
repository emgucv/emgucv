//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using System.Threading;

namespace Emgu.UI
{
   /// <summary>
   /// A dialog that display a waiting box
   /// </summary>
   public partial class WaitingBoxDialog : Form
   {
      private bool _continute;
      private Thread _t;

      /// <summary>
      /// Close the dialog
      /// </summary>
      public new void Close()
      {
         StopProgress();
         base.Close();
      }

      /// <summary>
      /// Create the waiting box dialog
      /// </summary>
      public WaitingBoxDialog()
      {
         InitializeComponent();
      }

      /// <summary>
      /// Start the progress
      /// </summary>
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
                      if (components != null)
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

      /// <summary>
      /// stop the progress
      /// </summary>
      public void StopProgress()
      {
         _continute = false;
         if (_t != null)
            _t.Join();
      }
   }
}