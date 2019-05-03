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

using System.Text.RegularExpressions;

namespace MediaPortal.WebEPG.Parser
{
  /// <summary>
  ///  Finds the rows in source data
  /// </summary>
  public class DataRows
  {
    #region Variables

    private MatchCollection _rows;
    private string _source;
    private string _rowDelimiter;

    #endregion

    #region Constructors/Destructors

    public DataRows(string delimiter)
    {
      _rowDelimiter = delimiter;
    }

    #endregion

    #region Public Methods

    public int RowCount(string source)
    {
      Regex rowRegex = new Regex(_rowDelimiter);
      _source = source;
      _rows = rowRegex.Matches(_source);
      return _rows.Count;
    }

    public string GetSource(int index)
    {
      Match row = _rows[index];

      int start;
      if (index == 0)
      {
        start = 0;
      }
      else
      {
        Match last = _rows[index - 1];
        start = last.Index + last.Length;
      }

      int end = row.Index;

      return _source.Substring(start, end - start);
    }

    #endregion
  }
}