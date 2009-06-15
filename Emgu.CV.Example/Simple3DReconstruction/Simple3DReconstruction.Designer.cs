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
         
         this.viewer3D = new OsgControl();

         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.imageBox1 = new Emgu.CV.UI.ImageBox();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
         this.SuspendLayout();
         
         // 
         // simpleOsgControl
         // 
         this.viewer3D.AccumBits = ((byte)(0));
         this.viewer3D.AutoCheckErrors = false;
         this.viewer3D.AutoFinish = false;
         this.viewer3D.AutoMakeCurrent = true;
         this.viewer3D.AutoSwapBuffers = true;
         this.viewer3D.BackColor = System.Drawing.Color.Black;
         this.viewer3D.ColorBits = ((byte)(32));
         this.viewer3D.DepthBits = ((byte)(16));
         this.viewer3D.Dock = System.Windows.Forms.DockStyle.Fill;
         this.viewer3D.Location = new System.Drawing.Point(0, 0);
         this.viewer3D.Name = "simpleOpenGlControl1";
         this.viewer3D.Size = new System.Drawing.Size(367, 309);
         this.viewer3D.StencilBits = ((byte)(0));
         this.viewer3D.TabIndex = 0;
         this.viewer3D.Paint += new System.Windows.Forms.PaintEventHandler(this.viewer3D_Paint);
         
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
         this.splitContainer1.Panel2.Controls.Add(this.viewer3D);
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

      private OsgControl viewer3D;
      private System.Windows.Forms.SplitContainer splitContainer1;
      private Emgu.CV.UI.ImageBox imageBox1;
   }
}

