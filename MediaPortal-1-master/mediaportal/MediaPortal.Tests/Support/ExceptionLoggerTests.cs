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
using System.IO;
using MediaPortal.Support;
using NUnit.Framework;

namespace MediaPortal.Tests.Support
{
  [TestFixture]
  public class ExceptionLoggerTests
  {
    [Test]
    public void CreateLogger()
    {
      try
      {
        throw new Exception("some message");
      }
      catch (Exception exc)
      {
        ILogCreator logger = new ExceptionLogger(exc);
        logger.CreateLogs("Support\\TestData\\TestOutput");
        Assert.IsTrue(File.Exists("Support\\TestData\\TestOutput\\exception.log"));
      }
    }
  }
}