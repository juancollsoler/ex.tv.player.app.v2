/**
*  MultiFileWriter.h
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

#ifndef MULTIFILEWRITER
#define MULTIFILEWRITER

#include "FileWriter.h"
#include "CDiskBuff.h"
#include <vector>

//Variable size buffers are used - CDiskBuff::CDiskBuff(int size)
#define FULL_BUFFERS 256
#define NOT_FULL_BUFFERS 192

//Number of retries allowed when writing buffer files
#define FILE_WRITE_RETRIES 19

//System timer resolution in ms
#define SYS_TIMER_RES 5

typedef struct 
{
	long 	minFiles;
	long 	maxFiles;
	__int64	maxSize;
	__int64	chunkSize;
} MultiFileWriterParam;


class MultiFileWriter
{
public:
	MultiFileWriter(MultiFileWriterParam *pWriterParams);
	virtual ~MultiFileWriter();

	HRESULT Open(LPCWSTR pszFileName);
	HRESULT Close();
	HRESULT AddToBuffer(byte* pbData, int len, int newBuffSize);
	HRESULT DiscardBuffer();

	void GetPosition(__int64 * position, long * bufferId);

protected:
  	  
	HRESULT OpenFile();
	HRESULT WriteToDisk(PBYTE pbData, ULONG lDataLength);
	HRESULT GetAvailableDiskSpace(__int64* llAvailableDiskSpace);

	HRESULT PushBuffer();
	HRESULT NewBuffer(int size);
	void ClearBuffers();

	HRESULT PrepareTSFile();
	HRESULT CreateNewTSFile();
	HRESULT ReuseTSFile();

	HRESULT WriteTSBufferFile();
	HRESULT CleanupFiles();
	BOOL IsFileLocked(LPWSTR pFilename);

	HANDLE m_hTSBufferFile;
	LPWSTR m_pTSBufferFileName;
	CDiskBuff* m_pDiskBuffer;

	CCritSec m_Lock;
	CCritSec m_posnLock;

	FileWriter *m_pCurrentTSFile;
	std::vector<LPWSTR> m_tsFileNames;
	long m_filesAdded;
	long m_filesRemoved;
	long m_currentFilenameId;
	long m_currentFileId;

	long m_minTSFiles;
	long m_maxTSFiles;
	__int64 m_maxTSFileSize;
	__int64 m_chunkReserve;
	
	UINT m_maxBuffersUsed;
	BOOL m_bDiskFull;
	BOOL m_bBufferFull;

	CCritSec m_qLock;	
  std::vector<CDiskBuff*> m_writeQueue;
  typedef std::vector<CDiskBuff*>::iterator ivecDiskBuff;

  BOOL m_bThreadRunning;
  HANDLE m_hThreadProc;
  CAMEvent m_WakeThreadEvent;

	static unsigned __stdcall thread_function(void* p);
  unsigned __stdcall ThreadProc();
  HRESULT StartThread();
  void StopThread();
  DWORD m_dwTimerResolution;   //Timer resolution variable
  
  UINT m_totalBuffers;
  UINT m_totalWakes;
};

#endif
