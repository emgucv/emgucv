namespace Emgu.CV.UI
{
    partial class PropertyDialog
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
           System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyDialog));
           this.imageProperty1 = new Emgu.CV.UI.ImageProperty();
           this.SuspendLayout();
           // 
           // imageProperty1
           // 
           this.imageProperty1.AccessibleDescription = null;
           this.imageProperty1.AccessibleName = null;
           resources.ApplyResources(this.imageProperty1, "imageProperty1");
           this.imageProperty1.BackgroundImage = null;
           this.imageProperty1.Font = null;
           this.imageProperty1.ImageBox = null;
           this.imageProperty1.Name = "imageProperty1";
           // 
           // PropertyDialog
           // 
           this.AccessibleDescription = null;
           this.AccessibleName = null;
           resources.ApplyResources(this, "$this");
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.BackgroundImage = null;
           this.Controls.Add(this.imageProperty1);
           this.Font = null;
           this.Icon = null;
           this.Name = "PropertyDialog";
           this.ResumeLayout(false);

        }

        #endregion

        private ImageProperty imageProperty1;
    }
}