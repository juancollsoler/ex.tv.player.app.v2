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
using TvControl;
using TvDatabase;
using TvLibrary.Interfaces;

namespace TvService
{
  public class AdvancedCardAllocationStatic : AdvancedCardAllocation
  {
    public AdvancedCardAllocationStatic(TvBusinessLayer businessLayer, TVController controller)
      : base(businessLayer, controller)
    {      
    }    

    #region overrides

    public override bool CheckTransponder(IUser user, ITvCardHandler tvcard, IChannel tuningDetail, int checkLevel)
    {
      return (checkLevel != 0);
    }

    protected override bool IsSameTransponder(ITvCardHandler tvcard, IChannel tuningDetail)
    {
      return false;
    }

    protected override int NumberOfOtherUsersOnCurrentCard(ITvCardHandler card, IUser user)
    {
      return 0;
    }   

    protected override bool IsStatic()
    {
      return true;
    }

    #endregion
  }
}
