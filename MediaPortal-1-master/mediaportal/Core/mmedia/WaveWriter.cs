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
using System.IO;
using WaveLib;

namespace Yeti.MMedia

{
  /// <summary>
  /// Save RAW PCM data to a stream in WAVE format
  /// </summary>
  public class WaveWriter : AudioWriter

  {
    private const uint WaveHeaderSize = 38;

    private const uint WaveFormatSize = 18;

    private uint m_AudioDataSize = 0;

    private uint m_WrittenBytes = 0;

    private bool closed = false;


    public WaveWriter(Stream Output, WaveFormat Format, uint AudioDataSize)
      : base(Output, Format)

    {
      m_AudioDataSize = AudioDataSize;

      WriteWaveHeader();
    }


    public WaveWriter(Stream Output, WaveFormat Format)
      : base(Output, Format)

    {
      if (!OutStream.CanSeek)

      {
        throw new ArgumentException("The stream must supports seeking if AudioDataSize is not supported", "Output");
      }

      OutStream.Seek(WaveHeaderSize + 8, SeekOrigin.Current);
    }


    private byte[] Int2ByteArr(uint val)

    {
      byte[] res = new byte[4];

      for (int i = 0; i < 4; i++)

      {
        res[i] = (byte)(val >> (i * 8));
      }

      return res;
    }


    private byte[] Int2ByteArr(short val)

    {
      byte[] res = new byte[2];

      for (int i = 0; i < 2; i++)

      {
        res[i] = (byte)(val >> (i * 8));
      }

      return res;
    }


    protected void WriteWaveHeader()

    {
      Write(new byte[] {(byte)'R', (byte)'I', (byte)'F', (byte)'F'});

      Write(Int2ByteArr(m_AudioDataSize + WaveHeaderSize));

      Write(new byte[] {(byte)'W', (byte)'A', (byte)'V', (byte)'E'});

      Write(new byte[] {(byte)'f', (byte)'m', (byte)'t', (byte)' '});

      Write(Int2ByteArr(WaveFormatSize));

      Write(Int2ByteArr(m_InputDataFormat.wFormatTag));

      Write(Int2ByteArr(m_InputDataFormat.nChannels));

      Write(Int2ByteArr((uint)m_InputDataFormat.nSamplesPerSec));

      Write(Int2ByteArr((uint)m_InputDataFormat.nAvgBytesPerSec));

      Write(Int2ByteArr(m_InputDataFormat.nBlockAlign));

      Write(Int2ByteArr(m_InputDataFormat.wBitsPerSample));

      Write(Int2ByteArr(m_InputDataFormat.cbSize));

      Write(new byte[] {(byte)'d', (byte)'a', (byte)'t', (byte)'a'});

      Write(Int2ByteArr(m_AudioDataSize));

      m_WrittenBytes -= (WaveHeaderSize + 8);
    }


    public override void Close()

    {
      if (!closed)

      {
        if (m_AudioDataSize == 0)

        {
          Seek(-(int)m_WrittenBytes - (int)WaveHeaderSize - 8, SeekOrigin.Current);

          m_AudioDataSize = m_WrittenBytes;

          WriteWaveHeader();
        }
      }

      closed = true;

      base.Close();
    }


    public override void Write(byte[] buffer, int index, int count)

    {
      base.Write(buffer, index, count);

      m_WrittenBytes += (uint)count;
    }


    public override void Write(byte[] buffer)

    {
      base.Write(buffer);

      m_WrittenBytes += (uint)buffer.Length;
    }


    protected override int GetOptimalBufferSize()

    {
      return m_InputDataFormat.nAvgBytesPerSec / 10;
    }


    public static IEditAudioWriterConfig GetConfigControl(AudioWriterConfig config)

    {
      IEditAudioWriterConfig cfg = new EditWaveWriter();

      cfg.Config = config;

      return cfg;
    }
  }
}