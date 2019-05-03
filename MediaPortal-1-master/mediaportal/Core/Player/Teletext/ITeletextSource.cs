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
using System.Runtime.InteropServices;

namespace MediaPortal.Player.Teletext
{
  internal enum TeletextEvent
  {
    SEEK_START = 0,
    SEEK_END = 1,
    RESET = 2,
    BUFFER_IN_UPDATE = 3,
    BUFFER_OUT_UPDATE = 4,
    PACKET_PCR_UPDATE = 5,
    //CURRENT_PCR_UPDATE = 6,
    COMPENSATION_UPDATE = 7
  }

  /// <summary>
  /// Interface to the TsReader filter wich provides information about the 
  /// Teletext streams and allows us to set the current Teletext callbacks
  /// </summary>
  /// 
  [Guid("3AB7E208-7962-11DC-9F76-850456D89593"),
   InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface ITeletextSource
  {
    void SetTeletextTSPacketCallback(IntPtr callback);
    void SetTeletextEventCallback(IntPtr callback);
    void SetTeletextServiceInfoCallback(IntPtr callback);
    void GetTeletextStreamType(Int32 stream, ref Int32 type);
    void GetTeletextStreamCount(ref Int32 count);
    void GetTeletextStreamLanguage(Int32 stream, ref TELETEXT_LANGUAGE szLanguage);
  }
  
  /// <summary>
  /// Structure to pass the Teletext language data from TsReader to this class
  /// </summary>
  /// 
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct TELETEXT_LANGUAGE
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)] public string lang;
  }
}