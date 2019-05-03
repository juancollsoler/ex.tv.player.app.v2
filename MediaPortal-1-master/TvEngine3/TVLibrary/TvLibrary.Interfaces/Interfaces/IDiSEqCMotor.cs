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

namespace TvLibrary.Interfaces
{

  #region enums

  /// <summary>
  /// DisEqC directions
  /// </summary>
  public enum DiSEqCDirection
  {
    /// <summary>
    /// move west
    /// </summary>
    West,

    /// <summary>
    /// move east
    /// </summary>
    East,

    /// <summary>
    /// move Up
    /// </summary>
    Up,

    /// <summary>
    /// move down
    /// </summary>
    Down
  }

  #endregion

  /// <summary>
  /// interface for controlling DiSEqC motors
  /// </summary>
  public interface IDiSEqCMotor
  {
    /// <summary>
    /// Stops the motor.
    /// </summary>
    void StopMotor();

    /// <summary>
    /// Reset.
    /// </summary>
    void Reset();

    /// <summary>
    /// Sets the east limit.
    /// </summary>
    void SetEastLimit();

    /// <summary>
    /// Sets the west limit.
    /// </summary>
    void SetWestLimit();

    /// <summary>
    /// Enable/disables the west/east limits.
    /// </summary>
    bool ForceLimits { set; }

    /// <summary>
    /// Drives the motor.
    /// </summary>
    /// <param name="direction">The direction.</param>
    /// <param name="numberOfSeconds">the number of seconds to move.</param>
    void DriveMotor(DiSEqCDirection direction, byte numberOfSeconds);

    /// <summary>
    /// Stores the position.
    /// </summary>
    void StorePosition(byte position);

    /// <summary>
    /// Goto's the satellite reference position.
    /// </summary>
    void GotoReferencePosition();

    /// <summary>
    /// Goto's the position.
    /// </summary>
    void GotoPosition(byte position);

    /// <summary>
    /// Gets the current motor position.
    /// </summary>
    void GetPosition(out int satellitePosition, out int stepsAzimuth, out int stepsElevation);
  }
}