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

namespace MediaPortal.GUI.Library
{
  public enum GUILayers
  {
    all = 0,
    under = 1,
    over = 2
  }

  /// <summary>
  /// Interface which is used for VMR9 players to render the GUI
  /// </summary>
  public interface IRender
  {
    void RenderFrame(float timePassed, GUILayers layers);
    void RenderFrame(float timePassed, GUILayers layers, ref bool uiVisible);
  }
}