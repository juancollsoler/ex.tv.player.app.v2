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
using MediaPortal.Utils.Time;
using NUnit.Framework;

namespace MediaPortal.Tests.Utils.Time
{
  [TestFixture]
  [Category("BasicTime")]
  public class BasicTimeTest
  {
    [Test]
    public void Constructors()
    {
      BasicTime bt = new BasicTime("06:15");
      Assert.IsTrue(bt.Hour == 6);
      Assert.IsTrue(bt.Minute == 15);

      bt = new BasicTime("06h15");
      Assert.IsTrue(bt.Hour == 6);
      Assert.IsTrue(bt.Minute == 15);
    }

    [Test]
    public void OperatorsWithDateTime()
    {
      BasicTime bt = new BasicTime("06:00");

      DateTime dt = new DateTime(2009, 3, 5, 23, 30, 0, 0);
      Assert.IsTrue(bt < dt);

      dt = new DateTime(2009, 3, 5, 6, 0, 0, 0);
      Assert.IsFalse(bt < dt);
      Assert.IsTrue(bt <= dt);
      Assert.IsTrue(bt >= dt);

      dt = new DateTime(2009, 3, 5, 3, 0, 0, 0);
      Assert.IsTrue(bt > dt);
    }
  }
}