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

using MediaPortal.GUI.Library;

namespace MediaPortal.GUI.Music
{
  /// <summary>
  /// 
  /// </summary>
  public class MusicState
  {
    protected static int m_iTempPlaylistWindow = 0;
    protected static string m_strTempPlaylistDirectory = "";
    protected static int m_iStartWindow = 0;
    protected static string view;

    static MusicState()
    {
      m_iStartWindow = (int)GUIWindow.Window.WINDOW_MUSIC_FILES;
    }

    public static bool AutoDJEnabled { get; set; }
    
    public static int StartWindow
    {
        get { return m_iStartWindow; }
        set { m_iStartWindow = value; }
    }

    public static string View
    {
        get { return view; }
        set { view = value; }
    }

    public static string TempPlaylistDirectory
    {
      get { return m_strTempPlaylistDirectory; }
      set { m_strTempPlaylistDirectory = value; }
    }

    public static int TempPlaylistWindow
    {
      get { return m_iTempPlaylistWindow; }
      set { m_iTempPlaylistWindow = value; }
    }
  }
}