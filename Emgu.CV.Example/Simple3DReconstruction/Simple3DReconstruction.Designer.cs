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
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.imageBox1 = new Emgu.CV.UI.ImageBox();
         this.View3DGlControl = new OpenTK.GLControl();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
         this.SuspendLayout();
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
         this.splitContainer1.Panel2.Controls.Add(this.View3DGlControl);
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
         // View3DGlControl
         // 
         this.View3DGlControl.BackColor = System.Drawing.Color.Black;
         this.View3DGlControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.View3DGlControl.Location = new System.Drawing.Point(0, 0);
         this.View3DGlControl.Name = "View3DGlControl";
         this.View3DGlControl.Size = new System.Drawing.Size(367, 309);
         this.View3DGlControl.TabIndex = 0;
         this.View3DGlControl.VSync = false;
         this.View3DGlControl.Load += new System.EventHandler(this.View3DGlControl_Load);
         this.View3DGlControl.Paint += new System.Windows.Forms.PaintEventHandler(this.View3DGlControl_Paint);
         this.View3DGlControl.Resize += new System.EventHandler(this.View3DGlControl_Resize);
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

      //private OsgControl viewer3D;
      private System.Windows.Forms.SplitContainer splitContainer1;
      private Emgu.CV.UI.ImageBox imageBox1;
      private OpenTK.GLControl View3DGlControl;
   }
}

