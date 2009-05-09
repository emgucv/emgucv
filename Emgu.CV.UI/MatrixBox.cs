using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.UI
{
   /// <summary>
   /// A control that is used to visualize a matrix
   /// </summary>
   public partial class MatrixBox : UserControl
   {
      private UnmanagedObject _matrix;

      /// <summary>
      /// Create a MatrixBox
      /// </summary>
      public MatrixBox()
      {
         InitializeComponent();
      }

      /// <summary>
      /// Get or Set the Matrix&lt;&gt; object; for this MatrixBox
      /// </summary>
      public UnmanagedObject Matrix
      {
         get
         {
            return _matrix;
         }
         set
         {
            _matrix = value;
            if (_matrix != null)
            {
               Size size = CvInvoke.cvGetSize(_matrix.Ptr);
               int numberOfChannels = ((MCvMat)Marshal.PtrToStructure(_matrix.Ptr, typeof(MCvMat))).NumberOfChannels;

               for (int i = 0; i < size.Width; i++)
               {
                  string columnName = String.Format("Col {0}", i);
                  dataGridView1.Columns.Add(columnName, columnName);
                  dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;               
               }
               dataGridView1.Rows.Add(size.Height);

               switch (numberOfChannels)
               {
                  case 1:
                     for (int i = 0; i < size.Height; i++)
                     {
                        dataGridView1.Rows[i].HeaderCell.Value = String.Format("Row {0}", i);
                        for (int j = 0; j < size.Width; j++)
                           dataGridView1[j, i].Value = CvInvoke.cvGet2D(_matrix.Ptr, i, j).v0;
                     }
                     break;
                  case 2:
                     for (int i = 0; i < size.Height; i++)
                     {
                        dataGridView1.Rows[i].HeaderCell.Value = String.Format("Row {0}", i);
                        for (int j = 0; j < size.Width; j++)
                        {
                           MCvScalar scalar = CvInvoke.cvGet2D(_matrix.Ptr, i, j);
                           dataGridView1[j, i].Value = String.Format("[{0},{1}]", scalar.v0, scalar.v1);
                        }
                     }
                     break;
                  case 3:
                     for (int i = 0; i < size.Height; i++)
                     {
                        dataGridView1.Rows[i].HeaderCell.Value = String.Format("Row {0}", i);
                        for (int j = 0; j < size.Width; j++)
                        {
                           MCvScalar scalar = CvInvoke.cvGet2D(_matrix.Ptr, i, j);
                           dataGridView1[j, i].Value = String.Format("[{0},{1},{2}]", scalar.v0, scalar.v1, scalar.v2);
                        }
                     }
                     break;
                  case 4:
                     for (int i = 0; i < size.Height; i++)
                     {
                        dataGridView1.Rows[i].HeaderCell.Value = String.Format("Row {0}", i);
                        for (int j = 0; j < size.Width; j++)
                        {
                           MCvScalar scalar = CvInvoke.cvGet2D(_matrix.Ptr, i, j);
                           dataGridView1[j, i].Value = String.Format("[{0},{1},{2},{3}]", scalar.v0, scalar.v1, scalar.v2, scalar.v3);
                        }
                     }
                     break;
               }
            }
         }
      }
   }
}
