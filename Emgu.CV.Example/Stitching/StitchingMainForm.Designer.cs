namespace Stitching
{
   partial class StitchingMainForm
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
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.splitContainer2 = new System.Windows.Forms.SplitContainer();
         this.selectImagesButton = new System.Windows.Forms.Button();
         this.sourceImageDataGridView = new System.Windows.Forms.DataGridView();
         this.resultImageBox = new Emgu.CV.UI.ImageBox();
         this.ThumbnailColumn = new System.Windows.Forms.DataGridViewImageColumn();
         this.FileNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.label1 = new System.Windows.Forms.Label();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         this.splitContainer2.Panel1.SuspendLayout();
         this.splitContainer2.Panel2.SuspendLayout();
         this.splitContainer2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.sourceImageDataGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.resultImageBox)).BeginInit();
         this.SuspendLayout();
         // 
         // splitContainer1
         // 
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point(0, 0);
         this.splitContainer1.Name = "splitContainer1";
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.resultImageBox);
         this.splitContainer1.Size = new System.Drawing.Size(1127, 664);
         this.splitContainer1.SplitterDistance = 374;
         this.splitContainer1.TabIndex = 0;
         // 
         // splitContainer2
         // 
         this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer2.Location = new System.Drawing.Point(0, 0);
         this.splitContainer2.Name = "splitContainer2";
         this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
         // 
         // splitContainer2.Panel1
         // 
         this.splitContainer2.Panel1.Controls.Add(this.label1);
         this.splitContainer2.Panel1.Controls.Add(this.selectImagesButton);
         // 
         // splitContainer2.Panel2
         // 
         this.splitContainer2.Panel2.Controls.Add(this.sourceImageDataGridView);
         this.splitContainer2.Size = new System.Drawing.Size(374, 664);
         this.splitContainer2.SplitterDistance = 69;
         this.splitContainer2.TabIndex = 0;
         // 
         // selectImagesButton
         // 
         this.selectImagesButton.Location = new System.Drawing.Point(12, 29);
         this.selectImagesButton.Name = "selectImagesButton";
         this.selectImagesButton.Size = new System.Drawing.Size(138, 23);
         this.selectImagesButton.TabIndex = 0;
         this.selectImagesButton.Text = "Select";
         this.selectImagesButton.UseVisualStyleBackColor = true;
         this.selectImagesButton.Click += new System.EventHandler(this.selectImagesButton_Click);
         // 
         // sourceImageDataGridView
         // 
         this.sourceImageDataGridView.AllowUserToAddRows = false;
         this.sourceImageDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.sourceImageDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ThumbnailColumn,
            this.FileNameColumn});
         this.sourceImageDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
         this.sourceImageDataGridView.Location = new System.Drawing.Point(0, 0);
         this.sourceImageDataGridView.Name = "sourceImageDataGridView";
         this.sourceImageDataGridView.ReadOnly = true;
         this.sourceImageDataGridView.Size = new System.Drawing.Size(374, 591);
         this.sourceImageDataGridView.TabIndex = 0;
         // 
         // resultImageBox
         // 
         this.resultImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.resultImageBox.Location = new System.Drawing.Point(0, 0);
         this.resultImageBox.Name = "resultImageBox";
         this.resultImageBox.Size = new System.Drawing.Size(749, 664);
         this.resultImageBox.TabIndex = 2;
         this.resultImageBox.TabStop = false;
         // 
         // ThumbnailColumn
         // 
         this.ThumbnailColumn.FillWeight = 200F;
         this.ThumbnailColumn.HeaderText = "Thumbnail";
         this.ThumbnailColumn.Name = "ThumbnailColumn";
         this.ThumbnailColumn.ReadOnly = true;
         this.ThumbnailColumn.Width = 200;
         // 
         // FileNameColumn
         // 
         this.FileNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
         this.FileNameColumn.HeaderText = "File Name";
         this.FileNameColumn.Name = "FileNameColumn";
         this.FileNameColumn.ReadOnly = true;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(13, 13);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(78, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Source Images";
         // 
         // StitchingMainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1127, 664);
         this.Controls.Add(this.splitContainer1);
         this.Name = "StitchingMainForm";
         this.Text = "Stitching";
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel2.ResumeLayout(false);
         this.splitContainer1.ResumeLayout(false);
         this.splitContainer2.Panel1.ResumeLayout(false);
         this.splitContainer2.Panel1.PerformLayout();
         this.splitContainer2.Panel2.ResumeLayout(false);
         this.splitContainer2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.sourceImageDataGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.resultImageBox)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.SplitContainer splitContainer2;
      private System.Windows.Forms.Button selectImagesButton;
      private System.Windows.Forms.DataGridView sourceImageDataGridView;
      private Emgu.CV.UI.ImageBox resultImageBox;
      private System.Windows.Forms.DataGridViewImageColumn ThumbnailColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn FileNameColumn;
      private System.Windows.Forms.Label label1;
   }
}

