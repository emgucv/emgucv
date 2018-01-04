//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Windows.Forms;

namespace Emgu.UI
{
   public partial class ComboBoxSelector : UserControl
   {
      /// <summary>
      /// Create a ComboBox Selector
      /// </summary>
      public ComboBoxSelector()
      {
         InitializeComponent();
         comboBox1.DataSourceChanged += delegate
            {
               System.Collections.IEnumerable e = (System.Collections.IEnumerable)comboBox1.DataSource;
               int count = 0;
               if (e != null)
               {
                  System.Collections.IEnumerator etr = e.GetEnumerator();
                  etr.Reset();
                  while (etr.MoveNext())
                     count++;
               }
               ItemSizeLabel.Text = count.ToString();
            };
         comboBox1.SelectedIndexChanged += delegate
            {
               NextButton.Enabled = (comboBox1.SelectedIndex != comboBox1.Items.Count - 1);
               PreviousButton.Enabled = (comboBox1.SelectedIndex != 0);
               SelectedIndexLabel.Text = String.Format("{0}", comboBox1.SelectedIndex + 1);
            };
      }

      /// <summary>
      /// Get the ComboBox in this control
      /// </summary>
      public ComboBox ComboBox
      {
         get
         {
            return comboBox1;
         }
      }

      /// <summary>
      /// Clear the combobox
      /// </summary>
      public void Clear()
      {
         comboBox1.DataSource = null;
         comboBox1.SelectedIndex = -1;
         NextButton.Enabled = false;
         PreviousButton.Enabled = false;
         comboBox1.Text = String.Empty;
      }

      private void NextButton_Click_1(object sender, EventArgs e)
      {
         comboBox1.SelectedIndex = comboBox1.SelectedIndex + 1;
      }

      private void PreviousButton_Click_1(object sender, EventArgs e)
      {
         comboBox1.SelectedIndex = comboBox1.SelectedIndex - 1;
      }
   }
}
