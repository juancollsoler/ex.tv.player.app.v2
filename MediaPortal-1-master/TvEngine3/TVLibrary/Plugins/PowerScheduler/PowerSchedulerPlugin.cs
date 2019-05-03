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

using System.Reflection;
using PowerScheduler.Setup;
using SetupTv;
using TvControl;
using TvEngine;

#endregion

namespace TvEngine.PowerScheduler
{
  public class PowerSchedulerPlugin : ITvServerPlugin
  {
    #region Variables

    /// <summary>
    /// Reference to the tvservice's TVcontroller
    /// </summary>
    private IController _controller;

    #endregion

    #region Constructor

    /// <summary>
    /// Creates a new PowerScheduler plugin
    /// </summary>
    public PowerSchedulerPlugin() {}

    #endregion

    #region ITvServerPlugin implementation

    /// <summary>
    /// Called by the tvservice PluginLoader to start the PowerScheduler plugin
    /// </summary>
    /// <param name="controller">Reference to the tvservice's TVController</param>
    public void Start(IController controller)
    {
      _controller = controller;
      PowerScheduler.Instance.Start(controller);
    }

    /// <summary>
    /// Called by the tvservice PluginLoader to stop the PowerScheduler plugin
    /// </summary>
    public void Stop()
    {
      PowerScheduler.Instance.Stop();
    }

    /// <summary>
    /// Author of this plugin
    /// </summary>
    public string Author
    {
        get { return "michael_t (based on the work of micheloe and others)"; }
    }

    /// <summary>
    /// Should this plugin run only on a master tvserver?
    /// </summary>
    public bool MasterOnly
    {
      get { return false; }
    }

    /// <summary>
    /// Name of this plugin
    /// </summary>
    public string Name
    {
      get
      {
        object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
        if (attributes.Length > 0)
        {
          AssemblyProductAttribute attribute = attributes[0] as AssemblyProductAttribute;
          if (attribute != null && attribute.Product != "MediaPortal")
            return attribute.Product;
        }
        return "PowerScheduler";
      }
    }

    /// <summary>
    /// Returns the SectionSettings setup part of this plugin
    /// </summary>
    public SectionSettings Setup
    {
      get { return new PowerSchedulerSetup(); }
    }

    /// <summary>
    /// Plugin version
    /// </summary>
    public string Version
    {
      get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
    }

    #endregion
  }
}