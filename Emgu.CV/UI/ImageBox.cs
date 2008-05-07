using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Emgu.CV
{
    public partial class ImageBox : UserControl
    {
        private IImage _image;
        
        public ImageBox()
        {
            InitializeComponent();
            AddOperationMenuItem();
        }

        public void AddOperationMenuItem()
        {
            foreach (MemberInfo mi in GetImageMethods())
            {
                ToolStripMenuItem _methodToolStripMenuItem = new ToolStripMenuItem();
                _methodToolStripMenuItem.Name = mi.Name;
                _methodToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                _methodToolStripMenuItem.Text = mi.Name;
                //_methodToolStripMenuItem.Click += new System.EventHandler(this.loadImageToolStripMenuItem_Click);

                operationsToolStripMenuItem.DropDownItems.Add(_methodToolStripMenuItem);
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
                if (_image != null)
                {
                    
                    pictureBox.Width = _image.Width;
                    pictureBox.Height = _image.Height;
                    pictureBox.Image = _image.AsBitmap();
                }

                operationsToolStripMenuItem.Enabled = (_image != null);
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

        public static IEnumerable<MemberInfo> GetImageMethods()
        {
            foreach (MemberInfo mi in typeof(IImage).GetMembers())
            {
                if (mi.MemberType == MemberTypes.Method)
                {
                    Object[] atts = mi.GetCustomAttributes(typeof(ExposableMethodAttribute), false);
                    if (atts.Length == 0 || ((ExposableMethodAttribute)atts[0]).Exposable)
                        yield return mi;
                }
            }
        }
    }
}
