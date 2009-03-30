namespace Emgu.CV.UI
{
    partial class ImageBox
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
           this.verticalScrollBar = new System.Windows.Forms.VScrollBar();
           this.horizontalScrollBar = new System.Windows.Forms.HScrollBar();
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
           // verticalScrollBar
           // 
           this.verticalScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
           this.verticalScrollBar.Location = new System.Drawing.Point(-17, 0);
           this.verticalScrollBar.Name = "verticalScrollBar";
           this.verticalScrollBar.Size = new System.Drawing.Size(17, -17);
           this.verticalScrollBar.TabIndex = 1;
           this.verticalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HandleScroll);
           // 
           // horizontalScrollBar
           // 
           this.horizontalScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
           this.horizontalScrollBar.Location = new System.Drawing.Point(0, -17);
           this.horizontalScrollBar.Name = "horizontalScrollBar";
           this.horizontalScrollBar.Size = new System.Drawing.Size(0, 17);
           this.horizontalScrollBar.TabIndex = 2;
           this.horizontalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HandleScroll);
           // 
           // ImageBox
           // 
           this.ContextMenuStrip = this.contextMenuStrip1;
           this.Controls.Add(this.verticalScrollBar);
           this.Controls.Add(this.horizontalScrollBar);
           this.Cursor = _defaultCursor;
           this.Size = new System.Drawing.Size(0, 0);
           this.TabIndex = 2;
           this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseWheel);
           this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseMove);
           this.Resize += new System.EventHandler(this.ImageBox_Resize);
           this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseDown);
           this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseUp);
           this.MouseEnter += new System.EventHandler(this.ImageBox_MouseEnter);
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
       private System.Windows.Forms.VScrollBar verticalScrollBar;
       private System.Windows.Forms.HScrollBar horizontalScrollBar;

    }
}
