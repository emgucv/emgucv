namespace Emgu.CV.UI
{
    partial class PropertyDlg
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
            this.imageProperty1 = new Emgu.CV.UI.ImageProperty();
            this.SuspendLayout();
            // 
            // imageProperty1
            // 
            this.imageProperty1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageProperty1.Location = new System.Drawing.Point(0, 0);
            this.imageProperty1.Name = "imageProperty1";
            this.imageProperty1.Size = new System.Drawing.Size(256, 266);
            this.imageProperty1.TabIndex = 0;
            // 
            // PropertyDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 266);
            this.Controls.Add(this.imageProperty1);
            this.Name = "PropertyDlg";
            this.Text = "PropertyDlg";
            this.ResumeLayout(false);

        }

        #endregion

        private ImageProperty imageProperty1;
    }
}