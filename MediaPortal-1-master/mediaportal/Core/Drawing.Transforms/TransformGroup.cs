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
using System.Windows.Serialization;

namespace MediaPortal.Drawing.Transforms
{
  public sealed class TransformGroup : Transform, IAddChild
  {
    #region Constructors

    public TransformGroup() {}

    public TransformGroup(TransformCollection transforms)
    {
      _children = transforms;
    }

    #endregion Constructors

    #region Methods

    protected override Matrix PrepareValue()
    {
      Matrix matrix = Matrix.Identity;

      for (int index = 0; index < ((_children != null) ? _children.Count : 0); index++)
      {
        matrix.Multiply(_children[index].Value);
      }

      return matrix;
    }

    void IAddChild.AddChild(object child)
    {
      if (child is Transform == false)
      {
        throw new ArgumentException(string.Format("must be of type {0}", this.GetType()));
      }

      if (_children == null)
      {
        _children = new TransformCollection();
      }

      _children.Add((Transform)child);
    }

    void IAddChild.AddText(string text) {}

    #endregion Methods

    #region Properties

    public TransformCollection Children
    {
      get
      {
        if (_children == null)
        {
          _children = new TransformCollection();
        }
        return _children;
      }
    }

    #endregion Properties

    #region Fields

    private TransformCollection _children;

    #endregion Fields
  }
}