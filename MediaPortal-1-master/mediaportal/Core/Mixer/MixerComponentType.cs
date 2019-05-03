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
  internal enum MixerComponentType : int
  {
    DestinationNone = 0,
    DestinationDigital = 1,
    DestinationLine = 2,
    DestinationMonitor = 3,
    DestinationSpeakers = 4,
    DestinationHeadphones = 5,
    DestinationTelephone = 6,
    DestinationWave = 7,
    DestinationVoice = 8,

    SourceNone = 4096,
    SourceDigital = 4097,
    SourceLine = 4098,
    SourceMicrophone = 4099,
    SourceSynthesizer = 4100,
    SourceCompactDisc = 4101,
    SourceTelephone = 4102,
    SourceSpeaker = 4103,
    SourceWave = 4104,
    SourceAuxiliary = 4105,
    SourceAnalog = 4106,
  }
}