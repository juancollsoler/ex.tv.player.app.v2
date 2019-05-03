/*
    Copyright (C) 2007-2010 Team MediaPortal
    http://www.team-mediaportal.com

    This file is part of MediaPortal 2

    MediaPortal 2 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal 2 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal 2.  If not, see <http://www.gnu.org/licenses/>.
*/

#pragma once

#ifndef __PARAMETER_DEFINED
#define __PARAMETER_DEFINED

#include "MPIPTVSourceExports.h"

#include <tchar.h>

class MPIPTVSOURCE_API CParameter
{
public:
  CParameter(const TCHAR *name, const TCHAR *value);
  ~CParameter(void);

  // get parameter name
  // @return : the reference to parameter name
  TCHAR *GetName(void);

  // get parameter name length
  // @return : parameter name length or UINT_MAX if error
  unsigned int GetNameLength(void);

  // get parameter value
  // @return : the reference to parameter value
  TCHAR *GetValue(void);

  // get parameter value length
  // @return : parameter value length or UINT_MAX if error
  unsigned int GetValueLength(void);

  // clear stored parameter and its value
  void Clear(void);

  // test if instance is valid (if parameter name and its value are not NULL)
  // @return : true if instance is valid, false otherwise
  bool IsValid(void);

  // deep clone of current instance
  // @return : reference to clone of parameter
  CParameter* Clone(void);
protected:
  TCHAR *name;
  TCHAR *value;
};

typedef CParameter* PCParameter;

#endif
