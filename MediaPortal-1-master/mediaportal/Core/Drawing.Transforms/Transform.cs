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

using System;
using System.ComponentModel;

namespace MediaPortal.Drawing.Transforms
{
  [TypeConverter(typeof (TransformConverter))]
  public abstract class Transform
  {
    #region Methods

    protected abstract Matrix PrepareValue();

    protected void RaiseChanged()
    {
      RaiseChanged(EventArgs.Empty);
    }

    protected void RaiseChanged(EventArgs e)
    {
      _isDirty = true;
    }

    #endregion Methods

    #region Properties

    public Matrix Value
    {
      get
      {
        if (_isDirty)
        {
          _value = PrepareValue();
          _isDirty = false;
        }
        return _value;
      }
    }

    #endregion Properties

    #region Fields

    private bool _isDirty = true;
    private Matrix _value;

    #endregion Fields
  }
}