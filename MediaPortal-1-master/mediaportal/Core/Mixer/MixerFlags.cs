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
  internal enum MixerFlags : uint
  {
    Auxillary = 0x50000000,
    CallbackDelegate = 0x00030000,
    CallbackNone = 0x00000000,
    CallbackWindow = 0x00010000,
    Handle = 0x80000000,
    Mixer = 0x00000000,
    MixerHandle = Handle | Mixer,
    WaveOut = 0x10000000,
    WaveOutHandle = Handle | WaveOut,
    WaveIn = 0x20000000,
    WaveInHandle = Handle | WaveIn,
    MidiOut = 0x30000000,
    MidiOutHandle = Handle | MidiOut,
    MidiIn = 0x40000000,
    MidiInHandle = Handle | MidiIn,
  }
}