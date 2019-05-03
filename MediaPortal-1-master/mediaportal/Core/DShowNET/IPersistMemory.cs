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
using System.Runtime.InteropServices;

namespace DShowNET
{
  [ComVisible(true), ComImport,
   Guid("BD1AE5E0-A6AE-11CE-BD37-504200C10000"),
   InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  internal interface IPersistMemory
  {
    #region "IPersist Methods"

    [PreserveSig]
    int GetClassID(
      [Out] out Guid pClassID);

    #endregion

    [PreserveSig]
    int IsDirty();

    [PreserveSig]
    int Load([In] IntPtr pMem, [In] uint cbSize);

    [PreserveSig]
    int Save([Out] IntPtr pMem, [In] bool fClearDirty, [In] uint cbSize);

    [PreserveSig]
    int GetSizeMax([Out] out uint pCbSize);

    [PreserveSig]
    int InitNew();
  }
}