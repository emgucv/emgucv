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
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.SuspendLayout();
            // 
            // imageBox1
            // 
            this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox1.Image = null;
            this.imageBox1.Location = new System.Drawing.Point(0, 0);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(563, 482);
            this.imageBox1.TabIndex = 0;
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 482);
            this.Controls.Add(this.imageBox1);
            this.Name = "ImageViewer";
            this.Text = "ImageViewer";
            this.ResumeLayout(false);

        }

        #endregion

        private ImageBox imageBox1;

    }
}