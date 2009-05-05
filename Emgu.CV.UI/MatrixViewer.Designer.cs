namespace Emgu.CV.UI
{
   partial class MatrixViewer
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
         this.matrixBox = new MatrixBox();
         this.SuspendLayout();
         // 
         // matrixBox
         // 
         this.matrixBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.matrixBox.Location = new System.Drawing.Point(0, 0);
         this.matrixBox.Matrix = null;
         this.matrixBox.Name = "matrixBox";
         this.matrixBox.Size = new System.Drawing.Size(292, 266);
         this.matrixBox.TabIndex = 0;
         // 
         // MatrixViewer
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(292, 266);
         this.Controls.Add(this.matrixBox);
         this.Name = "MatrixViewer";
         this.Text = "MatrixViewer";
         this.ResumeLayout(false);

      }

      private MatrixBox matrixBox;
      #endregion
   }
}