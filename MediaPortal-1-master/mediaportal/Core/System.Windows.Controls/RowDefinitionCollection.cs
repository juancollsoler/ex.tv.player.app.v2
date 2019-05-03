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

using System.Collections;

namespace System.Windows.Controls
{
  public sealed class RowDefinitionCollection : CollectionBase
  {
    #region Constructors

    public RowDefinitionCollection() {}

    #endregion Constructors

    #region Methods

    public void Add(RowDefinition row)
    {
      if (row == null)
      {
        throw new ArgumentNullException("row");
      }

      List.Add(row);
    }

    public bool Contains(RowDefinition row)
    {
      if (row == null)
      {
        throw new ArgumentNullException("row");
      }

      return List.Contains(row);
    }

    public void CopyTo(RowDefinition[] array, int arrayIndex)
    {
      if (array == null)
      {
        throw new ArgumentNullException("array");
      }

      List.CopyTo(array, arrayIndex);
    }

    public int IndexOf(RowDefinition row)
    {
      if (row == null)
      {
        throw new ArgumentNullException("row");
      }

      return List.IndexOf(row);
    }

    public void Insert(int index, RowDefinition row)
    {
      if (row == null)
      {
        throw new ArgumentNullException("row");
      }

      List.Insert(index, row);
    }

    public bool Remove(RowDefinition row)
    {
      if (row == null)
      {
        throw new ArgumentNullException("row");
      }

      if (List.Contains(row) == false)
      {
        return false;
      }

      List.Remove(row);

      return true;
    }

    #endregion Methods

    #region Properties

    public RowDefinition this[int index]
    {
      get { return (RowDefinition)List[index]; }
      set { List[index] = value; }
    }

    #endregion Properties
  }
}