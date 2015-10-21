namespace Emgu.UI
{
    /// <summary>
    /// A control to display progress status
    /// </summary>
    partial class StatusField
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         this.label1 = new System.Windows.Forms.Label();
         this.Progressbar1 = new System.Windows.Forms.ProgressBar();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.label1.Location = new System.Drawing.Point(0, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(96, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "unknown               ";
         // 
         // Progressbar1
         // 
         this.Progressbar1.BackColor = System.Drawing.SystemColors.ActiveBorder;
         this.Progressbar1.ForeColor = System.Drawing.Color.RoyalBlue;
         this.Progressbar1.Location = new System.Drawing.Point(3, 18);
         this.Progressbar1.Name = "Progressbar1";
         this.Progressbar1.Size = new System.Drawing.Size(100, 14);
         this.Progressbar1.Step = 5;
         this.Progressbar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
         this.Progressbar1.TabIndex = 59;
         // 
         // StatusField
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.Progressbar1);
         this.Controls.Add(this.label1);
         this.Name = "StatusField";
         this.Size = new System.Drawing.Size(107, 35);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar Progressbar1;
    }
}
