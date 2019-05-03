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
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MpeCore.Classes
{
  public class ExtensionCollection
  {
    public ExtensionCollection()
    {
      Items = new List<PackageClass>();
      IgnoredUpdates = new List<string>();
    }

    public List<PackageClass> Items { get; set; }

    [XmlIgnore]
    public List<string> IgnoredUpdates { get; set; }

    /// <summary>
    /// Gets the unique list of extensions with highest version
    /// </summary>
    /// <returns></returns>
    public ExtensionCollection GetUniqueList()
    {
      return GetUniqueList(null);
    }

    /// <summary>
    /// Gets the unique list of extensions with highest version
    /// </summary>
    /// <param name="exlude">Exlude these extensions</param>
    /// <returns></returns>
    public ExtensionCollection GetUniqueList(ExtensionCollection exlude)
    {
      HashSet<string> exludedIds = new HashSet<string>();
      if (exlude != null) exlude.Items.ForEach(p => exludedIds.Add(p.GeneralInfo.Id));
      Dictionary<string, PackageClass> ids = new Dictionary<string, PackageClass>();
      
      foreach (PackageClass item in Items)
      {
        if (item.IsHiden || exludedIds.Contains(item.GeneralInfo.Id))
          continue;

        PackageClass currentVersion = null;
        if (!ids.TryGetValue(item.GeneralInfo.Id, out currentVersion))
        {
          ids.Add(item.GeneralInfo.Id, item);
        }
        else
        {
          // overwrite if has a higher version
          if (item.GeneralInfo.Version.CompareTo(currentVersion.GeneralInfo.Version) > 0)
          {
            ids[item.GeneralInfo.Id] = item;
          }
        }
      }

      var collection = new ExtensionCollection();
      foreach (var package in ids.Values) collection.Add(package);
      return collection;
    }

    /// <summary>
    /// Gets a list with unique update urls
    /// </summary>
    /// <returns></returns>
    public List<string> GetUpdateUrls(List<string> urls)
    {
      foreach (PackageClass item in Items)
      {
        if (item.IsHiden)
          continue;
        if (string.IsNullOrEmpty(item.GeneralInfo.UpdateUrl))
          continue;
        if (!urls.Contains(item.GeneralInfo.UpdateUrl))
          urls.Add(item.GeneralInfo.UpdateUrl);
      }
      return urls;
    }


    public void Add(ExtensionCollection collection)
    {
      foreach (PackageClass item in collection.Items)
      {
        Add(item);
      }
    }


    public TagCollection TagList
    {
      get
      {
        TagCollection collection = new TagCollection();
        foreach (PackageClass item in Items)
        {
          collection.Add(item.GeneralInfo.TagList);
        }
        return collection;
      }
    }

    /// <summary>
    /// Adds the specified package class. But without copy all the structure the GeneralInfo and the group Names
    /// If the package with same version number already exist, will be replaced
    /// </summary>
    /// <param name="packageClass">The package class.</param>
    public void Add(PackageClass packageClass)
    {
      PackageClass oldpak = Get(packageClass);
      if (oldpak != null)
        Items.Remove(oldpak);
      PackageClass pak = new PackageClass();
      pak.GeneralInfo = packageClass.GeneralInfo;
      foreach (GroupItem groupItem in packageClass.Groups.Items)
      {
        pak.Groups.Items.Add(new GroupItem(groupItem.Name, groupItem.Checked));
      }
      pak.Dependencies = packageClass.Dependencies;
      pak.PluginDependencies = packageClass.PluginDependencies;
      pak.IsSkin = packageClass.IsSkin;
      pak.Version = packageClass.Version;
      pak.Parent = this;
      Items.Add(pak);
    }

    /// <summary>
    /// Removes the specified package class fromr the list.
    /// </summary>
    /// <param name="packageClass">The package class.</param>
    public void Remove(PackageClass packageClass)
    {
      PackageClass pak = Get(packageClass);
      if (pak == null)
        return;
      Items.Remove(pak);
    }

    public PackageClass Get(PackageClass pak)
    {
      return Get(pak.GeneralInfo.Id, pak.GeneralInfo.Version.ToString());
    }

    /// <summary>
    /// Gets the specified id.
    /// </summary>
    /// <param name="id">The package GUID.</param>
    /// <param name="version">The version.</param>
    /// <returns>If found a package with specified Version and GUID return the package else NULL</returns>
    public PackageClass Get(string id, string version)
    {
      foreach (PackageClass item in Items)
      {
        if (item.GeneralInfo.Id == id && String.Compare(item.GeneralInfo.Version.ToString(), version, StringComparison.Ordinal) == 0)
          return item;
      }
      return null;
    }

    /// <summary>
    /// Gets the hightes version number package with specified id.
    /// </summary>
    /// <param name="id">The package GUID.</param>
    /// <returns>If found a package with specified GUID return the package else NULL</returns>
    public PackageClass Get(string id)
    {
      PackageClass ret = null;
      foreach (PackageClass item in Items)
      {
        if (item.IsHiden)
          continue;
        if (item.GeneralInfo.Id == id)
        {
          if (ret == null)
          {
            ret = item;
          }
          else
          {
            if (item.GeneralInfo.Version.CompareTo(ret.GeneralInfo.Version) > 0)
              ret = item;
          }
        }
      }
      return ret;
    }

    /// <summary>
    /// Gets a list of extensions with same ids
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    public ExtensionCollection GetList(string id)
    {
      ExtensionCollection resp = new ExtensionCollection();
      foreach (PackageClass item in Items)
      {
        if (item.IsHiden)
          continue;
        if (item.GeneralInfo.Id == id)
        {
          resp.Add(item);
        }
      }
      resp.Items.Sort(PackageClass.Compare);
      return resp;
    }

    /// <summary>
    /// Sorts this instance.
    /// </summary>
    public void Sort(bool versionAscending)
    {
      if (versionAscending)
        Items.Sort(PackageClass.CompareVersionAscending);
      else
        Items.Sort(PackageClass.Compare);
    }

    /// <summary>
    /// Gets the latest version of a package. If not found or no new version return Null
    /// </summary>
    /// <param name="pak">The package</param>
    /// <returns></returns>
    public PackageClass GetUpdate(PackageClass pak)
    {
      if (IgnoredUpdates.Contains(pak.GeneralInfo.Id))
        return null;
      PackageClass ret = Get(pak.GeneralInfo.Id);
      if (ret == null)
        return null;
      if (ret.GeneralInfo.Version.CompareTo(pak.GeneralInfo.Version) > 0)
        return ret;
      return null;
    }


    public void Save(string fileName)
    {
      if (!Directory.Exists(Path.GetDirectoryName(fileName)))
        Directory.CreateDirectory(Path.GetDirectoryName(fileName));
      var serializer = new XmlSerializer(typeof (ExtensionCollection));
      TextWriter writer = new StreamWriter(fileName);
      serializer.Serialize(writer, this);
      writer.Close();
    }

    /// <summary>
    /// Loads the specified file name.
    /// if some error ocure, empty list will be returned
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns></returns>
    public static ExtensionCollection Load(string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        return new ExtensionCollection();

      if (!File.Exists(fileName))
        return new ExtensionCollection();


      FileStream fs = null;
      try
      {
        XmlSerializer serializer = new XmlSerializer(typeof (ExtensionCollection));
        fs = new FileStream(fileName, FileMode.Open);
        ExtensionCollection extensionCollection = (ExtensionCollection)serializer.Deserialize(fs);
        fs.Close();
        extensionCollection.Items.Sort(PackageClass.Compare);
        foreach (PackageClass item in extensionCollection.Items)
        {
          item.Parent = extensionCollection;
        }
        return extensionCollection;
      }
      catch
      {
        if (fs != null)
          fs.Dispose();
        return new ExtensionCollection();
      }
    }

    /// <summary>
    /// Hide extensions which not are stabe releases
    /// </summary>
    public void HideByRelease()
    {
      foreach (PackageClass item in Items)
      {
        item.IsHiden = false;
        if (item.GeneralInfo.DevelopmentStatus != ParamNamesConst.DEVELOPMENTSTATUS_STABE)
        {
          item.IsHiden = true;
        }
      }
    }

    /// <summary>
    /// Hide extensions which isn't compatible
    /// </summary>
    public void HideByDependencies()
    {
      foreach (PackageClass item in Items)
      {
        item.IsHiden = false;
        if (!item.CheckDependency(true))
        {
          item.IsHiden = true;
        }
      }
    }

    public void Hide(bool onlystable, bool onlycompatible)
    {
      foreach (PackageClass item in Items)
      {
        item.IsHiden = false;
        if (onlycompatible)
        {
          if (!item.CheckDependency(true))
          {
            item.IsHiden = true;
          }
        }
        if (onlystable)
        {
          if (item.GeneralInfo.DevelopmentStatus != ParamNamesConst.DEVELOPMENTSTATUS_STABE)
          {
            item.IsHiden = true;
          }
        }
      }
    }

    public int GetHiddenExtensionCount()
    {
      int i = 0;
      foreach (PackageClass item in Items)
      {
        if (item.IsHiden)
          i++;
      }
      return i;
    }

    public void ShowAll()
    {
      foreach (PackageClass item in Items)
      {
        item.IsHiden = false;
      }
    }
  }
}