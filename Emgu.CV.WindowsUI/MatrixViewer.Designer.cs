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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatrixViewer));
         this.matrixBox = new Emgu.CV.UI.MatrixBox();
         this.SuspendLayout();
         // 
         // matrixBox
         // 
         this.matrixBox.AccessibleDescription = null;
         this.matrixBox.AccessibleName = null;
         resources.ApplyResources(this.matrixBox, "matrixBox");
         this.matrixBox.BackgroundImage = null;
         this.matrixBox.Font = null;
         this.matrixBox.Matrix = null;
         this.matrixBox.Name = "matrixBox";
         // 
         // MatrixViewer
         // 
         this.AccessibleDescription = null;
         this.AccessibleName = null;
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackgroundImage = null;
         this.Controls.Add(this.matrixBox);
         this.Font = null;
         this.Icon = null;
         this.Name = "MatrixViewer";
         this.ResumeLayout(false);

      }

      private MatrixBox matrixBox;
      #endregion
   }
}