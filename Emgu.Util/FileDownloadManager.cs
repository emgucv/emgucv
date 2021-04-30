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

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
using UnityEngine;
#endif

namespace Emgu.Util
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
        /// <param name="localSubfolder">The local subfolder name to download the model to.</param>
        /// <param name="sha256Hash">The sha256 hash value for the file</param>
        public void AddFile(String url, String localSubfolder, String sha256Hash = null)
        {
            _files.Add(new DownloadableFile(url, localSubfolder, sha256Hash));
        }

        /// <summary>
        /// Add a file to download
        /// </summary>
        /// <param name="downloadableFile">The file to be downloaded</param>
        public void AddFile(DownloadableFile downloadableFile)
        {
            _files.Add(downloadableFile);
        }

        /// <summary>
        /// Return true if all files has been downloaded and are valid.
        /// </summary>
        public bool AllFilesDownloaded
        {
            get
            {
                bool allDownloaded = true;
                foreach (DownloadableFile file in _files)
                {
                    allDownloaded &= file.IsLocalFileValid;
                }

                return allDownloaded;
            }
        }

        /// <summary>
        /// Get the files that will be downloaded by this download manager.
        /// </summary>
        public DownloadableFile[] Files
        {
            get
            {
                return _files.ToArray();
            }
        }

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public UnityEngine.Networking.UnityWebRequest CurrentWebClient = null;

        public IEnumerator Download()
        {
            foreach (DownloadableFile df in _files)
            {
                //Uncomment the following to force re-download every time
                //File.Delete(localFileName);
                if (!df.IsLocalFileValid)
                {
                    String localFileName = df.LocalFile;
                    using (UnityEngine.Networking.UnityWebRequest webclient = new UnityEngine.Networking.UnityWebRequest(df.Url))
                    {
                        CurrentWebClient = webclient;
                        UnityEngine.Debug.Log(String.Format("Downloading file from '{0}' to '{1}'", df.Url, localFileName));

                        webclient.downloadHandler = new UnityEngine.Networking.DownloadHandlerFile(localFileName);
                        yield return webclient.SendWebRequest();
                        if (webclient.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError 
                            || webclient.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
                        {
                            UnityEngine.Debug.LogError(webclient.error);
                        }

                        if (df.IsLocalFileValid)
                        {
                            UnityEngine.Debug.Log("File successfully downloaded and saved to " + localFileName);
                        }
                        else
                        {
                            UnityEngine.Debug.LogError(String.Format("Failed to download file {0}.", localFileName));
                        }
                        CurrentWebClient = null;

                    }
                }
            }

            UnityEngine.Debug.Log("All download completed.");
        }
#else
        /// <summary>
        /// Download the files. 
        /// </summary>
        /// <param name="retry">The number of retries.</param>
        /// <returns>The async Task</returns>
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
            if (!downloadableFile.IsLocalFileValid)
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
                    if (!downloadableFile.IsLocalFileValid)
                    {
                        //The downloaded file may be corrupted, should delete it
                        File.Delete(downloadableFile.LocalFile);
                    }

                    if (retry > 0)
                    {
                        await DownloadHelper( downloadableFile,  retry - 1);
                    }
                    else
                    {
                        Trace.WriteLine(e);
                        throw;
                    }
                }
            } 
        }
#endif
    }

}
