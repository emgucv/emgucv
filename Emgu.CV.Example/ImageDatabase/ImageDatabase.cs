//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ImageDatabase
{
   public sealed class ImageDatabase
   {
      //private const string CurrentSessionKey = "nhibernate.current_session";
      private static readonly ISessionFactory sessionFactory;
      private static ISession _currentSession;

      static ImageDatabase()
      {
         Configuration cfg = new Configuration().Configure("SqliteDB.XML");

         String dbFileName = "test.db";
         cfg.Properties["connection.connection_string"] = String.Format("Data Source={0};Version=3", dbFileName);
         cfg.AddAssembly(typeof(ImageDatabase).Assembly);

         //Create the table if this is a new database
         if (!System.IO.File.Exists(dbFileName))
            new SchemaExport(cfg).Execute(false, true, false);

         sessionFactory = cfg.BuildSessionFactory();
      }

      public static ISession GetCurrentSession()
      {
         if (_currentSession == null || !_currentSession.IsOpen )
            _currentSession = sessionFactory.OpenSession();
         return _currentSession;
      }

      public static void CloseSession()
      {
         if (_currentSession == null)
         {
            // No current session
            return;
         }

         _currentSession.Close();
         _currentSession = null;
      }

      public static void CloseSessionFactory()
      {
         if (sessionFactory != null)
         {
            sessionFactory.Close();
         }
      }
   }
}
