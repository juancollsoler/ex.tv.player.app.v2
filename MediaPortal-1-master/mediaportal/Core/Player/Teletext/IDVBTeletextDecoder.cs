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

namespace MediaPortal.Player.Teletext
{
  public interface IDVBTeletextDecoder
  {
    void OnTeletextPacket(byte[] data, UInt64 presentTime);

    void OnServiceInfo(int page, byte type, string iso_lang);

    bool AcceptsDataUnitID(byte id);

    // TODO:
    // We need different types of reset
    // if a new channel we need to reset subtitle
    // info, otherwise we just need to reset the teletext cache
    void Reset();
  }
}