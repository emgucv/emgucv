//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Emgu.UI
{
   /// <summary>
   /// Display the xml document using an explorer style form
   /// </summary>
   public partial class XmlViewDialog : Form
   {
      private string tempFile;

      /// <summary>
      /// Create the dialog to display the specific xml document
      /// </summary>
      /// <param name="doc">the document to be displayed</param>
      public XmlViewDialog(XmlDocument doc)
      {
         InitializeComponent();
         tempFile = Path.Combine(Directory.GetCurrentDirectory(), "emguTemperaryFile.xml");
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