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

using MediaPortal.Utils.Web;
using NUnit.Framework;

namespace MediaPortal.Tests.Utils.Web
{
  [TestFixture]
  [Category("Web")]
  public class HtmlStringTest
  {
    [Test]
    public void TagList()
    {
      MatchTagCollection tagList =
        HtmlString.TagList("<table><tr><#non tag><td><*junk></td><!-- comment --></tr></table>");
      Assert.IsTrue(tagList.Count == 7);
    }

    [Test]
    public void ToAscii()
    {
      string html = "Hello there <br> new line &amp; extra charaters &lt;&gt;";
      string ascii = "Hello there \n new line & extra charaters <>";

      Assert.IsTrue(ascii == HtmlString.ToAscii(html));
    }
  }
}