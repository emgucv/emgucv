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

namespace Emgu.CV.UI
{
    /// <summary>
    /// An image box is a user control that is similar to picture box, but display Emgu CV IImage and provides enhenced functionalities.
    /// </summary>
    public partial class ImageBox : UserControl
    {
        private IImage _image;
        private IImage _displayedImage;

        private Stack<ImageOperation> _operationStack;

        /// <summary>
        /// Create a ImageBox
        /// </summary>
        public ImageBox()
        {
            InitializeComponent();
            //panel2.Height = 0;
            splitContainer1.Panel2Collapsed = true;

            _operationStack = new Stack<ImageOperation>();
            
            AddOperationMenuItem();
        }

        private void PushOperation(ImageOperation operation)
        {
            _operationStack.Push(operation);
            imageProperty1.OperationStackText = OperationStackToString();
            Image = Image;
        }

        private void ClearOperation()
        {
            _operationStack.Clear();
            imageProperty1.OperationStackText = OperationStackToString();
            Image = Image;
        }

        private String OperationStackToString()
        {
            List<String> opStr = new List<string>();
            foreach (ImageOperation op in _operationStack)
            {
                opStr.Add(op.ToString());
            }
            String[] sArray = opStr.ToArray();
            System.Array.Reverse(sArray);
            return String.Join("->", sArray);
        }

        private void AddOperationMenuItem()
        {
            foreach (MethodInfo mi in GetImageMethods())
            {
                ToolStripMenuItem operationMenuItem = new ToolStripMenuItem();
                operationMenuItem.Size = new System.Drawing.Size(152, 22);
                operationMenuItem.Text = String.Format("{0}({1})", mi.Name, 
                    String.Join(",", System.Array.ConvertAll<ParameterInfo, String>( mi.GetParameters(), delegate(ParameterInfo pi) { return pi.Name; } )));

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
                            ImageOperation operation = new ImageOperation(miRef, paramList.ToArray());
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
                _image = value;
                IImage displayedImage = _image;

                if (displayedImage != null)
                {
                    ImageOperation[] ops = _operationStack.ToArray();
                    System.Array.Reverse(ops);
                    foreach (ImageOperation operation in ops)
                        displayedImage = operation.ProcessImage(displayedImage);

                    DisplayedImage = displayedImage;
                }

                operationsToolStripMenuItem.Enabled = (_image != null);
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

                pictureBox.Image = _displayedImage.AsBitmap();

                if (EnableProperty)
                {
                    imageProperty1.ImageWidth = _displayedImage.Width;
                    imageProperty1.ImageHeight = _displayedImage.Height;

                    #region display the color type
                    System.Type colorType = _displayedImage.TypeOfColor;
                    Object[] colorAttributes = colorType.GetCustomAttributes(typeof(ColorInfo), true);
                    if (colorAttributes.Length > 0)
                    {
                        ColorInfo info = (ColorInfo) colorAttributes[0];
                        imageProperty1.ColorType = info.ConversionCodeName;
                    }
                    #endregion 

                    imageProperty1.ColorDepth = _displayedImage.TypeOfDepth;
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

        private class ImageOperation
        {
            private MemberInfo _mi;

            public MemberInfo Method
            {
                get { return _mi; }
                set { _mi = value; }
            }

            private Object[] _parameters;

            public Object[] Parameters
            {
                get { return _parameters; }
                set { _parameters = value; }
            }
	
            public ImageOperation(MemberInfo mi, Object[] parameters)
            {
                _mi = mi;
                _parameters = parameters;
            }

            public IImage ProcessImage(IImage src)
            {
                Type IImageType = typeof(IImage);
                return IImageType.InvokeMember(_mi.Name, BindingFlags.InvokeMethod, null, src, _parameters) as IImage;
            }

            public override string ToString()
            {
                return String.Format("{0}({1})",
                    Method.Name,
                    String.Join(", ", System.Array.ConvertAll<Object, String>( Parameters, System.Convert.ToString) ));
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

        private bool EnableProperty
        {
            get
            {
                return !splitContainer1.Panel2Collapsed;
            }
            set
            {
                splitContainer1.Panel2Collapsed = !value;
                if (EnableProperty)
                {
                    // reset the image such that the property is updated
                    Image = Image; 
                }
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (EnableProperty)
            {
                Point2D<int> location = new Point2D<int>();

                ColorType color = null;

                if (DisplayedImage != null)
                {
                    location.X = Math.Min(e.X, DisplayedImage.Width-1);
                    location.Y = Math.Min(e.Y, DisplayedImage.Height-1);
                    
                    color = DisplayedImage.GetColor(location);
                    
                }
                
                imageProperty1.MousePositionOnImage = location;
                imageProperty1.ColorIntensity = color;
            }
        }
    }
}
