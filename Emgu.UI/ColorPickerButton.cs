//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Emgu.UI
{
   /// <summary>
   /// A button for picking the color. The picked color is accesible from the BackColor property.
   /// </summary>
   public class ColorPickerButton : Button
   {
      /// <summary>
      /// Create a button for picking the color
      /// </summary>
      public ColorPickerButton()
         : base()
      {
         InitializeComponent();
      }

      private ColorDialog colorDialog1;

      private void InitializeComponent()
      {
         this.colorDialog1 = new System.Windows.Forms.ColorDialog();
         this.SuspendLayout();
         this.ResumeLayout(false);

         this.Click += new EventHandler(ColorPicker_Click);
      }

      void ColorPicker_Click(object sender, EventArgs e)
      {
         if (colorDialog1.ShowDialog() == DialogResult.OK)
         {
            BackColor = colorDialog1.Color;
         }
      }
   }
}
