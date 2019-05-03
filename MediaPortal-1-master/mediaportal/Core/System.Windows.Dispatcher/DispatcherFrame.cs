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

using System.Windows.Threading;

namespace System.Windows.Dispatcher
{
  public class DispatcherFrame : DispatcherObject
  {
    // samples stack
    // http://www.64bit-world.com/forums/microsoft-public-developer-winfx-avalon/10124-xamlpad-exe-nullreferencexception-winfx-ctp-sept.html

    #region Constructors

    public DispatcherFrame() {}

    public DispatcherFrame(bool exitWhenRequested) {}

    #endregion Constructors

    #region Properties

    public bool Continue
    {
      get { return _isContinue; }
      set { _isContinue = value; }
    }

    #endregion Properties

    #region Fields

    private bool _isContinue;

    #endregion Fields
  }
}