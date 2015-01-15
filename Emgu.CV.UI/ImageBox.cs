//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV.UI
{
   /// <summary>
   /// An image box is a user control that is similar to picture box, but display Emgu CV IImage and provides enhenced functionalities.
   /// </summary>
   public partial class ImageBox : PanAndZoomPictureBox
   {
      #region Private Fileds
      /// <summary>
      /// The image that is setted throught the Image property
      /// </summary>
      private IImage _image;
      /// <summary>
      /// The image that is displayed
      /// </summary>
      private IImage _displayedImage;
      private ImagePropertyDialog _propertyDlg;

      private FunctionalModeOption _functionalMode = FunctionalModeOption.Everything;

      /// <summary>
      /// A cache to store the ToolStripMenuItems for different types of images
      /// </summary>
      private readonly Dictionary<Type, ToolStripMenuItem[]> _typeToToolStripMenuItemsDictionary = new Dictionary<Type, ToolStripMenuItem[]>();

      /// <summary>
      /// The list of operations binded to this ImageBox
      /// </summary>
      private List<Operation> _operationLists;

      /// <summary>
      /// Timer used for caculating the frame rate
      /// </summary>
      private DateTime _timerStartTime;

      /// <summary>
      /// One of the parameters used for caculating the frame rate
      /// </summary>
      private int _imagesReceivedSinceCounterStart;

      #endregion

      /// <summary>
      /// Create a ImageBox
      /// </summary>
      public ImageBox()
      {
         InitializeComponent();

         _operationLists = new List<Operation>();

         OnZoomScaleChange += delegate
         {
            if (_propertyDlg != null)
               _propertyDlg.ImagePropertyControl.UpdateZoomScale();
         };
         
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
            if (_functionalMode == value) return;

            PanableAndZoomable = ((int)value & (int)FunctionalModeOption.PanAndZoom) != 0;

            //right click menu enabled
            bool rightClickMenuEnabled = ((int)value & (int)FunctionalModeOption.RightClickMenu) != 0;
            foreach (ToolStripMenuItem mi in rightClickContextMenuStrip.Items)
            {
               if (mi == zoomToolStripMenuItem)
               {
                  mi.Visible = PanableAndZoomable && rightClickMenuEnabled;
               }
               else
               {
                  mi.Visible = rightClickMenuEnabled;
               }
            }

            _functionalMode = value;

            if (OnFunctionalModeChanged != null)
               OnFunctionalModeChanged(this, new EventArgs());
         }
      }

      /// <summary>
      /// The event which will be trigerred when functional mode is changed
      /// </summary>
      public event EventHandler OnFunctionalModeChanged;

      private void ImageBox_MouseMove(object sender, MouseEventArgs e)
      {
         int offsetX = (int)(e.Location.X / ZoomScale);
         int offsetY = (int)(e.Location.Y / ZoomScale);

         if (EnablePropertyPanel)
         {
            int horizontalScrollBarValue = HorizontalScrollBar.Visible ? (int)HorizontalScrollBar.Value : 0;
            int verticalScrollBarValue = VerticalScrollBar.Visible ? (int)VerticalScrollBar.Value : 0;

            ImagePropertyPanel.SetMousePositionOnImage(new Point(
               offsetX + horizontalScrollBarValue,
               offsetY + verticalScrollBarValue));
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
                  _propertyDlg = new ImagePropertyDialog(this);

               _propertyDlg.Show();

               _propertyDlg.FormClosed +=
                   delegate
                   {
                      _propertyDlg = null;
                   };

               ImagePropertyPanel.SetOperations(_operationLists);

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
      [Browsable(false)]
      public new IImage Image
      {
         get
         {
            return _image;
         }
         set
         {
            if (Disposing || IsDisposed)
               return;
            else if (InvokeRequired)
            {
               Invoke(new MethodInvoker(delegate { Image = value; }));
            }
            else
            {
               bool emptyOldImage = (_image == null);
               bool emptyNewImage = (value == null);

               IImage imageToBeDisplayed = _image = value;

               if (!emptyNewImage)
               {
                  if (emptyOldImage)
                  {
                     filtersToolStripMenuItem.Enabled = true;
                     zoomToolStripMenuItem.Enabled = true;
                     saveImageToolStripMenuItem.Enabled = true;
                     propertyToolStripMenuItem.Enabled = true;
                  }

                  #region perform operations on _operationList if there is any
                  if (_operationLists.Count > 0)
                  {
                     bool isCloned = false;

                     foreach (Operation operation in _operationLists)
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
                           throw new NotImplementedException(string.Format("Return type of {0} is not implemented.", operation.Method.ReturnType));
                        }
                     }
                  }
                  #endregion

                  if (_propertyDlg != null)
                  {
                     _propertyDlg.ImagePropertyControl.SetImage(imageToBeDisplayed);
                     Point pos = PointToClient(Cursor.Position);
                     this.ImageBox_MouseMove(this, new MouseEventArgs(System.Windows.Forms.MouseButtons.None, 0, pos.X, pos.Y, 0));
                  }
               }
               else //empty new image
               {
                  filtersToolStripMenuItem.Enabled = false;
                  zoomToolStripMenuItem.Enabled = false;
                  saveImageToolStripMenuItem.Enabled = false;
                  propertyToolStripMenuItem.Enabled = false;
               }

               DisplayedImage = imageToBeDisplayed;
            }
         }
      }

      /// <summary>
      /// The image that is being displayed. It's the Image following by some user defined image operation
      /// </summary>
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      [Browsable(false)]
      public IImage DisplayedImage
      {
         get
         {
            return _displayedImage;
         }
         set
         {
            //release the old Bitmap Image if there is any              
            if (base.Image != null)
               base.Image.Dispose();

            _displayedImage = value;
            if (_displayedImage != null)
            {
               if (_functionalMode != FunctionalModeOption.Minimum)
                  BuildOperationMenuItem(_displayedImage);

               base.Image = _displayedImage.Bitmap;

               if (EnablePropertyPanel)
               {
                  ImagePropertyPanel.SetImage(_displayedImage);

                  #region calculate the frame rate
                  TimeSpan ts = DateTime.Now.Subtract(_timerStartTime);
                  if (ts.TotalSeconds > 1)
                  {
                     ImagePropertyPanel.FramesPerSecondText = _imagesReceivedSinceCounterStart;
                     //reset the counter
                     _timerStartTime = DateTime.Now;
                     _imagesReceivedSinceCounterStart = 0;
                  }
                  else
                  {
                     _imagesReceivedSinceCounterStart++;
                  }
                  #endregion
               }
            }
            else
            {
               base.Image = null;
            }
         }
      }
      #endregion

      /// <summary>
      /// Push the specific operation to the operation collection
      /// </summary>
      /// <param name="operation">The operation to be pushed onto the operation collection</param>
      public void PushOperation(Operation operation)
      {
         _operationLists.Add(operation);
         ImageProperty panel = ImagePropertyPanel;
         if (panel != null)
            panel.SetOperations(_operationLists);

         try
         {
            Image = Image;
         }
         catch
         {  //if pushing the operation generate exceptions
            PopOperation(); //remove the operation 
            throw; //rethrow the exception
         }
      }

      /// <summary>
      /// Remove all the operations from the collection
      /// </summary>
      public void ClearOperation()
      {
         _operationLists.Clear();
         ImageProperty panel = ImagePropertyPanel;
         if (panel != null) panel.SetOperations(_operationLists);
         Image = Image;
      }

      /// <summary>
      /// Remove the last added operation
      /// </summary>
      public void PopOperation()
      {
         if (_operationLists.Count > 0)
         {
            _operationLists.RemoveAt(_operationLists.Count - 1);
            ImageProperty panel = ImagePropertyPanel;
            if (panel != null) panel.SetOperations(_operationLists);
            Image = Image;
         }
      }

      private ToolStripMenuItem[] BuildOperationTree(IEnumerable<KeyValuePair<string, MethodInfo>> catalogMiPairList)
      {
         List<ToolStripMenuItem> res = new List<ToolStripMenuItem>();
         SortedDictionary<String, List<KeyValuePair<String, MethodInfo>>> catalogDic = new SortedDictionary<string, List<KeyValuePair<String, MethodInfo>>>();
         SortedDictionary<String, MethodInfo> operationItem = new SortedDictionary<string, MethodInfo>();

         foreach (KeyValuePair<string, MethodInfo> pair in catalogMiPairList)
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

               if (!catalogDic.ContainsKey(category))
                  catalogDic.Add(category, new List<KeyValuePair<String, MethodInfo>>());
               catalogDic[category].Add(new KeyValuePair<String, MethodInfo>(subcategory, pair.Value));
            }
         }

         #region Add catalogs to the menu
         foreach (String catalog in catalogDic.Keys)
         {
            ToolStripMenuItem catalogMenuItem = new ToolStripMenuItem();
            catalogMenuItem.Text = catalog;
            catalogMenuItem.DropDownItems.AddRange(BuildOperationTree(catalogDic[catalog]));
            res.Add(catalogMenuItem);
         }
         #endregion

         #region Add Methods to the menu
         foreach (MethodInfo mi in operationItem.Values)
         {
            ToolStripMenuItem operationMenuItem = new ToolStripMenuItem();
            operationMenuItem.Size = new Size(152, 22);

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
               mi.Name.Substring(0, 1).Equals("_") ? mi.Name.Substring(1, mi.Name.Length -1) : mi.Name, //remove leading underscore
               genericArgString,
               String.Join(",", System.Array.ConvertAll<ParameterInfo, String>(mi.GetParameters(), delegate(ParameterInfo pi) { return pi.Name; })));

            //This is necessary to handle delegate with a loop
            //Cause me lots of headache before reading the article on
            //http://decav.com/blogs/andre/archive/2007/11/18/wtf-quot-problems-quot-with-anonymous-delegates-linq-lambdas-and-quot-foreach-quot-or-quot-for-quot-loops.aspx
            //I wishes MSFT handle this better
            MethodInfo methodInfoRef = mi;

            operationMenuItem.Click += delegate
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
                         MessageBox.Show((expt.InnerException ?? expt).Message);

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

         filtersToolStripMenuItem.DropDownItems.Clear();

         //check if the menu for the specific image type has been built before
         if (!_typeToToolStripMenuItemsDictionary.ContainsKey(typeOfImage))
         {
            //if not built, build it and save to the cache.
            _typeToToolStripMenuItemsDictionary.Add(
               typeOfImage,
               BuildOperationTree(Reflection.ReflectIImage.GetImageMethods(image))
               );
         }

         filtersToolStripMenuItem.DropDownItems.AddRange(_typeToToolStripMenuItemsDictionary[typeOfImage]);
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

      private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (saveImageToFileDialog.ShowDialog() == DialogResult.OK)
            try
            {
               if (this.InvokeRequired)
               {
                  this.Invoke((Action)
                     delegate()
                     {
                        DisplayedImage.Save(saveImageToFileDialog.FileName);
                     });
               }
               else
               {
                  DisplayedImage.Save(saveImageToFileDialog.FileName);
               }
            }
            catch (Exception excpt)
            {
               MessageBox.Show(excpt.Message);
            }
      }

      private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (Image != null)
         {
            SetZoomScale(ZoomScale * 2.0, new Point());
         }
      }

      private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (Image != null)
         {
            SetZoomScale(ZoomScale / 2.0, new Point());
         }
      }

      private void unZoomToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (Image != null)
         {
            SetZoomScale(1.0, new Point());
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
            return EnablePropertyPanel ? _propertyDlg.ImagePropertyControl : null;
         }
      }

      /// <summary>
      /// The functional mode for ImageBox
      /// </summary>
      [Flags]
      public enum FunctionalModeOption
      {
         /// <summary>
         /// The ImageBox is only used for displaying image. 
         /// No right-click menu nor Pan/Zoom available
         /// </summary>
         Minimum = 0,
         /// <summary>
         /// Enable the right click menu
         /// </summary>
         RightClickMenu = 1,
         /// <summary>
         /// Enable Pan and Zoom
         /// </summary>
         PanAndZoom = 2,
         /// <summary>
         /// Support for the right click menu, Pan and Zoom
         /// </summary>
         Everything = RightClickMenu | PanAndZoom,
      }

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            //if (Image != null) Image.Dispose();
            if (_propertyDlg != null) _propertyDlg.Dispose();
            components.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
