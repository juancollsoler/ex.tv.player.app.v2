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

namespace MediaPortal.Mixer
{
  internal enum MixerControlClass : uint
  {
    Mask = 0xF0000000,
    Custom = 0x00000000,
    Meter = 0x10000000,
    Switch = 0x20000000,
    Number = 0x30000000,
    Slider = 0x40000000,
    Fader = 0x50000000,
    Time = 0x60000000,
    List = 0x70000000,
  }
}