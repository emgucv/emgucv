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
using Emgu.Reflection;

namespace Emgu.CV.UI
{
    /// <summary>
    /// An image box is a user control that is similar to picture box, but display Emgu CV IImage and provides enhenced functionalities.
    /// </summary>
    public partial class ImageBox : UserControl
    {
        private IImage _image;
        private IImage _displayedImage;
        private PropertyDlg _propertyDlg;

        private Stack<Operation<IImage>> _operationStack;

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
        {
            InitializeComponent();

            _operationStack = new Stack<Operation<IImage>>();

            AddOperationMenuItem();
        }

        /// <summary>
        /// Push the specific operation onto the stack
        /// </summary>
        /// <param name="operation">The operation to be pushed onto the stack</param>
        private void PushOperation(Operation<IImage> operation)
        {
            _operationStack.Push(operation);
            ImageProperty panel = ImagePropertyPanel;
            if (panel != null) panel.OperationStackText = OperationStackToString();
            Image = Image;
        }

        /// <summary>
        /// Remove all the operations from the stack
        /// </summary>
        private void ClearOperation()
        {
            _operationStack.Clear();
            ImageProperty panel = ImagePropertyPanel;
            if (panel != null) panel.OperationStackText = OperationStackToString();
            Image = Image;
        }

        private String OperationStackToString()
        {
            String CSharpFunction = "public static IImage Function(IImage image){0}{{{0}\t{1}{0}\treturn image;{0}}}";
            List<String> opStr = new List<string>();
            foreach (Operation<IImage> op in _operationStack)
            {
                if (op.Method.ReturnType == typeof(void))
                {
                    opStr.Add(op.ToCode("image") + ";");
                }
                else
                {
                    opStr.Add("image = " + op.ToCode("image") + ";");
                }
            }
            String[] sArray = opStr.ToArray();
            System.Array.Reverse(sArray);
            return String.Format(CSharpFunction, Environment.NewLine, String.Join(Environment.NewLine + "\t", sArray));
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
                MethodInfo miRef = mi;

                operationMenuItem.Click += delegate(Object o, EventArgs e)
                    {
                        List<Object> paramList = new List<object>();
                        if (ParamInputDlg.GetParams(miRef, paramList))
                        {
                            Operation<IImage> operation = new Operation<IImage>(miRef, paramList.ToArray());
                            PushOperation(operation);
                        }
                    };

                operationsToolStripMenuItem.DropDownItems.Add(operationMenuItem);
            }
        }

        /// <summary>
        /// Set the image for this image box
        /// </summary>
        public IImage Image
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
                                operation.ProcessMethod(imageToBeDisplayed);
                            }
                            else if (operation.Method.ReturnType == typeof(IImage))
                            {
                                imageToBeDisplayed = operation.ProcessMethod(imageToBeDisplayed) as IImage;
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

        private IImage DisplayedImage
        {
            get
            {
                return _displayedImage;
            }
            set
            {
                _displayedImage = value;
                if (pictureBox.Width != _displayedImage.Width) pictureBox.Width = _displayedImage.Width;
                if (pictureBox.Height != _displayedImage.Height) pictureBox.Height = _displayedImage.Height;

                pictureBox.Image = _displayedImage.Bitmap;

                if (EnableProperty)
                {
                    ImagePropertyPanel.ImageWidth = _displayedImage.Width;
                    ImagePropertyPanel.ImageHeight = _displayedImage.Height;

                    ImagePropertyPanel.TypeOfColor = _displayedImage.TypeOfColor;
                    ImagePropertyPanel.TypeOfDepth = _displayedImage.TypeOfDepth;

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

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                String filename = openFileDialog1.FileName;
                Image<Bgr, Byte> img = new Image<Bgr, byte>(filename);
                Image = img;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                String filename = saveFileDialog1.FileName;
                _image.Save(filename);
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

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearOperation();
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
                    if (_propertyDlg == null) _propertyDlg = new PropertyDlg();                    
                    _propertyDlg.Show();

                    _propertyDlg.FormClosed += 
                        delegate(object sender, FormClosedEventArgs e ) 
                        {   
                            _propertyDlg = null;
                        };

                    ImagePropertyPanel.OperationStackText = OperationStackToString();

                    // reset the image such that the property is updated
                    Image = Image;
                }
                else
                {
                    if (_propertyDlg != null)
                    {
                        _propertyDlg.Focus();
                        //_propertyDlg.Close();
                        //_propertyDlg = null;
                    }
                }
            }
        }

        /// <summary>
        /// Used for tracking the mouse position on the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (EnableProperty)
            {
                Point2D<int> location = new Point2D<int>();

                ColorType color = null;

                if (DisplayedImage != null)
                {
                    location.X = Math.Min(e.X, DisplayedImage.Width - 1);
                    location.Y = Math.Min(e.Y, DisplayedImage.Height - 1);

                    color = DisplayedImage.GetColor(location);
                }

                ImagePropertyPanel.MousePositionOnImage = location;
                ImagePropertyPanel.ColorIntensity = color;
            }
        }
    }
}
