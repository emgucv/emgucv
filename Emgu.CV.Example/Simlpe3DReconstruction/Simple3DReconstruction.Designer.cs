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
         this.simpleOsgControl = new OsgControl();
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
         this.simpleOsgControl.AccumBits = ((byte)(0));
         this.simpleOsgControl.AutoCheckErrors = false;
         this.simpleOsgControl.AutoFinish = false;
         this.simpleOsgControl.AutoMakeCurrent = true;
         this.simpleOsgControl.AutoSwapBuffers = true;
         this.simpleOsgControl.BackColor = System.Drawing.Color.Black;
         this.simpleOsgControl.ColorBits = ((byte)(32));
         this.simpleOsgControl.DepthBits = ((byte)(16));
         this.simpleOsgControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.simpleOsgControl.Location = new System.Drawing.Point(0, 0);
         this.simpleOsgControl.Name = "simpleOpenGlControl1";
         this.simpleOsgControl.Size = new System.Drawing.Size(367, 309);
         this.simpleOsgControl.StencilBits = ((byte)(0));
         this.simpleOsgControl.TabIndex = 0;
         this.simpleOsgControl.Paint += new System.Windows.Forms.PaintEventHandler(this.simpleOpenGlControl1_Paint);
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
         this.splitContainer1.Panel2.Controls.Add(this.simpleOsgControl);
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

      private OsgControl simpleOsgControl;
      private System.Windows.Forms.SplitContainer splitContainer1;
      private Emgu.CV.UI.ImageBox imageBox1;
   }
}

