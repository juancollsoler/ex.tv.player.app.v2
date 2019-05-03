#region Copyright (C) 2005-2013 Team MediaPortal

// Copyright (C) 2005-2013 Team MediaPortal
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

#region Usings

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

#endregion

namespace TvEngine.PowerScheduler
{
  /// <summary>
  /// Implements a timer which the process can be waiting on. The 
  /// timer supports waking up the system from a hibernated state.
  /// </summary>
  public sealed unsafe class WaitableTimer : WaitHandle
  {
    /// <summary>
    /// Wrap the system function <i>SetWaitableTimer</i>.
    /// </summary>
    [DllImport("Kernel32.dll", EntryPoint = "SetWaitableTimer", SetLastError = true)]
    private static extern bool SetWaitableTimer(SafeWaitHandle hTimer, Int64* pDue, Int32 lPeriod, IntPtr rNotify,
                                                IntPtr pArgs, bool bResume);

    /// <summary>
    /// Wrap the system function <i>CreateWaitableTimer</i>.
    /// </summary>
    [DllImport("Kernel32.dll", EntryPoint = "CreateWaitableTimer")]
    private static extern SafeWaitHandle CreateWaitableTimer(IntPtr pSec, bool bManual, string szName);

    /// <summary>
    /// Wrap the system function <i>CancelWaitableTimer</i>.
    /// </summary>
    [DllImport("Kernel32.dll", EntryPoint = "CancelWaitableTimer")]
    private static extern bool CancelWaitableTimer(SafeWaitHandle hTimer);

    /// <summary>
    /// Wrap the system function <i>CloseHandle</i>.
    /// </summary>
    [DllImport("Kernel32.dll", EntryPoint = "CloseHandle")]
    private static extern bool CloseHandle(IntPtr hObject);

    /// <summary>
    /// Event handler to be used when the timer expires.
    /// </summary>
    public delegate void TimerExpiredHandler();

    public delegate void TimerExceptionHandler(WaitableTimer sender, TimerException exception);

    /// <summary>
    /// Clients can register for the expiration of this timer.
    /// </summary>
    public event TimerExpiredHandler OnTimerExpired;

    public event TimerExceptionHandler OnTimerException;

    /// <summary>
    /// This <see cref="Thread"/> will be create by <see cref="SecondsToWait"/> and
    /// runs <see cref="WaitThread"/>.
    /// </summary>
    private Thread m_Waiting = null;

    /// <summary>
    /// <see cref="DateTime.ToFileTime"/> of the time when the timer should
    /// expire.
    /// </summary>
    private long m_ExpirationTime = 0;

    /// <summary>
    /// Create the timer. The caller should call <see cref="Close"/> as soon as
    /// the timer is no longer needed.
    /// </summary>
    /// <remarks>
    /// <see cref="WaitHandle.Handle"/> will be used to store the system API
    /// handle of the newly created timer.
    /// </remarks>
    /// <exception cref="TimerException">When the timer could not be created.</exception>
    public WaitableTimer()
    {
      // Create it
      SafeWaitHandle = CreateWaitableTimer(IntPtr.Zero, false, null);

      // Test
      if (SafeWaitHandle.Equals(IntPtr.Zero))
      {
        throw new TimerException("Unable to create Waitable Timer");
      }
    }

    /// <summary>
    /// Make sure that <see cref="Close"/> is called.
    /// </summary>
    ~WaitableTimer()
    {
      // Forward
      Close();
    }

    /// <summary>
    /// Stop <see cref="m_Waiting"/> if necessary. To do so <see cref="Thread.Abort"/>
    /// is used.
    /// <seealso cref="SecondsToWait"/>
    /// <seealso cref="Close"/>
    /// </summary>
    private void AbortWaiter()
    {
      // Kill thread
      if (null == m_Waiting)
      {
        return;
      }

      // Terminate it
      try
      {
        m_Waiting.Abort();
      }
      catch (Exception) {}

      // Detach
      m_Waiting = null;
    }

    /// <summary>
    /// Activate the timer to stop at the indicated date/time
    /// </summary>
    /// <remarks>
    /// This method will always call <see cref="AbortWaiter"/>. If the date/time
    /// is in the future a new <see cref="m_Waiting"/> <see cref="Thread"/>
    /// will be created running <see cref="WaitThread"/>. Before calling
    /// <see cref="Thread.Start"/> the <see cref="m_ExpirationTime"/> is initialized
    /// with the correct value. If the date/time is in the past or infinite
    /// the timer is canceled.
    /// </remarks>
    public DateTime TimeToWakeup
    {
      set
      {
        // Done with thread
        AbortWaiter();
        // Check mode
        if (value > DateTime.Now && value != DateTime.MaxValue)
        {
          // Calculate
          m_ExpirationTime = value.ToUniversalTime().ToFileTimeUtc();

          // Create thread
          m_Waiting = new Thread(new ParameterizedThreadStart(WaitThread));
          // Causes 100 % CPU load on XP systems
          // m_Waiting.Priority = ThreadPriority.AboveNormal;
          m_Waiting.Name = "PowerScheduler Waiter";

          using (ManualResetEvent handshake = new ManualResetEvent(false))
          {
            // Run it
            m_Waiting.Start(handshake);
            // wait until wakeup timer is set
            handshake.WaitOne();
          }
        }
        else
        {
          // No timer
          CancelWaitableTimer(SafeWaitHandle);
        }
      }
    }

    /// <summary>
    /// Activate the timer to stop after the indicated number of seconds.
    /// </summary>
    /// <remarks>
    /// This method will always call <see cref="AbortWaiter"/>. If the number
    /// of seconds is positive a new <see cref="m_Waiting"/> <see cref="Thread"/>
    /// will be created running <see cref="WaitThread"/>. Before calling
    /// <see cref="Thread.Start"/> the <see cref="m_ExpirationTime"/> is initialized
    /// with the correct value. If the number of seconds is zero or negative
    /// the timer is canceled.
    /// </remarks>
    public double SecondsToWait
    {
      set
      {
        // Done with thread
        AbortWaiter();
        // Check mode
        if (value > 0)
        {
          // Calculate
          m_ExpirationTime = DateTime.UtcNow.AddSeconds(value).ToFileTimeUtc();

          // Create thread
          m_Waiting = new Thread(new ParameterizedThreadStart(WaitThread));
          // Causes 100 % CPU load on XP systems
          // m_Waiting.Priority = ThreadPriority.AboveNormal;
          m_Waiting.Name = "PowerScheduler Waiter";

          using (ManualResetEvent handshake = new ManualResetEvent(false))
          {
            // Run it
            m_Waiting.Start(handshake);
            // wait until wakeup timer is set
            handshake.WaitOne();
          }
        }
        else
        {
          // No timer
          CancelWaitableTimer(SafeWaitHandle);
        }
      }
    }

    /// <summary>
    /// Initializes the timer with <see cref="m_ExpirationTime"/> and waits for it
    /// to expire. If the timer expires <see cref="OnTimerExpired"/> is fired.
    /// </summary>
    /// <remarks>
    /// The <see cref="Thread"/> may be terminated with a call to <see cref="AbortWaiter"/>
    /// before the time expires.
    /// </remarks>
    private void WaitThread(object arg)
    {
      ManualResetEvent handshake = (ManualResetEvent)arg;
      // Ignore aborts
      try
      {
        // Start timer
        long dueTime = m_ExpirationTime;
        if (!SetWaitableTimer(SafeWaitHandle, &dueTime, 0, IntPtr.Zero, IntPtr.Zero, true))
        {
          throw new TimerException("Could not start Timer", new Win32Exception(Marshal.GetLastWin32Error()));
        }
        handshake.Set();
        handshake = null;

        // Wait for the timer to expire
        WaitOne();

        // Forward
        if (null != OnTimerExpired)
        {
          OnTimerExpired();
        }
      }
      catch (TimerException e)
      {
        if (handshake != null)
        {
          handshake.Set();
          handshake = null;
        }

        if (OnTimerException != null)
        {
          OnTimerException(this, e);
        }
      }
      catch (ThreadAbortException)
      {
        // Ignore
      }
      catch (Exception)
      {
        // Ignore. We should log this but we do not yet have a common logging infrastructure between MP and TVServer
      }
      finally
      {
        if (handshake != null)
        {
          handshake.Set();
        }
      }
    }

    /// <summary>
    /// Calles <see cref="AbortWaiter"/> and forwards to the base <see cref="WaitHandle.Close"/>
    /// method.
    /// </summary>
    public override void Close()
    {
      // Kill thread
      AbortWaiter();

      // Forward
      base.Close();
    }
  }
}