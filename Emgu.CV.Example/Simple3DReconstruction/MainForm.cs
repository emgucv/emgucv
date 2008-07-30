using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace Simple3DReconstruction
{
    public partial class Form1 : Form
    {
        private List<Triangle3D<float>> triangles = new List<Triangle3D<float>>();
        private Image<Bgr, Byte> texture;
        private float angle = 0.0f;

        public Form1()
        {
            InitializeComponent();

            Image<Bgr, Byte> leftImage = new Image<Bgr, byte>("left.png");
            Image<Bgr, Byte> rightImage = new Image<Bgr, byte>("right.png");
            Image<Gray, Byte> disparity = Utils.FindStereoCorrespondence(leftImage.Convert<Gray, Byte>(), rightImage.Convert<Gray, Byte>(), 200);

            texture = leftImage.Resize(512, 512).Rotate(90, new Bgr(0.0, 0.0, 0.0));
            
            imageBox1.Image = 255.0 - disparity.Resize(200, 200, true);

            //imageBox1.Image = rightImage.Resize(100, 100, true).Convert<Gray, Byte>();

            Emgu.Utils.Func<int, int, Point3D<float>> GetPoint =
                delegate(int row, int col)
                {
                    float r = ((float)row) / disparity.Rows - 0.5f;
                    float c = ((float)col) / disparity.Cols - 0.5f;
                    return new Point3D<float>(r, c, ((float)disparity[row, col].Intensity / 255.0f) * 3.0f);
                };

            for (int i = 0; i < disparity.Rows - 1; i++)
                for (int j = 0; j < disparity.Cols - 1; j++)
                {
                    Point3D<float> p0 = GetPoint(i, j);
                    Point3D<float> p1 = GetPoint(i, j + 1);
                    Point3D<float> p2 = GetPoint(i + 1, j + 1);
                    Point3D<float> p3 = GetPoint(i + 1, j);
                    triangles.Add(new Triangle3D<float>(p0, p1, p2));
                    triangles.Add(new Triangle3D<float>(p0, p2, p3));
                }

            simpleOpenGlControl1.InitializeContexts();

            #region Setup OpenGL enviorment
            //Gl.glEnable(Gl.GL_DEPTH_TEST);
            //Gl.glDepthFunc(Gl.GL_LEQUAL);
            
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, texture.Width, texture.Height, 0, Gl.GL_BGR_EXT, Gl.GL_UNSIGNED_BYTE, texture.MIplImage.imageData);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER,
                    Gl.GL_NEAREST);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER,
                    Gl.GL_NEAREST);

            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);

            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glShadeModel(Gl.GL_FLAT);

            Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glOrtho(-0.5, 0.5, -0.5, 0.5, -2.0, 3.0);
            #endregion
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                simpleOpenGlControl1.DestroyContexts();
            }
            base.Dispose(disposing);
        }

        private void Gl_Paint()
        {
            //Force the OpenGL Control to repaint itself. Make the current function similar to mainloop in Glut
            simpleOpenGlControl1.Invalidate();

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            Gl.glColor3f(0.5f, 0.5f, 0.5f);
            Gl.glPushMatrix();
            Gl.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
            angle += 10.0f;
            Gl.glRotatef(angle, 1.0f, 0.0f, 0.0f);

            Gl.glBegin(Gl.GL_TRIANGLES);
            foreach (Triangle3D<float> t in triangles)
            {
                foreach (Point3D<float> p in t.Vertices)
                {
                    float[] pV = p.Coordinate;
                    Gl.glTexCoord2f(pV[0]+0.5f, pV[1]+0.5f);
                    Gl.glVertex3f(pV[0], pV[1], pV[2]);
                }
            }
            Gl.glEnd();

            Gl.glPopMatrix();
            Gl.glFlush();
        }

        private void GlControl_Paint(object sender, System.Windows.Forms.PaintEventArgs args)
        {
            Gl_Paint();
        }

    }
}