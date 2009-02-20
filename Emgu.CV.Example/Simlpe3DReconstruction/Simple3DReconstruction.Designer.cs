namespace Simlpe3DReconstruction
{
   partial class Simple3DReconstruction
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.simpleOpenGlControl1 = new Tao.Platform.Windows.SimpleOpenGlControl();
         this.SuspendLayout();
         // 
         // simpleOpenGlControl1
         // 
         this.simpleOpenGlControl1.AccumBits = ((byte)(0));
         this.simpleOpenGlControl1.AutoCheckErrors = false;
         this.simpleOpenGlControl1.AutoFinish = false;
         this.simpleOpenGlControl1.AutoMakeCurrent = true;
         this.simpleOpenGlControl1.AutoSwapBuffers = true;
         this.simpleOpenGlControl1.BackColor = System.Drawing.Color.Black;
         this.simpleOpenGlControl1.ColorBits = ((byte)(32));
         this.simpleOpenGlControl1.DepthBits = ((byte)(16));
         this.simpleOpenGlControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.simpleOpenGlControl1.Location = new System.Drawing.Point(0, 0);
         this.simpleOpenGlControl1.Name = "simpleOpenGlControl1";
         this.simpleOpenGlControl1.Size = new System.Drawing.Size(292, 266);
         this.simpleOpenGlControl1.StencilBits = ((byte)(0));
         this.simpleOpenGlControl1.TabIndex = 0;
         this.simpleOpenGlControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.simpleOpenGlControl1_Paint);
         // 
         // Simple3DReconstruction
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(292, 266);
         this.Controls.Add(this.simpleOpenGlControl1);
         this.Name = "Simple3DReconstruction";
         this.Text = "Form1";
         this.ResumeLayout(false);

      }

      #endregion

      private Tao.Platform.Windows.SimpleOpenGlControl simpleOpenGlControl1;
   }
}

