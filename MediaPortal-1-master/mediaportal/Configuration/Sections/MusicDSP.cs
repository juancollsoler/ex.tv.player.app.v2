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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using MediaPortal.GUI.Library;
using MediaPortal.MusicPlayer.BASS;
using MediaPortal.Player;
using MediaPortal.Player.DSP;
using MediaPortal.UserInterface.Controls;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.AddOn.Vst;
using Un4seen.Bass.AddOn.WaDsp;
using Un4seen.Bass.Misc;

namespace MediaPortal.Configuration.Sections
{
  public partial class MusicDSP : SectionSettings
  {
    #region Variables

    // Private Variables
    private string pluginPath = Path.Combine(Application.StartupPath, @"musicplayer\plugins\dsp");
    private int _stream;
    private string _initialMusicDirectory;
    // BASS DSP / FX variables
    private DSP_Gain _gain = null;
    private BASS_BFX_DAMP _damp = null;
    private BASS_BFX_COMPRESSOR _comp = null;
    private int _dampHandle = 0;
    private int _dampPrio = 3;
    private int _compHandle = 0;
    private int _compPrio = 2;
    // VST Related variables
    private int _vstHandle;
    private VSTPROC _vstProc;
    private Dictionary<string, int> _vstHandles = new Dictionary<string, int>();
    // Winamp related variables
    private int _waDspPlugin;
    private Dictionary<string, int> _waDspPlugins = new Dictionary<string, int>();

    // Protected Variables

    // Public Variables

    #endregion

    #region Constructors/Destructors

    public MusicDSP()
      : this("Music DSP") {}

    public MusicDSP(string name)
      : base(name)
    {
      InitializeComponent();

      // Init DSP specific vars
      try
      {
        BassWaDsp.BASS_WADSP_Init(this.Handle);
      }
      catch
      {
        Log.Debug("Couldn't Init DSP specific vars");
      }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Set the Tooltip and strings for the about page, when loading the control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MusicDSP_Load(object sender, EventArgs e)
    {
      toolTip.SetToolTip(textBoxMusicFile, "Select a Music file to test the effects.");
      // DSP Page
      toolTip.SetToolTip(DSPTabPg, "Allows setting of BASS DSP Effects.");
      toolTip.SetToolTip(groupBoxGain, "Amplifies the channel signal by a given factor.");
      toolTip.SetToolTip(checkBoxDAmp, "Applies dynamic Amplification with the selected Preset.");
      toolTip.SetToolTip(trackBarGain, "Changes the db value for the Amplification.");
      toolTip.SetToolTip(textBoxGainDBValue, "Enter the Gain Value in db.");
      toolTip.SetToolTip(groupBoxCompressor,
                         "Compressors are commonly used to control the level, by making loud passages quieter, and quiet passages louder.");
      toolTip.SetToolTip(checkBoxCompressor,
                         "Turn on the Compressor.\r\nCompressors are commonly used to control the level, by making loud passages quieter, and quiet passages louder.");
      toolTip.SetToolTip(trackBarCompressor, "Changes the threshold for the Compressor.");
      // VST / Winamp Page
      toolTip.SetToolTip(listBoxFoundPlugins, "Lists all VST / Winamp compatible plugins found in the Plugin directory.");
      toolTip.SetToolTip(listBoxSelectedPlugins,
                         "Lists all enabled VST /Winamp plugins.\r\nDouble click to open the Config dialogue.\r\n(If the plugin offers one)");
    }

    /// <summary>
    /// Select the Music file to Play for testing the DSP Settings
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btFileselect_Click(object sender, EventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.InitialDirectory = _initialMusicDirectory;
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        textBoxMusicFile.Text = ofd.FileName;
        _initialMusicDirectory = Path.GetDirectoryName(ofd.FileName);
      }
    }

    /// <summary>
    /// Play the selected Music File
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btPlay_Click(object sender, EventArgs e)
    {
      if (File.Exists(textBoxMusicFile.Text))
      {
        // Init BASS
        BassAudioEngine bassEngine = BassMusicPlayer.Player;
        if (bassEngine.BassFreed)
        {
          bassEngine.InitBass();
        }

        _stream = Bass.BASS_StreamCreateFile(textBoxMusicFile.Text, 0, 0,
                                             BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_AUTOFREE |
                                             BASSFlag.BASS_SAMPLE_SOFTWARE);
        if (_stream != 0)
        {
          // Attach the BASS DSP Effects to the stream
          if (_gain != null)
          {
            _gain.ChannelHandle = _stream;
            _gain.Start();
          }

          if (checkBoxDAmp.Checked)
          {
            _dampHandle = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_BFX_DAMP, _dampPrio);
            Bass.BASS_FXSetParameters(_dampHandle, _damp);
          }

          if (checkBoxCompressor.Checked)
          {
            _compHandle = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_BFX_COMPRESSOR, _compPrio);
            Bass.BASS_FXSetParameters(_compHandle, _comp);
          }

          // Attach the plugins to the stream
          foreach (DSPPluginInfo dsp in listBoxSelectedPlugins.Items)
          {
            if (dsp.DSPPluginType == DSPPluginInfo.PluginType.VST)
            {
              _vstHandle = BassVst.BASS_VST_ChannelSetDSP(_stream, dsp.FilePath, BASSVSTDsp.BASS_VST_DEFAULT, 1);
              // Copy the parameters of the old handle
              int vstold = _vstHandles[dsp.Name];
              BassVst.BASS_VST_SetParamCopyParams(vstold, _vstHandle);
              // Now find out to which stream the old handle was assigned and free it
              BASS_VST_INFO bassvstinfo = new BASS_VST_INFO();
              BassVst.BASS_VST_GetInfo(vstold, bassvstinfo);
              BassVst.BASS_VST_ChannelRemoveDSP(bassvstinfo.channelHandle, vstold);
              _vstHandles[dsp.Name] = _vstHandle;
            }
            else
            {
              _waDspPlugin = _waDspPlugins[dsp.FilePath];
              BassWaDsp.BASS_WADSP_Start(_waDspPlugin, 0, 0);
              BassWaDsp.BASS_WADSP_ChannelSetDSP(_waDspPlugin, _stream, 1);
            }
          }
          btPlay.Enabled = false;
          btStop.Enabled = true;
          Bass.BASS_ChannelPlay(_stream, false);
        }
        else
        {
          MessageBox.Show("Can't play file. Probably not a valid music file");
        }
      }
      else
      {
        MessageBox.Show("File specified does not exist");
      }
    }

    /// <summary>
    /// Stop Playing the active Music File
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btStop_Click(object sender, EventArgs e)
    {
      btPlay.Enabled = true;
      btStop.Enabled = false;
      // Stop the DSP Gain
      if (_gain != null)
      {
        _gain.Stop();
      }

      foreach (DSPPluginInfo dsp in listBoxSelectedPlugins.Items)
      {
        // Save the VST plugin parameters before freeing the stream
        if (dsp.DSPPluginType == DSPPluginInfo.PluginType.VST)
        {
          _vstHandle = BassVst.BASS_VST_ChannelSetDSP(0, dsp.FilePath, BASSVSTDsp.BASS_VST_DEFAULT, 1);
          // Copy the parameters of the old handle
          int vstold = _vstHandles[dsp.Name];
          BassVst.BASS_VST_SetParamCopyParams(vstold, _vstHandle);
          _vstHandles[dsp.Name] = _vstHandle;
        }
        else
        {
          // Stop the WinAmp DSP
          _waDspPlugin = _waDspPlugins[dsp.FilePath];
          BassWaDsp.BASS_WADSP_Stop(_waDspPlugin);
        }
      }
      Bass.BASS_ChannelStop(_stream);
    }

    /// <summary>
    /// Sets the parameter for a given Bass effect
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="format"></param>
    private void setBassDSP(string id, string name, string value)
    {
      switch (id)
      {
        case "Gain":
          if (name == "Gain_dbV")
          {
            double gainDB = double.Parse(value);
            SetDSPGain(gainDB);
            textBoxGainDBValue.Text = value;
            trackBarGain.Value = (int)(gainDB * 1000d);
          }
          break;

        case "DynAmp":
          if (name == "Preset")
          {
            checkBoxDAmp.Checked = true;
            comboBoxDynamicAmplification.SelectedIndex = Convert.ToInt32(value);
            SetDAmpPreset(Convert.ToInt32(value));
          }
          break;

        case "Compressor":
          if (name == "Threshold")
          {
            checkBoxCompressor.Checked = true;
            trackBarCompressor.Value = Convert.ToInt32(value);
          }
          break;
      }
    }

    #region VST / Winamp Plugins

    /// <summary>
    /// Add the selected VST plugin(s) to the Selected Plugin Listbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void buttonPluginAdd_Click(object sender, EventArgs e)
    {
      DSPPluginInfo pluginInfo = (DSPPluginInfo)listBoxFoundPlugins.SelectedItem;

      if (pluginInfo == null)
      {
        return;
      }

      if (pluginInfo.DSPPluginType == DSPPluginInfo.PluginType.VST)
      {
        // Get the VST handle and enable it
        _vstHandle = BassVst.BASS_VST_ChannelSetDSP(0, pluginInfo.FilePath, BASSVSTDsp.BASS_VST_DEFAULT, 1);
        if (_vstHandle > 0)
        {
          _vstHandles[pluginInfo.Name] = _vstHandle;
          listBoxSelectedPlugins.Items.Add(listBoxFoundPlugins.SelectedItem);
          listBoxFoundPlugins.Items.RemoveAt(listBoxFoundPlugins.SelectedIndex);
        }
        else
        {
          MessageBox.Show("Error loading VST Plugin. Probably not valid", "VST Plugin", MessageBoxButtons.OK);
          Log.Debug("Couldn't load VST Plugin {0}. Error code: {1}", pluginInfo.Name, Bass.BASS_ErrorGetCode());
        }
      }
      else
      {
        // Get the winamp handle and enable it
        _waDspPlugin = BassWaDsp.BASS_WADSP_Load(pluginInfo.FilePath, 5, 5, 100, 100, null);
        if (_waDspPlugin > 0)
        {
          _waDspPlugins[pluginInfo.FilePath] = _waDspPlugin;
          listBoxSelectedPlugins.Items.Add(listBoxFoundPlugins.SelectedItem);
          listBoxFoundPlugins.Items.RemoveAt(listBoxFoundPlugins.SelectedIndex);
        }
        else
        {
          MessageBox.Show("Error loading WinAmp Plugin. Probably not valid", "WinAmp Plugin", MessageBoxButtons.OK);
          Log.Debug("Couldn't load WinAmp Plugin {0}. Error code: {1}", pluginInfo.FilePath, Bass.BASS_ErrorGetCode());
        }
      }
    }

    /// <summary>
    /// Don't use the selected VST Plugin anymore
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void buttonPluginRemove_Click(object sender, EventArgs e)
    {
      DSPPluginInfo pluginInfo = (DSPPluginInfo)listBoxSelectedPlugins.SelectedItem;

      if (pluginInfo == null)
      {
        return;
      }

      if (pluginInfo.DSPPluginType == DSPPluginInfo.PluginType.VST)
      {
        // Remove VST Handle
        BassVst.BASS_VST_ChannelRemoveDSP(0, _vstHandles[pluginInfo.Name]);
        _vstHandles.Remove(pluginInfo.Name);
      }
      else
      {
        // Remove Winamp Handle
        BassWaDsp.BASS_WADSP_FreeDSP(_waDspPlugins[pluginInfo.FilePath]);
        _waDspPlugins.Remove(pluginInfo.Name);
      }
      listBoxFoundPlugins.Items.Add(listBoxSelectedPlugins.SelectedItem);
      listBoxSelectedPlugins.Items.RemoveAt(listBoxSelectedPlugins.SelectedIndex);
    }

    /// <summary>
    /// Open VST Plugin Configuration window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ShowConfigWindow()
    {
      DSPPluginInfo pluginInfo = (DSPPluginInfo)listBoxSelectedPlugins.SelectedItem;

      if (pluginInfo == null)
      {
        return;
      }

      if (pluginInfo.DSPPluginType == DSPPluginInfo.PluginType.VST)
      {
        _vstHandle = _vstHandles[pluginInfo.Name];
        BASS_VST_INFO vstInfo = new BASS_VST_INFO();
        if (BassVst.BASS_VST_GetInfo(_vstHandle, vstInfo) && vstInfo.hasEditor)
        {
          // Set a handle to the callback procedure
          _vstProc = new VSTPROC(vstEditorCallBack);
          BassVst.BASS_VST_SetCallback(_vstHandle, _vstProc, IntPtr.Zero);
          // create a new System.Windows.Forms.Form
          Form f = new MPConfigForm();
          f.Width = vstInfo.editorWidth + 4;
          f.Height = vstInfo.editorHeight + 34;
          f.Closing += new CancelEventHandler(f_Closing);
          f.Text = vstInfo.effectName;
          BassVst.BASS_VST_EmbedEditor(_vstHandle, f.Handle);
          f.ShowDialog();
        }
        else
        {
          MessageBox.Show("Plugin has no Configuration");
        }
      }
      else
      {
        _waDspPlugin = _waDspPlugins[pluginInfo.FilePath];
        BassWaDsp.BASS_WADSP_Start(_waDspPlugin, 0, 0);
        BassWaDsp.BASS_WADSP_Config(_waDspPlugin);
      }
    }

    /// <summary>
    /// The VST Editor window has been closed. Free the resources
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void f_Closing(object sender, CancelEventArgs e)
    {
      // unembed the VST editor
      BassVst.BASS_VST_EmbedEditor(_vstHandle, IntPtr.Zero);
    }

    /// <summary>
    /// This routine is called, whenever a change is done in the VST Editor
    /// </summary>
    /// <param name="vstEditor"></param>
    /// <param name="action"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    private int vstEditorCallBack(int vstEditor, BASSVSTAction action, int param1, int param2, IntPtr user)
    {
      switch (action)
      {
        case BASSVSTAction.BASS_VST_PARAM_CHANGED:
          // Some slider has been changed in the editor
          BASS_VST_PARAM_INFO paramInfo = new BASS_VST_PARAM_INFO();
          for (int i = BassVst.BASS_VST_GetParamCount(vstEditor) - 1; i >= 0; i--)
          {
            BassVst.BASS_VST_SetParam(_vstHandle, i, BassVst.BASS_VST_GetParam(vstEditor, i));
            BassVst.BASS_VST_GetParamInfo(_vstHandle, i, paramInfo);
          }
          break;
        case BASSVSTAction.BASS_VST_EDITOR_RESIZED:
          // the editor window requests a new size,
          break;
        case BASSVSTAction.BASS_VST_AUDIO_MASTER:
          break;
      }
      return 0;
    }

    private void listBoxFoundPlugins_DoubleClick(object sender, EventArgs e)
    {
      buttonPluginAdd_Click(sender, e);
    }

    private void listBoxSelectedPlugins_DoubleClick(object sender, EventArgs e)
    {
      buttonPluginRemove_Click(sender, e);
    }

    private void btnConfig_Click(object sender, EventArgs e)
    {
      ShowConfigWindow();
    }

    #endregion

    #region DSP Gain

    private void buttonSetGain_Click(object sender, EventArgs e)
    {
      if (_gain == null)
      {
        _gain = new DSP_Gain();
      }

      try
      {
        double gainDB = double.Parse(this.textBoxGainDBValue.Text);
        trackBarGain.Value = (int)(gainDB * 1000d);
        SetDSPGain(gainDB);
      }
      catch {}
    }

    private void trackBarGain_ValueChanged(object sender, EventArgs e)
    {
      if (_gain == null)
      {
        _gain = new DSP_Gain();
      }

      this.textBoxGainDBValue.Text = Convert.ToString(trackBarGain.Value / 1000d);
      buttonSetGain_Click(this, EventArgs.Empty);
    }

    private void SetDSPGain(double gainDB)
    {
      if (_gain == null)
      {
        _gain = new DSP_Gain();
      }

      if (gainDB == 0.0)
      {
        _gain.SetBypass(true);
      }
      else
      {
        _gain.SetBypass(false);
        _gain.Gain_dBV = gainDB;
      }
    }

    #endregion DSP Gain

    #region Dynamic Amplification

    private void checkBoxDAmp_CheckedChanged(object sender, EventArgs e)
    {
      comboBoxDynamicAmplification.Enabled = checkBoxDAmp.Checked;
      if (comboBoxDynamicAmplification.SelectedIndex == -1)
      {
        comboBoxDynamicAmplification.SelectedIndex = 0;
      }

      if (_stream == 0)
      {
        return;
      }

      if (checkBoxDAmp.Checked)
      {
        SetDAmpPreset(comboBoxDynamicAmplification.SelectedIndex);
        _dampHandle = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_BFX_DAMP, _dampPrio);
        Bass.BASS_FXSetParameters(_dampHandle, _damp);
      }
      else
      {
        Bass.BASS_FXReset(_dampHandle);
      }
    }

    private void SetDAmpPreset(int preset)
    {
      if (_damp == null)
      {
        _damp = new BASS_BFX_DAMP();
      }

      switch (preset)
      {
        case 0:
          _damp.Preset_Soft();
          break;
        case 1:
          _damp.Preset_Medium();
          break;
        case 2:
          _damp.Preset_Hard();
          break;
      }
    }

    #endregion Dynamic Amplification

    #region Compressor

    private void checkBoxCompressor_CheckedChanged(object sender, EventArgs e)
    {
      if (_comp == null)
      {
        _comp = new BASS_BFX_COMPRESSOR();
      }

      if (_stream == 0)
      {
        return;
      }

      if (checkBoxCompressor.Checked)
      {
        _comp.Preset_Medium();
        _compHandle = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_BFX_COMPRESSOR, _compPrio);
        Bass.BASS_FXSetParameters(_compHandle, _comp);
      }
      else
      {
        Bass.BASS_FXReset(_compHandle);
      }
    }

    private void trackBarCompressor_ValueChanged(object sender, EventArgs e)
    {
      labelCompThreshold.Text = String.Format("Threshold: {0:#0.0} dB", trackBarCompressor.Value / 10d);

      if (_stream == 0)
      {
        return;
      }

      _comp.fThreshold = (float)Un4seen.Bass.Utils.DBToLevel(trackBarCompressor.Value / 10d, 1.0);
      Bass.BASS_FXSetParameters(_stream, _comp);
    }

    #endregion Compressor

    #endregion

    #region SectionSettings Overloads

    public override void OnSectionActivated()
    {
      //
      // Disable the complete Tab Page, when the Music Player is not the BASS engine
      //
      SectionSettings section = GetSection("Music");

      if (section != null)
      {
        string player = (string)section.GetSetting("audioPlayer");
        if (player.ToLowerInvariant().IndexOf("internal dshow player") > -1)
        {
          MusicDSPTabCtl.Enabled = false;
          MessageBox.Show(this, "DSP effects are only available with the BASS music player selected.",
                          "MediaPortal - Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          MusicDSPTabCtl.Enabled = true;
        }
      }
    }

    /// <summary>
    /// Load Effects Settings
    /// </summary>
    public override void LoadSettings()
    {
      _initialMusicDirectory = Settings.Instance.MusicDirectory;

      // BASS DSP/FX
      foreach (BassEffect basseffect in Settings.Instance.BassEffects)
      {
        foreach (BassEffectParm parameter in basseffect.Parameter)
        {
          setBassDSP(basseffect.EffectName, parameter.Name, parameter.Value);
        }
      }

      DirectoryInfo di = new DirectoryInfo(pluginPath);
      if (Directory.Exists(pluginPath))
      {
        FileInfo[] fi = di.GetFiles("*.dll", SearchOption.AllDirectories);
        foreach (FileInfo vstplugin in fi)
        {
          try
          {
            BASS_VST_INFO vstInfo = new BASS_VST_INFO();
            _vstHandle = BassVst.BASS_VST_ChannelSetDSP(0, vstplugin.FullName, BASSVSTDsp.BASS_VST_DEFAULT, 1);
            // When Handle > 0 this Vst Plugin is a DSP Plugin
            if (_vstHandle > 0)
            {
              DSPPluginInfo pluginInfo = new DSPPluginInfo(DSPPluginInfo.PluginType.VST, vstplugin.FullName,
                vstplugin.Name);
              if (pluginInfo.IsBlackListed)
              {
                Log.Info("DSP Plugin {0} may not be used, as it is known for causing problems.", vstplugin.Name);
              }
              else
              {
                listBoxFoundPlugins.Items.Add(pluginInfo);
              }
            }
            BassVst.BASS_VST_ChannelRemoveDSP(0, _vstHandle);
          }
          catch (Exception ex)
          {
            Log.Error("Error reading VST Plugin Information: {0}", ex.Message);
          }
        }
      }

      // VST Plugins
      foreach (VSTPlugin plugins in Settings.Instance.VSTPlugins)
      {
        // Get the vst handle and enable it
        string plugin = String.Format(@"{0}\{1}", pluginPath, plugins.PluginDll);
        _vstHandle = BassVst.BASS_VST_ChannelSetDSP(0, plugin, BASSVSTDsp.BASS_VST_DEFAULT, 1);
        if (_vstHandle > 0)
        {
          DSPPluginInfo pluginInfo = new DSPPluginInfo(DSPPluginInfo.PluginType.VST, plugin, plugins.PluginDll);
          listBoxSelectedPlugins.Items.Add(pluginInfo);
          _vstHandles[plugins.PluginDll] = _vstHandle;
          // Set all parameters for the plugin
          foreach (VSTPluginParm paramter in plugins.Parameter)
          {
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
            try
            {
              BassVst.BASS_VST_SetParam(_vstHandle, paramter.Index, float.Parse(paramter.Value));
            }
            catch (Exception) {}
          }
        }
        else
        {
          Log.Debug("Couldn't load VST Plugin {0}. Error code: {1}", plugin, Bass.BASS_ErrorGetCode());
        }
      }
      // Now remove those already selected from the found listbox
      foreach (VSTPlugin plugins in Settings.Instance.VSTPlugins)
      {
        for (int i = 0; i < listBoxFoundPlugins.Items.Count; i++)
        {
          DSPPluginInfo dsp = (DSPPluginInfo)listBoxFoundPlugins.Items[i];
          if (dsp.DSPPluginType == DSPPluginInfo.PluginType.VST && dsp.Name == plugins.PluginDll)
          {
            listBoxFoundPlugins.Items.RemoveAt(i);
          }
        }
      }

      // WinAmp Plugins

      // Get the available plugins in the directory and fill the found listbox
      if (Directory.Exists(pluginPath))
        WINAMP_DSP.FindPlugins(pluginPath);
      foreach (WINAMP_DSP winampPlugin in WINAMP_DSP.PlugIns)
      {
        DSPPluginInfo pluginInfo = new DSPPluginInfo(DSPPluginInfo.PluginType.Winamp, winampPlugin.File,
                                                     winampPlugin.Description);
        if (pluginInfo.IsBlackListed)
        {
          Log.Info("DSP Plugin {0} may not be used, as it is known for causing problems.", pluginInfo.FilePath);
        }
        else
        {
          listBoxFoundPlugins.Items.Add(pluginInfo);
        }
      }
      // Now remove those already selected from the found listbox
      foreach (WinAmpPlugin plugins in Settings.Instance.WinAmpPlugins)
      {
        for (int i = 0; i < listBoxFoundPlugins.Items.Count; i++)
        {
          DSPPluginInfo dsp = (DSPPluginInfo)listBoxFoundPlugins.Items[i];
          if (dsp.DSPPluginType == DSPPluginInfo.PluginType.Winamp && dsp.FilePath == plugins.PluginDll)
          {
            listBoxFoundPlugins.Items.RemoveAt(i);
            _waDspPlugin = BassWaDsp.BASS_WADSP_Load(plugins.PluginDll, 5, 5, 100, 100, null);
            if (_waDspPlugin > 0)
            {
              listBoxSelectedPlugins.Items.Add(dsp);
              _waDspPlugins[plugins.PluginDll] = _waDspPlugin;
              break;
            }
            else
            {
              Log.Debug("Couldn't load WinAmp Plugin {0}. Error code: {1}", plugins.PluginDll, Bass.BASS_ErrorGetCode());
            }
          }
        }
      }
    }

    /// <summary>
    /// Save Effects Settings
    /// </summary>
    public override void SaveSettings()
    {
      Settings.Instance.MusicDirectory = _initialMusicDirectory;

      // Settings for BASS DSP/FX
      Settings.Instance.BassEffects.Clear();
      BassEffect basseffect;

      // Gain
      if (textBoxGainDBValue.Text != "0")
      {
        basseffect = new BassEffect();
        basseffect.EffectName = "Gain";
        basseffect.Parameter.Add(new BassEffectParm("Gain_dbV", textBoxGainDBValue.Text));
        Settings.Instance.BassEffects.Add(basseffect);
      }

      // Dynamic Amplification
      if (checkBoxDAmp.Checked)
      {
        basseffect = new BassEffect();
        basseffect.EffectName = "DynAmp";
        basseffect.Parameter.Add(new BassEffectParm("Preset", comboBoxDynamicAmplification.SelectedIndex.ToString()));
        Settings.Instance.BassEffects.Add(basseffect);
      }

      // Compressor
      if (checkBoxCompressor.Checked)
      {
        basseffect = new BassEffect();
        basseffect.EffectName = "Compressor";
        basseffect.Parameter.Add(new BassEffectParm("Threshold", trackBarCompressor.Value.ToString()));
        Settings.Instance.BassEffects.Add(basseffect);
      }

      // Settings for VST Plugings
      Settings.Instance.VSTPlugins.Clear();
      VSTPlugin vstplugin;
      foreach (string plugindll in _vstHandles.Keys)
      {
        vstplugin = new VSTPlugin();
        vstplugin.PluginDll = plugindll;
        _vstHandle = _vstHandles[plugindll];
        BASS_VST_PARAM_INFO paramInfo = new BASS_VST_PARAM_INFO();
        for (int i = BassVst.BASS_VST_GetParamCount(_vstHandle) - 1; i >= 0; i--)
        {
          BassVst.BASS_VST_GetParamInfo(_vstHandle, i, paramInfo);
          float value = BassVst.BASS_VST_GetParam(_vstHandle, i);
          vstplugin.Parameter.Add(new VSTPluginParm(paramInfo.name, i, value.ToString()));
        }
        Settings.Instance.VSTPlugins.Add(vstplugin);
      }

      // Settings for WinAmpPlugins
      WinAmpPlugin winampplugin;

      // Clear all settings first
      Settings.Instance.WinAmpPlugins.Clear();
      foreach (DSPPluginInfo pluginInfo in listBoxSelectedPlugins.Items)
      {
        if (pluginInfo.DSPPluginType == DSPPluginInfo.PluginType.Winamp)
        {
          winampplugin = new WinAmpPlugin();
          winampplugin.PluginDll = pluginInfo.FilePath;
          Settings.Instance.WinAmpPlugins.Add(winampplugin);
        }
      }
      Settings.SaveSettings();
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }

      // Clean up BASS Resources
      try
      {
        // Some Winamp dsps might raise an exception when closing
        BassWaDsp.BASS_WADSP_Free();
      }
      catch {}
      Bass.BASS_Stop();
      Bass.BASS_Free();

      base.Dispose(disposing);
    }

    #endregion
  }
}