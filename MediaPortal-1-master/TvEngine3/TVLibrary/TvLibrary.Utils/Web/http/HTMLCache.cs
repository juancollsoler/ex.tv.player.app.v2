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

namespace MediaPortal.Utils.Web
{
  /// <summary>
  /// Class provides HTML caching
  /// </summary>
  public class HTMLCache : IHtmlCache
  {
    #region Enums

    public enum Mode
    {
      Disabled = 0,
      Enabled = 1,
      Replace = 2
    }

    #endregion

    #region Variables

    private const string CACHE_DIR = "WebCache";
    private static bool _initialised = false;
    private static Mode _cacheMode = Mode.Disabled;
    private static string _strPageSource;

    #endregion

    #region Constructors/Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="HTMLCache"/> class.
    /// </summary>
    public HTMLCache() {}

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether this <see cref="IHtmlCache"/> is initialised.
    /// </summary>
    /// <value><c>true</c> if initialised; otherwise, <c>false</c>.</value>
    public bool Initialised
    {
      get { return _initialised; }
    }

    public Mode CacheMode
    {
      get { return _cacheMode; }
      set { _cacheMode = value; }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initialises the WebCache.
    /// </summary>
    public void WebCacheInitialise()
    {
      if (!Directory.Exists(CACHE_DIR))
      {
        Directory.CreateDirectory(CACHE_DIR);
      }

      _initialised = true;
    }

    /// <summary>
    /// Deletes a cached page.
    /// </summary>
    /// <param name="pageUri">The page URI.</param>
    public void DeleteCachePage(HTTPRequest page)
    {
      string file = GetCacheFileName(page);

      if (File.Exists(file))
      {
        File.Delete(file);
      }
    }

    /// <summary>
    /// Loads a page from cache.
    /// </summary>
    /// <param name="pageUri">The page URI.</param>
    /// <returns>bool - true if the page is in the cache</returns>
    public bool LoadPage(HTTPRequest page)
    {
      if (_cacheMode == Mode.Enabled)
      {
        if (LoadCacheFile(GetCacheFileName(page)))
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Saves a page to the cache.
    /// </summary>
    /// <param name="pageUri">The page URI.</param>
    /// <param name="strSource">The HTML source.</param>
    public void SavePage(HTTPRequest page, string strSource)
    {
      if (_cacheMode != Mode.Disabled)
      {
        SaveCacheFile(GetCacheFileName(page), strSource);
      }
    }

    /// <summary>
    /// Gets the page source of the current loaded page.
    /// </summary>
    /// <returns>HTML source as a string</returns>
    public string GetPage() //string strURL, string strEncode)
    {
      return _strPageSource;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Loads the cache file.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <returns></returns>
    private bool LoadCacheFile(string file)
    {
      if (File.Exists(file))
      {
        TextReader CacheFile = new StreamReader(file);
        _strPageSource = CacheFile.ReadToEnd();
        CacheFile.Close();

        return true;
      }

      return false;
    }

    /// <summary>
    /// Saves the cache file.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <param name="source">The source.</param>
    private void SaveCacheFile(string file, string source)
    {
      if (File.Exists(file))
      {
        File.Delete(file);
      }

      TextWriter CacheFile = new StreamWriter(file);
      CacheFile.Write(source);
      CacheFile.Close();
    }

    /// <summary>
    /// Gets the name of the cache file.
    /// </summary>
    /// <param name="Page">The page.</param>
    /// <returns>filename</returns>
    private static string GetCacheFileName(HTTPRequest Page)
    {
      uint gethash = (uint)Page.Uri.GetHashCode();

      if (Page.PostQuery == null || Page.PostQuery == string.Empty)
      {
        return CACHE_DIR + "/" + Page.Host + "_" + gethash.ToString() + ".html";
      }

      uint posthash = (uint)Page.PostQuery.GetHashCode();

      return CACHE_DIR + "/" + Page.Host + "_" + gethash.ToString() + "_" + posthash.ToString() + ".html";
    }

    #endregion
  }
}