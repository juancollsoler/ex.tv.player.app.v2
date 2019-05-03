#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
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
using System.Linq;
using TvControl;
using TvDatabase;
using TvLibrary.Interfaces;
using TvLibrary.Log;
using TvService;

namespace TvService
{

  public class AdvancedCardAllocationTicket : AdvancedCardAllocation
  {
    private readonly IDictionary<int, ICardTuneReservationTicket> _tickets;

    public AdvancedCardAllocationTicket(TvBusinessLayer businessLayer, TVController controller, IEnumerable<ICardTuneReservationTicket> tickets)
      : base(businessLayer, controller)
    {
      _tickets = new Dictionary<int, ICardTuneReservationTicket>();
      foreach (ICardTuneReservationTicket cardTuneReservationTicket in tickets)
      {
        if (cardTuneReservationTicket != null)
        {
          int idCard = cardTuneReservationTicket.CardId;
          if (!_tickets.ContainsKey(idCard))
          {
            _tickets.Add(idCard, cardTuneReservationTicket);
          }
        }
      }
    }   

    private void LogNumberOfOtherUsersFound(CardDetail cardDetail)
    {
      if (LogEnabled && cardDetail.Card.IdCard > 0)
      {
        var tvController = _controller as TVController;
        if (tvController != null)
        {
          ITvCardHandler card = tvController.CardCollection[cardDetail.Card.IdCard];
          Log.Info("AdvancedCardAllocationTicket.LogNumberOfOtherUsersFound: card:{0} type:{1} users: {2}", cardDetail.Card.IdCard, card.Type, cardDetail.NumberOfOtherUsers);
        }
      }
    }

    public IList<CardDetail> UpdateFreeCardsForChannelBasedOnTicket(ICollection<CardDetail> cardsAvailable, IUser user, out TvResult result)
    {
      if (LogEnabled)
      {
        Log.Debug("UpdateFreeCardsForChannelBasedOnTicket: user: {0}", user.Name);
      }
      
      var cardsFree = new List<CardDetail>();
      
      // first check if card can be added
      bool currLogEn = LogEnabled;
      LogEnabled = false;
      foreach (CardDetail cardDetail in cardsAvailable)
      {
        ICardTuneReservationTicket ticket = GetCardTuneReservationTicket(cardDetail.Card.IdCard);

        if (ticket != null)
        {
          cardDetail.SameTransponder = ticket.IsSameTransponder;
          cardDetail.NumberOfOtherUsers = ticket.NumberOfOtherUsersOnCurrentCard;
          LogNumberOfOtherUsersFound(cardDetail);
          IDictionary<int, ITvCardHandler> cards = _controller.CardCollection;
          IChannel tuningDetail = cardDetail.TuningDetail;
          
          for (int i = 0; i <= 2; i++)
          {
            // Try up to 3 times with increasing user priority level 
            bool checkTransponder = CheckTransponder(user, cards[cardDetail.Card.IdCard], tuningDetail, i);
            if (i == 0)
            {
              cardDetail.SameTranspCAMavail = checkTransponder;
            }
            if (checkTransponder)                                                                     
            {                                                                                         
              cardDetail.TransponderCheckLevel = i;
              cardsFree.Add(cardDetail);
              break;                                                         
            }     
          }                                                                                              
        }
      }
      LogEnabled = currLogEn;

      //Sort the list so that the 'most preferred' Card Details are at the front (see 'CardDetail.cs' for sort order)
      cardsFree.SortStable();

      if (cardsFree.Count > 0)
      {
        result = TvResult.Succeeded;
      }
      else
      {
        result = cardsAvailable.Count == 0 ? TvResult.ChannelNotMappedToAnyCard : TvResult.AllCardsBusy;
      }
      if (LogEnabled)
      {
        Log.Info("UpdateFreeCardsForChannelBasedOnTicket found {0} free card(s), user:{1}", cardsFree.Count, user.Name);
        for (int i = 0; i < cardsFree.Count; i++)
        {                                                                                           
          Log.Debug("UpdateFreeCardsForChannelBasedOnTicket, free card:{0}, id:{1}, STCA:{2}, ST:{3}, PRI:{4}, CL:{5}, NOU:{6}",
                          i, cardsFree[i].Id, cardsFree[i].SameTranspCAMavail, cardsFree[i].SameTransponder, cardsFree[i].Priority, 
                          cardsFree[i].TransponderCheckLevel, cardsFree[i].NumberOfOtherUsers);
        }                                                                                                     
      }

      return cardsFree;
    }

    private ICardTuneReservationTicket GetCardTuneReservationTicket(int cardId)
    {
      ICardTuneReservationTicket ticket;
      _tickets.TryGetValue(cardId, out ticket);
      return ticket;
    }

    #region overrides

    protected override bool CanCardTuneChannel(ITvCardHandler cardHandler, Channel dbChannel, IChannel tuningDetail)
    {
      return true;
    }

    protected override int GetNumberOfUsersOnCurrentChannel(ITvCardHandler tvcard, IUser user)
    {
      int numberOfUsersOnCurrentChannel = 0;
      ICardTuneReservationTicket ticket = GetCardTuneReservationTicket(tvcard.DataBaseCard.IdCard);

      if (ticket != null)
      {
        //TODO: check code
        numberOfUsersOnCurrentChannel = ticket.NumberOfUsersOnSameCurrentChannel;
      }
      return numberOfUsersOnCurrentChannel;
    }

    protected override bool IsFreeToAir(ITvCardHandler tvcard, ref IUser user)
    {
      bool isFreeToAir = true;
      ICardTuneReservationTicket ticket = GetCardTuneReservationTicket(tvcard.DataBaseCard.IdCard);

      if (ticket != null)
      {
        isFreeToAir = (ticket.IsFreeToAir);
      }
      return isFreeToAir;
    }

    protected override int NumberOfChannelsDecrypting(ITvCardHandler tvcard)
    {
      int numberOfChannelsDecrypting = 0;
      ICardTuneReservationTicket ticket = GetCardTuneReservationTicket(tvcard.DataBaseCard.IdCard);

      if (ticket != null)
      {
        numberOfChannelsDecrypting = (ticket.NumberOfChannelsDecrypting);
      }
      return numberOfChannelsDecrypting;
    }

    protected override bool IsCamAlreadyDecodingChannel(ITvCardHandler tvcard, IChannel tuningDetail)
    {
      bool isCamAlreadyDecodingChannel = false;
      ICardTuneReservationTicket ticket = GetCardTuneReservationTicket(tvcard.DataBaseCard.IdCard);

      if (ticket != null)
      {
        //TODO: check code
        isCamAlreadyDecodingChannel = (ticket.IsCamAlreadyDecodingChannel);
      }
      return isCamAlreadyDecodingChannel;
    }


    protected override bool IsOwnerOfCard(ITvCardHandler tvcard, IUser user)
    {
      bool isOwnerOfCard = false;
      ICardTuneReservationTicket ticket = GetCardTuneReservationTicket(tvcard.DataBaseCard.IdCard);

      if (ticket != null)
      {
        bool hasHighestPriority = ticket.HasHighestPriority;
        if (hasHighestPriority)
        {
          isOwnerOfCard = true;
        }
        else
        {
          bool hasEqualOrHigherPriority = ticket.HasEqualOrHigherPriority;
          if (hasEqualOrHigherPriority)
          {
            isOwnerOfCard = ticket.IsOwner;
          }
        }
      }
      return isOwnerOfCard;
    }

    protected override bool IsSameTransponder(ITvCardHandler tvcard, IChannel tuningDetail)
    {
      bool isSameTransponder = false;
      ICardTuneReservationTicket ticket = GetCardTuneReservationTicket(tvcard.DataBaseCard.IdCard);

      if (ticket != null)
      {
        isSameTransponder = ticket.IsSameTransponder;
      }
      return isSameTransponder;
    }

    protected override int NumberOfOtherUsersOnCurrentCard(ITvCardHandler tvcard, IUser user)
    {
      int numberOfOtherUsersOnCurrentCard = 0;
      ICardTuneReservationTicket ticket = GetCardTuneReservationTicket(tvcard.DataBaseCard.IdCard);

      if (ticket != null)
      {
        numberOfOtherUsersOnCurrentCard = ticket.NumberOfOtherUsersOnCurrentCard;
      }
      return numberOfOtherUsersOnCurrentCard;
    }

    #endregion

  }
}
