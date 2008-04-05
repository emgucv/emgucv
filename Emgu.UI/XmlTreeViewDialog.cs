using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Emgu.UI
{
    public partial class XmlTreeViewDialog : Form
    {
        public XmlTreeViewDialog()
        {
            InitializeComponent();
        }

        public XmlTreeViewDialog(XmlDocument doc)
            :this()
        {
            xmlTreeView1.XmlDocument = doc;
        }
    }
}