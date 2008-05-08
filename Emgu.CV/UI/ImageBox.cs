using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace Emgu.CV
{
    public partial class ImageBox : UserControl
    {
        private IImage _image;
        private IImage _displayedImage;

        private Stack<ImageOperation> _operationStack;

        public ImageBox()
        {
            InitializeComponent();
            //panel2.Height = 0;
            splitContainer1.Panel2Collapsed = true;

            _operationStack = new Stack<ImageOperation>();
            AddOperationMenuItem();
        }

        public void AddOperationMenuItem()
        {
            foreach (MethodInfo mi in GetImageMethods())
            {
                ToolStripMenuItem operationMenuItem = new ToolStripMenuItem();
                operationMenuItem.Name = mi.Name;
                operationMenuItem.Size = new System.Drawing.Size(152, 22);
                operationMenuItem.Text = mi.Name;
                
                ImageOperation operation = new ImageOperation(mi);
                EventHandler handler = delegate(Object o, EventArgs e)
                    {
                        _operationStack.Push(operation);
                        Image = Image;
                    };
                operationMenuItem.Click += handler;

                operationsToolStripMenuItem.DropDownItems.Add(operationMenuItem);
            }
        }

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
                    foreach (ImageOperation operation in _operationStack.ToArray())
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
                if (pictureBox.Width != value.Width) pictureBox.Width = value.Width;
                if (pictureBox.Height != value.Height) pictureBox.Height = value.Height;
                pictureBox.Image = value.AsBitmap();

                if (EnableProperty)
                {
                    imageProperty1.Width = value.Width;
                    imageProperty1.Height = value.Height;

                    ColorType color = value.Color;

                    Object[] colorAttributes = color.GetType().GetCustomAttributes(typeof(ColorInfo), true);
                    if (colorAttributes.Length > 0)
                    {
                        ColorInfo info = (ColorInfo) colorAttributes[0];
                        imageProperty1.ColorType = info.ConversionCodeName;
                    }
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

        public static IEnumerable<MethodInfo> GetImageMethods()
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
	
            public ImageOperation(MemberInfo mi, params Object[] parameters)
            {
                _mi = mi;
                _parameters = parameters;
            }

            public IImage ProcessImage(IImage src)
            {
                Type IImageType = typeof(IImage);
                return IImageType.InvokeMember(_mi.Name, BindingFlags.InvokeMethod, null, src, _parameters) as IImage;
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _operationStack.Clear();
            Image = Image;
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
                /*
                int x = 0, y = 0;
                if (DisplayedImage != null)
                {
                    x = Math.Min(e.X, DisplayedImage.Width);
                    y = Math.Min(e.Y, DisplayedImage.Height);
                }
                */
                imageProperty1.MousePosition = new Point2D<int>(e.X, e.Y);
            }
        }
    }
}
