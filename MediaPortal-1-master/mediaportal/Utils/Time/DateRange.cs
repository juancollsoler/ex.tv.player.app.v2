﻿#region Copyright (C) 2005-2011 Team MediaPortal

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

using System;

namespace MediaPortal.Time
{
  public class DateRange
  {
    #region Variables

    private readonly BasicDate _start;
    private readonly BasicDate _end;
    private readonly bool _overYear;

    #endregion

    #region Constructors/Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DateRange"/> class.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="end">The end.</param>
    public DateRange(DateTime start, DateTime end)
    {
      _start = new BasicDate(start);
      _end = new BasicDate(end);
      _overYear = false;

      if (_end.Month < _start.Month)
      {
        _overYear = true;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateRange"/> class.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="end">The end.</param>
    public DateRange(string start, string end)
    {
      _start = new BasicDate(start);
      _end = new BasicDate(end);
      _overYear = false;

      if (_end.Month < _start.Month)
      {
        _overYear = true;
      }
    }

    #endregion Constructors/Destructors

    #region Public Methods

    /// <summary>
    /// Determines whether [is in range] [the specified check time].
    /// </summary>
    /// <param name="checkTime">The check time.</param>
    /// <returns>
    /// 	<c>true</c> if [is in range] [the specified check time]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsInRange(DateTime checkTime)
    {
      if (_overYear)
      {
        if (_start < checkTime && checkTime.Month < 12 ||
            _end > checkTime && checkTime.Month >= 1)
        {
          return true;
        }
      }
      else
      {
        if (_start < checkTime && _end > checkTime)
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Determines whether [is in range] [the specified time].
    /// </summary>
    /// <param name="time">The time.</param>
    /// <returns>
    /// 	<c>true</c> if [is in range] [the specified time]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsInRange(long time)
    {
      var checkTime = new BasicDate(time);

      if (_overYear)
      {
        if (_start < checkTime && checkTime.Month < 12 ||
            _end > checkTime && checkTime.Month >= 1)
        {
          return true;
        }
      }
      else
      {
        if (_start < checkTime && _end > checkTime)
        {
          return true;
        }
      }
      return false;
    }

    public override string ToString()
    {
      return _start + "-" + _end;
    }

    #endregion
  }
}