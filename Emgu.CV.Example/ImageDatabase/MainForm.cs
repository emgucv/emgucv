//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.CvEnum;
using NHibernate;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Linq;
using Emgu.CV.UI;
using System.IO;

namespace ImageDatabase
{
   public partial class MainForm : Form
   {
      public MainForm()
      {
         InitializeComponent();
         RefreshGrid();
         //AddImagesToDatabase();
         //IList<PersistentImage<Bgr, Byte>> images = GetImages();
         //int count = Enumerable.Count < PersistentImage<Bgr, Byte> > (images);
      }

      public void AddImagesToDatabase(int count)
      {
         ISession session = ImageDatabase.GetCurrentSession();
         ITransaction tx = session.BeginTransaction();

         long tick = DateTime.Now.Ticks;
         for (int i = 0; i < count; i++, tick++)
         {
            PersistentImage image = new PersistentImage(400, 200);
            image.SetRandUniform(new MCvScalar(), new MCvScalar(50, 50, 50));
            
            image.Draw(tick.ToString(), new Point(10, 100), FontFace.HersheySimplex, 1.0, new Bgr(Color.White));

            image.SerializationCompressionRatio = 9;
            session.Save(image);
         }

         tx.Commit();
         session.Close();
      }

      public IList<PersistentImage> GetImages()
      {
         IList<PersistentImage> images;
         ISession session = ImageDatabase.GetCurrentSession();
         images = session.CreateCriteria(typeof(PersistentImage)).List<PersistentImage>();
         session.Close();
         return images;
      }

      private void RefreshGrid()
      {
         dataGridView1.Rows.Clear();
         IList<PersistentImage> images = GetImages();
         if (images.Count > 0)
         {
            dataGridView1.Rows.Add(images.Count);
            for (int i = 0; i < images.Count; i++)
            {
               DataGridViewRow row = dataGridView1.Rows[i];
               row.Cells["idColumn"].Value = images[i].Id;
               row.Cells["timeColumn"].Value = images[i].DateCreated;
               row.Cells["viewColumn"].Value = "View Image";
            }
         }
      }

      private void addTenImages_Click(object sender, EventArgs e)
      {
         AddImagesToDatabase(10);
         RefreshGrid();
      }

      private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
      {
         if (e.ColumnIndex == dataGridView1.Columns["viewColumn"].Index)
         {
            int imageID = (int)dataGridView1.Rows[e.RowIndex].Cells["idColumn"].Value;
            ISession session = ImageDatabase.GetCurrentSession();
            Image<Bgr, Byte> image = session.Load<PersistentImage>(imageID);
            session.Close();
            using (ImageViewer viewer = new ImageViewer())
            {
               viewer.Image = image;
               viewer.ShowDialog();
            }
         }
      }

      private void ClearDatabase()
      {
         ISession session = ImageDatabase.GetCurrentSession();
         ITransaction tx = session.BeginTransaction();
         session.CreateSQLQuery("delete from Images").ExecuteUpdate();
         tx.Commit();
         session.Close();
      }

      private void addHundredImages_Click(object sender, EventArgs e)
      {
         AddImagesToDatabase(100);
         RefreshGrid();
      }

      private void clearDatabase_Click(object sender, EventArgs e)
      {
         ClearDatabase();
         RefreshGrid();
      }

      private void addImagesFromFiles_Click(object sender, EventArgs e)
      {
         if (openFileDialog1.ShowDialog() == DialogResult.OK)
         {
            ISession session = ImageDatabase.GetCurrentSession();
            ITransaction tx = session.BeginTransaction();
            foreach (String file in openFileDialog1.FileNames)
            {
               try
               {
                  FileInfo fi = new FileInfo(file);
                  if (fi.Exists)
                  using (Image<Bgr, Byte> image = new Image<Bgr, byte>(file))
                  {
                     PersistentImage pImage = new PersistentImage(image.Width, image.Height);
                     CvInvoke.cvCopy(image, pImage, IntPtr.Zero);
                     pImage.DateCreated = fi.CreationTime;
                     session.Save(pImage);
                  }
               }
               catch
               {
               }
            }
            tx.Commit();
            session.Close();
         }
         RefreshGrid();
      }
   }
}