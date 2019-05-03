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

using System.Runtime.InteropServices;

namespace TvLibrary.Interfaces.Analyzer
{
  /// <summary>
  /// Enumeration of pid's for the video/audio observer
  /// </summary>
  public enum PidType
  {
    /// <summary>
    /// Video pid
    /// </summary>
    Video = 0,
    /// <summary>
    /// Audio pid
    /// </summary>
    Audio,
    /// <summary>
    /// Other pid
    /// </summary>
    Other
  }

  ///<summary>
  /// TsWriter video/audio observer
  ///</summary>
  [ComVisible(true), ComImport, Guid("08177EB2-65D6-4d0a-A2A8-E7B7280A95A3"),
   InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IVideoAudioObserver
  {
    ///<summary>
    /// Called when a pid is detected
    ///</summary>
    ///<param name="pidType">The pid type</param>
    ///<returns>Error code</returns>
    [PreserveSig]
    int OnNotify(PidType pidType);
  }

  /// <summary>
  /// MPFileWiter video/audio observer
  /// </summary>
  [ComVisible(true), ComImport, Guid("F94D89B5-C888-4da1-9782-15C1C0CBFE4D"),
   InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IAnalogVideoAudioObserver
  {
    ///<summary>
    /// Called when a pid is detected
    ///</summary>
    ///<param name="pidType">The pid type</param>
    ///<returns>Error code</returns>
    [PreserveSig]
    int OnNotify(PidType pidType);
  }
}