//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;

namespace ImageDatabase
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         if (64 == CvInvoke.UnmanagedCodeBitness)
         {
            MessageBox.Show("This program is only designed to be run with the 32-bit Emgu CV package.");
            return;
         }
         if (Marshal.SizeOf(typeof(IntPtr)) == 8)
         {
            MessageBox.Show("This program can only be run in 32-bit mode. Conside changing the platform target to x86.");
            return;
         }
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new MainForm());
      }
   }
}