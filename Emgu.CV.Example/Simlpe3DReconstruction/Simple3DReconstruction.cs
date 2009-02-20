using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Tao.OpenGl;

namespace Simlpe3DReconstruction
{
   public partial class Simple3DReconstruction : Form
   {
      private MCvPoint3D32f[] _points;
      private Image<Gray, Byte> left;
      private double _angle = 0.0;

      public Simple3DReconstruction()
      {
         InitializeComponent();

         left = new Image<Gray, byte>("left.jpg");
         Image<Gray, Byte> right = new Image<Gray, byte>("right.jpg");
         Size size = left.Size;
         Image<Gray, Int16> leftDisparity = new Image<Gray, Int16>(size);
         Image<Gray, Int16> rightDisparity = new Image<Gray, Int16>(size);

         StereoGC gc = new StereoGC(10, 5);
         gc.FindStereoCorrespondence(left, right, leftDisparity, rightDisparity);
         leftDisparity._Mul(-16);

         Matrix<double> q = new Matrix<double>(4, 4);
         q.SetIdentity();

         _points = PointCollection.ReprojectImageTo3D(leftDisparity, q);
         float scale = Math.Max(size.Width, size.Height);
         for (int i = 0; i < _points.Length; i++)
         {
            _points[i].x /= scale;
            _points[i].y /= scale;
            _points[i].z /= scale;
         }

         simpleOpenGlControl1.InitializeContexts();
         Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
         Gl.glMatrixMode(Gl.GL_PROJECTION);
         Gl.glLoadIdentity();
         
         Gl.glOrtho(0, 1.0, 0.0, 1.0, -1.0, 1.0);
      }

      private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
      {
         Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

         Gl.glPushMatrix();
         Gl.glTranslatef(0.5f, 0.0f, 0.0f);

         {
            Gl.glPushMatrix();
            Gl.glTranslatef(-0.5f, 0.0f, 0.0f);
            Gl.glRotated(_angle, 0.0, 1.0, 0.0);

            //Gl.glColor3b(255, 255, 255);

            foreach (MCvPoint3D32f p in _points)
            {
               //float color = left.Data[(int)p.y, (int)p.x, 0];
               //Gl.glColor3f( color / 127f + 0.5f, color/127f + 0.5f, color/127f + 0.5f);
               Gl.glColor3f(1.0f, 1.0f, 1.0f);
               Gl.glBegin(Gl.GL_POINTS);
               Gl.glVertex3f(p.x, p.y, p.z);
               Gl.glEnd();
            }

            Gl.glPopMatrix();
         }

         Gl.glPopMatrix();
         Gl.glFlush();

         _angle += 0.5;
         simpleOpenGlControl1.Invalidate();
      }

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            simpleOpenGlControl1.DestroyContexts();
            components.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}