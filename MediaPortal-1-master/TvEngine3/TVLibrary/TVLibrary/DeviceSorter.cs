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
using DirectShowLib;
using similaritymetrics;

namespace TvLibrary
{
  internal class DeviceSorter
  {
    private class SortItem : IComparable
    {
      public float rate;
      public DsDevice device;

      #region IComparable Members

      public int CompareTo(object obj)
      {
        SortItem item = obj as SortItem;
        if (item == null || item.rate < rate)
          return -1;
        return 1;
      }

      #endregion
    } ;

    public static DsDevice[] Sort(DsDevice[] devices, params object[] arg)
    {
      try
      {
        if (devices == null)
          return devices;
        if (devices.Length <= 1)
          return devices;
        List<string> compareNames = new List<string>();
        foreach (object obj in arg)
        {
          String name = obj as String;
          if (!string.IsNullOrEmpty(name))
          {
            compareNames.Add(name);
            continue;
          }
          DsDevice dev = obj as DsDevice;
          if (dev == null)
            continue;
          if (dev.Name == null)
            continue;
          if (dev.Name.Length == 0)
            continue;
          compareNames.Add(dev.Name);
        }
        if (compareNames.Count == 0)
          return devices;

        List<string> names = new List<string>();
        for (int i = 0; i < devices.Length; ++i)
        {
          if (devices[i] == null)
            continue;
          if (devices[i].Name == null)
            continue;
          if (devices[i].Name.Length == 0)
            continue;
          names.Add(devices[i].Name);
        }

        //sort the devices based 
        float[] results = new float[devices.Length + 20];
        for (int x = 0; x < results.Length; ++x)
        {
          results[x] = 0.0f;
        }

        for (int i = 0; i < compareNames.Count; ++i)
        {
          Levenstein comparer = new Levenstein();
          float[] tmpResults = comparer.batchCompareSet(names.ToArray(), compareNames[i]);
          for (int x = 0; x < tmpResults.Length; ++x)
          {
            results[x] += tmpResults[x];
          }
        }
        List<SortItem> items = new List<SortItem>();
        for (int i = 0; i < devices.Length; ++i)
        {
          SortItem item = new SortItem();
          item.rate = results[i];
          item.device = devices[i];
          items.Add(item);
        }
        items.Sort();
        DsDevice[] newDevices = new DsDevice[items.Count];

        int index = 0;
        foreach (SortItem item in items)
        {
          newDevices[index] = item.device;
          index++;
        }
        return newDevices;
      }
      catch (Exception ex)
      {
        Log.Log.Write(ex);
        return devices;
      }
    }
  }
}