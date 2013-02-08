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
         this.rightClickContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.fileOperationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.loadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.saveImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.unZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.loadImageFromFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.saveImageToFileDialog = new System.Windows.Forms.SaveFileDialog();
         this.rightClickContextMenuStrip.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
         this.SuspendLayout();
         // 
         // rightClickContextMenuStrip
         // 
         this.rightClickContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileOperationToolStripMenuItem,
            this.filtersToolStripMenuItem,
            this.zoomToolStripMenuItem,
            this.propertyToolStripMenuItem});
         this.rightClickContextMenuStrip.Name = "contextMenuStrip1";
         this.rightClickContextMenuStrip.Size = new System.Drawing.Size(117, 92);
         // 
         // fileOperationToolStripMenuItem
         // 
         this.fileOperationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadImageToolStripMenuItem,
            this.saveImageToolStripMenuItem});
         this.fileOperationToolStripMenuItem.Name = "fileOperationToolStripMenuItem";
         this.fileOperationToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
         this.fileOperationToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.File;
         // 
         // loadImageToolStripMenuItem
         // 
         this.loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
         this.loadImageToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
         this.loadImageToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.LoadImage;
         this.loadImageToolStripMenuItem.Click += new System.EventHandler(this.loadImageToolStripMenuItem_Click);
         // 
         // saveImageToolStripMenuItem
         // 
         this.saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
         this.saveImageToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
         this.saveImageToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.SaveAs;
         this.saveImageToolStripMenuItem.Click += new System.EventHandler(this.saveImageToolStripMenuItem_Click);
         // 
         // filtersToolStripMenuItem
         // 
         this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
         this.filtersToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
         this.filtersToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.Filters;
         // 
         // zoomToolStripMenuItem
         // 
         this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem,
            this.unZoomToolStripMenuItem});
         this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
         this.zoomToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
         this.zoomToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.Zoom;
         // 
         // zoomInToolStripMenuItem
         // 
         this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
         this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
         this.zoomInToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.ZoomIn;
         this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
         // 
         // zoomOutToolStripMenuItem
         // 
         this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
         this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
         this.zoomOutToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.ZoomOut;
         this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
         // 
         // unZoomToolStripMenuItem
         // 
         this.unZoomToolStripMenuItem.Name = "unZoomToolStripMenuItem";
         this.unZoomToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
         this.unZoomToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.UnZoom;
         this.unZoomToolStripMenuItem.Click += new System.EventHandler(this.unZoomToolStripMenuItem_Click);
         // 
         // propertyToolStripMenuItem
         // 
         this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
         this.propertyToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
         this.propertyToolStripMenuItem.Text = global::Emgu.CV.UI.Properties.StringTable.Property;
         this.propertyToolStripMenuItem.Click += new System.EventHandler(this.propertyToolStripMenuItem_Click);
         // 
         // loadImageFromFileDialog
         // 
         this.loadImageFromFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.gif;*.bmp;*.png;*.tiff;*.tif)|*.jpg;*.jpeg;*.gif;*.bm" +
    "p;*.png;*.tiff;*.tif|All Files(*.*)|*.*";
         this.loadImageFromFileDialog.Title = global::Emgu.CV.UI.Properties.StringTable.LoadImage;
         // 
         // saveImageToFileDialog
         // 
         this.saveImageToFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.gif;*.bmp;*.png;*.tiff;*.tif)|*.jpg;*.jpeg;*.gif;*.bm" +
    "p;*.png;*.tiff;*.tif|All Files(*.*)|*.*";
         this.saveImageToFileDialog.Title = global::Emgu.CV.UI.Properties.StringTable.SaveImageDialogText;
         // 
         // ImageBox
         // 
         this.ContextMenuStrip = this.rightClickContextMenuStrip;
         this.Size = new System.Drawing.Size(0, 0);
         this.TabIndex = 2;
         this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseMove);
         this.rightClickContextMenuStrip.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ContextMenuStrip rightClickContextMenuStrip;
      private System.Windows.Forms.ToolStripMenuItem fileOperationToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem loadImageToolStripMenuItem;
      private System.Windows.Forms.OpenFileDialog loadImageFromFileDialog;
      private System.Windows.Forms.SaveFileDialog saveImageToFileDialog;
      private System.Windows.Forms.ToolStripMenuItem saveImageToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem propertyToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem unZoomToolStripMenuItem;

   }
}
