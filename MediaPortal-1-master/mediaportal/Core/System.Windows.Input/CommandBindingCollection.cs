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

namespace System.Windows.Input
{
  public sealed class CommandBindingCollection : CollectionBase
  {
    #region Constructors

    public CommandBindingCollection() {}

    public CommandBindingCollection(IList commandBindings)
    {
      throw new NotImplementedException();
    }

    #endregion Constructors

    #region Methods

    public void Add(CommandBinding binding)
    {
      if (binding == null)
      {
        throw new ArgumentNullException("binding");
      }

      List.Add(binding);
    }

    public bool Contains(CommandBinding binding)
    {
      if (binding == null)
      {
        throw new ArgumentNullException("binding");
      }

      return List.Contains(binding);
    }

    public void CopyTo(CommandBinding[] array, int arrayIndex)
    {
      if (array == null)
      {
        throw new ArgumentNullException("array");
      }

      List.CopyTo(array, arrayIndex);
    }

    public int IndexOf(CommandBinding binding)
    {
      if (binding == null)
      {
        throw new ArgumentNullException("binding");
      }

      return List.IndexOf(binding);
    }

    public void Insert(int index, CommandBinding binding)
    {
      if (binding == null)
      {
        throw new ArgumentNullException("binding");
      }

      List.Insert(index, binding);
    }

    public bool Remove(CommandBinding binding)
    {
      if (binding == null)
      {
        throw new ArgumentNullException("binding");
      }

      if (List.Contains(binding) == false)
      {
        return false;
      }

      List.Remove(binding);

      return true;
    }

    #endregion Methods

    #region Properties

    public CommandBinding this[int index]
    {
      get { return (CommandBinding)List[index]; }
      set { List[index] = value; }
    }

    #endregion Properties
  }
}