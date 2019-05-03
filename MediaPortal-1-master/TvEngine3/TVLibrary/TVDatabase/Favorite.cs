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
  /// Instances of this class represent the properties and methods of a row in the table <b>Favorite</b>.
  /// </summary>
  [TableName("Favorite")]
  public class Favorite : Persistent
  {
    #region Members

    private bool isChanged;
    [TableColumn("idFavorite", NotNull = true), PrimaryKey(AutoGenerated = true)] private int idFavorite;
    [TableColumn("idProgram", NotNull = true), ForeignKey("Program", "idProgram")] private int idProgram;
    [TableColumn("priority", NotNull = true)] private int priority;
    [TableColumn("timesWatched", NotNull = true)] private int timesWatched;

    #endregion

    #region Constructors

    /// <summary> 
    /// Create a new object by specifying all fields (except the auto-generated primary key field). 
    /// </summary> 
    public Favorite(int idProgram, int priority, int timesWatched)
    {
      isChanged = true;
      this.idProgram = idProgram;
      this.priority = priority;
      this.timesWatched = timesWatched;
    }

    /// <summary> 
    /// Create an object from an existing row of data. This will be used by Gentle to 
    /// construct objects from retrieved rows. 
    /// </summary> 
    public Favorite(int idFavorite, int idProgram, int priority, int timesWatched)
    {
      this.idFavorite = idFavorite;
      this.idProgram = idProgram;
      this.priority = priority;
      this.timesWatched = timesWatched;
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
    /// Property relating to database column idFavorite
    /// </summary>
    public int IdFavorite
    {
      get { return idFavorite; }
    }

    /// <summary>
    /// Property relating to database column idProgram
    /// </summary>
    public int IdProgram
    {
      get { return idProgram; }
      set
      {
        isChanged |= idProgram != value;
        idProgram = value;
      }
    }

    /// <summary>
    /// Property relating to database column priority
    /// </summary>
    public int Priority
    {
      get { return priority; }
      set
      {
        isChanged |= priority != value;
        priority = value;
      }
    }

    /// <summary>
    /// Property relating to database column timesWatched
    /// </summary>
    public int TimesWatched
    {
      get { return timesWatched; }
      set
      {
        isChanged |= timesWatched != value;
        timesWatched = value;
      }
    }

    #endregion

    #region Storage and Retrieval

    /// <summary>
    /// Static method to retrieve all instances that are stored in the database in one call
    /// </summary>
    public static IList<Favorite> ListAll()
    {
      return Broker.RetrieveList<Favorite>();
    }

    /// <summary>
    /// Retrieves an entity given it's id.
    /// </summary>
    public static Favorite Retrieve(int id)
    {
      // Return null if id is smaller than seed and/or increment for autokey
      if (id < 1)
      {
        return null;
      }
      Key key = new Key(typeof (Favorite), true, "idFavorite", id);
      return Broker.RetrieveInstance<Favorite>(key);
    }

    /// <summary>
    /// Retrieves an entity given it's id, using Gentle.Framework.Key class.
    /// This allows retrieval based on multi-column keys.
    /// </summary>
    public static Favorite Retrieve(Key key)
    {
      return Broker.RetrieveInstance<Favorite>(key);
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
          Log.Error("Exception in Favorite.Persist() with Message {0}", ex.Message);
          return;
        }
        isChanged = false;
      }
    }

    #endregion

    #region Relations

    /// <summary>
    ///
    /// </summary>
    public Program ReferencedProgram()
    {
      return Program.Retrieve(IdProgram);
    }

    #endregion
  }
}