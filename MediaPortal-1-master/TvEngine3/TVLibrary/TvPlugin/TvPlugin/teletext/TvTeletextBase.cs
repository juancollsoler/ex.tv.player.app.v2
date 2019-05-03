#region Copyright (C) 2005-2010 Team MediaPortal

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

#endregion

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Profile;
using TvLibrary.Teletext;
using Action = MediaPortal.GUI.Library.Action;

namespace TvPlugin
{

  #region enum

  public enum TeletextButton
  {
    Red,
    Green,
    Yellow,
    Blue
  }

  #endregion

  /// <summary>
  /// Common class for both teletext windows
  /// </summary>
  public class TvTeletextBase : GUIInternalWindow
  {
    #region gui controls

    [SkinControl(27)] protected GUILabelControl lblMessage = null;
    [SkinControl(500)] protected GUIImage imgTeletextForeground = null;
    [SkinControl(501)] protected GUIImage imgTeletextBackground = null;

    #endregion

    #region variables

    protected string inputLine = String.Empty;
    protected int currentPageNumber = 0x100;
    protected int currentSubPageNumber;
    protected int receivedPageNumber = 0x100;
    protected int receivedSubPageNumber;
    protected byte[] receivedPage;
    protected bool _updatingForegroundImage;
    protected bool _updatingBackgroundImage;
    protected bool _waiting;
    protected DateTime _startTime = DateTime.MinValue;
    protected TeletextPageRenderer _renderer = new TeletextPageRenderer();
    protected bool _hiddenMode;
    protected bool _transparentMode;
    protected Thread _updateThread;
    protected bool _updateThreadStop;
    protected int _numberOfRequestedUpdates;
    protected bool _rememberLastValues;
    protected int _percentageOfMaximumHeight;
    protected bool _redrawForeground = true;
    protected bool _showFirstAvailableSubPage = false;
    protected DateTime _trottling = DateTime.MinValue;

    #endregion

    #region Properties

    public override bool IsTv
    {
      get 
      { 
        return true;
      }
    }

    public bool Waiting
    {
      get
      { 
        return _waiting;
      }
      set
      { 
        if (_waiting != value) {
          _waiting = value;
          //_renderer.Waiting = value;
          RequestUpdate(false);
        }
      }
    }

    #endregion

    #region Initialization methods

    /// <summary>
    /// Initialize the window
    /// </summary>
    /// <param name="fullscreenMode">Indicate, if is fullscreen mode</param>
    protected void InitializeWindow(bool fullscreenMode)
    {
      LoadSettings();
      _showFirstAvailableSubPage = false;
      _numberOfRequestedUpdates = 0;

      lblMessage.Label = "";
      lblMessage.Visible = false;
      // Activate teletext grabbing in the server
      TVHome.Card.GrabTeletext = true;
      // Set the current page to the index page
      currentPageNumber = 0x100;
      currentSubPageNumber = 0;

      // Remember the start time
      _startTime = DateTime.MinValue;
      _trottling = DateTime.MinValue;

      // Initialize the render
      _renderer = new TeletextPageRenderer();
      _renderer.TransparentMode = _transparentMode;
      _renderer.FullscreenMode = fullscreenMode;
      _renderer.HiddenMode = _hiddenMode;
      _renderer.PageSelectText = Convert.ToString(currentPageNumber, 16);
      _renderer.PercentageOfMaximumHeight = _percentageOfMaximumHeight;

      _waiting = false;

      // Create an update thread and set it's priority to lowest
      _updateThreadStop = false;
      _updateThread = new Thread(UpdatePage);
      _updateThread.Name = "Teletext Updater";
      _updateThread.Priority = ThreadPriority.BelowNormal;
      _updateThread.IsBackground = true;
      _updateThread.Start();

      // Load the mp logo page into teletext data array
      LoadLogoPage();
      // Request an update
      RequestUpdate();
    }

    protected void Join()
    {
      _updateThreadStop = true;
      _updateThread.Join();
    }

    /// <summary>
    /// Loads the logo page from the assembly
    /// </summary>
    protected void LoadLogoPage()
    {
      Assembly assm = Assembly.GetExecutingAssembly();
      Stream stream = assm.GetManifestResourceStream("TvPlugin.teletext.LogoPage");
      if (stream != null)
      {
        using (BinaryReader reader = new BinaryReader(stream))
        {
          receivedPage = new byte[stream.Length];
          reader.Read(receivedPage, 0, (int)stream.Length);
          receivedPageNumber = 0;
          receivedSubPageNumber = 0;
        }
      }
    }

    #endregion

    #region OnAction

    public override void OnAction(Action action)
    {
      // if we have a keypress or a remote button press then check if it is a number and add it to the inputLine
      char key = (char)0;
      if (action.wID == Action.ActionType.ACTION_KEY_PRESSED)
      {
        if (action.m_key != null)
        {
          if (action.m_key.KeyChar >= '0' && action.m_key.KeyChar <= '9')
          {
            // Get offset to item
            key = (char)action.m_key.KeyChar;
          }
        }
        if (key == (char)0)
        {
          return;
        }
        UpdateInputLine(key);
      }
      switch (action.wID)
      {
        case Action.ActionType.ACTION_REMOTE_RED_BUTTON:
          // Red teletext button
          showTeletextButtonPage(TeletextButton.Red);
          break;
        case Action.ActionType.ACTION_REMOTE_GREEN_BUTTON:
          // Green teletext button
          showTeletextButtonPage(TeletextButton.Green);
          break;
        case Action.ActionType.ACTION_REMOTE_YELLOW_BUTTON:
          // Yellow teletext button
          showTeletextButtonPage(TeletextButton.Yellow);
          break;
        case Action.ActionType.ACTION_REMOTE_BLUE_BUTTON:
          // Blue teletext button
          showTeletextButtonPage(TeletextButton.Blue);
          break;
        case Action.ActionType.ACTION_REMOTE_SUBPAGE_UP:
          // Subpage up
          SubpageUp();
          break;
        case Action.ActionType.ACTION_REMOTE_SUBPAGE_DOWN:
          // Subpage down
          SubpageDown();
          break;
        case Action.ActionType.ACTION_NEXT_TELETEXTPAGE:
          // Page up
          PageUp();
          break;
        case Action.ActionType.ACTION_PREV_TELETEXTPAGE:
          // Page down
          PageDown();
          break;
        case Action.ActionType.ACTION_CONTEXT_MENU:
          // Show previous window
          GUIWindowManager.ShowPreviousWindow();
          break;
        case Action.ActionType.ACTION_SWITCH_TELETEXT_HIDDEN:
          //Change Hidden Mode
          _hiddenMode = !_hiddenMode;
          _renderer.HiddenMode = _hiddenMode;
          // Rerender the image
          RequestUpdate(false);
          break;
        case Action.ActionType.ACTION_SHOW_INDEXPAGE:
          // Index page
          showNewPage(0x100);
          break;
      }
      base.OnAction(action);
    }

    #endregion

    #region Navigation methods

    /// <summary>
    /// Selects the next subpage, if possible
    /// </summary>
    protected void SubpageUp()
    {
      if (currentSubPageNumber < 0x79)
      {
        currentSubPageNumber++;
        while (((currentSubPageNumber + 1) & 0x0F) > 9)
        {
          currentSubPageNumber++;
        }
        RequestUpdate(false);
        Log.Info("dvb-teletext: select page {0:X} / subpage {1:X}", currentPageNumber, currentSubPageNumber);
      }
    }

    /// <summary>
    /// Selects the previous subpage, if possible
    /// </summary>
    protected void SubpageDown()
    {
      if (currentSubPageNumber > 0)
      {
        currentSubPageNumber--;
        while (((currentSubPageNumber + 1) & 0x0F) > 9)
        {
          currentSubPageNumber--;
        }
        RequestUpdate(false);
        Log.Info("dvb-teletext: select page {0:X} / subpage {1:X}", currentPageNumber, currentSubPageNumber);
      }
    }

    /// <summary>
    /// Selects the next page, if possible
    /// </summary>
    protected void PageUp()
    {
      if (currentPageNumber < 0x899)
      {
        currentPageNumber++;
        while ((currentPageNumber & 0x0F) > 9)
        {
          currentPageNumber++;
        }
        while ((currentPageNumber & 0xF0) > 0x90)
        {
          currentPageNumber += 16;
        }
        _renderer.PageSelectText = Convert.ToString(currentPageNumber, 16);
        currentSubPageNumber = 0;
        RequestUpdate();
        Log.Info("dvb-teletext: select page {0:X} / subpage {1:X}", currentPageNumber, currentSubPageNumber);
        inputLine = "";
        return;
      }
    }

    /// <summary>
    /// Selects the previous subpage, if possible
    /// </summary>
    protected void PageDown()
    {
      if (currentPageNumber > 0x100)
      {
        currentPageNumber--;
        while ((currentPageNumber & 0xF0) > 0x90)
        {
          currentPageNumber -= 16;
        }
        while ((currentPageNumber & 0x0F) > 9)
        {
          currentPageNumber--;
        }
        _renderer.PageSelectText = Convert.ToString(currentPageNumber, 16);
        currentSubPageNumber = 0;
        RequestUpdate();
        Log.Info("dvb-teletext: select page {0:X} / subpage {1:X}", currentPageNumber, currentSubPageNumber);
        inputLine = "";
        return;
      }
    }

    /// <summary>
    /// Updates the header and the selected page text
    /// </summary>
    /// <param name="key">Key</param>
    protected void UpdateInputLine(char key)
    {
      Log.Info("dvb-teletext: key received: " + key);
      if (inputLine.Length == 0 && (key == '0' || key == '9'))
      {
        return;
      }
      inputLine += key;
      _renderer.PageSelectText = inputLine;
      if (inputLine.Length == 3)
      {
        // change channel
        currentPageNumber = Convert.ToInt16(inputLine, 16);
        currentSubPageNumber = 0;
        if (currentPageNumber < 0x100)
        {
          currentPageNumber = 0x100;
        }
        if (currentPageNumber > 0x899)
        {
          currentPageNumber = 0x899;
        }
        RequestUpdate();
        Log.Info("dvb-teletext: select page {0:X} / subpage {1:X}", currentPageNumber, currentSubPageNumber);
        inputLine = "";
      }
      else
      {
        RequestUpdate(false);
      }
    }

    /// <summary>
    /// Selects a teletext page, based on the teletext button
    /// </summary>
    /// <param name="button"></param>
    protected void showTeletextButtonPage(TeletextButton button)
    {
      switch (button)
      {
        case TeletextButton.Red:
          showNewPage(TVHome.Card.GetTeletextRedPageNumber());
          break;
        case TeletextButton.Green:
          showNewPage(TVHome.Card.GetTeletextGreenPageNumber());
          break;
        case TeletextButton.Yellow:
          showNewPage(TVHome.Card.GetTeletextYellowPageNumber());
          break;
        case TeletextButton.Blue:
          showNewPage(TVHome.Card.GetTeletextBluePageNumber());
          break;
      }
    }

    /// <summary>
    /// Displays a new page, with the give page number
    /// </summary>
    /// <param name="hexPage">Page number (hexnumber)</param>
    protected void showNewPage(int hexPage)
    {
      if (hexPage >= 0x100 && hexPage <= 0x899)
      {
        currentPageNumber = hexPage;
        _renderer.PageSelectText = Convert.ToString(currentPageNumber, 16);
        currentSubPageNumber = 0;
        inputLine = "";
        RequestUpdate();
        Log.Info("dvb-teletext: select page {0:X} / subpage {1:X}", currentPageNumber, currentSubPageNumber);
        return;
      }
    }

    #endregion

    #region Update, Process and Redraw

    /// <summary>
    /// Gets called by MP. Rotate the subpages and updates the pages
    /// </summary>
    public override void Process()
    {
      TimeSpan ts = DateTime.Now - _startTime;

      // Only check when no requested updates
      if (_numberOfRequestedUpdates > 0)
      {
        return;
      }
      // Only every second, we check
      if (ts.TotalMilliseconds < 1000)
      {
        return;
      }

      if (_trottling != DateTime.MinValue && DateTime.Now > _trottling)
      {
        currentSubPageNumber = 0; // start from beginning
        _trottling = DateTime.MinValue;
        _showFirstAvailableSubPage = true;
      }

      // Still waiting for a page, then request an update again
      if (_waiting)
      {
        RequestUpdate(_showFirstAvailableSubPage);
        _startTime = DateTime.Now;
        return;
      }
      // Check the rotation speed
      TimeSpan tsRotation = TVHome.Card.TeletextRotation(currentPageNumber);
      // Should we rotate?
      if (ts.TotalMilliseconds < tsRotation.TotalMilliseconds)
      {
        return;
      }
      // Rotate --> Check the subpagenumber and update time variable
      _startTime = DateTime.Now;

      if (currentPageNumber < 0x100)
      {
        currentPageNumber = 0x100;
      }
      if (currentPageNumber > 0x899)
      {
        currentPageNumber = 0x899;
      }
      int NumberOfSubpages = TVHome.Card.SubPageCount(currentPageNumber) - 1;
      NumberOfSubpages = NumberOfSubpages < -1 ? -1 : NumberOfSubpages;
      if (currentSubPageNumber <= NumberOfSubpages)
      {
        currentSubPageNumber++;
        while (((currentSubPageNumber + 1) & 0x0F) > 9)
        {
          currentSubPageNumber++;
        }
      }
      if (currentSubPageNumber > NumberOfSubpages)
      {
        byte[] page = TVHome.Card.GetTeletextPage(currentPageNumber, 0);
        if (page == null && _trottling == DateTime.MinValue)
        {
          _trottling = DateTime.Now.AddMilliseconds(tsRotation.TotalMilliseconds);
        }
        else
        {
          currentSubPageNumber = 0;
        }
      }

      Log.Info("dvb-teletext page updated. {0:X}/{1:X} total:{2} rotspeed:{3}", currentPageNumber, currentSubPageNumber,
               NumberOfSubpages, tsRotation.TotalMilliseconds);
      // Request the update
      RequestUpdate(false);
    }

    /// <summary>
    /// Method of the update thread
    /// </summary>
    protected void UpdatePage()
    {
      imgTeletextForeground.Centered = false;
      imgTeletextForeground.KeepAspectRatio = false;
      imgTeletextForeground.ColorKey = Color.HotPink.ToArgb();
      imgTeletextForeground.SetMemoryImageSize(_renderer.Width, _renderer.Height);
      imgTeletextForeground.FileName = "[teletextpage]";

      imgTeletextBackground.Centered = false;
      imgTeletextBackground.KeepAspectRatio = false;
      imgTeletextBackground.ColorKey = Color.HotPink.ToArgb();
      imgTeletextBackground.SetMemoryImageSize(_renderer.Width, _renderer.Height);
      imgTeletextBackground.FileName = "[teletextpage2]";

      // While not stop the thread, continue
      while (!_updateThreadStop)
      {
        // Is there an update request, than update
        if (_numberOfRequestedUpdates > 0 && !_updateThreadStop)
        {
          GetNewPage();
          _numberOfRequestedUpdates--;
        }
        else
        {
          // Otherwise sleep for 300ms
          Thread.Sleep(100);
        }
      }
      imgTeletextForeground.RemoveMemoryImageTexture();
      imgTeletextBackground.RemoveMemoryImageTexture();
    }

    /// <summary>
    /// Redraws the images
    /// </summary>
    /*protected void Redraw()
    {
      Log.Info("dvb-teletext redraw()");
      try
      {
        // First update the foreground image. Step 1 make it invisible
        _updatingForegroundImage = true;
        imgTeletextForeground.IsVisible = false;
        // Clear the old image
        Image img = (Image) bmpTeletextPage.Clone();
        imgTeletextForeground.FileName = "";
        GUITextureManager.ReleaseTexture("[teletextpage]");
        // Set the new image and make the image visible again
        imgTeletextForeground.MemoryImage = img;
        imgTeletextForeground.FileName = "[teletextpage]";
        imgTeletextForeground.Centered = false;
        imgTeletextForeground.KeepAspectRatio = false;
        imgTeletextForeground.IsVisible = true;
        _updatingForegroundImage = false;
        // Update the background image now. Therefor make image invisible
        _updatingBackgroundImage = true;
        imgTeletextBackground.IsVisible = false;
        // Clear the old image
        Image img2 = (Image) bmpTeletextPage.Clone();
        imgTeletextBackground.FileName = "";
        GUITextureManager.ReleaseTexture("[teletextpage2]");
        // Set the new image and make the image visible again
        imgTeletextBackground.MemoryImage = img2;
        imgTeletextBackground.FileName = "[teletextpage2]";
        imgTeletextBackground.Centered = false;
        imgTeletextBackground.KeepAspectRatio = false;
        imgTeletextBackground.IsVisible = true;
        _updatingBackgroundImage = false;
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }*/
    protected void Redraw()
    {
      Bitmap bitmap;
      if (_redrawForeground)
      {
        imgTeletextForeground.IsVisible = false;
        if (!imgTeletextForeground.LockMemoryImageTexture(out bitmap))
          return;
        _renderer.RenderPage(ref bitmap, receivedPage, receivedPageNumber, receivedSubPageNumber, _waiting);
        imgTeletextForeground.UnLockMemoryImageTexture();
        imgTeletextForeground.IsVisible = true;
      }
      else
      {
        imgTeletextBackground.IsVisible = false;
        if (!imgTeletextBackground.LockMemoryImageTexture(out bitmap))
          return;
        _renderer.RenderPage(ref bitmap, receivedPage, receivedPageNumber, receivedSubPageNumber, _waiting);
        imgTeletextBackground.UnLockMemoryImageTexture();
        imgTeletextBackground.IsVisible = true;
      }
      _redrawForeground = !_redrawForeground;
    }

    /// <summary>
    /// Retrieve the new page from the server
    /// </summary>
    protected void GetNewPage()
    {
      int sub = currentSubPageNumber;
      int maxSubs = TVHome.Card.SubPageCount(currentPageNumber) - 1;
      maxSubs = maxSubs < -1 ? -1 : maxSubs;

      //Log.Info("dvb-teletext: GetNewPage: page = {0}, subpage = {1}, maxsubpages = {2}", currentPageNumber, currentSubPageNumber, maxSubs);

      // Check if the page is available
      if (maxSubs < 0) // we don't have anything yet...
      {
        bool wasWaiting = _waiting;
        _waiting = true;
        _renderer.SubPageSelectText = "";
        if (receivedPage != null)// && !wasWaiting)
        {
          Redraw();
          Log.Info("dvb-teletext: nothing, received page {0:X} / subpage {1:X}", receivedPageNumber, receivedSubPageNumber);
        }
        return;
      }
      if (sub > maxSubs)
      {
        if (_trottling == DateTime.MinValue)
          sub = maxSubs;
      }
      if (sub < 0)
      {
        sub = 0;
      }
      currentSubPageNumber = sub;
      _renderer.SubPageSelectText = Convert.ToString(currentSubPageNumber + 1, 16);

      // Try to get the page
      byte[] page = TVHome.Card.GetTeletextPage(currentPageNumber, currentSubPageNumber);
      if (page == null && _showFirstAvailableSubPage)
      {
        page = GetExistingTeletextPage(currentSubPageNumber + 1, maxSubs, ref currentSubPageNumber);
      }

      // Was the page available, then render it. Otherwise render the last page again and update the header line, if
      // it was for the first time
      if (page != null)
      {
        _startTime = DateTime.Now;
        _trottling = DateTime.MinValue;
        receivedPage = page;
        receivedPageNumber = currentPageNumber;
        receivedSubPageNumber = currentSubPageNumber;
        _waiting = false;
        _showFirstAvailableSubPage = false;
        Redraw();
        Log.Info("dvb-teletext: select page {0:X} / subpage {1:X}", currentPageNumber, currentSubPageNumber);
      }
      else
      {
        bool wasWaiting = _waiting;
        _waiting = true;
        if (receivedPage != null)// && !wasWaiting)
        {
          Redraw();
          Log.Info("dvb-teletext: received page {0:X} / subpage {1:X}", receivedPageNumber, receivedSubPageNumber);
        }
      }
    }

    protected byte[] GetExistingTeletextPage(int startSubPage, int maxSubPage, ref int foundPage)
    {
      for (int i = startSubPage; i <= maxSubPage; i++)
      {
        byte[] page = TVHome.Card.GetTeletextPage(currentPageNumber, i);
        if (page != null)
        {
          foundPage = i;
          return page;
        }
      }
      return null;
    }

    protected void RequestUpdate()
    {
      RequestUpdate(true);
    }

    protected void RequestUpdate(bool anySubPage)
    {
      _showFirstAvailableSubPage = anySubPage;
      _numberOfRequestedUpdates++;
    }

    #endregion

    #region Helper

    protected int Decimal(int bcd)
    {
      return ((bcd >> 4) * 10) + bcd % 16;
    }

    protected int BCD(int dec)
    {
      return ((dec / 10) << 4) + (dec % 10);
    }

    #endregion

    #region Serialisation

    /// <summary>
    /// Load the settings
    /// </summary>
    protected void LoadSettings()
    {
      using (Settings xmlreader = new MPSettings())
      {
        _hiddenMode = xmlreader.GetValueAsBool("mytv", "teletextHidden", false);
        _transparentMode = xmlreader.GetValueAsBool("mytv", "teletextTransparent", false);
        _rememberLastValues = xmlreader.GetValueAsBool("mytv", "teletextRemember", true);
        _percentageOfMaximumHeight = xmlreader.GetValueAsInt("mytv", "teletextMaxFontSize", 100);
      }
    }

    /// <summary>
    /// Store the settings, if the user wants it
    /// </summary>
    protected void SaveSettings()
    {
      if (_rememberLastValues)
      {
        using (Settings xmlreader = new MPSettings())
        {
          xmlreader.SetValueAsBool("mytv", "teletextHidden", _hiddenMode);
          xmlreader.SetValueAsBool("mytv", "teletextTransparent", _transparentMode);
        }
      }
    }

    #endregion
  }
}