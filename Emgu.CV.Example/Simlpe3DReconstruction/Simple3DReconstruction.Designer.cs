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
         this.components = new System.ComponentModel.Container();
         this.simpleOpenGlControl1 = new Tao.Platform.Windows.SimpleOpenGlControl();
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.imageBox1 = new Emgu.CV.UI.ImageBox();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
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
         this.simpleOpenGlControl1.Size = new System.Drawing.Size(367, 309);
         this.simpleOpenGlControl1.StencilBits = ((byte)(0));
         this.simpleOpenGlControl1.TabIndex = 0;
         this.simpleOpenGlControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.simpleOpenGlControl1_Paint);
         // 
         // splitContainer1
         // 
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point(0, 0);
         this.splitContainer1.Name = "splitContainer1";
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.imageBox1);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.simpleOpenGlControl1);
         this.splitContainer1.Size = new System.Drawing.Size(791, 309);
         this.splitContainer1.SplitterDistance = 420;
         this.splitContainer1.TabIndex = 1;
         // 
         // imageBox1
         // 
         this.imageBox1.Cursor = System.Windows.Forms.Cursors.Cross;
         this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.imageBox1.Location = new System.Drawing.Point(0, 0);
         this.imageBox1.Name = "imageBox1";
         this.imageBox1.Size = new System.Drawing.Size(420, 309);
         this.imageBox1.TabIndex = 2;
         this.imageBox1.TabStop = false;
         // 
         // Simple3DReconstruction
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(791, 309);
         this.Controls.Add(this.splitContainer1);
         this.Name = "Simple3DReconstruction";
         this.Text = "Form1";
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel2.ResumeLayout(false);
         this.splitContainer1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private Tao.Platform.Windows.SimpleOpenGlControl simpleOpenGlControl1;
      private System.Windows.Forms.SplitContainer splitContainer1;
      private Emgu.CV.UI.ImageBox imageBox1;
   }
}

