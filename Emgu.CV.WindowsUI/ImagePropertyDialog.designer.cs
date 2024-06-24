namespace Emgu.CV.UI
{
    partial class ImagePropertyDialog
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
           System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImagePropertyDialog));
           this.imagePropertyControl = new Emgu.CV.UI.ImageProperty();
           this.SuspendLayout();
           // 
           // imageProperty1
           // 
           resources.ApplyResources(this.imagePropertyControl, "imageProperty1");
           this.imagePropertyControl.ImageBox = null;
           this.imagePropertyControl.Name = "imageProperty1";
           // 
           // PropertyDialog
           // 
           resources.ApplyResources(this, "$this");
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.Controls.Add(this.imagePropertyControl);
           this.Name = "PropertyDialog";
           this.ResumeLayout(false);

        }

        #endregion

        private ImageProperty imagePropertyControl;
    }
}