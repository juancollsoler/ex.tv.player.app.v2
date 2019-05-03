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
using System.Drawing;
using System.Windows.Forms;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Profile;
using MediaPortal.Util;
using Log = MediaPortal.ServiceImplementations.Log;
using Mapping = MediaPortal.InputDevices.InputHandler.Mapping;

namespace MediaPortal.InputDevices
{
  public class CentareaRemote : IInputDevice
  {
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;
    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_RBUTTONDOWN = 0x0204;
    private const int WM_MOUSEMOVE = 0x0200;
    private const int WM_SETCURSOR = 0x0020;

    private enum MouseDirection
    {
      Up,
      Right,
      Down,
      Left,
      None
    } ;

    private bool _remoteActive = false; // Centarea Remote enabled and mapped
    private bool _verboseLogging = false; // Log key presses
    private bool _mapMouseButton = true; // Interpret the joystick push as "ok" button
    private bool _mapJoystick = true; // Map any mouse movement to cursor keys
    private int _lastMouseTick = 0; // When did the last mouse action occur
    private int _ignoreDupMsg = 0; // Offset to compensate the self-induced mouse movement
    private InputHandler _inputHandler; // Input Mapper
    private bool _remoteConfigured = false;
    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public CentareaRemote() {}

    #endregion

    #region Init && Deinit

    public void Init(IntPtr hwnd)
    {
      Init();
    }

    public void Init()
    {
     
      using (Settings xmlreader = new MPSettings())
      {
          _remoteConfigured = xmlreader.GetValueAsBool("remote", "Centarea", false);
        _verboseLogging = xmlreader.GetValueAsBool("remote", "CentareaVerbose", false);
        _mapMouseButton = xmlreader.GetValueAsBool("remote", "CentareaMouseOkMap", true);
        _mapJoystick = xmlreader.GetValueAsBool("remote", "CentareaJoystickMap", false);
      }
      if (!_remoteConfigured)
      {
        return;
      }

      Log.Debug("Centarea: Initializing Centarea HID remote");

      _inputHandler = new InputHandler("Centarea HID");
      if (!_inputHandler.IsLoaded)
      {
        Log.Error("Centarea: Error loading default mapping file - please reinstall MediaPortal");
        DeInit();
        return;
      }
      else
      {
        Log.Info("Centarea: Centarea HID mapping loaded successfully");
        _remoteActive = true;
      }
    }

    /// <summary>
    /// Remove all device handling
    /// </summary>
    public void DeInit()
    {
      if (_remoteActive)
      {
        Log.Info("Centarea: Stopping Centarea HID remote");
        _remoteActive = false;
        _inputHandler = null;
      }
    }

    #endregion

    #region Key handling

    /// <summary>
    /// Let everybody know that this HID message may not be handled by anyone else
    /// </summary>
    /// <param name="msg">System.Windows.Forms.Message</param>
    /// <returns>Command handled</returns>
    public bool WndProc(ref Message msg)
    {
      if (_remoteActive)
      {
        AppCommands appCommand = (AppCommands) Win32.Macro.GET_APPCOMMAND_LPARAM(msg.LParam);
        // find out which request the MCE remote handled last
        if ((appCommand == InputDevices.LastHidRequest) && (appCommand != AppCommands.VolumeDown) &&
            (appCommand != AppCommands.VolumeUp))
        {
          if (Enum.IsDefined(typeof (AppCommands), InputDevices.LastHidRequest))
          {
            // possible that it is the same request mapped to an app command?
            if (Environment.TickCount - InputDevices.LastHidRequestTick < 500)
            {
              return true;
            }
          }
        }

        return MapWndProcMessage(msg) != null;

      }
      return false;
    }

    /// <summary>
    /// Map a WndProc message and optionally execute it
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    private Mapping MapWndProcMessage(Message msg, bool shouldRaiseAction = true)
    {
        Mapping result = null;

        if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN || msg.Msg == Win32.Const.WM_APPCOMMAND || msg.Msg == WM_LBUTTONDOWN ||
                  msg.Msg == WM_RBUTTONDOWN || msg.Msg == WM_MOUSEMOVE)
        {
            switch ((Keys)msg.WParam)
            {
                case Keys.ControlKey:
                    break;
                case Keys.ShiftKey:
                    break;
                case Keys.Menu:
                    break;
                default:
                    int keycode = (int)msg.WParam;

                    AppCommands appCommand = (AppCommands)Win32.Macro.GET_APPCOMMAND_LPARAM(msg.LParam);
                    InputDevices.LastHidRequest = appCommand;

                    // Due to the non-perfect placement of the OK button we allow the user to remap the joystick to okay.
                    if (_mapMouseButton)
                    {
                        if (msg.Msg == WM_LBUTTONDOWN)
                        {
                            if (_verboseLogging)
                            {
                                Log.Debug("Centarea: Command \"{0}\" mapped for left mouse button", keycode);
                            }
                            keycode = 13;
                        }
                        if (msg.Msg == WM_RBUTTONDOWN)
                        {
                            if (_verboseLogging)
                            {
                                Log.Debug("Centarea: Command \"{0}\" mapped for right mouse button", keycode);
                            }
                            keycode = 10069;
                        }
                    }
                    // Since mouse support is semi optimal we have this option to use the joystick like cursor keys
                    if (_mapJoystick && GUIGraphicsContext.Fullscreen)
                    {
                        if (msg.Msg == WM_MOUSEMOVE)
                        {
                            Point p = new Point(msg.LParam.ToInt32());
                            _ignoreDupMsg++;
                            // since our ResetCursor() triggers a mouse move MSG as well we ignore every second event
                            if (_ignoreDupMsg % 2 == 0)
                            {
                                GUIGraphicsContext.ResetCursor(false);
                            }
                            // we ignore double actions for the configured time
                            if (Environment.TickCount - _lastMouseTick < 400)
                            {
                              return null;
                            }

                            MouseDirection mmove = OnMouseMoved(p);
                            _lastMouseTick = Environment.TickCount;
                            _ignoreDupMsg = 0;

                            switch (mmove)
                            {
                                case MouseDirection.Up:
                                    keycode = 38;
                                    break;
                                case MouseDirection.Right:
                                    keycode = 39;
                                    break;
                                case MouseDirection.Down:
                                    keycode = 40;
                                    break;
                                case MouseDirection.Left:
                                    keycode = 37;
                                    break;
                            }
                            if (mmove != MouseDirection.None)
                            {
                                GUIGraphicsContext.ResetCursor(false);
                                if (_verboseLogging)
                                {
                                    Log.Debug("Centarea: Command \"{0}\" mapped for mouse movement", mmove.ToString());
                                }
                            }
                        }
                    }
                    // The Centarea Remote sends key combos. Therefore we use this trick to get a 1:1 mapping
                    if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        keycode += 1000;
                    }
                    if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        keycode += 10000;
                    }
                    if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
                    {
                        keycode += 100000;
                    }

                    try
                    {
                        result = _inputHandler.GetMapping(keycode.ToString());
                        if (shouldRaiseAction)
                        {
                            // Get & execute Mapping
                            if (_inputHandler.MapAction(keycode))
                            {
                                if (_verboseLogging)
                                {
                                    Log.Debug("Centarea: Command \"{0}\" mapped", keycode);
                                }
                            }
                            else
                            {
                                if (keycode > 0)
                                {
                                    Log.Debug("Centarea: Command \"{0}\" not mapped", keycode);
                                }
                                return null;
                            }
                        }
                    }
                    catch (ApplicationException)
                    {
                      return null;
                    }
                    msg.Result = new IntPtr(1);
                    break;
            }
        }
        return result;
    }

    /// <summary>
    /// Required for the IInputDevice interface
    /// </summary>    
    /// <param name="msg"></param>
    /// <param name="action"></param>
    /// <param name="key"></param>
    /// <param name="keyCode"></param>
    /// <returns></returns>
    public bool WndProc(ref System.Windows.Forms.Message msg, out GUI.Library.Action action, out char key, out Keys keyCode)
    {
      action = null;
      key = (char)0;
      keyCode = Keys.A;
      return WndProc(ref msg);
    }

    /// <summary>
    /// Get the mapping for this wndproc action
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public MediaPortal.InputDevices.InputHandler.Mapping GetMapping(Message msg)
    {
      if (_remoteConfigured)
      {
        return MapWndProcMessage(msg, false);
      }
      return null;
    }
    #endregion

    #region Joystick handling

    /// <summary>
    /// Calculates the deviation of the new position from the app area's center point
    /// </summary>
    /// <param name="aVerticalMove">Calculate a horizontal or vertical movement</param>
    /// <param name="aNewPosition">The position after movement</param>
    /// <returns>The amount of pixels the cursor has moved. A negative value indicates left/up movement</returns>
    private int GetPointDeviation(bool aVerticalMove, int aNewPosition)
    {
      int OldPos = aVerticalMove ? GUIGraphicsContext.OutputScreenCenter.Y : GUIGraphicsContext.OutputScreenCenter.X;
      if (OldPos > aNewPosition)
      {
        return (OldPos - aNewPosition) * -1;
      }
      else
      {
        return aNewPosition - OldPos;
      }
    }

    /// <summary>
    /// Translates a mouse movement into a direction
    /// </summary>
    /// <param name="p">The new cursor coordinates</param>
    /// <returns>None for minimal movement, else up/down/left/right</returns>
    private MouseDirection OnMouseMoved(Point p)
    {
      MouseDirection direction = MouseDirection.None;
      int xMove = GetPointDeviation(false, p.X);
      int yMove = GetPointDeviation(true, p.Y);
      // using the pythagoras theorem to get the total movement length
      double TotalWay =
        Math.Sqrt(((double)((Math.Abs(xMove) * Math.Abs(xMove))) + (double)((Math.Abs(yMove) * Math.Abs(yMove)))));
      // set a direction only if movement exceeds a minimum limit
      // usually pushing the joystick knob once results in a one pixel movement.      
      if (TotalWay > 0.9)
      {
        direction = GetDirection(xMove, yMove);
      }
      if (_verboseLogging)
      {
        Log.Debug("Centarea: Mouse movement of {0} pixels heading {1}", TotalWay, direction);
      }
      return direction;
    }

    /// <summary>
    /// Calculates one of four directions based on the length of two vectors
    /// </summary>
    /// <param name="axMove">The horizontal movement</param>
    /// <param name="ayMove">The vertical movement</param>
    /// <returns>up/down/left/right</returns>
    private MouseDirection GetDirection(int axMove, int ayMove)
    {
      if (ayMove <= 0) // up
      {
        if (axMove <= 0) // left
        {
          // if we move the mouse more to the left than up the resulting direction is plain left.
          return (Math.Abs(axMove) <= Math.Abs(ayMove)) ? MouseDirection.Up : MouseDirection.Left;
        }
        else // right
        {
          return (Math.Abs(axMove) <= Math.Abs(ayMove)) ? MouseDirection.Up : MouseDirection.Right;
        }
      }
      else // down
      {
        if (axMove <= 0) // left
        {
          return (Math.Abs(axMove) <= Math.Abs(ayMove)) ? MouseDirection.Down : MouseDirection.Left;
        }
        else // right
        {
          return (Math.Abs(axMove) <= Math.Abs(ayMove)) ? MouseDirection.Down : MouseDirection.Right;
        }
      }
    }

    //private MouseDirection OnMouseMoved(Point p)
    //{
    //  int x_Val, y_Val;
    //  MouseDirection direction = MouseDirection.None;
    //  //Translating mouse motion on the control to a 60 x 60 cartesian grid.
    //  y_Val = (p.Y * 61 / MediaPortal.GUI.Library.GUIGraphicsContext.form.ClientSize.Height) - 30;
    //  x_Val = (p.X * 61 / MediaPortal.GUI.Library.GUIGraphicsContext.form.ClientSize.Width) - 30;
    //  y_Val = -y_Val;

    //  int radius = (int)Math.Sqrt(y_Val * y_Val + x_Val * x_Val);
    //  if (radius > 1)// && radius < 15)
    //    direction = GetDirection(x_Val, y_Val);
    //  return direction;
    //}

    //private MouseDirection GetDirection(float x, float y)
    //{
    //  //Changing cartesian coordinates from control surface to more usable polar coordinates
    //  double theta;
    //  if (x >= 0 && y > 0)
    //    theta = (Math.Atan(y / x) * (180 / Math.PI));
    //  else if (x < 0)
    //    theta = ((Math.PI + Math.Atan(y / x)) * (180 / Math.PI));
    //  else theta = (((2 * Math.PI) + Math.Atan(y / x)) * (180 / Math.PI));

    //  if (theta <= 48.5 || theta > 318.5)
    //    return MouseDirection.Right;
    //  else if (theta > 48.5 && theta <= 138.5)
    //    return MouseDirection.Up;
    //  else if (theta > 138.5 && theta <= 228.5)
    //    return MouseDirection.Left;
    //  else if (theta > 228.5 && theta <= 318.5)
    //    return MouseDirection.Down;
    //  else
    //    return MouseDirection.None;
    //}

    #endregion
  }
}