namespace Emgu.CV.UI
{
    partial class HistogramViewer
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
           System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistogramViewer));
           this.histogramCtrl1 = new Emgu.CV.UI.HistogramBox();
           this.SuspendLayout();
           // 
           // histogramCtrl1
           // 
           this.histogramCtrl1.AccessibleDescription = null;
           this.histogramCtrl1.AccessibleName = null;
           resources.ApplyResources(this.histogramCtrl1, "histogramCtrl1");
           this.histogramCtrl1.BackgroundImage = null;
           this.histogramCtrl1.Font = null;
           this.histogramCtrl1.Name = "histogramCtrl1";
           // 
           // HistogramViewer
           // 
           this.AccessibleDescription = null;
           this.AccessibleName = null;
           resources.ApplyResources(this, "$this");
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.BackgroundImage = null;
           this.Controls.Add(this.histogramCtrl1);
           this.Font = null;
           this.Icon = null;
           this.Name = "HistogramViewer";
           this.ResumeLayout(false);

        }

        #endregion

        private HistogramBox histogramCtrl1;
    }
}
