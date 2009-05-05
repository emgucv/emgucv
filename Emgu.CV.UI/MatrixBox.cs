using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;

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

               for (int i = 0; i < size.Width; i++)
               {
                  string columnName = String.Format("Col {0}", i);
                  dataGridView1.Columns.Add(columnName, columnName);
                  dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;               
               }
               dataGridView1.Rows.Add(size.Height);

               for (int i = 0; i < size.Height; i++)
               {
                  dataGridView1.Rows[i].HeaderCell.Value = String.Format("Row {0}", i);
                  for (int j = 0; j < size.Width; j++)
                      dataGridView1[j,i].Value = CvInvoke.cvGet2D(_matrix.Ptr, i, j).v0;
               }
            }
         }
      }
   }
}
