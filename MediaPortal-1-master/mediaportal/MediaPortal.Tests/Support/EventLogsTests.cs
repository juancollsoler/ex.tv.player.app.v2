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

using System.IO;
using MediaPortal.Support;
using NUnit.Framework;

namespace MediaPortal.Tests.Support
{
  [TestFixture]
  public class EventLogsTests
  {
    private string outputDir = "Support\\TestData\\TestOutput\\";
    private string[] logNames = {"Application", "System"};

    private class MyProcRunner : ProcessRunner
    {
      public bool ranTwice = false;
      public int count = 0;

      public override void Run()
      {
        count++;
      }
    }

    [SetUp]
    public void Init()
    {
      foreach (string logName in logNames)
      {
        if (File.Exists(outputDir + logName + ".evt"))
        {
          File.Delete(outputDir + logName + ".evt");
        }
      }
    }

    [Test]
    public void CreateLogs()
    {
      MyProcRunner runner = new MyProcRunner();
      PsLogEventsLogger logs = new PsLogEventsLogger(runner);

      logs.CreateLogs(outputDir);

      Assert.IsTrue(runner.Executable.EndsWith("psloglist.exe"), "Wrong process has been run!");
      Assert.AreEqual(2, runner.count);
    }
  }
}