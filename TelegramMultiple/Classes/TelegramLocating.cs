using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ICSharpCode.SharpZipLib.Zip;

namespace TelegramMultiple.Classes
{
    class TelegramLocating
    {
        private const String TelegramDownloadURL = "https://telegram.org/dl/desktop/win_portable";

        public static String TelegramLocation
        {
            get => Properties.Settings.Default.TelegramLocation;
            set => Properties.Settings.Default.TelegramLocation = value;
        }

        public delegate void OnProgressUpdate(int progress);

        public TelegramLocating()
        {

        }

        public static void DownloadTelegram(String downloadLocation, 
            OnProgressUpdate downloadProgressUpdate = null,
            OnProgressUpdate unzipProgressUpdate = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //Getting actual telegram download link
            Uri downloadUri;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TelegramDownloadURL);
            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                downloadUri = response.ResponseUri;

            //Downloading archive
            using (WebClient client = new WebClient())
            {
                TelegramDownloadCompletedArgs args = new TelegramDownloadCompletedArgs
                {
                    FileName = downloadLocation + "\\cache.zip",
                    DownloadProgressUpdate = downloadProgressUpdate,
                    UnzipProgressUpdate = unzipProgressUpdate
                };

                client.DownloadProgressChanged += (sender, eventArgs)
                    => downloadProgressUpdate?.Invoke(eventArgs.ProgressPercentage);
                client.DownloadFileCompleted += TelegramDownloadCompleted;
                client.DownloadFileAsync(downloadUri, args.FileName, args);
            }
        }
        private struct TelegramDownloadCompletedArgs
        {
            public String FileName;
            public OnProgressUpdate DownloadProgressUpdate;
            public OnProgressUpdate UnzipProgressUpdate;
        }
        private static void TelegramDownloadCompleted(object sender, AsyncCompletedEventArgs args)
        {
            TelegramDownloadCompletedArgs info = (TelegramDownloadCompletedArgs) args.UserState;
            info.DownloadProgressUpdate?.Invoke(100);

            Thread workingThread = new Thread(() => TelegramUnzipping(info));
            workingThread.Start();
        }
        private static void TelegramUnzipping(TelegramDownloadCompletedArgs info)
        {
            //Unzipping downloaded archive
            using (ZipInputStream stream = 
                new ZipInputStream(File.OpenRead(info.FileName)))
            {
                ZipEntry entry;
                bool succesfullyCompleted = false;
                while ((entry = stream.GetNextEntry()) != null)
                {
                    if(!entry.Name.Contains(".exe")) continue;

                    using (FileStream writer = File.Create(
                        Path.GetDirectoryName(info.FileName) + "\\" + Path.GetFileName(entry.Name)))
                    {
                        int bufferSize = 2048, bytesReaded;
                        byte[] buffer = new byte[bufferSize];
                        long FileSize = entry.Size, Progress = 0;
                        while ((bytesReaded = stream.Read(buffer, 0, bufferSize)) > 0)
                        {
                            writer.Write(buffer, 0, bytesReaded);
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                new Action(() => {
                                    info.UnzipProgressUpdate?.Invoke(
                                        (int) ((Progress += bytesReaded) * 100 / FileSize));
                                }));
                        }
                    }

                    succesfullyCompleted = true;
                    break;
                }

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() => {
                        if (succesfullyCompleted) info.UnzipProgressUpdate?.Invoke(100);
                        else info.UnzipProgressUpdate?.Invoke(-1);
                    }));
            }

            //Deleting cache file
            File.Delete(info.FileName);
        }
    }
}
