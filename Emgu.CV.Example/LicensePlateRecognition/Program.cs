using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace LicensePlateRecognition
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         if (64 == CvInvoke.UnmanagedCodeBitness && Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows)
         {
            MessageBox.Show("This program is only designed to be run with the 32-bit Emgu CV package on windows.");
            return;
         }

         if (!IsPlaformCompatable()) return;

         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new LicensePlateRecognitionForm());
      }

      /// <summary>
      /// Check if both the managed and unmanaged code are compiled for the same architecture
      /// </summary>
      /// <returns>Returns true if both the managed and unmanaged code are compiled for the same architecture</returns>
      static bool IsPlaformCompatable()
      {
         int clrBitness = Marshal.SizeOf(typeof(IntPtr)) * 8;
         if (clrBitness != CvInvoke.UnmanagedCodeBitness)
         {
            MessageBox.Show(String.Format("Platform mismatched: CLR is {0} bit, C++ code is {1} bit."
               + " Please consider recompiling the executable with the same platform target as C++ code.",
               clrBitness, CvInvoke.UnmanagedCodeBitness));
            return false;
         }
         return true;
      }
   }
}