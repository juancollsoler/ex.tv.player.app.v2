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

namespace TvLibrary.Interfaces
{
  /// <summary>
  /// State of ci menu
  /// </summary>
  public enum CiMenuState
  {
    /// menu is closed
    Closed = 0,
    /// opened
    Opened = 1,
    /// ready
    Ready = 2,
    /// request
    Request = 3,
    /// no choices
    NoChoices = 4,
    /// error
    Error = 5,
    /// open menu is to be closed
    Close = 6
  }

  /// <summary>
  /// Interface for all DVB cards to support CI menu
  /// </summary>
  public interface ICiMenuCallbacks
  {
    /// <summary>
    /// Callback on opening menu
    /// </summary>
    /// <param name="lpszTitle">Title</param>
    /// <param name="lpszSubTitle">Subtitle</param>
    /// <param name="lpszBottom">Bottom text</param>
    /// <param name="nNumChoices">number of choices</param>
    /// <returns></returns>
    int OnCiMenu(
      string lpszTitle,
      string lpszSubTitle,
      string lpszBottom,
      int nNumChoices);

    /// <summary>
    /// Callback for each menu entry
    /// </summary>
    /// <param name="nChoice">choice number</param>
    /// <param name="lpszText">choice text</param>
    /// <returns></returns>
    int OnCiMenuChoice(
      int nChoice,
      string lpszText);

    /// <summary>
    /// Callback on closing display
    /// </summary>
    /// <param name="nDelay">delay in seconds</param>
    /// <returns></returns>
    int OnCiCloseDisplay(
      int nDelay);

    /// <summary>
    /// Callback on requesting user input (PIN,...)
    /// </summary>
    /// <param name="bBlind">true if password</param>
    /// <param name="nAnswerLength">expected (max) answer length</param>
    /// <param name="lpszText">request text</param>
    /// <returns></returns>
    int OnCiRequest(
      bool bBlind,
      uint nAnswerLength,
      string lpszText);
  } ;
}