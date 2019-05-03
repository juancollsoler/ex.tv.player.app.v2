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

using System.Collections.Generic;
using TvControl;
using TvDatabase;

namespace TvService
{
  public interface ICardAllocation
  {
    /// <summary>
    /// Gets a list of all free cards which can receive the channel specified
    /// List is sorted by "same transponder" and priority
    /// </summary>
    /// <returns>list containg all free cards which can receive the channel</returns>
    List<CardDetail> GetFreeCardsForChannel(IDictionary<int, ITvCardHandler> cards, Channel dbChannel, ref IUser user,
                                            out TvResult result);

    /// <summary>
    /// Gets a list of all available cards which can receive the channel specified
    /// List is sorted by "same transponder" and priority
    /// </summary>
    /// <returns>list containg all free cards which can receive the channel</returns>
    List<CardDetail> GetAvailableCardsForChannel(IDictionary<int, ITvCardHandler> cards, Channel dbChannel, ref IUser user, out IDictionary<int, TvResult> cardsUnAvailable);

    /// <summary>
    /// Gets a list of all available cards which can receive the channel specified
    /// List is sorted by "same transponder" and priority
    /// </summary>
    /// <returns>list containg all free cards which can receive the channel</returns>
    List<CardDetail> GetAvailableCardsForChannel(IDictionary<int, ITvCardHandler> cards, Channel dbChannel, ref IUser user);

    /// <summary>
    /// Gets a list of all free cards which can receive the channel specified
    /// List is sorted.
    /// </summary>
    /// <returns>list containg all free cards which can receive the channel</returns>
    List<CardDetail> GetFreeCardsForChannel(IDictionary<int, ITvCardHandler> cards, Channel dbChannel,
                                                            ref IUser user);
  }
}