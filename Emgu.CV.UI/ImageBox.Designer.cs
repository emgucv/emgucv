namespace Emgu.CV.UI
{
    partial class ImageBox
    {
       
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
       


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
           this.components = new System.ComponentModel.Container();
           this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
           this.fileOperationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
           this.loadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
           this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
           this.operationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
           this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
           this.loadImageFromFileDialog = new System.Windows.Forms.OpenFileDialog();
           this.saveImageToFileDialog = new System.Windows.Forms.SaveFileDialog();
           this.contextMenuStrip1.SuspendLayout();
           ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
           this.SuspendLayout();
           // 
           // contextMenuStrip1
           // 
           this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileOperationToolStripMenuItem,
            this.operationsToolStripMenuItem,
            this.propertyToolStripMenuItem});
           this.contextMenuStrip1.Name = "contextMenuStrip1";
           this.contextMenuStrip1.Size = new System.Drawing.Size(128, 70);
           // 
           // fileOperationToolStripMenuItem
           // 
           this.fileOperationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadImageToolStripMenuItem,
            this.saveAsToolStripMenuItem});
           this.fileOperationToolStripMenuItem.Name = "fileOperationToolStripMenuItem";
           this.fileOperationToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
           this.fileOperationToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.File;
           // 
           // loadImageToolStripMenuItem
           // 
           this.loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
           this.loadImageToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
           this.loadImageToolStripMenuItem.Text = "Load Image";
           this.loadImageToolStripMenuItem.Click += new System.EventHandler(this.loadImageToolStripMenuItem_Click);
           // 
           // saveAsToolStripMenuItem
           // 
           this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
           this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
           this.saveAsToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.SaveAs;
           this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
           // 
           // operationsToolStripMenuItem
           // 
           this.operationsToolStripMenuItem.Name = "operationsToolStripMenuItem";
           this.operationsToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
           this.operationsToolStripMenuItem.Text = "Operations";
           // 
           // propertyToolStripMenuItem
           // 
           this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
           this.propertyToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
           this.propertyToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.Property;
           this.propertyToolStripMenuItem.Click += new System.EventHandler(this.propertyToolStripMenuItem_Click);
           // 
           // loadImageFromFileDialog
           // 
           this.loadImageFromFileDialog.Filter = "Image Files (*.jpg; *.bmp;*.png)|*.jpg;*.bmp;*.png|All Files(*.*)|*.*";
           this.loadImageFromFileDialog.Title = "Open Image File";
           // 
           // saveImageToFileDialog
           // 
           this.saveImageToFileDialog.Title = global::Emgu.CV.UI.Properties.StringTable.SaveImageDialogText;
           // 
           // ImageBox
           // 
           this.ContextMenuStrip = this.contextMenuStrip1;
           this.Size = new System.Drawing.Size(0, 0);
           this.TabIndex = 2;
           this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseMove);
           this.contextMenuStrip1.ResumeLayout(false);
           ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
           this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileOperationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadImageToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog loadImageFromFileDialog;
        private System.Windows.Forms.SaveFileDialog saveImageToFileDialog;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertyToolStripMenuItem;

    }
}
