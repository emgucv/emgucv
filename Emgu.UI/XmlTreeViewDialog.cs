//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using System.Xml;

namespace Emgu.UI
{
   /// <summary>
   /// Display an xml document in a tree view dialog
   /// </summary>
   public partial class XmlTreeViewDialog : Form
   {
      /// <summary>
      /// Create the tree view dialog
      /// </summary>
      public XmlTreeViewDialog()
      {
         InitializeComponent();
      }

      /// <summary>
      /// Create the tree view dialog to display the <paramref name="doc"/>
      /// </summary>
      /// <param name="doc">The document to be displayed</param>
      public XmlTreeViewDialog(XmlDocument doc)
         : this()
      {
         xmlTreeView1.XmlDocument = doc;
      }
   }
}