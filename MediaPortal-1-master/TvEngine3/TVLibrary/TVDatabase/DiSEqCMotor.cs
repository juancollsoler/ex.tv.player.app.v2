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
  /// Instances of this class represent the properties and methods of a row in the table <b>Server</b>.
  /// </summary>
  [TableName("DiSEqCMotor")]
  public class DiSEqCMotor : Persistent
  {
    #region Members

    private bool isChanged;
    [TableColumn("idDiSEqCMotor", NotNull = true), PrimaryKey(AutoGenerated = true)] private int idDiSEqCMotor;
    [TableColumn("idCard", NotNull = true)] private int idCard;
    [TableColumn("idSatellite", NotNull = true)] private int idSatellite;
    [TableColumn("position", NotNull = true)] private int position;

    #endregion

    #region Constructors

    /// <summary> 
    /// Create a new object by specifying all fields (except the auto-generated primary key field). 
    /// </summary> 
    public DiSEqCMotor(int idCard, int idSatellite, int position)
    {
      isChanged = true;
      this.idCard = idCard;
      this.idSatellite = idSatellite;
      this.position = position;
    }

    /// <summary> 
    /// Create an object from an existing row of data. This will be used by Gentle to 
    /// construct objects from retrieved rows. 
    /// </summary> 
    public DiSEqCMotor(int idDiSEqCMotor, int idCard, int idSatellite, int position)
    {
      this.idDiSEqCMotor = idDiSEqCMotor;
      this.idCard = idCard;
      this.idSatellite = idSatellite;
      this.position = position;
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
    /// Property relating to database column idServer
    /// </summary>
    public int IdDiSEqCMotor
    {
      get { return idDiSEqCMotor; }
    }

    /// <summary>
    /// Property relating to database column hostName
    /// </summary>
    public int IdCard
    {
      get { return idCard; }
      set
      {
        isChanged |= idCard != value;
        idCard = value;
      }
    }

    /// <summary>
    /// Property relating to database column hostName
    /// </summary>
    public int IdSatellite
    {
      get { return idSatellite; }
      set
      {
        isChanged |= idSatellite != value;
        idSatellite = value;
      }
    }

    /// <summary>
    /// Property relating to database column hostName
    /// </summary>
    public int Position
    {
      get { return position; }
      set
      {
        isChanged |= position != value;
        position = value;
      }
    }

    #endregion

    #region Storage and Retrieval

    /// <summary>
    /// Static method to retrieve all instances that are stored in the database in one call
    /// </summary>
    public static IList<DiSEqCMotor> ListAll()
    {
      return Broker.RetrieveList<DiSEqCMotor>();
    }

    /// <summary>
    /// Retrieves an entity given it's id.
    /// </summary>
    public static DiSEqCMotor Retrieve(int id)
    {
      // Return null if id is smaller than seed and/or increment for autokey
      if (id < 1)
      {
        return null;
      }
      Key key = new Key(typeof (DiSEqCMotor), true, "idDiSEqCMotor", id);
      return Broker.RetrieveInstance<DiSEqCMotor>(key);
    }

    /// <summary>
    /// Retrieves an entity given it's id, using Gentle.Framework.Key class.
    /// This allows retrieval based on multi-column keys.
    /// </summary>
    public static DiSEqCMotor Retrieve(Key key)
    {
      return Broker.RetrieveInstance<DiSEqCMotor>(key);
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
          Log.Error("Exception in DiseqcMotor.Persist() with Message {0}", ex.Message);
          return;
        }
        isChanged = false;
      }
    }

    #endregion

    #region Relations

    /// <summary>
    /// Get a list of Card referring to the current entity.
    /// </summary>
    public IList<Card> ReferringCard()
    {
      //select * from 'foreigntable'
      SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof (Card));

      // where foreigntable.foreignkey = ourprimarykey
      sb.AddConstraint(Operator.Equals, "idCard", idCard);

      // passing true indicates that we'd like a list of elements, i.e. that no primary key
      // constraints from the type being retrieved should be added to the statement
      SqlStatement stmt = sb.GetStatement(true);

      // execute the statement/query and create a collection of User instances from the result set
      return ObjectFactory.GetCollection<Card>(stmt.Execute());
    }

    /// <summary>
    /// Get a list of Satellite referring to the current entity.
    /// </summary>
    public IList<Satellite> ReferringSatellite()
    {
      //select * from 'foreigntable'
      SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof (Satellite));

      // where foreigntable.foreignkey = ourprimarykey
      sb.AddConstraint(Operator.Equals, "idSatellite", idSatellite);

      // passing true indicates that we'd like a list of elements, i.e. that no primary key
      // constraints from the type being retrieved should be added to the statement
      SqlStatement stmt = sb.GetStatement(true);

      // execute the statement/query and create a collection of User instances from the result set
      return ObjectFactory.GetCollection<Satellite>(stmt.Execute());
    }

    #endregion
  }
}