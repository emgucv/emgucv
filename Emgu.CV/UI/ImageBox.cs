using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using Emgu.CV;
using Emgu.Util;

namespace Emgu.CV.UI
{
   /// <summary>
   /// An image box is a user control that is similar to picture box, but display Emgu CV IImage and provides enhenced functionalities.
   /// </summary>
   public partial class ImageBox : PictureBox
   {
      private IImage _image;
      private IImage _displayedImage;
      private PropertyDlg _propertyDlg;

      private Stack<Operation<IImage>> _operationStack;

      //private double _zoomLevel = 1.0;

      /// <summary>
      /// one of the parameters used for caculating the frame rate
      /// </summary>
      private DateTime _counterStartTime;
      /// <summary>
      /// one of the parameters used for caculating the frame rate
      /// </summary>
      private int _imageReceivedSinceCounterStart;

      /// <summary>
      /// Create a ImageBox
      /// </summary>
      public ImageBox()
         : base()
      {
         InitializeComponent();

         _operationStack = new Stack<Operation<IImage>>();

         AddOperationMenuItem();
      }

      /// <summary>
      /// Push the specific operation onto the stack
      /// </summary>
      /// <param name="operation">The operation to be pushed onto the stack</param>
      public void PushOperation(Operation<IImage> operation)
      {
         _operationStack.Push(operation);
         ImageProperty panel = ImagePropertyPanel;
         if (panel != null)
            panel.SetOperationStack(_operationStack);

         try
         {
            Image = Image;
         }
         catch (Exception e)
         {
            _operationStack.Pop();
            if (panel != null)
               panel.SetOperationStack(_operationStack);
            Image = Image;
            throw (e);
         }
      }

      /// <summary>
      /// Remove all the operations from the stack
      /// </summary>
      public void ClearOperation()
      {
         _operationStack.Clear();
         ImageProperty panel = ImagePropertyPanel;
         if (panel != null) panel.SetOperationStack(_operationStack);
         Image = Image;
      }

      /// <summary>
      /// Remove the last added operation from the stack
      /// </summary>
      public void PopOperation()
      {
         if (_operationStack.Count > 0)
         {
            _operationStack.Pop();
            ImageProperty panel = ImagePropertyPanel;
            if (panel != null) panel.SetOperationStack(_operationStack);
            Image = Image;
         }
      }

      private void AddOperationMenuItem()
      {
         foreach (MethodInfo mi in GetImageMethods())
         {
            ToolStripMenuItem operationMenuItem = new ToolStripMenuItem();
            operationMenuItem.Size = new System.Drawing.Size(152, 22);
            operationMenuItem.Text = String.Format("{0}({1})", mi.Name,
                String.Join(",", System.Array.ConvertAll<ParameterInfo, String>(mi.GetParameters(), delegate(ParameterInfo pi) { return pi.Name; })));

            //This is necessary to handle delegate with a loop
            //Cause me lots of headache before reading the article on
            //http://decav.com/blogs/andre/archive/2007/11/18/wtf-quot-problems-quot-with-anonymous-delegates-linq-lambdas-and-quot-foreach-quot-or-quot-for-quot-loops.aspx
            //I wishes MSFT handle this better
            MethodInfo methodInfoRef = mi;

            operationMenuItem.Click += delegate(Object o, EventArgs e)
                {
                   /*
                   Object[] paramList = null;
                   paramList = ParamInputDlg.GetParams(methodInfoRef, paramList);
                   if (paramList != null)
                   {
                      Operation<IImage> operation = new Operation<IImage>(methodInfoRef, paramList);
                      try
                      {
                         PushOperation(operation);
                      }
                      catch (Exception expt)
                      {
                         MessageBox.Show(expt.InnerException.Message);
                      }
                   }*/
                   Object[] paramList = null;
                   do
                   {
                      paramList = ParamInputDlg.GetParams(methodInfoRef, paramList);
                      if (paramList != null)
                      {
                         Operation<IImage> operation = new Operation<IImage>(methodInfoRef, paramList);
                         try
                         {
                            PushOperation(operation);
                            paramList = null;
                         }
                         catch (Exception expt)
                         {
                            MessageBox.Show(expt.InnerException.Message);
                         }
                      }
                   } while (paramList != null);
                };

            operationsToolStripMenuItem.DropDownItems.Add(operationMenuItem);
         }
      }

      /// <summary>
      /// Set the image for this image box
      /// </summary>
      public new IImage Image
      {
         get
         {
            return _image;
         }
         set
         {
            if (InvokeRequired)
            {
               this.Invoke(new MethodInvoker(delegate() { Image = value; }));
            }
            else
            {
               _image = value;
               IImage imageToBeDisplayed = _image;

               if (imageToBeDisplayed != null)
               {
                  Operation<IImage>[] ops = _operationStack.ToArray();
                  System.Array.Reverse(ops);
                  foreach (Operation<IImage> operation in ops)
                  {
                     if (operation.Method.ReturnType == typeof(void))
                     {
                        //if the operation has return type of void, just execute the operation
                        operation.InvokeMethod(imageToBeDisplayed);
                     }
                     else if (operation.Method.ReturnType == typeof(IImage))
                     {
                        imageToBeDisplayed = operation.InvokeMethod(imageToBeDisplayed) as IImage;
                     }
                     else
                     {
                        throw new System.NotImplementedException(string.Format("Return type of {0} is not implemented.", operation.Method.ReturnType));
                     }
                  }
                  DisplayedImage = imageToBeDisplayed;
               }

               operationsToolStripMenuItem.Enabled = (_image != null);
            }
         }
      }

      /// <summary>
      /// The image that is being displayed. It's the Image following by some user defined image operation
      /// </summary>
      public IImage DisplayedImage
      {
         get
         {
            return _displayedImage;
         }
         set
         {
            _displayedImage = value;
            if (_displayedImage != null)
            {
               if (Width != _displayedImage.Width) Width = _displayedImage.Width;
               if (Height != _displayedImage.Height) Height = _displayedImage.Height;

               base.Image = _displayedImage.Bitmap;

               if (EnableProperty)
               {
                  ImagePropertyPanel.ImageWidth = _displayedImage.Width;
                  ImagePropertyPanel.ImageHeight = _displayedImage.Height;
                  Type imageType = Toolbox.GetBaseType(_displayedImage.GetType(), "Image`2");
                  ImagePropertyPanel.TypeOfColor = imageType.GetGenericArguments()[0];
                  ImagePropertyPanel.TypeOfDepth = imageType.GetGenericArguments()[1];

                  #region calculate the frame rate
                  TimeSpan ts = DateTime.Now.Subtract(_counterStartTime);
                  if (ts.TotalSeconds > 1)
                  {
                     ImagePropertyPanel.FramesPerSecondText = _imageReceivedSinceCounterStart;
                     //reset the counter
                     _counterStartTime = DateTime.Now;
                     _imageReceivedSinceCounterStart = 0;
                  }
                  else
                  {
                     _imageReceivedSinceCounterStart++;
                  }
                  #endregion
               }
            }
         }
      }

      private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
      {
         DialogResult res = openFileDialog1.ShowDialog();
         if (res == DialogResult.OK)
         {
            try
            {
               String filename = openFileDialog1.FileName;
               Image<Bgr, Byte> img = new Image<Bgr, byte>(filename);
               Image = img;
            }
            catch (Exception excpt)
            {
               MessageBox.Show(excpt.Message);
            }
         }
      }

      private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         DialogResult res = saveFileDialog1.ShowDialog();
         if (res == DialogResult.OK)
         {
            try
            {
               String filename = saveFileDialog1.FileName;
               _image.Save(filename);
            }
            catch (Exception excpt)
            {
               MessageBox.Show(excpt.Message);
            }
         }
      }

      private static IEnumerable<MethodInfo> GetImageMethods()
      {
         foreach (MemberInfo mi in typeof(IImage).GetMembers())
         {
            if (mi.MemberType == MemberTypes.Method)
            {
               Object[] atts = mi.GetCustomAttributes(typeof(ExposableMethodAttribute), false);
               if (atts.Length == 0 || ((ExposableMethodAttribute)atts[0]).Exposable)
                  yield return mi as MethodInfo;
            }
         }
      }

      private void propertyToolStripMenuItem_Click(object sender, EventArgs e)
      {
         EnableProperty = !EnableProperty;
      }

      private ImageProperty ImagePropertyPanel
      {
         get
         {
            return EnableProperty ? _propertyDlg.ImagePropertyPanel : null;
         }
      }

      private bool EnableProperty
      {
         get
         {
            return _propertyDlg != null;
         }
         set
         {
            if (value)
            {   //this is a call to enable the property dlg
               if (_propertyDlg == null) _propertyDlg = new PropertyDlg(this);
               _propertyDlg.Show();

               _propertyDlg.FormClosed +=
                   delegate(object sender, FormClosedEventArgs e)
                   {
                      _propertyDlg = null;
                   };

               ImagePropertyPanel.SetOperationStack(_operationStack);

               // reset the image such that the property is updated
               Image = Image;
            }
            else
            {
               if (_propertyDlg != null)
               {
                  _propertyDlg.Focus();
               }
            }
         }
      }

      /// <summary>
      /// Used for tracking the mouse position on the image
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void onMouseMove(object sender, MouseEventArgs e)
      {
         if (EnableProperty)
         {
            Point2D<int> location = new Point2D<int>();

            ColorType color = null;

            IImage img = DisplayedImage;

            if (img != null)
            {
               location.X = Math.Min(e.X, DisplayedImage.Width - 1);
               location.Y = Math.Min(e.Y, DisplayedImage.Height - 1);

               Type t = img.GetType();
               MethodInfo indexers = t.GetMethod("get_Item", new Type[2] { typeof(int), typeof(int) });
               if (indexers != null)
               {
                  color = indexers.Invoke(img, new object[2] { location.Y, location.X }) as ColorType;
               }
               else
               {
                  color = new Bgra();
               }
            }

            ImagePropertyPanel.MousePositionOnImage = location;
            ImagePropertyPanel.ColorIntensity = color;
         }
      }
   }
}
