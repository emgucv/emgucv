//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;

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
        /// The call back when download progress has been changed.
        /// </summary>
        /// <param name="totalBytesToReceive">The total number of bytes to receive. If null, it is unknown.</param>
        /// <param name="bytesReceived">The total number of bytes currently received.</param>
        /// <param name="progressPercentage">The progress percentage. If null, it is unknown.</param>
        public delegate void DownloadProgressChangedEventHandler(long? totalBytesToReceive, long bytesReceived, double? progressPercentage);

        /// <summary>
        /// This event will be fired when the download progress is changed
        /// </summary>
        public event DownloadProgressChangedEventHandler OnDownloadProgressChanged;

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
            DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            await DownloadHelperMultiple(files, retry, onDownloadProgressChanged);
        }

        private static async Task DownloadHelperMultiple(
            DownloadableFile[] downloadableFiles,
            int retry = 1,
            DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
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

        /// <summary>
        /// An http client with progress based on source code from https://stackoverflow.com/questions/20661652/progress-bar-with-httpclient with modification
        /// </summary>
        public class HttpClientWithProgress : HttpClient
        {
            //private string _downloadUrl;
            //private string _destinationFilePath;

            //private HttpClient _httpClient;

            
            /// <summary>
            /// Handle download download progress change.
            /// </summary>
            public event DownloadProgressChangedEventHandler DownloadProgressChanged;

            /// <summary>
            /// Download file asynchronously
            /// </summary>
            /// <param name="downloadUrl">The download url</param>
            /// <param name="destinationFilePath">The destination file path</param>
            /// <returns>The task</returns>
            public async Task DownloadFileTaskAsync(string downloadUrl, string destinationFilePath)
            {

                //_httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };

                using (var response = await GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                    await DownloadFileFromHttpResponseMessage(response, destinationFilePath);
            }

            private async Task DownloadFileFromHttpResponseMessage(HttpResponseMessage response, string destinationFilePath)
            {
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength;

                using (var contentStream = await response.Content.ReadAsStreamAsync())
                    await ProcessContentStream(totalBytes, contentStream, destinationFilePath);
            }

            private async Task ProcessContentStream(long? totalDownloadSize, Stream contentStream, String destinationFilePath)
            {
                var totalBytesRead = 0L;
                var readCount = 0L;
                var buffer = new byte[8192];
                var isMoreToRead = true;

                using (var fileStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    do
                    {
                        var bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            isMoreToRead = false;
                            TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                            continue;
                        }

                        await fileStream.WriteAsync(buffer, 0, bytesRead);

                        totalBytesRead += bytesRead;
                        readCount += 1;

                        if (readCount % 100 == 0)
                            TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                    }
                    while (isMoreToRead);
                }
            }

            private void TriggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
            {
                if (DownloadProgressChanged == null)
                    return;

                double? progressPercentage = null;
                if (totalDownloadSize.HasValue)
                    progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);

                DownloadProgressChanged(totalDownloadSize, totalBytesRead, progressPercentage);
            }

        }

        private static async Task DownloadHelper(
            DownloadableFile downloadableFile,
            int retry = 1, 
            DownloadProgressChangedEventHandler onDownloadProgressChanged = null
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

                    HttpClientWithProgress downloadClient = new HttpClientWithProgress();
                    if (onDownloadProgressChanged != null)
                        downloadClient.DownloadProgressChanged += onDownloadProgressChanged;
                    
                    FileInfo fi = new FileInfo(downloadableFile.LocalFile);
                    if (!fi.Directory.Exists)
                    {
                        fi.Directory.Create();
                    }
                    await downloadClient.DownloadFileTaskAsync(downloadableFile.Url, downloadableFile.LocalFile);
                    
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
