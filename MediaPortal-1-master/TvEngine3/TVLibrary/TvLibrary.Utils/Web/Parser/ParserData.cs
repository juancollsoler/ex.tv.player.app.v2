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

using System.Collections.Generic;

namespace MediaPortal.Utils.Web
{
  /// <summary>
  /// Simple Parser Data class. Stores any Element tag and value in a dictionary.
  /// </summary>
  public class ParserData : IParserData
  {
    #region Variables

    private Dictionary<string, string> _data;

    #endregion

    #region Constructors/Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ParserData"/> class.
    /// </summary>
    public ParserData()
    {
      _data = new Dictionary<string, string>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the number of elements stored.
    /// </summary>
    /// <value>The count.</value>
    public int Count
    {
      get { return _data.Count; }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets an element by tag name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>element value</returns>
    public string GetElement(string name)
    {
      return _data[name];
    }

    /// <summary>
    /// Gets the Element name by index number.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>tag</returns>
    public string GetElementName(int index)
    {
      Dictionary<string, string>.Enumerator enumerator = _data.GetEnumerator();

      enumerator.MoveNext();
      for (int i = 0; i < index; i++)
      {
        enumerator.MoveNext();
      }

      return enumerator.Current.Key;
    }

    /// <summary>
    /// Gets the Element data by index number.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>value</returns>
    public string GetElementValue(int index)
    {
      Dictionary<string, string>.Enumerator enumerator = _data.GetEnumerator();

      enumerator.MoveNext();
      for (int i = 0; i < index; i++)
      {
        enumerator.MoveNext();
      }

      return enumerator.Current.Value;
    }

    #endregion

    #region IParserData Implementations

    /// <summary>
    /// Sets an element.
    /// </summary>
    /// <param name="tag">Element tag.</param>
    /// <param name="value">Element value.</param>
    public void SetElement(string tag, string value)
    {
      _data.Add(tag, value);
    }

    #endregion
  }
}