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

namespace TvLibrary.Interfaces
{
  /// <summary>
  /// callback interface for the ChannelLinkageScanner
  /// </summary>
  [ComVisible(true), ComImport,
   Guid("F8A86679-C80A-42fd-A148-20D681A67024"),
   InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IChannelLinkageCallback
  {
    /// <summary>
    /// Called when channel linkages are received.
    /// </summary>
    /// <returns></returns>
    [PreserveSig]
    int OnLinkageReceived();
  } ;

  /// <summary>
  /// Base class used for channel linkage grabbing
  /// </summary>
  public abstract class BaseChannelLinkageScanner : IChannelLinkageCallback
  {
    /// <summary>
    /// Gets called when linkage infos have been received
    /// Should be overriden by the class
    /// </summary>
    /// <returns></returns>
    public virtual int OnLinkageReceived()
    {
      return 0;
    }
  }
}