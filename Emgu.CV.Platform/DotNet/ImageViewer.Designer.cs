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
           System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageViewer));
           this.imageBox1 = new Emgu.CV.UI.ImageBox();
           ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
           this.SuspendLayout();
           // 
           // imageBox1
           // 
           this.imageBox1.AccessibleDescription = null;
           this.imageBox1.AccessibleName = null;
           resources.ApplyResources(this.imageBox1, "imageBox1");
           this.imageBox1.BackgroundImage = null;
           this.imageBox1.Cursor = System.Windows.Forms.Cursors.Cross;
           this.imageBox1.Font = null;
           this.imageBox1.ImageLocation = null;
           this.imageBox1.Name = "imageBox1";
           this.imageBox1.TabStop = false;
           // 
           // ImageViewer
           // 
           this.AccessibleDescription = null;
           this.AccessibleName = null;
           resources.ApplyResources(this, "$this");
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.BackgroundImage = null;
           this.Controls.Add(this.imageBox1);
           this.Font = null;
           this.Icon = null;
           this.Name = "ImageViewer";
           ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
           this.ResumeLayout(false);

        }

        #endregion

        private ImageBox imageBox1;

    }
}
