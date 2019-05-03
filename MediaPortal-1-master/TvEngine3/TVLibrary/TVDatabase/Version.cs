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
using Gentle.Framework;
using TvLibrary.Log;

namespace TvDatabase
{
  /// <summary>
  /// Instances of this class represent the properties and methods of a row in the table <b>Version</b>.
  /// </summary>
  [TableName("Version")]
  public class Version : Persistent
  {
    #region Members

    private bool isChanged;
    [TableColumn("idVersion", NotNull = true), PrimaryKey(AutoGenerated = true)] private int idVersion;
    [TableColumn("versionNumber", NotNull = true)] private int versionNumber;

    #endregion

    #region Constructors

    /// <summary> 
    /// Create a new object by specifying all fields (except the auto-generated primary key field). 
    /// </summary> 
    public Version(int versionNumber)
    {
      isChanged = true;
      this.versionNumber = versionNumber;
    }

    /// <summary> 
    /// Create an object from an existing row of data. This will be used by Gentle to 
    /// construct objects from retrieved rows. 
    /// </summary> 
    public Version(int idVersion, int versionNumber)
    {
      this.idVersion = idVersion;
      this.versionNumber = versionNumber;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Indicates whether the entity is changed and requires saving or not.
    /// </summary>
    public bool IsChanged
    {
      get { return isChanged; }
    }

    /// <summary>
    /// Property relating to database column idVersion
    /// </summary>
    public int IdVersion
    {
      get { return idVersion; }
    }

    /// <summary>
    /// Property relating to database column versionNumber
    /// </summary>
    public int VersionNumber
    {
      get { return versionNumber; }
      set
      {
        isChanged |= versionNumber != value;
        versionNumber = value;
      }
    }

    #endregion

    #region Storage and Retrieval

    /// <summary>
    /// Static method to retrieve all instances that are stored in the database in one call
    /// </summary>
    public static IList<Version> ListAll()
    {
      return Broker.RetrieveList<Version>();
    }

    /// <summary>
    /// Retrieves an entity given it's id.
    /// </summary>
    public static Version Retrieve(int id)
    {
      // Return null if id is smaller than seed and/or increment for autokey
      if (id < 1)
      {
        return null;
      }
      Key key = new Key(typeof (Version), true, "idVersion", id);
      return Broker.RetrieveInstance<Version>(key);
    }

    /// <summary>
    /// Retrieves an entity given it's id, using Gentle.Framework.Key class.
    /// This allows retrieval based on multi-column keys.
    /// </summary>
    public static Version Retrieve(Key key)
    {
      return Broker.RetrieveInstance<Version>(key);
    }

    /// <summary>
    /// Persists the entity if it was never persisted or was changed.
    /// </summary>
    public override void Persist()
    {
      if (IsChanged || !IsPersisted)
      {
        try
        {
          base.Persist();
        }
        catch (Exception ex)
        {
          Log.Error("Exception in Version.Persist() with Message {0}", ex.Message);
          return;
        }
        isChanged = false;
      }
    }

    #endregion
  }
}