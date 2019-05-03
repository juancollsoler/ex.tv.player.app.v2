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

using MediaPortal.Playlists;
using NUnit.Framework;

namespace MediaPortal.Tests.Core.Playlists
{
  [TestFixture]
  public class PlayListM3UTests
  {
    [SetUp]
    public void Init() {}

    [Test]
    public void LoadM3U()
    {
      PlayList playlist = new PlayList();
      IPlayListIO loader = new PlayListM3uIO();

      Assert.IsTrue(loader.Load(playlist, "Core\\Playlists\\TestData\\exampleList.m3u"), "playlist could not even load!");
      Assert.IsTrue(playlist[0].FileName.EndsWith("Bob Marley - 01 - Judge Not.mp3"));
      Assert.IsTrue(playlist[1].FileName.EndsWith("Bob Marley - 02 - One Cup of Coffee.mp3"));
      Assert.IsTrue(playlist[2].FileName.EndsWith("Bob Marley - 03 - Simmer Down.mp3"));
      Assert.IsTrue(playlist[3].FileName.EndsWith("Bob Marley - 05 - Guava Jelly.mp3"));
    }
  }
}