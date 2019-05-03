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

using System.IO;
using TvLibrary.Log;

namespace TvLibrary.Interfaces
{
  /// <summary>
  /// The global service provider
  /// </summary>
  public class GlobalServiceProvider
  {
    private static ServiceProvider _provider;

    /// <summary>
    /// returns the instance of the global ServiceProvider
    /// </summary>
    public static ServiceProvider Instance
    {
      get
      {
        if (_provider == null)
        {
          _provider = new ServiceProvider();
        }
        return _provider;
      }
    }
  }
}