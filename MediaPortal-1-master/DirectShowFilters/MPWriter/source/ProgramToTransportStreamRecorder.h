#pragma once

#include "livemedia.hh"
#include "BasicUsageEnvironment.hh"
#include "MPTaskScheduler.h"
#include "MemoryBuffer.h"
#include "TSThread.h"
#include "FileSinkRecorder.h"
#include "MemoryStreamSource.h" 
#include "AnalogVideoAudioObserver.h"

class CProgramToTransportStreamRecorder: public TSThread
{
public:
	CProgramToTransportStreamRecorder(void);
	virtual ~CProgramToTransportStreamRecorder(void);
	void Initialize(wchar_t* fileNameOut);
	void SetProgramType(int programType);
	void ClearStreams();
	void Close();
	void Write(byte* data, int len);

	void StartBufferThread();
	void StopBufferThread();
	void Flush();
	virtual void ThreadProc();
	bool m_BufferThreadActive;
	void SetVideoAudioObserver(IAnalogVideoAudioObserver* callback);

private:
	UsageEnvironment* m_env;
	CMemoryBuffer m_buffer;
	CFileSinkRecorder* m_outputSink;
	CMemoryStreamSource* m_inputSource;
	FramedSource* m_tsFrames;
	bool m_bSendVideoAudioObserverEvents;
	bool m_bRunning;
	bool m_bStarting;
	int  m_iPacketsToSkip;
	IAnalogVideoAudioObserver* m_pCallback;
	int m_iProgramType;
};
