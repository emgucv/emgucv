namespace OCR
{
   partial class MainForm
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
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.imageBox1 = new Emgu.CV.UI.ImageBox();
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
         this.button1 = new System.Windows.Forms.Button();
         this.fileNameTextBox = new System.Windows.Forms.TextBox();
         this.numericalOnlyCheckBox = new System.Windows.Forms.CheckBox();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
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
         this.splitContainer1.Panel1.Controls.Add(this.numericalOnlyCheckBox);
         this.splitContainer1.Panel1.Controls.Add(this.fileNameTextBox);
         this.splitContainer1.Panel1.Controls.Add(this.button1);
         this.splitContainer1.Panel1.Controls.Add(this.label2);
         this.splitContainer1.Panel1.Controls.Add(this.label1);
         this.splitContainer1.Panel1.Controls.Add(this.textBox1);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.imageBox1);
         this.splitContainer1.Size = new System.Drawing.Size(1066, 635);
         this.splitContainer1.SplitterDistance = 334;
         this.splitContainer1.TabIndex = 0;
         // 
         // imageBox1
         // 
         this.imageBox1.Cursor = System.Windows.Forms.Cursors.Cross;
         this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.imageBox1.Location = new System.Drawing.Point(0, 0);
         this.imageBox1.Name = "imageBox1";
         this.imageBox1.Size = new System.Drawing.Size(728, 635);
         this.imageBox1.TabIndex = 2;
         this.imageBox1.TabStop = false;
         // 
         // textBox1
         // 
         this.textBox1.Location = new System.Drawing.Point(3, 107);
         this.textBox1.Multiline = true;
         this.textBox1.Name = "textBox1";
         this.textBox1.ReadOnly = true;
         this.textBox1.Size = new System.Drawing.Size(329, 516);
         this.textBox1.TabIndex = 0;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 91);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(89, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Text From Image:";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(3, 19);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(39, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "Image:";
         // 
         // openFileDialog1
         // 
         this.openFileDialog1.FileName = "openFileDialog1";
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(286, 19);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(30, 23);
         this.button1.TabIndex = 3;
         this.button1.Text = "...";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // fileNameTextBox
         // 
         this.fileNameTextBox.Location = new System.Drawing.Point(49, 19);
         this.fileNameTextBox.Name = "fileNameTextBox";
         this.fileNameTextBox.ReadOnly = true;
         this.fileNameTextBox.Size = new System.Drawing.Size(231, 20);
         this.fileNameTextBox.TabIndex = 4;
         // 
         // numericalOnlyCheckBox
         // 
         this.numericalOnlyCheckBox.AutoSize = true;
         this.numericalOnlyCheckBox.Location = new System.Drawing.Point(11, 61);
         this.numericalOnlyCheckBox.Name = "numericalOnlyCheckBox";
         this.numericalOnlyCheckBox.Size = new System.Drawing.Size(97, 17);
         this.numericalOnlyCheckBox.TabIndex = 5;
         this.numericalOnlyCheckBox.Text = "Numerical Only";
         this.numericalOnlyCheckBox.UseVisualStyleBackColor = true;
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1066, 635);
         this.Controls.Add(this.splitContainer1);
         this.Name = "MainForm";
         this.Text = "OCR (a.k.a. Text Recognition)";
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel1.PerformLayout();
         this.splitContainer1.Panel2.ResumeLayout(false);
         this.splitContainer1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.SplitContainer splitContainer1;
      private Emgu.CV.UI.ImageBox imageBox1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox textBox1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.OpenFileDialog openFileDialog1;
      private System.Windows.Forms.TextBox fileNameTextBox;
      private System.Windows.Forms.CheckBox numericalOnlyCheckBox;
   }
}

