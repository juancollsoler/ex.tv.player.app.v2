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

namespace TvLibrary
{
  /// <summary>
  /// class which holds all parameters needed during scanning for channels
  /// </summary>
  public class ScanParameters
  {
    private int _timeoutTune = 2;
    private int _timeoutPAT = 5;
    private int _timeoutCAT = 5;
    private int _timeoutPMT = 10;
    private int _timeoutSDT = 20;
    private int _timeoutAnalog = 20;

    private int _lnbLowFrequency = -1;
    private int _lnbHighFrequency = -1;
    private int _lnbSwitchFrequency = -1;
    private bool _useDefaultLnbFrequencies = true;
    private int _minFiles = 6;
    private int _maxFiles = 20;
    private UInt32 _maxFileSize = (256 * 1000 * 1000);

    /// <summary>
    /// Gets or sets the minimium number of timeshifting files.
    /// </summary>
    /// <value>The minimium files.</value>
    public int MinimumFiles
    {
      get { return _minFiles; }
      set { _minFiles = value; }
    }

    /// <summary>
    /// Gets or sets the maximum number of timeshifting files.
    /// </summary>
    /// <value>The maximum files.</value>
    public int MaximumFiles
    {
      get { return _maxFiles; }
      set { _maxFiles = value; }
    }

    /// <summary>
    /// Gets or sets the maximum filesize for each timeshifting file.
    /// </summary>
    /// <value>The maximum filesize.</value>
    public UInt32 MaximumFileSize
    {
      get { return _maxFileSize; }
      set { _maxFileSize = value; }
    }

    /// <summary>
    /// Gets or sets the use default LNB frequencies.
    /// </summary>
    /// <value>The use default LNB frequencies.</value>
    public bool UseDefaultLnbFrequencies
    {
      get { return _useDefaultLnbFrequencies; }
      set { _useDefaultLnbFrequencies = value; }
    }

    /// <summary>
    /// Gets or sets the LNB low frequency.
    /// </summary>
    /// <value>The LNB low frequency.</value>
    public int LnbLowFrequency
    {
      get { return _lnbLowFrequency; }
      set { _lnbLowFrequency = value; }
    }

    /// <summary>
    /// Gets or sets the LNB switch frequency.
    /// </summary>
    /// <value>The LNB switch frequency.</value>
    public int LnbSwitchFrequency
    {
      get { return _lnbSwitchFrequency; }
      set { _lnbSwitchFrequency = value; }
    }

    /// <summary>
    /// Gets or sets the LNB high frequency.
    /// </summary>
    /// <value>The LNB high frequency.</value>
    public int LnbHighFrequency
    {
      get { return _lnbHighFrequency; }
      set { _lnbHighFrequency = value; }
    }

    /// <summary>
    /// Gets or sets the time out PAT.
    /// </summary>
    /// <value>The time out PAT.</value>
    public int TimeOutPAT
    {
      get { return _timeoutPAT; }
      set { _timeoutPAT = value; }
    }

    /// <summary>
    /// Gets or sets the time out CAT.
    /// </summary>
    /// <value>The time out CAT.</value>
    public int TimeOutCAT
    {
      get { return _timeoutCAT; }
      set { _timeoutCAT = value; }
    }

    /// <summary>
    /// Gets or sets the time out PMT.
    /// </summary>
    /// <value>The time out PMT.</value>
    public int TimeOutPMT
    {
      get { return _timeoutPMT; }
      set { _timeoutPMT = value; }
    }

    /// <summary>
    /// Gets or sets the time out tune.
    /// </summary>
    /// <value>The time out tune.</value>
    public int TimeOutTune
    {
      get { return _timeoutTune; }
      set { _timeoutTune = value; }
    }

    /// <summary>
    /// Gets or sets the time out SDT.
    /// </summary>
    /// <value>The time out SDT.</value>
    public int TimeOutSDT
    {
      get { return _timeoutSDT; }
      set { _timeoutSDT = value; }
    }

    /// <summary>
    /// Gets or sets the time out Analog scanning.
    /// </summary>
    /// <value>The time out Analog scanning.</value>
    public int TimeOutAnalog
    {
      get { return _timeoutAnalog; }
      set { _timeoutAnalog = value; }
    }
  }
}