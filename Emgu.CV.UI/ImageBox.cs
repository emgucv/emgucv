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
using Emgu.CV.Structure;

namespace Emgu.CV.UI
{
   /// <summary>
   /// An image box is a user control that is similar to picture box, but display Emgu CV IImage and provides enhenced functionalities.
   /// </summary>
   public partial class ImageBox : PictureBox
   {
      #region private fileds
      private IImage _image;
      private IImage _displayedImage;
      private PropertyDialog _propertyDlg;

      private FunctionalModeOption _functionalMode = FunctionalModeOption.Everything;

      /// <summary>
      /// A cache to store the ToolStripMenuItems for different types of images
      /// </summary>
      private static readonly Dictionary<Type, ToolStripMenuItem[]> _typeToToolStripMenuItemsDictionary = new Dictionary<Type, ToolStripMenuItem[]>();

      private Stack<Operation> _operationStack;

      //private double _zoomLevel = 1.0;

      /// <summary>
      /// timer used for caculating the frame rate
      /// </summary>
      private DateTime _timerStartTime;

      /// <summary>
      /// one of the parameters used for caculating the frame rate
      /// </summary>
      private int _imageReceivedSinceCounterStart;
      #endregion

      /// <summary>
      /// Create a ImageBox
      /// </summary>
      public ImageBox()
         : base()
      {
         InitializeComponent();
         _operationStack = new Stack<Operation>();
      }

      #region properties
      /// <summary>
      /// Get or set the functional mode for the ImageBox
      /// </summary>
      [Bindable(false)]
      [Category("Design")]
      [DefaultValue(Emgu.CV.UI.ImageBox.FunctionalModeOption.Everything)]
      public FunctionalModeOption FunctionalMode
      {
         get { return _functionalMode; }
         set
         {
            //hide all menu items
            foreach (ToolStripMenuItem mi in contextMenuStrip1.Items)
               mi.Visible = false;

            if (value == FunctionalModeOption.Everything)
               foreach (ToolStripMenuItem mi in contextMenuStrip1.Items)
                  mi.Visible = true;

            _functionalMode = value;
         }
      }

      private bool EnablePropertyPanel
      {
         get
         {
            return _propertyDlg != null;
         }
         set
         {
            if (value)
            {   //this is a call to enable the property dlg
               if (_propertyDlg == null)
                  _propertyDlg = new PropertyDialog(this);

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
      /// Set the image for this image box
      /// </summary>
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
                  #region perform operations on _operationStack if there is any
                  if (_operationStack.Count > 0)
                  {
                     bool isCloned = false;
                     Operation[] ops = _operationStack.ToArray();
                     System.Array.Reverse(ops);
                     foreach (Operation operation in ops)
                     {
                        if (operation.Method.ReturnType == typeof(void))
                        {  //if this is an inplace operation 

                           if (!isCloned)
                           {  //make a clone if not already done so
                              imageToBeDisplayed = _image.Clone() as IImage;
                              isCloned = true;
                           }

                           //execute the inplace operation
                           operation.InvokeMethod(imageToBeDisplayed);
                        }
                        else if (operation.Method.ReturnType.GetInterface("IImage") != null)
                        {  //if this operation has return value

                           IImage tmp = null;
                           if (isCloned == true) //if intermediate image exist, keep a reference of it
                              tmp = imageToBeDisplayed;

                           isCloned = true;
                           imageToBeDisplayed = operation.InvokeMethod(imageToBeDisplayed) as IImage;

                           if (tmp != null) //dispose the intermediate image
                              tmp.Dispose();
                        }
                        else
                        {
                           throw new System.NotImplementedException(string.Format("Return type of {0} is not implemented.", operation.Method.ReturnType));
                        }
                     }
                  }
                  #endregion

                  DisplayedImage = imageToBeDisplayed;
               }

               operationsToolStripMenuItem.Enabled = (_image != null);
            }
         }
      }

      /// <summary>
      /// The image that is being displayed. It's the Image following by some user defined image operation
      /// </summary>
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
               if (_functionalMode != FunctionalModeOption.Minimum)
                  BuildOperationMenuItem(_displayedImage);

               //release the old Bitmap Image
               if (base.Image != null)
                  base.Image.Dispose();

               base.Image = _displayedImage.Bitmap;

               if (EnablePropertyPanel)
               {
                  ImagePropertyPanel.ImageSize = _displayedImage.Size;

                  ImagePropertyPanel.TypeOfColor = Reflection.ReflectIImage.GetTypeOfColor(_displayedImage);
                  ImagePropertyPanel.TypeOfDepth = Reflection.ReflectIImage.GetTypeOfDepth(_displayedImage);

                  #region calculate the frame rate
                  TimeSpan ts = DateTime.Now.Subtract(_timerStartTime);
                  if (ts.TotalSeconds > 1)
                  {
                     ImagePropertyPanel.FramesPerSecondText = _imageReceivedSinceCounterStart;
                     //reset the counter
                     _timerStartTime = DateTime.Now;
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
      #endregion

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
         catch
         {  //if pushing the operation generate exceptions

            _operationStack.Pop(); //remove the operation from the stack
            if (panel != null)
               panel.SetOperationStack(_operationStack); //update the operation stack

            Image = Image; //update the image
            throw; //rethrow the exception
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

      private ToolStripMenuItem[] BuildOperationTree(IEnumerable<KeyValuePair<string, MethodInfo>> catelogMiPairList)
      {
         List<ToolStripMenuItem> res = new List<ToolStripMenuItem>();

         SortedDictionary<String, List<KeyValuePair<String, MethodInfo>>> catelogDic = new SortedDictionary<string, List<KeyValuePair<String, MethodInfo>>>();
         SortedDictionary<String, MethodInfo> operationItem = new SortedDictionary<string, MethodInfo>();

         foreach (KeyValuePair<string, MethodInfo> pair in catelogMiPairList)
         {
            if (String.IsNullOrEmpty(pair.Key))
            {  //this is an operation
               operationItem.Add(pair.Value.ToString(), pair.Value);
            }
            else
            {  //this is a category
               int index = pair.Key.IndexOf('|');
               String category;
               String subcategory;
               if (index == -1)
               {  //There is no sub category
                  category = pair.Key;
                  subcategory = string.Empty;
               }
               else
               {  //There are sub categories
                  category = pair.Key.Substring(0, index);
                  subcategory = pair.Key.Substring(index + 1, pair.Key.Length - index - 1);
               }
               
               if (!catelogDic.ContainsKey(category))
                  catelogDic.Add(category, new List<KeyValuePair<String, MethodInfo>>());
               catelogDic[category].Add(new KeyValuePair<String, MethodInfo>(subcategory, pair.Value));
            }
         }

         #region Add catelogs to the menu
         foreach (String catelog in catelogDic.Keys)
         {
            ToolStripMenuItem catelogMenuItem = new ToolStripMenuItem();
            catelogMenuItem.Text = catelog;
            catelogMenuItem.DropDownItems.AddRange(BuildOperationTree(catelogDic[catelog]));
            res.Add(catelogMenuItem);
         }
         #endregion

         #region Add Methods to the menu
         foreach (MethodInfo mi in operationItem.Values)
         {
            ToolStripMenuItem operationMenuItem = new ToolStripMenuItem();
            operationMenuItem.Size = new System.Drawing.Size(152, 22);

            Type[] genericArgs = mi.GetGenericArguments();

            String genericArgString = genericArgs.Length > 0 ?
               String.Format(
                  "<{0}>",
                  String.Join(",", Array.ConvertAll<Type, String>(
                     genericArgs,
                     System.Convert.ToString))) 
               : string.Empty;           

            operationMenuItem.Text = String.Format(
               "{0}{1}({2})",
               mi.Name,
               genericArgString,
               String.Join(",", System.Array.ConvertAll<ParameterInfo, String>(mi.GetParameters(), delegate(ParameterInfo pi) { return pi.Name; })));

            //This is necessary to handle delegate with a loop
            //Cause me lots of headache before reading the article on
            //http://decav.com/blogs/andre/archive/2007/11/18/wtf-quot-problems-quot-with-anonymous-delegates-linq-lambdas-and-quot-foreach-quot-or-quot-for-quot-loops.aspx
            //I wishes MSFT handle this better
            MethodInfo methodInfoRef = mi;

            operationMenuItem.Click += delegate(Object o, EventArgs e)
                {
                   Object[] paramList = null;

                   while (true)
                   {
                      //Get the parameters for the method
                      //this pop up an input dialog and ask for user input
                      paramList = ParameterInputDialog.GetParams(methodInfoRef, paramList);

                      if (paramList == null) break; //user click cancel on the input dialog

                      //create an operation from the specific methodInfo and parameterlist
                      Operation operation = new Operation(methodInfoRef, paramList);
                      try
                      {
                         PushOperation(operation);
                         break;
                      }
                      catch (Exception expt)
                      {
                         MessageBox.Show(
                            expt.InnerException == null ?
                              expt.Message
                              : expt.InnerException.Message);

                         //special case, then there is no parameter and the method throw an exception
                         //break the loop
                         if (methodInfoRef.GetParameters().Length == 0)
                            break;
                      }
                   }
                };
            res.Add(operationMenuItem);
         }
         #endregion

         return res.ToArray();
      }

      private void BuildOperationMenuItem(IImage image)
      {
         Type typeOfImage = image.GetType();

         operationsToolStripMenuItem.DropDownItems.Clear();

         //check if the menu for the specific image type has been built before
         if (!_typeToToolStripMenuItemsDictionary.ContainsKey(typeOfImage))
         {
            //if not built, build it and save to the cache.
            _typeToToolStripMenuItemsDictionary.Add(
               typeOfImage,
               BuildOperationTree(Reflection.ReflectIImage.GetImageMethods(image))
               );
         }

         operationsToolStripMenuItem.DropDownItems.AddRange(_typeToToolStripMenuItemsDictionary[typeOfImage]);
      }

      private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (loadImageFromFileDialog.ShowDialog() == DialogResult.OK)
            try
            {
               String filename = loadImageFromFileDialog.FileName;
               Image = new Image<Bgr, byte>(filename);
            }
            catch (Exception excpt)
            {
               MessageBox.Show(excpt.Message);
            }
      }

      private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (saveImageToFileDialog.ShowDialog() == DialogResult.OK)
            try
            {
               DisplayedImage.Save(saveImageToFileDialog.FileName);
            }
            catch (Exception excpt)
            {
               MessageBox.Show(excpt.Message);
            }
      }

      private void propertyToolStripMenuItem_Click(object sender, EventArgs e)
      {
         EnablePropertyPanel = !EnablePropertyPanel;
      }

      private ImageProperty ImagePropertyPanel
      {
         get
         {
            return EnablePropertyPanel ? _propertyDlg.ImagePropertyPanel : null;
         }
      }

      /// <summary>
      /// Used for tracking the mouse position on the image
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void onMouseMove(object sender, MouseEventArgs e)
      {
         if (EnablePropertyPanel)
         {
            IImage img = DisplayedImage;

            IColor color = img == null ?
               null :
               Reflection.ReflectIImage.GetPixelColor(img, e.Location);

            ImagePropertyPanel.MousePositionOnImage = e.Location;
            ImagePropertyPanel.ColorIntensity = color;
         }
      }

      /// <summary>
      /// The display mode for ImageBox
      /// </summary>
      public enum FunctionalModeOption
      {
         /// <summary>
         /// The ImageBox is only used for displaying image. 
         /// No right-click menu available
         /// </summary>
         Minimum = 0,
         /// <summary>
         /// This is the ImageBox with all functions enabled.
         /// </summary>
         Everything = 1
      }

      /// <summary>
      /// Release the this Imagebox and all memory associate with it.
      /// </summary>
      public virtual new void Dispose()
      {
         if (this.Image != null)
            this.Image.Dispose();
         base.Dispose();
      }
   }
}
