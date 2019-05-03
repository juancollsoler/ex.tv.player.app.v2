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

using Microsoft.DirectX.Direct3D;

namespace MediaPortal
{
  /// <summary>
  /// Various helper functions for graphics samples
  /// </summary>
  public class D3DUtil
  {
    /// <summary>
    /// Private Constructor 
    /// </summary>
    private D3DUtil() {}

    /// <summary>
    /// Gets the number of ColorChanelBits from a format
    /// </summary>
    public static int GetColorChannelBits(Format format)
    {
      switch (format)
      {
        case Format.R8G8B8:
          return 8;
        case Format.A8R8G8B8:
          return 8;
        case Format.X8R8G8B8:
          return 8;
        case Format.R5G6B5:
          return 5;
        case Format.X1R5G5B5:
          return 5;
        case Format.A1R5G5B5:
          return 5;
        case Format.A4R4G4B4:
          return 4;
        case Format.R3G3B2:
          return 2;
        case Format.A8R3G3B2:
          return 2;
        case Format.X4R4G4B4:
          return 4;
        case Format.A2B10G10R10:
          return 10;
        case Format.A2R10G10B10:
          return 10;
        default:
          return 0;
      }
    }


    /// <summary>
    /// Gets the number of alpha channel bits 
    /// </summary>
    public static int GetAlphaChannelBits(Format format)
    {
      switch (format)
      {
        case Format.R8G8B8:
          return 0;
        case Format.A8R8G8B8:
          return 8;
        case Format.X8R8G8B8:
          return 0;
        case Format.R5G6B5:
          return 0;
        case Format.X1R5G5B5:
          return 0;
        case Format.A1R5G5B5:
          return 1;
        case Format.A4R4G4B4:
          return 4;
        case Format.R3G3B2:
          return 0;
        case Format.A8R3G3B2:
          return 8;
        case Format.X4R4G4B4:
          return 0;
        case Format.A2B10G10R10:
          return 2;
        case Format.A2R10G10B10:
          return 2;
        default:
          return 0;
      }
    }


    /// <summary>
    /// Gets the number of depth bits
    /// </summary>
    public static int GetDepthBits(DepthFormat format)
    {
      switch (format)
      {
        case DepthFormat.D16:
          return 16;
        case DepthFormat.D15S1:
          return 15;
        case DepthFormat.D24X8:
          return 24;
        case DepthFormat.D24S8:
          return 24;
        case DepthFormat.D24X4S4:
          return 24;
        case DepthFormat.D32:
          return 32;
        default:
          return 0;
      }
    }


    /// <summary>
    /// Gets the number of stencil bits
    /// </summary>
    public static int GetStencilBits(DepthFormat format)
    {
      switch (format)
      {
        case DepthFormat.D16:
          return 0;
        case DepthFormat.D15S1:
          return 1;
        case DepthFormat.D24X8:
          return 0;
        case DepthFormat.D24S8:
          return 8;
        case DepthFormat.D24X4S4:
          return 4;
        case DepthFormat.D32:
          return 0;
        default:
          return 0;
      }
    }
  }
} ;