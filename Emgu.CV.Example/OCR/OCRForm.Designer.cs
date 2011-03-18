namespace OCR
{
   partial class OCRForm
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
         this.components = new System.ComponentModel.Container();
         this.fileNameTextBox = new System.Windows.Forms.TextBox();
         this.loadImageButton = new System.Windows.Forms.Button();
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.languageNameLabel = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.loadLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.splitContainer2 = new System.Windows.Forms.SplitContainer();
         this.imageBox1 = new Emgu.CV.UI.ImageBox();
         this.ocrTextBox = new System.Windows.Forms.TextBox();
         this.openImageFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.openLanguageFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         this.menuStrip1.SuspendLayout();
         this.splitContainer2.Panel1.SuspendLayout();
         this.splitContainer2.Panel2.SuspendLayout();
         this.splitContainer2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
         this.SuspendLayout();
         // 
         // fileNameTextBox
         // 
         this.fileNameTextBox.Location = new System.Drawing.Point(67, 27);
         this.fileNameTextBox.Name = "fileNameTextBox";
         this.fileNameTextBox.ReadOnly = true;
         this.fileNameTextBox.Size = new System.Drawing.Size(519, 20);
         this.fileNameTextBox.TabIndex = 1;
         // 
         // loadImageButton
         // 
         this.loadImageButton.Location = new System.Drawing.Point(610, 24);
         this.loadImageButton.Name = "loadImageButton";
         this.loadImageButton.Size = new System.Drawing.Size(75, 23);
         this.loadImageButton.TabIndex = 2;
         this.loadImageButton.Text = "Load Image";
         this.loadImageButton.UseVisualStyleBackColor = true;
         this.loadImageButton.Click += new System.EventHandler(this.loadImageButton_Click);
         // 
         // splitContainer1
         // 
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point(0, 0);
         this.splitContainer1.Name = "splitContainer1";
         this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.languageNameLabel);
         this.splitContainer1.Panel1.Controls.Add(this.label1);
         this.splitContainer1.Panel1.Controls.Add(this.fileNameTextBox);
         this.splitContainer1.Panel1.Controls.Add(this.loadImageButton);
         this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
         this.splitContainer1.Size = new System.Drawing.Size(929, 609);
         this.splitContainer1.SplitterDistance = 101;
         this.splitContainer1.TabIndex = 3;
         // 
         // languageNameLabel
         // 
         this.languageNameLabel.AutoSize = true;
         this.languageNameLabel.Location = new System.Drawing.Point(98, 70);
         this.languageNameLabel.Name = "languageNameLabel";
         this.languageNameLabel.Size = new System.Drawing.Size(88, 13);
         this.languageNameLabel.TabIndex = 5;
         this.languageNameLabel.Text = "{language name}";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(33, 70);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(58, 13);
         this.label1.TabIndex = 4;
         this.label1.Text = "Language:";
         // 
         // menuStrip1
         // 
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
         this.menuStrip1.Location = new System.Drawing.Point(0, 0);
         this.menuStrip1.Name = "menuStrip1";
         this.menuStrip1.Size = new System.Drawing.Size(929, 24);
         this.menuStrip1.TabIndex = 3;
         this.menuStrip1.Text = "menuStrip1";
         // 
         // fileToolStripMenuItem
         // 
         this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadLanguageToolStripMenuItem});
         this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
         this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
         this.fileToolStripMenuItem.Text = "File";
         // 
         // loadLanguageToolStripMenuItem
         // 
         this.loadLanguageToolStripMenuItem.Name = "loadLanguageToolStripMenuItem";
         this.loadLanguageToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
         this.loadLanguageToolStripMenuItem.Text = "Load Language";
         this.loadLanguageToolStripMenuItem.Click += new System.EventHandler(this.loadLanguageToolStripMenuItem_Click);
         // 
         // splitContainer2
         // 
         this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer2.Location = new System.Drawing.Point(0, 0);
         this.splitContainer2.Name = "splitContainer2";
         // 
         // splitContainer2.Panel1
         // 
         this.splitContainer2.Panel1.Controls.Add(this.imageBox1);
         // 
         // splitContainer2.Panel2
         // 
         this.splitContainer2.Panel2.Controls.Add(this.ocrTextBox);
         this.splitContainer2.Size = new System.Drawing.Size(929, 504);
         this.splitContainer2.SplitterDistance = 465;
         this.splitContainer2.TabIndex = 0;
         // 
         // imageBox1
         // 
         this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.imageBox1.Location = new System.Drawing.Point(0, 0);
         this.imageBox1.Name = "imageBox1";
         this.imageBox1.Size = new System.Drawing.Size(465, 504);
         this.imageBox1.TabIndex = 2;
         this.imageBox1.TabStop = false;
         // 
         // ocrTextBox
         // 
         this.ocrTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ocrTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.ocrTextBox.Location = new System.Drawing.Point(0, 0);
         this.ocrTextBox.Multiline = true;
         this.ocrTextBox.Name = "ocrTextBox";
         this.ocrTextBox.ReadOnly = true;
         this.ocrTextBox.Size = new System.Drawing.Size(460, 504);
         this.ocrTextBox.TabIndex = 0;
         this.ocrTextBox.WordWrap = false;
         // 
         // openImageFileDialog
         // 
         this.openImageFileDialog.FileName = "openFileDialog1";
         // 
         // openLanguageFileDialog
         // 
         this.openLanguageFileDialog.DefaultExt = "traineddata";
         this.openLanguageFileDialog.Filter = "\"tesseract language file|*.traineddata|All files|*.*\"";
         // 
         // OCRForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(929, 609);
         this.Controls.Add(this.splitContainer1);
         this.MainMenuStrip = this.menuStrip1;
         this.Name = "OCRForm";
         this.Text = "Form1";
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel1.PerformLayout();
         this.splitContainer1.Panel2.ResumeLayout(false);
         this.splitContainer1.ResumeLayout(false);
         this.menuStrip1.ResumeLayout(false);
         this.menuStrip1.PerformLayout();
         this.splitContainer2.Panel1.ResumeLayout(false);
         this.splitContainer2.Panel2.ResumeLayout(false);
         this.splitContainer2.Panel2.PerformLayout();
         this.splitContainer2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TextBox fileNameTextBox;
      private System.Windows.Forms.Button loadImageButton;
      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.SplitContainer splitContainer2;
      private Emgu.CV.UI.ImageBox imageBox1;
      private System.Windows.Forms.TextBox ocrTextBox;
      private System.Windows.Forms.OpenFileDialog openImageFileDialog;
      private System.Windows.Forms.MenuStrip menuStrip1;
      private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
      private System.Windows.Forms.Label languageNameLabel;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ToolStripMenuItem loadLanguageToolStripMenuItem;
      private System.Windows.Forms.OpenFileDialog openLanguageFileDialog;
   }
}

