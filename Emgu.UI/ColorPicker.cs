using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
//using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Emgu.UI
{
   public partial class ColorPicker : UserControl
   {
      public ColorPicker()
      {
         InitializeComponent();
      }

      public Color SelectedColor
      {
         get
         {
            return colorPickerButton1.BackColor;
         }
         set
         {
            colorPickerButton1.BackColor = value;
         }
      }
   }
}
