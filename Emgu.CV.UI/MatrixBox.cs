//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
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
      /// Get or Set the Matrix&lt;TDepth&gt; object; of this MatrixBox
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
            if (_matrix == null) return;

            Size size = CvInvoke.cvGetSize(_matrix.Ptr);
            int numberOfChannels = ((MCvMat)Marshal.PtrToStructure(_matrix.Ptr, typeof(MCvMat))).NumberOfChannels;

            String[] channelNames = new string[numberOfChannels + 1];
            channelNames[0] = "All channels";
            for (int i = 1; i <= numberOfChannels; i++)
               channelNames[i] = String.Format("Channel {0}", i);

            channelSelectComboBox.DataSource = channelNames;
            channelSelectComboBox.SelectedIndex = 0;

            if (numberOfChannels == 1)
            {
               channelLabel.Visible = false;
               channelSelectComboBox.Visible = false;
            }
            else
            {
               channelLabel.Visible = true;
               channelSelectComboBox.Visible = true;
            }

            if (size.Width * size.Height * numberOfChannels > 1000)
            {
               channelLabel.Visible = false;
               channelSelectComboBox.Visible = false;
               errorMsg.Text = "The matrix is too big to be displayed.";
               return;
            }
            else
            {
               errorMsg.Text = String.Empty;
            }

            for (int i = 0; i < size.Width; i++)
            {
               string columnName = String.Format("Col {0}", i);
               dataGridView1.Columns.Add(columnName, columnName);
               dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.Rows.Add(size.Height);

            VisualizerChannel(size, numberOfChannels, channelSelectComboBox.SelectedIndex);
            channelSelectComboBox.SelectedIndexChanged += channelSelectComboBox_SelectedIndexChanged;

         }
      }

      void channelSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         Size size = CvInvoke.cvGetSize(Matrix.Ptr);
         int numberOfChannels = ((MCvMat)Marshal.PtrToStructure(_matrix.Ptr, typeof(MCvMat))).NumberOfChannels;
         VisualizerChannel(size, numberOfChannels, channelSelectComboBox.SelectedIndex);
      }

      private void VisualizerChannel(Size size, int numberOfChannels, int channelToBeDisplayed)
      {

         if (channelToBeDisplayed == 0)
         #region Display all channels
         {
            switch (numberOfChannels)
            {
               case 1:
                  for (int i = 0; i < size.Height; i++)
                  {
                     dataGridView1.Rows[i].HeaderCell.Value = String.Format("Row {0}", i);
                     for (int j = 0; j < size.Width; j++)
                        dataGridView1[j, i].Value = CvInvoke.cvGet2D(_matrix.Ptr, i, j).V0;
                  }
                  break;
               case 2:
                  for (int i = 0; i < size.Height; i++)
                  {
                     dataGridView1.Rows[i].HeaderCell.Value = String.Format("Row {0}", i);
                     for (int j = 0; j < size.Width; j++)
                     {
                        MCvScalar scalar = CvInvoke.cvGet2D(_matrix.Ptr, i, j);
                        dataGridView1[j, i].Value = String.Format("[{0},{1}]", scalar.V0, scalar.V1);
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
                        dataGridView1[j, i].Value = String.Format("[{0},{1},{2}]", scalar.V0, scalar.V1, scalar.V2);
                     }
                  }
                  break;
               case 4:
               default:
                  for (int i = 0; i < size.Height; i++)
                  {
                     dataGridView1.Rows[i].HeaderCell.Value = String.Format("Row {0}", i);
                     for (int j = 0; j < size.Width; j++)
                     {
                        MCvScalar scalar = CvInvoke.cvGet2D(_matrix.Ptr, i, j);
                        dataGridView1[j, i].Value = String.Format("[{0},{1},{2},{3}]", scalar.V0, scalar.V1, scalar.V2, scalar.V3);
                     }
                  }
                  break;
            }
         }
         #endregion
         else
         #region Display a specific channel
         {
            for (int i = 0; i < size.Height; i++)
            {
               dataGridView1.Rows[i].HeaderCell.Value = String.Format("Row {0}", i);
               for (int j = 0; j < size.Width; j++)
                  dataGridView1[j, i].Value = CvInvoke.cvGet2D(_matrix.Ptr, i, j).ToArray()[channelToBeDisplayed - 1];
            }
         }
         #endregion
      }
   }
}
