//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Xml.Serialization;
using System.IO;

namespace ImageDatabase
{
   public class PersistentImage : Image<Bgr, Byte>
   {
      private int _id;
      private DateTime _dateCreated;
      private static XmlSerializer _serializer = (new XmlSerializer(typeof(Image<Bgr, Byte>)));

      private PersistentImage()
      {
      }

      public PersistentImage(int width, int height)
         :base(width, height)
      {
         _dateCreated = DateTime.Now;
      }

      public virtual DateTime DateCreated
      {
         get
         {
            return _dateCreated;
         }
         set
         {
            _dateCreated = value;
         }
      }

      public virtual int Id
      {
         get { return _id; }
         set { _id = value; }
      }

      public String XmlImage
      {
         get
         {
            StringBuilder sb = new StringBuilder();
            _serializer.Serialize(new StringWriter(sb), this);
            return sb.ToString();
         }
         set
         {
            using (StringReader reader = new StringReader(value))
            {
               Image<Bgr, Byte> img = (Image<Bgr, Byte>)_serializer.Deserialize(reader);
               AllocateData(img.Rows, img.Cols, img.NumberOfChannels);
               CvInvoke.cvCopy(img.Ptr, this.Ptr, IntPtr.Zero);
            }
         }
      }
   }

}

