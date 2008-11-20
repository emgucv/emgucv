namespace Emgu.CV.UI
{
    partial class ImageViewer
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
           this.imageBox1 = new Emgu.CV.UI.ImageBox();
           ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
           this.SuspendLayout();
           // 
           // imageBox1
           // 
           this.imageBox1.Cursor = System.Windows.Forms.Cursors.Cross;
           this.imageBox1.Location = new System.Drawing.Point(0, 0);
           this.imageBox1.Name = "imageBox1";
           this.imageBox1.Size = new System.Drawing.Size(100, 100);
           this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
           this.imageBox1.TabIndex = 0;
           this.imageBox1.TabStop = false;
           // 
           // ImageViewer
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.AutoScroll = true;
           this.ClientSize = new System.Drawing.Size(563, 482);
           this.Controls.Add(this.imageBox1);
           this.Name = "ImageViewer";
           this.Text = "Image Viewer";
           ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
           this.ResumeLayout(false);
           this.PerformLayout();

        }

        #endregion

        private ImageBox imageBox1;

    }
}
