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

using System.Collections.Generic;
using TvLibrary.ChannelLinkage;
using TvLibrary.Epg;

namespace TvLibrary.Interfaces
{
  public delegate void OnNewSubChannelDelegate(int id);
  /// <summary>
  /// cam types
  /// </summary>
  public enum CamType
  {
    /// <summary>
    /// Default
    /// </summary>
    Default = 0,
    /// <summary>
    /// Astoncrypt 2 cam
    /// </summary>
    Astoncrypt2 = 1
  }

  /// <summary>
  /// interface for a tv card
  /// </summary>
  public interface ITVCard
  {
    #region properties

    /// <summary>
    /// Gets a value indicating whether card supports subchannels
    /// </summary>
    /// <value><c>true</c> if card supports sub channels; otherwise, <c>false</c>.</value>
    bool SupportsSubChannels { get; }

    /// <summary>
    /// Gets wether or not card supports pausing the graph.
    /// </summary>
    bool SupportsPauseGraph { get; }


    /// <summary>
    /// Gets or sets the timeout parameters.
    /// </summary>
    /// <value>The parameters.</value>
    ScanParameters Parameters { get; set; }

    /// <summary>
    /// Gets/sets the card name
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets/Sets that the card is present
    /// </summary>
    bool CardPresent { get; set; }

    /// <summary>
    /// Gets/sets the card device
    /// </summary>		
    string DevicePath { get; }

    /// <summary>
    /// Method to check if card can tune to the channel specified
    /// </summary>
    /// <returns>true if card can tune to the channel otherwise false</returns>
    bool CanTune(IChannel channel);

    /// <summary>
    /// Stops the current graph
    /// </summary>
    /// <returns></returns>
    void StopGraph();

    /// <summary>
    /// Pauses the current graph
    /// </summary>
    /// <returns></returns>
    void PauseGraph();

    /// <summary>
    /// returns the min. channel number for analog cards
    /// </summary>
    int MinChannel { get; }

    /// <summary>
    /// returns the max. channel number for analog cards
    /// </summary>
    /// <value>The max channel.</value>
    int MaxChannel { get; }

    /// <summary>
    /// Gets or sets the type of the cam.
    /// </summary>
    /// <value>The type of the cam.</value>
    CamType CamType { get; set; }

    /// <summary>
    /// Gets/sets the card type
    /// </summary>
    CardType CardType { get; }

    /// <summary>
    /// Gets the interface for controlling the diseqc motor
    /// </summary>
    /// <value>Theinterface for controlling the diseqc motor.</value>
    IDiSEqCMotor DiSEqCMotor { get; }

    /// <summary>
    /// Gets the number of channels the card is currently decrypting.
    /// </summary>
    /// <value>The number of channels decrypting.</value>
    int NumberOfChannelsDecrypting { get; }

    /// <summary>
    /// Does the card have a CA module.
    /// </summary>
    /// <value>The number of channels decrypting.</value>
    bool HasCA { get; }

    #endregion

    #region Channel linkage handling

    /// <summary>
    /// Starts scanning for linkage info
    /// </summary>
    void StartLinkageScanner(BaseChannelLinkageScanner callback);

    /// <summary>
    /// Stops/Resets the linkage scanner
    /// </summary>
    void ResetLinkageScanner();

    /// <summary>
    /// Returns the channel linkages grabbed
    /// </summary>
    List<PortalChannel> ChannelLinkages { get; }

    #endregion

    #region epg & scanning

    /// <summary>
    /// Register to receive EPG related events.
    /// </summary>
    /// <param name="eventListener">The event listener.</param>
    void RegisterEpgEventListener(IEpgEvents eventListener);

    /// <summary>
    /// Grabs the epg.
    /// </summary>
    /// <param name="callback">The callback which gets called when epg is received or canceled.</param>
    void GrabEpg(BaseEpgGrabber callback);

    /// <summary>
    /// Start grabbing the epg while timeshifting
    /// </summary>
    void GrabEpg();

    /// <summary>
    /// Aborts grabbing the epg. This also triggers the OnEpgReceived callback.
    /// </summary>
    void AbortGrabbing();

    /// <summary>
    /// returns a list of all epg data for each channel found.
    /// </summary>
    /// <value>The epg.</value>
    List<EpgChannel> Epg { get; }

    /// <summary>
    /// returns the ITVScanning interface used for scanning channels
    /// </summary>
    ITVScanning ScanningInterface { get; }

    #endregion

    #region tuning & recording

    /// <summary>
    /// Tunes the specified channel.
    /// </summary>
    /// <param name="subChannelId">The sub channel id.</param>
    /// <param name="channel">The channel.</param>
    /// <returns>true if succeeded else false</returns>
    ITvSubChannel Tune(int subChannelId, IChannel channel);

    /// <summary>
    /// Scans the specified channel.
    /// </summary>
    /// <param name="subChannelId">The sub channel id.</param>
    /// <param name="channel">The channel.</param>
    /// <returns>true if succeeded else false</returns>
    ITvSubChannel Scan(int subChannelId, IChannel channel);

    #endregion

    #region quality control

    /// <summary>
    /// Get/Set the quality
    /// </summary>
    IQuality Quality { get; set; }

    /// <summary>
    /// Property which returns true if card supports quality control
    /// </summary>
    bool SupportsQualityControl { get; }

    /// <summary>
    /// Reloads the card configuration
    /// </summary>
    void ReloadCardConfiguration();

    #endregion

    #region properties

    /// <summary>
    /// Returns if the tuner belongs to a hybrid card
    /// </summary>
    bool IsHybrid { get; set; }

    /// <summary>
    /// When the tuner is locked onto a signal this property will return true
    /// otherwise false
    /// </summary>
    bool IsTunerLocked { get; }

    /// <summary>
    /// returns the signal quality
    /// </summary>
    int SignalQuality { get; }

    /// <summary>
    /// returns the signal level
    /// </summary>
    int SignalLevel { get; }

    /// <summary>
    /// Updates the signal state for a card.
    /// </summary>
    void ResetSignalUpdate();

    /// <summary>
    /// Gets or sets the context.
    /// </summary>
    /// <value>The context.</value>
    object Context { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this card is epg grabbing.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is epg grabbing; otherwise, <c>false</c>.
    /// </value>
    bool IsEpgGrabbing { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this card is scanning for channels.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this card is scanning; otherwise, <c>false</c>.
    /// </value>
    bool IsScanning { get; set; }

    #endregion

    #region idisposable

    /// <summary>
    /// Disposes this instance.
    /// </summary>
    void Dispose();

    #endregion

    #region sub channels

    /// <summary>
    /// Gets the sub channel.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    ITvSubChannel GetSubChannel(int id);
    ITvSubChannel GetFirstSubChannel();

    /// <summary>
    /// Frees the sub channel.
    /// </summary>
    /// <param name="id">The id.</param>
    void FreeSubChannel(int id);

    /// <summary>
    /// Gets the sub channels.
    /// </summary>
    /// <value>The sub channels.</value>
    ITvSubChannel[] SubChannels { get; }

    #endregion

    void CancelTune(int subChannel);    
    event OnNewSubChannelDelegate OnNewSubChannelEvent;
  }
}