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

namespace TvLibrary
{  
  /// <summary>
  /// Exception class for the tv library
  /// </summary>
  [Serializable]
  public class TvExceptionTuneCancelled : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TvExceptionTuneCancelled"/> class.
    /// </summary>
    public TvExceptionTuneCancelled() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="TvExceptionTuneCancelled"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public TvExceptionTuneCancelled(string message)
      : base(message) {}


    /// <summary>
    /// Initializes a new instance of the <see cref="TvExceptionTuneCancelled"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TvExceptionTuneCancelled(string message, Exception innerException)
      : base(message, innerException) {}
  }

  /// <summary>
  /// Exception class for the tv library
  /// </summary>
  [Serializable]
  public class TvExceptionNoPMT : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TvExceptionNoPMT"/> class.
    /// </summary>
    public TvExceptionNoPMT() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="TvExceptionNoPMT"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public TvExceptionNoPMT(string message)
      : base(message) {}


    /// <summary>
    /// Initializes a new instance of the <see cref="TvExceptionNoPMT"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TvExceptionNoPMT(string message, Exception innerException)
      : base(message, innerException) {}
  }

  /// <summary>
  /// Exception class for the tv library
  /// </summary>
  [Serializable]
  public class TvExceptionNoSignal : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    public TvExceptionNoSignal() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public TvExceptionNoSignal(string message)
      : base(message) {}


    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TvExceptionNoSignal(string message, Exception innerException)
      : base(message, innerException) {}
  }

  /// <summary>
  /// Exception class for the tv library
  /// </summary>
  [Serializable]
  public class TvExceptionGraphBuildingFailed : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    public TvExceptionGraphBuildingFailed() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public TvExceptionGraphBuildingFailed(string message)
      : base(message) {}


    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TvExceptionGraphBuildingFailed(string message, Exception innerException)
      : base(message, innerException) {}
  }


  /// <summary>
  /// Exception class for the tv library
  /// </summary>
  [Serializable]
  public class TvExceptionSWEncoderMissing : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    public TvExceptionSWEncoderMissing() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public TvExceptionSWEncoderMissing(string message)
      : base(message) {}


    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TvExceptionSWEncoderMissing(string message, Exception innerException)
      : base(message, innerException) {}
  }

  /// <summary>
  /// Exception class for the tv library
  /// </summary>
  [Serializable]
  public class TvException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    public TvException() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public TvException(string message)
      : base(message) {}


    /// <summary>
    /// Initializes a new instance of the <see cref="TvException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TvException(string message, Exception innerException)
      : base(message, innerException) {}
  }
}