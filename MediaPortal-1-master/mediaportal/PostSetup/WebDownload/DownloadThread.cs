#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;

namespace PostSetup
{
  public delegate void DownloadCompleteHandler(byte[] dataDownloaded);

  public delegate void DownloadErrorHandler(Exception e);

  /// <summary>
  /// Summary description for DownloadThread.
  /// </summary>
  public class DownloadThread
  {
    public event DownloadCompleteHandler CompleteCallback;
    public event DownloadProgressHandler ProgressCallback;
    public event DownloadErrorHandler ErrorCallback;


    public string _downloadUrl = "";

    public string DownloadUrl
    {
      get { return _downloadUrl; }
      set { _downloadUrl = value; }
    }

    public void Download()
    {
      WebDownload webDL = null;
      try
      {
        if (CompleteCallback != null &&
            DownloadUrl != "")
        {
          webDL = new WebDownload();
          byte[] downloadedData = webDL.Download(DownloadUrl, ProgressCallback);
          CompleteCallback(downloadedData);
        }
      }
      catch (Exception e)
      {
        webDL.Cancel();
        ErrorCallback(e);
      }
    }
  }
}