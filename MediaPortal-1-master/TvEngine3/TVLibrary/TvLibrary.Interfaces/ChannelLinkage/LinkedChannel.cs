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

namespace TvLibrary.ChannelLinkage
{
  /// <summary>
  /// class which holds all linked channels
  /// </summary>
  [Serializable]
  public class LinkedChannel
  {
    #region variables

    private ushort _networkId;
    private ushort _transportId;
    private ushort _serviceId;
    private string _name;

    #endregion

    #region ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="LinkedChannel"/> class.
    /// </summary>
    public LinkedChannel()
    {
      _networkId = 0;
      _transportId = 0;
      _serviceId = 0;
      _name = "";
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets/Sets the network id
    /// </summary>
    public ushort NetworkId
    {
      get { return _networkId; }
      set { _networkId = value; }
    }

    /// <summary>
    /// Gets/Sets the transport id
    /// </summary>
    public ushort TransportId
    {
      get { return _transportId; }
      set { _transportId = value; }
    }

    /// <summary>
    /// Gets/Sets the service id
    /// </summary>
    public ushort ServiceId
    {
      get { return _serviceId; }
      set { _serviceId = value; }
    }

    /// <summary>
    /// Gets/Sets the linked channel name
    /// </summary>
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    #endregion
  }
}