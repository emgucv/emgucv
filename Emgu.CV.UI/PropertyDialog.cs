using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Emgu.CV.UI
{
   /// <summary>
   /// A dialog to display the property of an image
   /// </summary>
   internal partial class PropertyDialog : Form
   {
      private ImageBox _imageBox;

      /// <summary>
      /// Constructor
      /// </summary>
      public PropertyDialog(ImageBox imageBox)
      {
         InitializeComponent();
         _imageBox = imageBox;
         imageProperty1.ImageBox = _imageBox;
      }

      /// <summary>
      /// Get the image property panel
      /// </summary>
      public ImageProperty ImagePropertyPanel
      {
         get
         {
            return imageProperty1;
         }
      }
   }
}
