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

namespace MediaPortal.Music.Database
{
  /// <summary>
  /// Database object for album details
  /// </summary>
  [Serializable()]
  public class AlbumInfo
  {
    private string m_strAlbum = string.Empty;
    private string m_strAsin = string.Empty;
    private string m_strArtist = string.Empty;
    private string m_strAlbumArtist = string.Empty;
    private string m_strGenre = string.Empty;
    private string m_strTones = string.Empty;
    private string m_strStyles = string.Empty;
    private string m_strReview = string.Empty;
    private string m_strImage = string.Empty;
    private string m_strTracks = string.Empty;
    private int m_iRating = 0;
    private int m_iYear = 0;


    public AlbumInfo Clone()
    {
      var newalbum = new AlbumInfo
        {
          Album = Album,
          Asin = Asin,
          Artist = Artist,
          Genre = Genre,
          Image = Image,
          Rating = Rating,
          Review = Review,
          Styles = Styles,
          Tones = Tones,
          Tracks = Tracks,
          Year = Year
        };
      return newalbum;
    }

    public int Year
    {
      get { return m_iYear; }
      set { m_iYear = value; }
    }

    public int Rating
    {
      get { return m_iRating; }
      set { m_iRating = value; }
    }

    public string Image
    {
      get { return m_strImage; }
      set { m_strImage = value; }
    }

    public string Review
    {
      get { return m_strReview; }
      set { m_strReview = value; }
    }

    public string Styles
    {
      get { return m_strStyles; }
      set { m_strStyles = value; }
    }

    public string Tones
    {
      get { return m_strTones; }
      set { m_strTones = value; }
    }

    public string Genre
    {
      get { return m_strGenre; }
      set { m_strGenre = value; }
    }

    public string Artist
    {
      get { return m_strArtist; }
      set { m_strArtist = value; }
    }

    public string Album
    {
      get { return m_strAlbum; }
      set { m_strAlbum = value; }
    }

    public string Asin
    {
      get { return m_strAsin; }
      set { m_strAsin = value; }
    }

    public string AlbumArtist
    {
      get { return m_strAlbumArtist; }
      set { m_strAlbumArtist = value; }
    }

    public string Tracks
    {
      get { return m_strTracks; }
      set { m_strTracks = value; }
    }
  }
}