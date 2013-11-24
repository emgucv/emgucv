namespace Emgu.CV.UI.GLView
{
   partial class GLImageViewer
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
         this.glImageView = new Emgu.CV.UI.GLView.GLImageView();
         this.loadImageButton = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // glImageView
         // 
         this.glImageView.BackColor = System.Drawing.Color.Black;
         this.glImageView.Dock = System.Windows.Forms.DockStyle.Fill;
         this.glImageView.GridLines = 0;
         this.glImageView.Location = new System.Drawing.Point(0, 0);
         this.glImageView.Name = "glImageView";
         this.glImageView.Rectangles = null;
         this.glImageView.Rotation = 0F;
         this.glImageView.Size = new System.Drawing.Size(754, 493);
         this.glImageView.TabIndex = 0;
         this.glImageView.VSync = false;
         // 
         // loadImageButton
         // 
         this.loadImageButton.Location = new System.Drawing.Point(12, 12);
         this.loadImageButton.Name = "loadImageButton";
         this.loadImageButton.Size = new System.Drawing.Size(75, 23);
         this.loadImageButton.TabIndex = 1;
         this.loadImageButton.Text = "Load Image";
         this.loadImageButton.UseVisualStyleBackColor = true;
         this.loadImageButton.Click += new System.EventHandler(this.loadImageButton_Click);
         // 
         // GLImageViewer
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(754, 493);
         this.Controls.Add(this.loadImageButton);
         this.Controls.Add(this.glImageView);
         this.Name = "GLImageViewer";
         this.Text = "GLImageViewer";
         this.ResumeLayout(false);

      }

      #endregion

      private GLImageView glImageView;
      private System.Windows.Forms.Button loadImageButton;
   }
}