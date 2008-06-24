using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Emgu.UI
{
    public partial class XmlTreeView : UserControl
    {
        private XmlDocument _doc;

        public XmlTreeView()
        {
            InitializeComponent();
        }

        public XmlTreeView(XmlDocument doc)
            : this()
        {
            XmlDocument = doc;
        }

        public XmlDocument XmlDocument
        {
            set
            {
                _doc = value;
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(value.DocumentElement.Name);
                AddNode(value.DocumentElement, treeView1.Nodes[0]);
                treeView1.ExpandAll();
            }
            get
            {
                return _doc;
            }
        }

        private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList nodeList;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new TreeNode( NodeToString(xNode)));
                    tNode = inTreeNode.Nodes[i];
                    AddNode(xNode, tNode);
                }
            }
            else
            {
                // Here you need to pull the data from the XmlNode based on the
                // type of node, whether attribute values are required, and so forth.
                inTreeNode.Text = NodeToString(inXmlNode);
            }
        }

        private static String NodeToString(XmlNode node)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("{0} : {1} ", (node.Name).Trim(), node.Value));
            
            if (node.Attributes != null)
            {
                foreach (XmlAttribute att in node.Attributes)
                {
                    sb.Append(String.Format(" [ {0}: {1} ]", att.Name, att.Value));
                }
            }
            return sb.ToString();
        }
    }
}
