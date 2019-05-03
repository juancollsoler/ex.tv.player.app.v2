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

namespace MediaPortal.Utils.Web
{
  public class MatchTag
  {
    #region Variables

    private string _fullTag;
    private string _tagName;
    private int _index;
    private int _lenght;
    private bool _isClose;

    #endregion

    #region Constructors/Destructors

    public MatchTag(string source, int index, int length)
    {
      _index = index;
      _lenght = length;
      _fullTag = source.Substring(index, length);


      int pos = _fullTag.IndexOf(' ');
      if (pos == -1)
      {
        pos = _fullTag.Length - 1;
      }

      int start = 1;
      _isClose = false;
      if (_fullTag[start] == '/')
      {
        start++;
        _isClose = true;
      }
      _tagName = _fullTag.Substring(start, pos - start).ToLowerInvariant();
    }

    #endregion

    #region Properties

    public string FullTag
    {
      get { return _fullTag; }
    }

    public string TagName
    {
      get { return _tagName; }
    }

    public int Index
    {
      get { return _index; }
    }

    public int Length
    {
      get { return _lenght; }
    }

    public bool IsClose
    {
      get { return _isClose; }
    }

    #endregion

    #region Public Methods

    public bool SameType(MatchTag value)
    {
      if (_tagName == value._tagName &&
          _isClose == value._isClose)
      {
        return true;
      }

      return false;
    }

    public override string ToString()
    {
      return _fullTag;
    }

    #endregion
  }
}