//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using Emgu.Util;

namespace Emgu.Models
{
    /// <summary>
    /// This represent a file that can be downloaded from the internet
    /// </summary>
    public class DownloadableFile
    {
        private String _url;

        /// <summary>
        /// Create a downloadable file from the url
        /// </summary>
        /// <param name="url">The url where the file can be downloaded from</param>
        public DownloadableFile(String url)
        {
            _url = url;
        }

        private String _localFile = null;

        /// <summary>
        /// The url where this file can be downloaded from
        /// </summary>
        public String Url
        {
            get { return _url; }
        }

        /// <summary>
        /// The local file name
        /// </summary>
        public String LocalFile
        {
            get
            {
                if (_localFile != null)
                    return _localFile;
                else if (Url == null)
                    return null;
                else
                {
                    Uri uri = new Uri(Url);

                    string fileName = System.IO.Path.GetFileName(uri.LocalPath);
                    return GetLocalFileName(fileName);

                }
            }
            set
            {
                _localFile = value;
            }
        }

        /// <summary>
        /// The local path to the local file given the file name
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>The local path of the file</returns>
        public static String GetLocalFileName(String fileName)
        {
#if  UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE
            return System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, fileName);
#else
            if ( Emgu.Util.Platform.OperationSystem == Platform.OS.Android || 
                 Emgu.Util.Platform.OperationSystem == Platform.OS.IOS)
            { 
                String personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                return Path.Combine(personalFolder, fileName);
            }

            return fileName;
#endif
        }
    }

}
