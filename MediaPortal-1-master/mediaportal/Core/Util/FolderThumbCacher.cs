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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using MediaPortal.Services;
using MediaPortal.ServiceImplementations;
using MediaPortal.TagReader;
using MediaPortal.Threading;


namespace MediaPortal.Util
{
  /// <summary>
  /// searches for folder.jpg in the mp3 directory and creates cached thumbs in MP's thumbs\folder dir
  /// </summary>
  public class FolderThumbCacher
  {
    private string _filename = string.Empty;
    private bool _overWrite = false;
    private Work work;

    // aFilePath must only be the path of the directory
    public FolderThumbCacher(string aFilePath, bool aOverWriteExisting)
    {
      lock (aFilePath)
      {
        _filename = aFilePath;
        _overWrite = aOverWriteExisting;
        work = new Work(new DoWorkHandler(this.PerformRequest));
        work.ThreadPriority = ThreadPriority.Lowest;
        GlobalServiceProvider.Get<IThreadPool>().Add(work, QueuePriority.Low);
      }
    }

    private void PerformRequest()
    {
      bool replace = _overWrite;
      string filename = _filename;
      string strFolderThumb = string.Empty;
      strFolderThumb = Util.Utils.GetLocalFolderThumbForDir(filename);

      string strRemoteFolderThumb = string.Empty;
      strRemoteFolderThumb = String.Format(@"{0}\folder.jpg", Util.Utils.RemoveTrailingSlash(filename));

      if (Utils.FileExistsInCache(strRemoteFolderThumb))
      {
        // if there was no cached thumb although there was a folder.jpg then the user didn't scan his collection:
        // -- punish him with slowness and create the thumbs for the next time...
        try
        {
          Log.Info("FolderThumbCacher: Creating missing folder thumb cache for {0}", strRemoteFolderThumb);
          string localFolderLThumb = Util.Utils.ConvertToLargeCoverArt(strFolderThumb);

          if (!File.Exists(strFolderThumb) || replace)
          {
            Picture.CreateThumbnail(strRemoteFolderThumb, strFolderThumb, (int)Thumbs.ThumbResolution,
                                       (int)Thumbs.ThumbResolution, 0, true, false, false);
            {
              // Generate Large Thumb
              Picture.CreateThumbnail(strRemoteFolderThumb, localFolderLThumb, (int)Thumbs.ThumbLargeResolution,
                                         (int)Thumbs.ThumbLargeResolution, 0, true, true, false);
            }
          }
        }
        catch (Exception ex)
        {
          Log.Error("FolderThumbCacher: Error processing {0} - {1}", strRemoteFolderThumb, ex);
        }
      }
      else
        Log.Debug("FolderThumbCacher: No folder thumb at {0}", strRemoteFolderThumb);
    }
  }
}