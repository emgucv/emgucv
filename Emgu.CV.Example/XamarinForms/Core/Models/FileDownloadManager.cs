//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE
using UnityEngine;
#endif

namespace Emgu.Models
{
    /// <summary>
    /// Use to download files (e.g. models) from the internet
    /// </summary>
    public class FileDownloadManager
    {
        /// <summary>
        /// Create a file download manager
        /// </summary>
        public FileDownloadManager()
        {
        }

        /// <summary>
        /// This event will be fired when the download progress is changed
        /// </summary>
        public event System.Net.DownloadProgressChangedEventHandler OnDownloadProgressChanged;

        private List<DownloadableFile> _files = new List<DownloadableFile>();
        
        /// <summary>
        /// Clear the list of files
        /// </summary>
        public void Clear()
        {
            _files.Clear();
        }

        /// <summary>
        /// Add a file to download
        /// </summary>
        /// <param name="url">The url of the file to be downloaded</param>
        public void AddFile(String url, String localSubfolder)
        {
            _files.Add(new DownloadableFile(url, localSubfolder));
        }

        public DownloadableFile[] Files
        {
            get
            {
                return _files.ToArray();
            }
        }

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE
        public UnityEngine.Networking.UnityWebRequest CurrentWebClient = null;

        public IEnumerator Download()
        {
            foreach (DownloadableFile df in _files)
            {
                String localFileName = df.LocalFile;

                //Uncomment the following to force redownload every time
                //File.Delete(localFileName);
                if (!System.IO.File.Exists(localFileName) || !(new FileInfo(localFileName).Length > 0))
                {
                    using (UnityEngine.Networking.UnityWebRequest webclient = new UnityEngine.Networking.UnityWebRequest(df.Url))
                    {
                        CurrentWebClient = webclient;
                        UnityEngine.Debug.Log(String.Format("Downloading file from '{0}' to '{1}'", df.Url, localFileName));

                        webclient.downloadHandler = new UnityEngine.Networking.DownloadHandlerFile(localFileName);
                        yield return webclient.SendWebRequest();
                        if (webclient.isNetworkError || webclient.isHttpError)
                        {
                            UnityEngine.Debug.LogError(webclient.error);
                        }

                        if (!System.IO.File.Exists(localFileName) || !(new FileInfo(localFileName).Length > 0))
                        {
                            UnityEngine.Debug.LogError(String.Format("File {0} is empty, failed to download file.", localFileName));
                        }

                        UnityEngine.Debug.Log("File successfully downloaded and saved to " + localFileName);
                        CurrentWebClient = null;

                    }
                }
            }
            if (OnDownloadCompleted != null)
            {
                UnityEngine.Debug.Log("All download completed.");
                OnDownloadCompleted(this, null);
            }
        }
#else
        public async Task Download(int retry = 1)
        {
            await Download( _files.ToArray(), retry, this.OnDownloadProgressChanged);
        }

        private static async Task Download(
            DownloadableFile[] files,
            int retry = 1,
            System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            await DownloadHelperMultiple(files, retry, onDownloadProgressChanged);
        }

        private static async Task DownloadHelperMultiple(
            DownloadableFile[] downloadableFiles,
            int retry = 1, 
            System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (downloadableFiles == null || downloadableFiles.Length == 0)
            {
                return;
            } else if (downloadableFiles.Length == 1)
            {
                await DownloadHelper(downloadableFiles[0], retry, onDownloadProgressChanged);
            } else
            {
                DownloadableFile currentFile = downloadableFiles[0];
                DownloadableFile[] remainingFiles = new DownloadableFile[downloadableFiles.Length - 1];
                Array.Copy(downloadableFiles, 1, remainingFiles, 0, remainingFiles.Length);
                await DownloadHelper(currentFile, retry, onDownloadProgressChanged);
                    
                await DownloadHelperMultiple(remainingFiles, retry, onDownloadProgressChanged);
                    
            }
        }

        private static async Task DownloadHelper(
            DownloadableFile downloadableFile,
            int retry = 1, 
            System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null
            )
        {
            if (downloadableFile.Url == null)
                return;

            //uncomment the following line to force re-download every time.
            //File.Delete(downloadableFile.LocalFile);
            if (!File.Exists(downloadableFile.LocalFile) || new FileInfo(downloadableFile.LocalFile).Length == 0)
            {
                try
                {
                    //Download the file
                    Trace.WriteLine("downloading file from:" + downloadableFile.Url + " to: " + downloadableFile.LocalFile);
                    System.Net.WebClient downloadClient = new System.Net.WebClient();
                    
                    if (onDownloadProgressChanged != null)
                        downloadClient.DownloadProgressChanged += onDownloadProgressChanged;
                    
                    FileInfo fi = new FileInfo(downloadableFile.LocalFile);
                    if (!fi.Directory.Exists)
                    {
                        fi.Directory.Create();
                    }
                    await downloadClient.DownloadFileTaskAsync(new Uri(downloadableFile.Url), downloadableFile.LocalFile);
                    
                }
                catch (Exception e)
                {
                    if (File.Exists(downloadableFile.LocalFile))
                        //The downloaded file may be corrupted, should delete it
                        File.Delete(downloadableFile.LocalFile);

                    if (retry > 0)
                    {
                        await DownloadHelper( downloadableFile,  retry - 1);
                    }
                    else
                    {
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE
                        UnityEngine.Debug.Log(e.StackTrace);
#else
                        Trace.WriteLine(e);
#endif
                        throw;
                    }
                }
            } 
        }
#endif
    }

}
