//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using Emgu.Util;

namespace Emgu.CV.UI
{
   /// <summary>
   /// A MatrixViewer that is used to visualize a matrix
   /// </summary>
   public partial class MatrixViewer : Form
   {
      /// <summary>
      /// Create a MatrixViewer
      /// </summary>
      public MatrixViewer()
      {
         InitializeComponent();
      }

      /// <summary>
      /// Get or Set the Matrix&lt;&gt; object; for this MatrixViewer
      /// </summary>
      public UnmanagedObject Matrix
      {
         get
         {
            return matrixBox.Matrix;
         }
         set
         {
            matrixBox.Matrix = value;
         }
      }
   }
}