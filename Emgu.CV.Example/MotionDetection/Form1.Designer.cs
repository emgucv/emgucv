namespace MotionDetection
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
           this.label1 = new System.Windows.Forms.Label();
           this.label2 = new System.Windows.Forms.Label();
           this.label3 = new System.Windows.Forms.Label();
           this.motionImageBox = new Emgu.CV.UI.ImageBox();
           this.capturedImageBox = new Emgu.CV.UI.ImageBox();
           this.forgroundImageBox = new Emgu.CV.UI.ImageBox();
           this.label4 = new System.Windows.Forms.Label();
           ((System.ComponentModel.ISupportInitialize)(this.motionImageBox)).BeginInit();
           ((System.ComponentModel.ISupportInitialize)(this.capturedImageBox)).BeginInit();
           ((System.ComponentModel.ISupportInitialize)(this.forgroundImageBox)).BeginInit();
           this.SuspendLayout();
           // 
           // label1
           // 
           this.label1.AutoSize = true;
           this.label1.Location = new System.Drawing.Point(9, 18);
           this.label1.Name = "label1";
           this.label1.Size = new System.Drawing.Size(85, 13);
           this.label1.TabIndex = 1;
           this.label1.Text = "Captured Image:";
           // 
           // label2
           // 
           this.label2.AutoSize = true;
           this.label2.Location = new System.Drawing.Point(835, 18);
           this.label2.Name = "label2";
           this.label2.Size = new System.Drawing.Size(39, 13);
           this.label2.TabIndex = 3;
           this.label2.Text = "Motion";
           // 
           // label3
           // 
           this.label3.AutoSize = true;
           this.label3.Location = new System.Drawing.Point(12, 452);
           this.label3.Name = "label3";
           this.label3.Size = new System.Drawing.Size(35, 13);
           this.label3.TabIndex = 4;
           this.label3.Text = "label3";
           // 
           // motionImageBox
           // 
           this.motionImageBox.Location = new System.Drawing.Point(838, 53);
           this.motionImageBox.Name = "motionImageBox";
           this.motionImageBox.Size = new System.Drawing.Size(397, 353);
           this.motionImageBox.TabIndex = 2;
           this.motionImageBox.TabStop = false;
           // 
           // capturedImageBox
           // 
           this.capturedImageBox.Location = new System.Drawing.Point(4, 53);
           this.capturedImageBox.Name = "capturedImageBox";
           this.capturedImageBox.Size = new System.Drawing.Size(409, 353);
           this.capturedImageBox.TabIndex = 0;
           this.capturedImageBox.TabStop = false;
           // 
           // forgroundImageBox
           // 
           this.forgroundImageBox.Location = new System.Drawing.Point(427, 53);
           this.forgroundImageBox.Name = "forgroundImageBox";
           this.forgroundImageBox.Size = new System.Drawing.Size(397, 353);
           this.forgroundImageBox.TabIndex = 5;
           this.forgroundImageBox.TabStop = false;
           // 
           // label4
           // 
           this.label4.AutoSize = true;
           this.label4.Location = new System.Drawing.Point(424, 18);
           this.label4.Name = "label4";
           this.label4.Size = new System.Drawing.Size(84, 13);
           this.label4.TabIndex = 6;
           this.label4.Text = "Forground Mask";
           // 
           // Form1
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.ClientSize = new System.Drawing.Size(1258, 482);
           this.Controls.Add(this.label4);
           this.Controls.Add(this.forgroundImageBox);
           this.Controls.Add(this.label3);
           this.Controls.Add(this.label2);
           this.Controls.Add(this.motionImageBox);
           this.Controls.Add(this.label1);
           this.Controls.Add(this.capturedImageBox);
           this.Name = "Form1";
           this.Text = "Form1";
           this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
           ((System.ComponentModel.ISupportInitialize)(this.motionImageBox)).EndInit();
           ((System.ComponentModel.ISupportInitialize)(this.capturedImageBox)).EndInit();
           ((System.ComponentModel.ISupportInitialize)(this.forgroundImageBox)).EndInit();
           this.ResumeLayout(false);
           this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox capturedImageBox;
        private System.Windows.Forms.Label label1;
        private Emgu.CV.UI.ImageBox motionImageBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Emgu.CV.UI.ImageBox forgroundImageBox;
        private System.Windows.Forms.Label label4;
    }
}

