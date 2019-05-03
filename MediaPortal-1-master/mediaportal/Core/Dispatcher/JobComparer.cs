#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
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
using System.Collections;

namespace MediaPortal.Dispatcher
{
  internal class JobComparer : IComparer
  {
    #region Methods

    public int Compare(object l, object r)
    {
      if (l is Job == false)
      {
        throw new ArgumentException("argument l is not of type Job.");
      }

      if (r is Job == false)
      {
        throw new ArgumentException("argument r is not of type Job.");
      }

      return Compare((Job)l, (Job)r);
    }

    public int Compare(Job l, Job r)
    {
      return DateTime.Compare(l.Next, r.Next);
    }

    #endregion Methods
  }
}