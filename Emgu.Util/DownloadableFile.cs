//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;


namespace Emgu.Util
{
    /// <summary>
    /// This represent a file that can be downloaded from the internet
    /// </summary>
    public class DownloadableFile
    {
        private String _url;
        private String _localSubfolder;
        private String _sha256Hash;
        private String _localFile = null;

        /// <summary>
        /// Create a downloadable file from the url
        /// </summary>
        /// <param name="url">The url where the file can be downloaded from</param>
        /// <param name="localSubfolder">The sub-folder to store the model</param>
        /// <param name="sha256Hash">The SHA256 has for the file.</param>
        public DownloadableFile(String url, String localSubfolder, String sha256Hash = null)
        {
            _url = url;
            _localSubfolder = localSubfolder;
            if (sha256Hash != null)
                _sha256Hash = sha256Hash.ToUpper();
            else
            {
                _sha256Hash = null;
            }
        }

        /// <summary>
        /// The url where this file can be downloaded from
        /// </summary>
        public String Url
        {
            get { return _url; }
        }

        private static String ByteArrayToString(byte[] array)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append($"{array[i]:X2}");
            }
            return sb.ToString();
        }


        /// <summary>
        /// Return true if the local file exist and match the sha256hash (if specified in the constructor).
        /// </summary>
        public bool IsLocalFileValid
        {
            get
            {
                String localFile = LocalFile;
                if (!File.Exists(localFile))
                    return false;

                FileInfo fi = new FileInfo(localFile);
                if (fi.Length == 0)
                    return false;

                if (_sha256Hash != null)
                {
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        try
                        {
                            // Create a fileStream for the file.
                            using (FileStream fileStream = fi.Open(FileMode.Open))
                            {
                                // Be sure it's positioned to the beginning of the stream.
                                fileStream.Position = 0;
                                // Compute the hash of the fileStream.
                                byte[] hashValue = sha256.ComputeHash(fileStream);
                                String hashStr = ByteArrayToString(hashValue);
                                if (hashStr != _sha256Hash)
                                    return false;
                            }
                        }
                        catch (IOException e)
                        {
                            Trace.WriteLine($"I/O Exception: {e.Message}");
                            return false;
                        }
                        catch (UnauthorizedAccessException e)
                        {
                            Trace.WriteLine($"Access Exception: {e.Message}");
                            return false;
                        }
                    }
                }

                return true;
            }

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
        /// Return the directory where the local file is
        /// </summary>
        public String LocalFolder
        {
            get
            {
                String localFile = LocalFile;
                System.IO.FileInfo fi = new FileInfo(localFile);
                return fi.DirectoryName;
            }
        }

        /// <summary>
        /// The local path to the local file given the file name
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>The local path of the file</returns>
        public String GetLocalFileName(String fileName)
        {
#if  UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
            return System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, _localSubfolder, fileName);
#else
            //String personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //String appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            String appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(appDataFolder, _localSubfolder, fileName);
#endif
        }
    }

}
