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

namespace System.Windows.Media.Animation
{
  internal sealed class AnimationClockCollection : CollectionBase
  {
    #region Constructors

    public AnimationClockCollection() {}

    #endregion Constructors

    #region Methods

    public void Add(AnimationClock clock)
    {
      if (clock == null)
      {
        throw new ArgumentNullException("clock");
      }

      List.Add(clock);
    }

    public bool Contains(AnimationClock clock)
    {
      if (clock == null)
      {
        throw new ArgumentNullException("clock");
      }

      return List.Contains(clock);
    }

    public void CopyTo(AnimationClock[] array, int arrayIndex)
    {
      if (array == null)
      {
        throw new ArgumentNullException("array");
      }

      List.CopyTo(array, arrayIndex);
    }

    public int IndexOf(AnimationClock clock)
    {
      if (clock == null)
      {
        throw new ArgumentNullException("clock");
      }

      return List.IndexOf(clock);
    }

    public void Insert(int index, AnimationClock clock)
    {
      if (clock == null)
      {
        throw new ArgumentNullException("clock");
      }

      List.Insert(index, clock);
    }

    public bool Remove(AnimationClock clock)
    {
      if (clock == null)
      {
        throw new ArgumentNullException("clock");
      }

      if (List.Contains(clock) == false)
      {
        return false;
      }

      List.Remove(clock);

      return true;
    }

    #endregion Methods

    #region Properties

    public AnimationClock this[int index]
    {
      get { return (AnimationClock)List[index]; }
      set { List[index] = value; }
    }

    #endregion Properties
  }
}