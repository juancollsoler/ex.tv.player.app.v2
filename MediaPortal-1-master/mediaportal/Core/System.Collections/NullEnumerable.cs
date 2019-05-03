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

namespace System.Collections
{
  public sealed class NullEnumerable : IEnumerable
  {
    #region Constructors

    private NullEnumerable() {}

    #endregion Constructors

    #region Methods

    public IEnumerator GetEnumerator()
    {
      return NullEnumerator.Instance;
    }

    #endregion Methods

    #region Properties

    public static IEnumerable Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new NullEnumerable();
        }
        return _instance;
      }
    }

    #endregion Properties

    #region Fields

    private static NullEnumerable _instance;

    #endregion Fields
  }
}