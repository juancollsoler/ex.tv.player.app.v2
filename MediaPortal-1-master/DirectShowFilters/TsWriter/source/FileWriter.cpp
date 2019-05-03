// Copyright (C) 2006-2015 Team MediaPortal
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

/**
*  FileWriter.cpp
*  Copyright (C) 2006-2007      nate
*
*  This file is part of TSFileSource, a directshow push source filter that
*  provides an MPEG transport stream output.
*
*  TSFileSource is free software; you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation; either version 2 of the License, or
*  (at your option) any later version.
*
*  TSFileSource is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
*
*  You should have received a copy of the GNU General Public License
*  along with TSFileSource; if not, write to the Free Software
*  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*
*  nate can be reached on the forums at
*    http://forums.dvbowners.com/
*/

#define _WIN32_WINNT 0x0502

#pragma warning(disable : 4995)
#include <streams.h>
#include "FileWriter.h"
#include <atlbase.h>

extern void LogDebug(const char *fmt, ...) ;
extern void LogDebug(const wchar_t *fmt, ...) ;

FileWriter::FileWriter() :
	m_hFile(INVALID_HANDLE_VALUE),
	m_pFileName(0),
	m_bChunkReserve(FALSE),
	m_chunkReserveSize(2000000),
	m_chunkReserveFileSize(0),
	m_maxFileSize(0),
	m_bWriteFailed(FALSE)
{
}

FileWriter::~FileWriter()
{
	CloseFile();
	if (m_pFileName)
		delete m_pFileName;
}

HRESULT FileWriter::GetFileName(LPWSTR *lpszFileName)
{
	*lpszFileName = m_pFileName;
	return S_OK;
}

HRESULT FileWriter::SetFileName(LPCWSTR pszFileName)
{
	// Is this a valid filename supplied
	CheckPointer(pszFileName,E_POINTER);

	long length = wcslen(pszFileName);

	if(length > MAX_PATH)
		return ERROR_FILENAME_EXCED_RANGE;

	// Take a copy of the filename

	if (m_pFileName)
	{
		delete[] m_pFileName;
		m_pFileName = NULL;
	}
	m_pFileName = new wchar_t[length+1];
	if (m_pFileName == NULL)
		return E_OUTOFMEMORY;

	wcscpy(m_pFileName,pszFileName);

	return S_OK;
}

//
// OpenFile
//
HRESULT FileWriter::OpenFile()
{
	// Is the file already opened
	if (m_hFile != INVALID_HANDLE_VALUE)
	{
		return NOERROR;
	}

	// Has a filename been set yet
	if (m_pFileName == NULL)
	{
		return ERROR_INVALID_NAME;
	}

	// Check if the file is being read by another process.
	// (which should result in a 'sharing violation' error)
	m_hFile = CreateFileW(m_pFileName,          // The filename
						 (DWORD) GENERIC_WRITE,           // File access
						 (DWORD) NULL,                    // Share access
						 NULL,                            // Security
						 (DWORD) OPEN_ALWAYS,             // Open flags
						 (DWORD) 0,                       // More flags
						 NULL);                           // Template
	if (m_hFile == INVALID_HANDLE_VALUE)
	{
		DWORD dwErr = GetLastError();
		return HRESULT_FROM_WIN32(dwErr);
	}
	CloseHandle(m_hFile);

	if (wcsstr(m_pFileName, L".ts.tsbuffer") == NULL)   // not tsbuffer files
	{
  	// Try to open the file in normal mode
  	m_hFile = CreateFileW(m_pFileName,           // The filename
  						 (DWORD) GENERIC_WRITE,            // File access
  						 (DWORD) FILE_SHARE_READ,          // Share access
  						 NULL,                             // Security
  						 (DWORD) OPEN_ALWAYS,              // Open flags
  						 (DWORD) FILE_ATTRIBUTE_NORMAL,    // More flags
  						 NULL);                            // Template
  }
  else  // tsbuffer files
  {
  	m_hFile = CreateFileW(m_pFileName,           // The filename
  						 (DWORD) GENERIC_WRITE,            // File access
  						 (DWORD) (FILE_SHARE_READ | FILE_SHARE_WRITE),          // Share access
  						 NULL,                             // Security
  						 (DWORD) OPEN_ALWAYS,              // Open flags
  						 //(DWORD) FILE_ATTRIBUTE_NORMAL,    // More flags
							 (DWORD) (FILE_ATTRIBUTE_NORMAL | FILE_FLAG_RANDOM_ACCESS),     // More flags
  						 NULL);                            // Template
  }

	if (m_hFile == INVALID_HANDLE_VALUE)
	{
		DWORD dwErr = GetLastError();
		return HRESULT_FROM_WIN32(dwErr);
	}

	SetFilePointer(0, FILE_END);
	m_chunkReserveFileSize = GetFilePointer();
	SetFilePointer(0, FILE_BEGIN);
  m_bWriteFailed = FALSE;

  //LogDebug(L"FileWriter: OpenFile(), file %s: m_chunkReserveFileSize %I64d, m_maxFileSize: %I64d", m_pFileName, m_chunkReserveFileSize, m_maxFileSize);			  

	return S_OK;
}

//
// CloseFile (close all files)
//
HRESULT FileWriter::CloseFile()
{  
	if (m_hFile != INVALID_HANDLE_VALUE)
	{  
   	__int64 currentPosition = GetFilePointer();
   	
   	if (m_bChunkReserve)
   	{
   		if (currentPosition < m_chunkReserveFileSize)
   		{
     		SetFilePointer(currentPosition, FILE_BEGIN);
     		SetEndOfFile(m_hFile);
   	  }
   	}
    
  	if (!CloseHandle(m_hFile))
  	{
  	  LogDebug(L"FileWriter: CloseFile(), CloseHandle(m_hFile) failed, m_hFile 0x%x", m_hFile);
  	}
  	m_hFile = INVALID_HANDLE_VALUE; // Invalidate the file
  	
  	// if (m_pFileName)
  	// {
    //   LogDebug(L"FileWriter: CloseFile() : %s", m_pFileName);			  
    // }
	}

	return S_OK;
}


BOOL FileWriter::IsFileInvalid()
{
	return (m_hFile == INVALID_HANDLE_VALUE);
}

DWORD FileWriter::SetFilePointer(__int64 llDistanceToMove, DWORD dwMoveMethod)
{
	LARGE_INTEGER li;
	li.QuadPart = llDistanceToMove;
	return ::SetFilePointer(m_hFile, li.LowPart, &li.HighPart, dwMoveMethod);
}

__int64 FileWriter::GetFilePointer()
{
	LARGE_INTEGER li;
	li.QuadPart = 0;
	li.LowPart = ::SetFilePointer(m_hFile, 0, &li.HighPart, FILE_CURRENT);
	return li.QuadPart;
}

HRESULT FileWriter::Write(PBYTE pbData, ULONG lDataLength)
{
	return WriteWithRetry(pbData, lDataLength, 0);
}

HRESULT FileWriter::WriteWithRetry(PBYTE pbData, ULONG lDataLength, int retries)
{
	HRESULT hr;

	// If the file has already been closed, don't continue
	if (m_hFile == INVALID_HANDLE_VALUE)
		return S_FALSE;

  __int64 currentPosition = GetFilePointer();

	if (m_bChunkReserve)
	{
		if ((currentPosition + lDataLength > m_chunkReserveFileSize) &&
			(m_chunkReserveFileSize < m_maxFileSize))
		{
			while (currentPosition + lDataLength > m_chunkReserveFileSize)
				m_chunkReserveFileSize += m_chunkReserveSize;

			if (m_chunkReserveFileSize > m_maxFileSize)
				m_chunkReserveFileSize = m_maxFileSize;

			SetFilePointer(m_chunkReserveFileSize, FILE_BEGIN);
			SetEndOfFile(m_hFile);
			DWORD dwPtr = SetFilePointer(currentPosition, FILE_BEGIN);
			if (dwPtr == INVALID_SET_FILE_POINTER)
			{
        LogDebug(L"FileWriter: Write() SetFilePointer() error, file %s: pointer %d, hr: %d", m_pFileName, currentPosition, dwPtr);			  
			}
		}
	}

	DWORD written = 0;
  
  for (int retryCnt = 0; retryCnt <= retries; retryCnt++)
  {
  	written = 0;
	  hr = WriteFile(m_hFile, (PVOID)pbData, (DWORD)lDataLength, &written, NULL);
  	if (hr && (written == (DWORD)lDataLength))
    {
      if (retryCnt > 0)
      {
        LogDebug(L"FileWriter: Write() retry, file %s: retries %d", m_pFileName, retryCnt);
      }
      m_bWriteFailed = FALSE;
	    return S_OK;
    }
    else if (retries == 0)
    {
      break;
    }
    
    Sleep(50);
	  DWORD dwPtr = SetFilePointer(currentPosition, FILE_BEGIN);
		if (dwPtr == INVALID_SET_FILE_POINTER)
		{
      LogDebug(L"FileWriter: Write() retry, SetFilePointer() error, file %s: pointer %d, hr: %d", m_pFileName, currentPosition, dwPtr);			  
		}		
  }

  //Failed to write after retries
  if (!m_bWriteFailed) //Only log the first failure in a series...
  {
    LogDebug(L"FileWriter: Error writing to file %s: written %d of expected %d bytes, hr: %d", m_pFileName, written, lDataLength, hr);
  }    
  m_bWriteFailed = TRUE;
  
  return S_FALSE;
}

void FileWriter::SetChunkReserve( __int64 chunkReserveSize, __int64 maxFileSize)
{
	m_bChunkReserve = (chunkReserveSize > 0);
	
  LogDebug(L"FileWriter: SetChunkReserve(), chunkReserveSize: %I64d, maxFileSize: %I64d, m_bChunkReserve: %d", chunkReserveSize, maxFileSize, m_bChunkReserve);		
  	  
	if (m_bChunkReserve)
	{
		m_chunkReserveSize = chunkReserveSize;
		m_chunkReserveFileSize = 0;
	}
	else
	{
	  m_chunkReserveSize = 2000000;
		m_chunkReserveFileSize = 0;
	}
	
	m_maxFileSize = maxFileSize;
}

