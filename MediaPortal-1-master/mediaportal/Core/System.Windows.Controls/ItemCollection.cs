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
//	TODO: objectCollection is a far more involved collection and needs more work
//	public sealed class ItemCollection : CollectionView, IList, ICollection, IEnumerable
  public sealed class ItemCollection : CollectionBase
  {
    #region Constructors

    public ItemCollection() {}

    #endregion Constructors

    #region Methods

    public void Add(object item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }

      List.Add(item);
    }

    public bool Contains(object item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }

      return List.Contains(item);
    }

    public void CopyTo(Array array, int arrayIndex)
    {
      if (array == null)
      {
        throw new ArgumentNullException("array");
      }

      List.CopyTo(array, arrayIndex);
    }

    public int IndexOf(object item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }

      return List.IndexOf(item);
    }

    public void Insert(int index, object item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }

      List.Insert(index, item);
    }

    public bool Remove(object item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }

      if (List.Contains(item) == false)
      {
        return false;
      }

      List.Remove(item);

      return true;
    }

    #endregion Methods

    #region Properties

    public object this[int index]
    {
      get { return (object)List[index]; }
      set { List[index] = value; }
    }

    #endregion Properties
  }
}