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
using System.Collections;

namespace MediaPortal.Util
{
  public class Tokens : IEnumerable
  {
    private string[] elements = null;

    public Tokens(string source, char[] delimiters)
    {
      if (source != null)
      {
        elements = source.Split(delimiters);
      }
    }

    // IEnumerable Interface Implementation:

    public TokenEnumerator GetEnumerator() // non-IEnumerable version
    {
      return new TokenEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator() // IEnumerable version
    {
      return (IEnumerator)new TokenEnumerator(this);
    }

    // Inner class implements IEnumerator interface:

    public class TokenEnumerator : IEnumerator
    {
      private int position = -1;
      private Tokens t;

      public TokenEnumerator(Tokens t)
      {
        this.t = t;
      }

      public bool MoveNext()
      {
        if (null == t.elements) return false;
        if (position < t.elements.Length - 1)
        {
          position++;
          return true;
        }
        else
        {
          return false;
        }
      }

      public void Reset()
      {
        position = -1;
      }

      public string Current // non-IEnumerator version: type-safe
      {
        get { return t.elements[position]; }
      }

      object IEnumerator.Current // IEnumerator version: returns object
      {
        get { return t.elements[position]; }
      }
    }
  }
}