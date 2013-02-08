namespace ShapeDetection
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
           this.panel1 = new System.Windows.Forms.Panel();
           this.fileNameTextBox = new System.Windows.Forms.TextBox();
           this.label5 = new System.Windows.Forms.Label();
           this.loadImageButton = new System.Windows.Forms.Button();
           this.splitContainer1 = new System.Windows.Forms.SplitContainer();
           this.splitContainer2 = new System.Windows.Forms.SplitContainer();
           this.originalImageBox = new Emgu.CV.UI.ImageBox();
           this.panel2 = new System.Windows.Forms.Panel();
           this.label1 = new System.Windows.Forms.Label();
           this.circleImageBox = new Emgu.CV.UI.ImageBox();
           this.panel4 = new System.Windows.Forms.Panel();
           this.label3 = new System.Windows.Forms.Label();
           this.splitContainer3 = new System.Windows.Forms.SplitContainer();
           this.triangleRectangleImageBox = new Emgu.CV.UI.ImageBox();
           this.panel3 = new System.Windows.Forms.Panel();
           this.label2 = new System.Windows.Forms.Label();
           this.lineImageBox = new Emgu.CV.UI.ImageBox();
           this.panel5 = new System.Windows.Forms.Panel();
           this.label4 = new System.Windows.Forms.Label();
           this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
           this.panel1.SuspendLayout();
           this.splitContainer1.Panel1.SuspendLayout();
           this.splitContainer1.Panel2.SuspendLayout();
           this.splitContainer1.SuspendLayout();
           this.splitContainer2.Panel1.SuspendLayout();
           this.splitContainer2.Panel2.SuspendLayout();
           this.splitContainer2.SuspendLayout();
           ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
           this.panel2.SuspendLayout();
           ((System.ComponentModel.ISupportInitialize)(this.circleImageBox)).BeginInit();
           this.panel4.SuspendLayout();
           this.splitContainer3.Panel1.SuspendLayout();
           this.splitContainer3.Panel2.SuspendLayout();
           this.splitContainer3.SuspendLayout();
           ((System.ComponentModel.ISupportInitialize)(this.triangleRectangleImageBox)).BeginInit();
           this.panel3.SuspendLayout();
           ((System.ComponentModel.ISupportInitialize)(this.lineImageBox)).BeginInit();
           this.panel5.SuspendLayout();
           this.SuspendLayout();
           // 
           // panel1
           // 
           this.panel1.Controls.Add(this.fileNameTextBox);
           this.panel1.Controls.Add(this.label5);
           this.panel1.Controls.Add(this.loadImageButton);
           this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
           this.panel1.Location = new System.Drawing.Point(0, 0);
           this.panel1.Name = "panel1";
           this.panel1.Size = new System.Drawing.Size(822, 53);
           this.panel1.TabIndex = 0;
           // 
           // fileNameTextBox
           // 
           this.fileNameTextBox.Location = new System.Drawing.Point(49, 14);
           this.fileNameTextBox.Name = "fileNameTextBox";
           this.fileNameTextBox.ReadOnly = true;
           this.fileNameTextBox.Size = new System.Drawing.Size(215, 20);
           this.fileNameTextBox.TabIndex = 2;
           this.fileNameTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
           // 
           // label5
           // 
           this.label5.AutoSize = true;
           this.label5.Location = new System.Drawing.Point(16, 17);
           this.label5.Name = "label5";
           this.label5.Size = new System.Drawing.Size(26, 13);
           this.label5.TabIndex = 1;
           this.label5.Text = "File:";
           // 
           // loadImageButton
           // 
           this.loadImageButton.Location = new System.Drawing.Point(270, 12);
           this.loadImageButton.Name = "loadImageButton";
           this.loadImageButton.Size = new System.Drawing.Size(30, 23);
           this.loadImageButton.TabIndex = 0;
           this.loadImageButton.Text = "...";
           this.loadImageButton.UseVisualStyleBackColor = true;
           this.loadImageButton.Click += new System.EventHandler(this.loadImageButton_Click);
           // 
           // splitContainer1
           // 
           this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
           this.splitContainer1.Location = new System.Drawing.Point(0, 53);
           this.splitContainer1.Name = "splitContainer1";
           // 
           // splitContainer1.Panel1
           // 
           this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
           // 
           // splitContainer1.Panel2
           // 
           this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
           this.splitContainer1.Size = new System.Drawing.Size(822, 756);
           this.splitContainer1.SplitterDistance = 404;
           this.splitContainer1.TabIndex = 1;
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
           this.splitContainer2.Panel1.Controls.Add(this.originalImageBox);
           this.splitContainer2.Panel1.Controls.Add(this.panel2);
           // 
           // splitContainer2.Panel2
           // 
           this.splitContainer2.Panel2.Controls.Add(this.circleImageBox);
           this.splitContainer2.Panel2.Controls.Add(this.panel4);
           this.splitContainer2.Size = new System.Drawing.Size(404, 756);
           this.splitContainer2.SplitterDistance = 387;
           this.splitContainer2.TabIndex = 0;
           // 
           // originalImageBox
           // 
           this.originalImageBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
           this.originalImageBox.Cursor = System.Windows.Forms.Cursors.Cross;
           this.originalImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
           this.originalImageBox.Location = new System.Drawing.Point(0, 25);
           this.originalImageBox.Name = "originalImageBox";
           this.originalImageBox.Size = new System.Drawing.Size(404, 362);
           this.originalImageBox.TabIndex = 3;
           this.originalImageBox.TabStop = false;
           // 
           // panel2
           // 
           this.panel2.Controls.Add(this.label1);
           this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
           this.panel2.Location = new System.Drawing.Point(0, 0);
           this.panel2.Name = "panel2";
           this.panel2.Size = new System.Drawing.Size(404, 25);
           this.panel2.TabIndex = 2;
           // 
           // label1
           // 
           this.label1.AutoSize = true;
           this.label1.Location = new System.Drawing.Point(12, 5);
           this.label1.Name = "label1";
           this.label1.Size = new System.Drawing.Size(74, 13);
           this.label1.TabIndex = 0;
           this.label1.Text = "Original Image";
           // 
           // circleImageBox
           // 
           this.circleImageBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
           this.circleImageBox.Cursor = System.Windows.Forms.Cursors.Cross;
           this.circleImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
           this.circleImageBox.Location = new System.Drawing.Point(0, 25);
           this.circleImageBox.Name = "circleImageBox";
           this.circleImageBox.Size = new System.Drawing.Size(404, 340);
           this.circleImageBox.TabIndex = 4;
           this.circleImageBox.TabStop = false;
           // 
           // panel4
           // 
           this.panel4.Controls.Add(this.label3);
           this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
           this.panel4.Location = new System.Drawing.Point(0, 0);
           this.panel4.Name = "panel4";
           this.panel4.Size = new System.Drawing.Size(404, 25);
           this.panel4.TabIndex = 3;
           // 
           // label3
           // 
           this.label3.AutoSize = true;
           this.label3.Location = new System.Drawing.Point(12, 5);
           this.label3.Name = "label3";
           this.label3.Size = new System.Drawing.Size(85, 13);
           this.label3.TabIndex = 0;
           this.label3.Text = "Detected Circles";
           // 
           // splitContainer3
           // 
           this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
           this.splitContainer3.Location = new System.Drawing.Point(0, 0);
           this.splitContainer3.Name = "splitContainer3";
           this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
           // 
           // splitContainer3.Panel1
           // 
           this.splitContainer3.Panel1.Controls.Add(this.triangleRectangleImageBox);
           this.splitContainer3.Panel1.Controls.Add(this.panel3);
           // 
           // splitContainer3.Panel2
           // 
           this.splitContainer3.Panel2.Controls.Add(this.lineImageBox);
           this.splitContainer3.Panel2.Controls.Add(this.panel5);
           this.splitContainer3.Size = new System.Drawing.Size(414, 756);
           this.splitContainer3.SplitterDistance = 387;
           this.splitContainer3.TabIndex = 0;
           // 
           // triangleRectangleImageBox
           // 
           this.triangleRectangleImageBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
           this.triangleRectangleImageBox.Cursor = System.Windows.Forms.Cursors.Cross;
           this.triangleRectangleImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
           this.triangleRectangleImageBox.Location = new System.Drawing.Point(0, 25);
           this.triangleRectangleImageBox.Name = "triangleRectangleImageBox";
           this.triangleRectangleImageBox.Size = new System.Drawing.Size(414, 362);
           this.triangleRectangleImageBox.TabIndex = 4;
           this.triangleRectangleImageBox.TabStop = false;
           // 
           // panel3
           // 
           this.panel3.Controls.Add(this.label2);
           this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
           this.panel3.Location = new System.Drawing.Point(0, 0);
           this.panel3.Name = "panel3";
           this.panel3.Size = new System.Drawing.Size(414, 25);
           this.panel3.TabIndex = 3;
           // 
           // label2
           // 
           this.label2.AutoSize = true;
           this.label2.Location = new System.Drawing.Point(16, 5);
           this.label2.Name = "label2";
           this.label2.Size = new System.Drawing.Size(178, 13);
           this.label2.TabIndex = 0;
           this.label2.Text = "Detected Triangles and  Rectangles";
           // 
           // lineImageBox
           // 
           this.lineImageBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
           this.lineImageBox.Cursor = System.Windows.Forms.Cursors.Cross;
           this.lineImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
           this.lineImageBox.Location = new System.Drawing.Point(0, 25);
           this.lineImageBox.Name = "lineImageBox";
           this.lineImageBox.Size = new System.Drawing.Size(414, 340);
           this.lineImageBox.TabIndex = 4;
           this.lineImageBox.TabStop = false;
           // 
           // panel5
           // 
           this.panel5.Controls.Add(this.label4);
           this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
           this.panel5.Location = new System.Drawing.Point(0, 0);
           this.panel5.Name = "panel5";
           this.panel5.Size = new System.Drawing.Size(414, 25);
           this.panel5.TabIndex = 3;
           // 
           // label4
           // 
           this.label4.AutoSize = true;
           this.label4.Location = new System.Drawing.Point(16, 5);
           this.label4.Name = "label4";
           this.label4.Size = new System.Drawing.Size(79, 13);
           this.label4.TabIndex = 0;
           this.label4.Text = "Detected Lines";
           // 
           // openFileDialog1
           // 
           this.openFileDialog1.FileName = "openFileDialog1";
           // 
           // MainForm
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.ClientSize = new System.Drawing.Size(822, 809);
           this.Controls.Add(this.splitContainer1);
           this.Controls.Add(this.panel1);
           this.Name = "MainForm";
           this.Text = "Shape Detection";
           this.panel1.ResumeLayout(false);
           this.panel1.PerformLayout();
           this.splitContainer1.Panel1.ResumeLayout(false);
           this.splitContainer1.Panel2.ResumeLayout(false);
           this.splitContainer1.ResumeLayout(false);
           this.splitContainer2.Panel1.ResumeLayout(false);
           this.splitContainer2.Panel2.ResumeLayout(false);
           this.splitContainer2.ResumeLayout(false);
           ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
           this.panel2.ResumeLayout(false);
           this.panel2.PerformLayout();
           ((System.ComponentModel.ISupportInitialize)(this.circleImageBox)).EndInit();
           this.panel4.ResumeLayout(false);
           this.panel4.PerformLayout();
           this.splitContainer3.Panel1.ResumeLayout(false);
           this.splitContainer3.Panel2.ResumeLayout(false);
           this.splitContainer3.ResumeLayout(false);
           ((System.ComponentModel.ISupportInitialize)(this.triangleRectangleImageBox)).EndInit();
           this.panel3.ResumeLayout(false);
           this.panel3.PerformLayout();
           ((System.ComponentModel.ISupportInitialize)(this.lineImageBox)).EndInit();
           this.panel5.ResumeLayout(false);
           this.panel5.PerformLayout();
           this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label4;
        private Emgu.CV.UI.ImageBox originalImageBox;
        private Emgu.CV.UI.ImageBox circleImageBox;
        private Emgu.CV.UI.ImageBox triangleRectangleImageBox;
        private Emgu.CV.UI.ImageBox lineImageBox;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button loadImageButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

    }
}

