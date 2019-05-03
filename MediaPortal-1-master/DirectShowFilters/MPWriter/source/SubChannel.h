/* 
*	Copyright (C) 2006-2008 Team MediaPortal
*	http://www.team-mediaportal.com
*
*  This Program is free software; you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation; either version 2, or (at your option)
*  any later version.
*   
*  This Program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
*  GNU General Public License for more details.
*   
*  You should have received a copy of the GNU General Public License
*  along with GNU Make; see the file COPYING.  If not, write to
*  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
*  http://www.gnu.org/copyleft/gpl.html
*
*/
#include "filewriter.h"
#include "ProgramToTransportStream.h"
#include "ProgramToTransportStreamRecorder.h"
#include "AnalogVideoAudioObserver.h"
#include "TeletextGrabber.h"

class CSubChannel: CUnknown
{
	CCritSec m_Lock; // Main renderer critical section
public:
	CSubChannel(LPUNKNOWN pUnk, HRESULT *phr,int id);
	virtual ~CSubChannel(void);
	HRESULT Write(PBYTE pbData, LONG lDataLength);
	HRESULT WriteTeletext(PBYTE pbData, LONG lDataLength);
	int Handle() { return m_id;}

	STDMETHODIMP SetRecordingFileNameW(wchar_t* pwszFileName);
	STDMETHODIMP StartRecord();
	STDMETHODIMP StopRecord();

	STDMETHODIMP SetTimeShiftFileNameW(wchar_t* pwszFileName);
	STDMETHODIMP SetChannelType(int channelType);
	STDMETHODIMP StartTimeShifting();
	STDMETHODIMP StopTimeShifting();
	STDMETHODIMP PauseTimeShifting(int onOff);
	STDMETHODIMP SetTimeShiftParams(int minFiles, int maxFiles, ULONG maxFileSize);
	STDMETHODIMP TTxSetCallBack(IAnalogTeletextCallBack* callback);
	STDMETHODIMP SetVideoAudioObserver(IAnalogVideoAudioObserver* callback);
	STDMETHODIMP SetRecorderVideoAudioObserver(IAnalogVideoAudioObserver* callback);


	CProgramToTransportStreamRecorder* m_pTsRecorder;
	wchar_t m_wstrRecordingFileName[1024];
	bool m_bIsRecording;
	CProgramToTransportStream* m_pTsWriter;
	wchar_t m_wstrTimeShiftFileName[1024];
	bool m_bIsTimeShifting;
	bool m_bPaused;
	CTeletextGrabber* m_pTeletextGrabber;
	int m_id;
};
