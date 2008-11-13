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

      private Stack<Operation> _operationStack;

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

         _operationStack = new Stack<Operation>();
      }

      /// <summary>
      /// Push the specific operation onto the stack
      /// </summary>
      /// <param name="operation">The operation to be pushed onto the stack</param>
      public void PushOperation(Operation operation)
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
      
      private ToolStripMenuItem[] BuildOperationTree( IEnumerable<KeyValuePair<string, MethodInfo>> catelogMiPairList)
      {
         List<ToolStripMenuItem> res = new List<ToolStripMenuItem>();

         SortedDictionary<String, List<KeyValuePair<String, MethodInfo>>> catelogDic = new SortedDictionary<string, List<KeyValuePair<String, MethodInfo>>>();
         SortedDictionary<String, MethodInfo> operationItem = new SortedDictionary<string, MethodInfo>();

         foreach (KeyValuePair<string, MethodInfo> pair in catelogMiPairList)
         {
            if (!pair.Key.Equals(String.Empty))
            {

               String[] catelogs = pair.Key.Split('|');
               if (!catelogDic.ContainsKey(catelogs[0]))
               {
                  catelogDic.Add(catelogs[0], new List<KeyValuePair<String, MethodInfo>>());
               }
               string[] subcatelogs = new string[catelogs.Length-1];
               Array.Copy(catelogs, 1, subcatelogs, 0, subcatelogs.Length);

               catelogDic[catelogs[0]].Add(new KeyValuePair<String, MethodInfo>(String.Join("|", subcatelogs), pair.Value));
            }
            else
            {
               operationItem.Add(pair.Value.Name, pair.Value);
            }
         }

         foreach (String catelog in catelogDic.Keys)
         {
            ToolStripMenuItem catelogMenuItem = new ToolStripMenuItem();
            catelogMenuItem.Text = catelog;
            catelogMenuItem.DropDownItems.AddRange(BuildOperationTree(catelogDic[catelog]));
            res.Add(catelogMenuItem);  
         }

         foreach (MethodInfo mi in operationItem.Values)
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
                   Object[] paramList = null;
                   do
                   {
                      paramList = ParamInputDlg.GetParams(methodInfoRef, paramList);
                      if (paramList != null)
                      {
                         Operation operation = new Operation(methodInfoRef, paramList);
                         try
                         {
                            PushOperation(operation);
                            paramList = null;
                         }
                         catch (Exception expt)
                         {
                            if (expt.InnerException != null)
                               MessageBox.Show(expt.InnerException.Message);
                            else
                               MessageBox.Show(expt.Message);

                            if (methodInfoRef.GetParameters().Length == 0)
                            {
                               paramList = null;
                            }
                         }
                      }
                   } while (paramList != null);
                };
            res.Add(operationMenuItem);
         }

         return res.ToArray();
      }

      private void BuildOperationMenuItem(IImage image)
      {
         operationsToolStripMenuItem.DropDownItems.Clear();
         //List<KeyValuePair<String, MethodInfo>> l = new List<KeyValuePair<string,MethodInfo>>( Reflection.ReflectIImage.GetImageMethods(image));
         //int n = l.Count;
         operationsToolStripMenuItem.DropDownItems.AddRange(
            BuildOperationTree( Reflection.ReflectIImage.GetImageMethods(image)));
         
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
                  bool isCloned = false;
                  Operation[] ops = _operationStack.ToArray();
                  System.Array.Reverse(ops);
                  foreach (Operation operation in ops)
                  {
                     if (operation.Method.ReturnType == typeof(void))
                     {
                        if (!isCloned)
                        {
                           imageToBeDisplayed = _image.Clone() as IImage;
                           isCloned = true;
                        }
                        //if the operation has return type of void, just execute the operation
                        operation.InvokeMethod(imageToBeDisplayed);
                     }
                     else if (operation.Method.ReturnType.GetInterface("IImage") != null)
                     {
                        isCloned = true;
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
               BuildOperationMenuItem(_displayedImage);

               if (Width != _displayedImage.Width) Width = _displayedImage.Width;
               if (Height != _displayedImage.Height) Height = _displayedImage.Height;

               base.Image = _displayedImage.Bitmap;

               if (EnableProperty)
               {
                  ImagePropertyPanel.ImageWidth = _displayedImage.Width;
                  ImagePropertyPanel.ImageHeight = _displayedImage.Height;

                  ImagePropertyPanel.TypeOfColor = Reflection.ReflectIImage.GetTypeOfColor(_displayedImage);
                  ImagePropertyPanel.TypeOfDepth = Reflection.ReflectIImage.GetTypeOfDepth(_displayedImage);

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
            Point2D<int> location = new Point2D<int>(e.X, e.Y);
            IImage img = DisplayedImage;

            ColorType color = img == null ?
               null :
               Reflection.ReflectIImage.GetPixelColor(img, location);
            
            ImagePropertyPanel.MousePositionOnImage = location;
            ImagePropertyPanel.ColorIntensity = color;
         }
      }
   }
}
