namespace Emgu.UI
{
    partial class XmlTreeViewDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xmlTreeView1 = new Emgu.UI.XmlTreeView();
            this.SuspendLayout();
            // 
            // xmlTreeView1
            // 
            this.xmlTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xmlTreeView1.Location = new System.Drawing.Point(0, 0);
            this.xmlTreeView1.Name = "xmlTreeView1";
            this.xmlTreeView1.Size = new System.Drawing.Size(292, 273);
            this.xmlTreeView1.TabIndex = 0;
            // 
            // XmlTreeViewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.xmlTreeView1);
            this.Name = "XmlTreeViewDialog";
            this.Text = "XmlTreeViewDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private XmlTreeView xmlTreeView1;
    }
}