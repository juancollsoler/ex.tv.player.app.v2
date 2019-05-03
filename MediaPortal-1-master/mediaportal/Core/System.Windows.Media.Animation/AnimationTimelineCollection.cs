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
  internal sealed class AnimationTimelineCollection : CollectionBase
  {
    #region Constructors

    public AnimationTimelineCollection() {}

    #endregion Constructors

    #region Methods

    public void Add(AnimationTimeline timeline)
    {
      if (timeline == null)
      {
        throw new ArgumentNullException("timeline");
      }

      List.Add(timeline);
    }

    public bool Contains(AnimationTimeline timeline)
    {
      if (timeline == null)
      {
        throw new ArgumentNullException("timeline");
      }

      return List.Contains(timeline);
    }

    public void CopyTo(AnimationTimeline[] array, int arrayIndex)
    {
      if (array == null)
      {
        throw new ArgumentNullException("array");
      }

      List.CopyTo(array, arrayIndex);
    }

    public int IndexOf(AnimationTimeline timeline)
    {
      if (timeline == null)
      {
        throw new ArgumentNullException("timeline");
      }

      return List.IndexOf(timeline);
    }

    public void Insert(int index, AnimationTimeline timeline)
    {
      if (timeline == null)
      {
        throw new ArgumentNullException("timeline");
      }

      List.Insert(index, timeline);
    }

    public bool Remove(AnimationTimeline timeline)
    {
      if (timeline == null)
      {
        throw new ArgumentNullException("timeline");
      }

      if (List.Contains(timeline) == false)
      {
        return false;
      }

      List.Remove(timeline);

      return true;
    }

    #endregion Methods

    #region Properties

    public AnimationTimeline this[int index]
    {
      get { return (AnimationTimeline)List[index]; }
      set { List[index] = value; }
    }

    #endregion Properties
  }
}