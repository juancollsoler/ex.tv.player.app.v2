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

#region Usings

using System;
using System.Threading;

#endregion

namespace MediaPortal.Threading
{
  public interface IWork
  {
    /// <summary>
    /// The current state the work is in
    /// (set by the threadpool and used to cancel work which is still in the queue)
    /// </summary>
    WorkState State { get; set; }

    /// <summary>
    /// Description for this work (optional)
    /// </summary>
    string Description { get; set; }

    /// <summary>
    /// Placeholder for any exception thrown by this the workload code
    /// </summary>
    Exception Exception { get; set; }

    /// <summary>
    /// Specifies the scheduling priority for this work
    /// </summary>
    ThreadPriority ThreadPriority { get; set; }

    /// <summary>
    /// Method which contains the work that should be performed by the ThreadPool
    /// </summary>
    void Process();
  }
}