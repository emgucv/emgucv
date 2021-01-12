//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Emgu.CV.UI
{
   /// <summary>
   /// The Image viewer that display IImage
   /// </summary>
   public partial class ImageViewer : Form
   {
      /// <summary>
      /// Create an ImageViewer
      /// </summary>
      public ImageViewer()
      {
         InitializeComponent();
      }

      /// <summary>
      /// Create an ImageViewer from the specific <paramref name="image"/>
      /// </summary>
      /// <param name="image">The image to be displayed in this viewer</param>
      public ImageViewer(IInputArray image)
         : this()
      {
         if (image != null)
         {
             using (InputArray iaImage = image.GetInputArray())
             {
                 Size size = iaImage.GetSize();
                 size.Width += 12;
                 size.Height += 38;
                 if (!Size.Equals(size))
                     Size = size;
             }
         }
         Image = image;
      }

      /// <summary>
      /// Create an ImageViewer from the specific <paramref name="image"/>, using <paramref name="imageName"/> as window name
      /// </summary>
      /// <param name="image">The image to be displayed</param>
      /// <param name="imageName">The name of the image</param>
      public ImageViewer(IInputArray image, string imageName)
         : this(image)
      {
         Text = imageName;
      }

      /// <summary>
      /// Get or Set the image in this ImageViewer
      /// </summary>
      public IInputArray Image
      {
         get
         {
            return imageBox1.Image;
         }
         set
         {
            imageBox1.Image = value;
         }
      }

      /// <summary>
      /// Get the image box hosted in this viewer
      /// </summary>
      public ImageBox ImageBox
      {
         get
         {
            return imageBox1;
         }
      }

      /// <summary>
      /// Create a ImageViewer with the specific image and show it.
      /// </summary>
      /// <param name="image">The image to be displayed in ImageViewer</param>
      public static void Show(IInputArray image)
      {
         Application.Run(new ImageViewer(image));
      }

      /// <summary>
      /// Create a ImageViewer with the specific image and show it.
      /// </summary>
      /// <param name="image">The image to be displayed in ImageViewer</param>
      /// <param name="windowName">The name of the window</param>
      public static void Show(IInputArray image, String windowName)
      {
         Application.Run(new ImageViewer(image, windowName));
      }
   }
}
