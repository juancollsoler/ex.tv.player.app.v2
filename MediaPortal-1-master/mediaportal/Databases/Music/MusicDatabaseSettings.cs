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

namespace MediaPortal.Music.Database
{
  /// <summary>
  /// Contains all Music DB settings
  /// </summary>
  public class MusicDatabaseSettings
  {
    private bool _treatFolderAsAlbum;
    private bool _extractEmbededCoverArt;
    private bool _useFolderThumbs;
    private bool _useAllImages;
    private bool _createArtistThumbs;
    private bool _createGenreThumbs;
    private bool _createMissingFolderThumbs;
    private bool _stripArtistPrefixes;
    private bool _useLastImportDate;
    private bool _excludeHiddenFiles;
    private int _dateAddedValue;


    public MusicDatabaseSettings() {}

    public bool TreatFolderAsAlbum
    {
      get { return _treatFolderAsAlbum; }
      set { _treatFolderAsAlbum = value; }
    }

    public bool ExtractEmbeddedCoverArt
    {
      get { return _extractEmbededCoverArt; }
      set { _extractEmbededCoverArt = value; }
    }

    public bool UseFolderThumbs
    {
      get { return _useFolderThumbs; }
      set { _useFolderThumbs = value; }
    }

    public bool UseAllImages
    {
      get { return _useAllImages; }
      set { _useAllImages = value; }
    }

    public bool CreateArtistPreviews
    {
      get { return _createArtistThumbs; }
      set { _createArtistThumbs = value; }
    }

    public bool CreateGenrePreviews
    {
      get { return _createGenreThumbs; }
      set { _createGenreThumbs = value; }
    }

    public bool CreateMissingFolderThumb
    {
      get { return _createMissingFolderThumbs; }
      set { _createMissingFolderThumbs = value; }
    }

    public bool StripArtistPrefixes
    {
      get { return _stripArtistPrefixes; }
      set { _stripArtistPrefixes = value; }
    }

    public bool UseLastImportDate
    {
      get { return _useLastImportDate; }
      set { _useLastImportDate = value; }
    }

    public bool ExcludeHiddenFiles
    {
      get { return _excludeHiddenFiles; }
      set { _excludeHiddenFiles = value; }
    }

    public int DateAddedValue
    {
      get { return _dateAddedValue; }
      set { _dateAddedValue = value; }
    }
  }
}