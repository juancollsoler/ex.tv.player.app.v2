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
using System.IO;
using MediaPortal.Profile;
using NUnit.Framework;

namespace MediaPortal.Tests.Core.Profile
{
  [TestFixture]
  public class XmlSettingsPrefetchTest
  {
    [SetUp]
    public void Init()
    {
      rememberedSettings = new List<object[]>();
    }

    private List<object[]> rememberedSettings;

    private void Remember(string section, string entry, object value)
    {
      rememberedSettings.Add(new object[] {section, entry, value});
    }

    [Test]
    public void Prefetch1()
    {
      string xml =
        @"<?xml version=""1.0"" encoding=""utf-8""?>
<profile>
  <section name=""capture"">
    <entry name=""tuner"">Cable</entry>
  </section>
</profile>
";
      using (TextWriter writer = File.CreateText("prefetchtest.xml"))
      {
        writer.Write(xml);
      }

      XmlSettingsProvider provider = new XmlSettingsProvider("prefetchtest.xml");
      provider.Prefetch(this.Remember);

      Assert.AreEqual("capture", rememberedSettings[0][0]);
      Assert.AreEqual("tuner", rememberedSettings[0][1]);
      Assert.AreEqual("Cable", rememberedSettings[0][2]);
    }
  }
}