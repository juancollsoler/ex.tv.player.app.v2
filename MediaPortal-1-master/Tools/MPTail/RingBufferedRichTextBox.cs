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
using System.Windows.Forms;

namespace MPTail
{
  public class RingBufferedRichTextBox : RichTextBox
  {
    private long ringBufferSize = 1024 * 1024 * 3; // we init with 3 mb

    #region constructor

    public RingBufferedRichTextBox()
    {
      TextChanged += new EventHandler(RingBufferedRichTextBox_TextChanged);
    }

    #endregion

    private void RingBufferedRichTextBox_TextChanged(object sender, EventArgs e)
    {
      if (TextLength > ringBufferSize)
        Text = Text.Remove(0, (int)(TextLength - ringBufferSize));
    }

    public long RingBufferSizeInMB
    {
      get { return (ringBufferSize / 1024) / 1024; }
      set { ringBufferSize = 1024 * 1024 * value; }
    }
  }
}