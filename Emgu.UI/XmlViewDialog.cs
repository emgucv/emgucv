using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace Emgu.UI
{
    public partial class XmlViewDialog : Form
    {
        private string tempFile;

        public XmlViewDialog(XmlDocument doc)
        {
            InitializeComponent();
            tempFile = System.IO.Directory.GetCurrentDirectory() + "/emguTemperaryFile.xml";
            doc.Save(tempFile);
            webBrowser1.Navigate(tempFile);
            //webBrowser1.DocumentText = doc.OuterXml;
        }

        private void XmlViewDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete(tempFile); 
        }
    }
}