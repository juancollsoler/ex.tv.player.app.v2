#region Copyright (C) 2005-2010 Team MediaPortal
/*
// Copyright (C) 2005-2010 Team MediaPortal
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
*/
#endregion

/*
_____________________________________________________________________________

                       LanguageMacros
_____________________________________________________________________________

  These macros are used to simplify the translation files
  so that translators have to deal with as little with the
  scripting language as possible.
*/


!ifndef ___LanguageMacros__NSH___
!define ___LanguageMacros__NSH___


!macro LANG_LOAD LANGLOAD
!ifdef MUI_INCLUDED
  !insertmacro MUI_LANGUAGE "${LANGLOAD}"
!endif
  !include "${git_InstallScripts}\languages\${LANGLOAD}.nsh"
  !undef LANG
!macroend
 
!macro LANG_STRING NAME VALUE
  LangString "${NAME}" "${LANG_${LANG}}" "${VALUE}"
!macroend

!endif # !___LanguageMacros__NSH___
