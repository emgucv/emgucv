using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.Util.TypeEnum;

namespace Emgu.CV.UI
{
   internal partial class ImageProperty : UserControl
   {
      private ImageBox _imageBox;

      /// <summary>
      /// The parent imagebox for this property panel
      /// </summary>
      public ImageBox ImageBox
      {
         get { return _imageBox; }
         set { _imageBox = value; }
      }

      /// <summary>
      /// Create a ImageProperty control
      /// </summary>
      public ImageProperty()
      {
         InitializeComponent();
         cSharpOperationStackView.Language = ProgrammingLanguage.CSharp;
         cPlusPlusoperationStackView.Language = ProgrammingLanguage.CPlusPlus;
      }

      public System.Drawing.Size ImageSize
      {
         set
         {
            widthTextbox.Text = value.Width.ToString();
            heightTextBox.Text = value.Height.ToString();
         }
      }

      /// <summary>
      /// Set the Type of the color
      /// </summary>
      public Type TypeOfColor
      {
         set
         {
            Object[] colorAttributes = value.GetCustomAttributes(typeof(ColorInfoAttribute), true);
            if (colorAttributes.Length > 0)
            {
               ColorInfoAttribute info = (ColorInfoAttribute)colorAttributes[0];
               typeOfColorTexbox.Text = info.ConversionCodename;
            }
            else
            {
               typeOfColorTexbox.Text = "Unknown";
            }
         }
      }

      /// <summary>
      /// Set the mouse position over the image
      /// </summary>
      public System.Drawing.Point MousePositionOnImage
      {
         set
         {
            mousePositionTextbox.Text = String.Format("[{0}, {1}]", value.X, value.Y);
         }
      }

      /// <summary>
      /// Set the color intensity of the pixel on the image where is mouse is at
      /// </summary>
      public IColor ColorIntensity
      {
         set
         {
            colorIntensityTextbox.Text = 
                value == null ? String.Empty : value.ToString();
         }
      }

      /// <summary>
      /// Set the Depth of the image
      /// </summary>
      public Type TypeOfDepth
      {
         set
         {
            typeOfDepthTextBox.Text = value.Name;
         }
      }

      /// <summary>
      /// Set the description of the operation stack
      /// </summary>
      public void SetOperationStack(Stack<Operation> stack)
      {
         cSharpOperationStackView.DisplayOperationStack(stack);
         cPlusPlusoperationStackView.DisplayOperationStack(stack);
      }

      /// <summary>
      /// Set the frame rate
      /// </summary>
      public int FramesPerSecondText
      {
         set
         {
            fpsTextBox.Text = value.ToString();
         }
      }

      private void clearStackBtn_Click(object sender, EventArgs e)
      {
         _imageBox.ClearOperation();
      }

      private void popStackButton_Click(object sender, EventArgs e)
      {
         _imageBox.PopOperation();
      }

      private void showHistogramButton_Click(object sender, EventArgs e)
      {
         IImage image = _imageBox.DisplayedImage;

         if (image == null)
         {
            MessageBox.Show("Please load an image first");
            return;
         }

         HistogramViewer.Show(image);
      }
   }
}
