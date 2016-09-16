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
         this.ocrOptionsComboBox = new System.Windows.Forms.ComboBox();
         this.languageNameLabel = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.loadLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.splitContainer2 = new System.Windows.Forms.SplitContainer();
         this.imageBox1 = new Emgu.CV.UI.ImageBox();
         this.openImageFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.openLanguageFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.splitContainer3 = new System.Windows.Forms.SplitContainer();
         this.ocrTextBox = new System.Windows.Forms.TextBox();
         this.hocrTextBox = new System.Windows.Forms.TextBox();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         this.menuStrip1.SuspendLayout();
         this.splitContainer2.Panel1.SuspendLayout();
         this.splitContainer2.Panel2.SuspendLayout();
         this.splitContainer2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
         this.splitContainer3.Panel1.SuspendLayout();
         this.splitContainer3.Panel2.SuspendLayout();
         this.splitContainer3.SuspendLayout();
         this.SuspendLayout();
         // 
         // fileNameTextBox
         // 
         this.fileNameTextBox.Location = new System.Drawing.Point(100, 42);
         this.fileNameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
         this.fileNameTextBox.Name = "fileNameTextBox";
         this.fileNameTextBox.ReadOnly = true;
         this.fileNameTextBox.Size = new System.Drawing.Size(776, 26);
         this.fileNameTextBox.TabIndex = 1;
         // 
         // loadImageButton
         // 
         this.loadImageButton.Location = new System.Drawing.Point(915, 37);
         this.loadImageButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
         this.loadImageButton.Name = "loadImageButton";
         this.loadImageButton.Size = new System.Drawing.Size(112, 35);
         this.loadImageButton.TabIndex = 2;
         this.loadImageButton.Text = "Load Image";
         this.loadImageButton.UseVisualStyleBackColor = true;
         this.loadImageButton.Click += new System.EventHandler(this.loadImageButton_Click);
         // 
         // splitContainer1
         // 
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point(0, 0);
         this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
         this.splitContainer1.Name = "splitContainer1";
         this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.ocrOptionsComboBox);
         this.splitContainer1.Panel1.Controls.Add(this.languageNameLabel);
         this.splitContainer1.Panel1.Controls.Add(this.label1);
         this.splitContainer1.Panel1.Controls.Add(this.fileNameTextBox);
         this.splitContainer1.Panel1.Controls.Add(this.loadImageButton);
         this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
         this.splitContainer1.Size = new System.Drawing.Size(1525, 937);
         this.splitContainer1.SplitterDistance = 155;
         this.splitContainer1.SplitterWidth = 6;
         this.splitContainer1.TabIndex = 3;
         // 
         // ocrOptionsComboBox
         // 
         this.ocrOptionsComboBox.FormattingEnabled = true;
         this.ocrOptionsComboBox.Items.AddRange(new object[] {
            "Full Page OCR",
            "Text Region Detection"});
         this.ocrOptionsComboBox.Location = new System.Drawing.Point(100, 100);
         this.ocrOptionsComboBox.Name = "ocrOptionsComboBox";
         this.ocrOptionsComboBox.Size = new System.Drawing.Size(269, 28);
         this.ocrOptionsComboBox.TabIndex = 6;
         // 
         // languageNameLabel
         // 
         this.languageNameLabel.AutoSize = true;
         this.languageNameLabel.Location = new System.Drawing.Point(519, 108);
         this.languageNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.languageNameLabel.Name = "languageNameLabel";
         this.languageNameLabel.Size = new System.Drawing.Size(129, 20);
         this.languageNameLabel.TabIndex = 5;
         this.languageNameLabel.Text = "{language name}";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(426, 108);
         this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(85, 20);
         this.label1.TabIndex = 4;
         this.label1.Text = "Language:";
         // 
         // menuStrip1
         // 
         this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
         this.menuStrip1.Location = new System.Drawing.Point(0, 0);
         this.menuStrip1.Name = "menuStrip1";
         this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
         this.menuStrip1.Size = new System.Drawing.Size(1525, 35);
         this.menuStrip1.TabIndex = 3;
         this.menuStrip1.Text = "menuStrip1";
         // 
         // fileToolStripMenuItem
         // 
         this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadLanguageToolStripMenuItem});
         this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
         this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
         this.fileToolStripMenuItem.Text = "File";
         // 
         // loadLanguageToolStripMenuItem
         // 
         this.loadLanguageToolStripMenuItem.Name = "loadLanguageToolStripMenuItem";
         this.loadLanguageToolStripMenuItem.Size = new System.Drawing.Size(218, 30);
         this.loadLanguageToolStripMenuItem.Text = "Load Language";
         this.loadLanguageToolStripMenuItem.Click += new System.EventHandler(this.loadLanguageToolStripMenuItem_Click);
         // 
         // splitContainer2
         // 
         this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer2.Location = new System.Drawing.Point(0, 0);
         this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
         this.splitContainer2.Name = "splitContainer2";
         // 
         // splitContainer2.Panel1
         // 
         this.splitContainer2.Panel1.Controls.Add(this.imageBox1);
         // 
         // splitContainer2.Panel2
         // 
         this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
         this.splitContainer2.Size = new System.Drawing.Size(1525, 776);
         this.splitContainer2.SplitterDistance = 763;
         this.splitContainer2.SplitterWidth = 6;
         this.splitContainer2.TabIndex = 0;
         // 
         // imageBox1
         // 
         this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.imageBox1.Location = new System.Drawing.Point(0, 0);
         this.imageBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
         this.imageBox1.Name = "imageBox1";
         this.imageBox1.Size = new System.Drawing.Size(763, 776);
         this.imageBox1.TabIndex = 2;
         this.imageBox1.TabStop = false;
         // 
         // openImageFileDialog
         // 
         this.openImageFileDialog.FileName = "openFileDialog1";
         // 
         // openLanguageFileDialog
         // 
         this.openLanguageFileDialog.DefaultExt = "traineddata";
         this.openLanguageFileDialog.Filter = "tesseract language file|*.traineddata|All files|*.*";
         // 
         // splitContainer3
         // 
         this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer3.Location = new System.Drawing.Point(0, 0);
         this.splitContainer3.Name = "splitContainer3";
         // 
         // splitContainer3.Panel1
         // 
         this.splitContainer3.Panel1.Controls.Add(this.ocrTextBox);
         // 
         // splitContainer3.Panel2
         // 
         this.splitContainer3.Panel2.Controls.Add(this.hocrTextBox);
         this.splitContainer3.Size = new System.Drawing.Size(756, 776);
         this.splitContainer3.SplitterDistance = 397;
         this.splitContainer3.TabIndex = 0;
         // 
         // ocrTextBox
         // 
         this.ocrTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ocrTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.ocrTextBox.Location = new System.Drawing.Point(0, 0);
         this.ocrTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
         this.ocrTextBox.Multiline = true;
         this.ocrTextBox.Name = "ocrTextBox";
         this.ocrTextBox.ReadOnly = true;
         this.ocrTextBox.Size = new System.Drawing.Size(397, 776);
         this.ocrTextBox.TabIndex = 1;
         this.ocrTextBox.WordWrap = false;
         // 
         // hocrTextBox
         // 
         this.hocrTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.hocrTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.hocrTextBox.Location = new System.Drawing.Point(0, 0);
         this.hocrTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
         this.hocrTextBox.Multiline = true;
         this.hocrTextBox.Name = "hocrTextBox";
         this.hocrTextBox.ReadOnly = true;
         this.hocrTextBox.Size = new System.Drawing.Size(355, 776);
         this.hocrTextBox.TabIndex = 2;
         this.hocrTextBox.WordWrap = false;
         // 
         // OCRForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1525, 937);
         this.Controls.Add(this.splitContainer1);
         this.MainMenuStrip = this.menuStrip1;
         this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
         this.splitContainer2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
         this.splitContainer3.Panel1.ResumeLayout(false);
         this.splitContainer3.Panel1.PerformLayout();
         this.splitContainer3.Panel2.ResumeLayout(false);
         this.splitContainer3.Panel2.PerformLayout();
         this.splitContainer3.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TextBox fileNameTextBox;
      private System.Windows.Forms.Button loadImageButton;
      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.SplitContainer splitContainer2;
      private Emgu.CV.UI.ImageBox imageBox1;
      private System.Windows.Forms.OpenFileDialog openImageFileDialog;
      private System.Windows.Forms.MenuStrip menuStrip1;
      private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
      private System.Windows.Forms.Label languageNameLabel;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ToolStripMenuItem loadLanguageToolStripMenuItem;
      private System.Windows.Forms.OpenFileDialog openLanguageFileDialog;
      private System.Windows.Forms.ComboBox ocrOptionsComboBox;
      private System.Windows.Forms.SplitContainer splitContainer3;
      private System.Windows.Forms.TextBox ocrTextBox;
      private System.Windows.Forms.TextBox hocrTextBox;
   }
}

