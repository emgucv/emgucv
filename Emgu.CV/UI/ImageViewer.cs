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
         Image = image;
      }

      /// <summary>
      /// Create an ImageViewer from the specific <paramref name="img"/>, using <paramref name="windowName"/> as window name
      /// </summary>
      /// <param name="image">The image to be displayed</param>
      /// <param name="windowName">The name of the window</param>
      public ImageViewer(IImage image, string windowName)
         : this(image)
      {
         this.Text = windowName;
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
            this.Width = value.Width + 8;
            this.Height = value.Height + 32;
            imageBox1.Image = value;
         }
      }
   }
}