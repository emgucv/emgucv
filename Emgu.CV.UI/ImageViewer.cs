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
      /// Create an ImageViewer from the specific <paramref name="img"/>
      /// </summary>
      /// <param name="image">The image to be displayed in this viewer</param>
      public ImageViewer(IImage image)
         : this()
      {
         if (image != null)
         {
            System.Drawing.Size size = image.Size;
            int width = size.Width + 8;
            int height = size.Height + 32;
            if (this.Width != width)
            {
               this.Width = width;
            }
            if (this.Height != height)
            {
               this.Height = height;
            }
         }
         Image = image;
      }

      /// <summary>
      /// Create an ImageViewer from the specific <paramref name="img"/>, using <paramref name="imageName"/> as window name
      /// </summary>
      /// <param name="image">The image to be displayed</param>
      /// <param name="imageName">The name of the image</param>
      public ImageViewer(IImage image, string imageName)
         : this(image)
      {
         this.Text = imageName;
      }

      /// <summary>
      /// Get or Set the image in this ImageViewer
      /// </summary>
      public IImage Image
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
      /// Create a ImageViewer with the specific image and show it.
      /// </summary>
      /// <param name="image">The image to be displayed in ImageViewer</param>
      public static void Show(IImage image)
      {
         Application.Run(new ImageViewer(image));
      }

      /// <summary>
      /// Create a ImageViewer with the specific image and show it.
      /// </summary>
      /// <param name="image">The image to be displayed in ImageViewer</param>
      /// <param name="windowName">The name of the window</param>
      public static void Show(IImage image, String windowName)
      {
         Application.Run(new ImageViewer(image, windowName));
      }
   }
}
