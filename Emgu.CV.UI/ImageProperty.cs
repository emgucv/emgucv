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
         cSharpOperationView.Language = ProgrammingLanguage.CSharp;
         cPlusPlusOperationView.Language = ProgrammingLanguage.CPlusPlus;
         
         for (int i = 0; i < ImageBox.ZoomLevels.Length; i++)
         {
            zoomLevelComboBox.Items.Add(String.Format("{0}%", (int)(ImageBox.ZoomLevels[i] * 100)));
         }
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
            mousePositionTextbox.Text = value.ToString();
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
      /// Set the description of the operation view
      /// </summary>
      public void SetOperations(List<Operation> operations)
      {
         cSharpOperationView.DisplayOperations(operations);
         cPlusPlusOperationView.DisplayOperations(operations);
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

      private void clearOperationBtn_Click(object sender, EventArgs e)
      {
         _imageBox.ClearOperation();
      }

      private void popOperationButton_Click(object sender, EventArgs e)
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

      private void zoomLevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (_imageBox != null)
         {
            _imageBox.ZoomScale = ImageBox.ZoomLevels[zoomLevelComboBox.SelectedIndex];;
         }
      }
   }
}
