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

using System.IO;
using System.Text;

namespace TvEngine
{
  internal class IOUtil
  {
    public static void CheckFileAccessRights(string fileName, ref bool canRead, ref bool canWrite)
    {
      canRead = false;
      canWrite = false;
      FileStream fs = null;

      try
      {
        fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        canRead = fs.CanRead;
      }
      finally
      {
        if (fs != null) fs.Close();
      }

      try
      {
        fs = new FileStream(fileName, FileMode.Open, FileAccess.Write);
        canWrite = fs.CanWrite;
      }
      finally
      {
        if (fs != null) fs.Close();
      }
    }

    /// <summary>
    /// Check's if the file has the accessrights specified in the input parameters
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="fa">Read,Write,ReadWrite</param>
    /// <param name="fs">Read,ReadWrite...</param>
    /// <returns></returns>
    public static void CheckFileAccessRights(string fileName, FileMode fm, FileAccess fa, FileShare fs)
    {
      FileStream fileStream = null;
      StreamReader streamReader = null;
      try
      {
        Encoding fileEncoding = Encoding.Default;
        fileStream = File.Open(fileName, fm, fa, fs);
        streamReader = new StreamReader(fileStream, fileEncoding, true);
      }
      finally
      {
        try
        {
          if (fileStream != null)
          {
            fileStream.Close();
            fileStream.Dispose();
          }
          if (streamReader != null)
          {
            streamReader.Close();
            streamReader.Dispose();
          }
        }
        finally {}
      }
    }
  }
}