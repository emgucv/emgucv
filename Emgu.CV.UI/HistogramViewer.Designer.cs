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
           this.histogramCtrl1 = new Emgu.CV.UI.HistogramCtrl();
           this.SuspendLayout();
           // 
           // histogramCtrl1
           // 
           this.histogramCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
           this.histogramCtrl1.Location = new System.Drawing.Point(0, 0);
           this.histogramCtrl1.Name = "histogramCtrl1";
           this.histogramCtrl1.Size = new System.Drawing.Size(372, 284);
           this.histogramCtrl1.TabIndex = 0;
           // 
           // HistogramViewer
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.ClientSize = new System.Drawing.Size(372, 284);
           this.Controls.Add(this.histogramCtrl1);
           this.Name = "HistogramViewer";
           this.Text = "Color Histogram ";
           this.ResumeLayout(false);

        }

        #endregion

        private HistogramCtrl histogramCtrl1;
    }
}