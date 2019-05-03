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
  [TableName("Server")]
  public class Server : Persistent
  {
    #region Constants

    private const int DEFAULT_RTSP_PORT = 554;

    #endregion

    #region Members

    private bool isChanged;
    [TableColumn("idServer", NotNull = true), PrimaryKey(AutoGenerated = true)] private int idServer;
    [TableColumn("isMaster", NotNull = true)] private bool isMaster;
    [TableColumn("hostName", NotNull = true)] private string hostName;
    [TableColumn("rtspPort", NotNull = true)] private int rtspPort;

    #endregion

    #region Constructors

    /// <summary> 
    /// Create a new object by specifying all fields (except the auto-generated primary key field). 
    /// </summary> 
    public Server(bool isMaster, string hostName, int rtspPort)
    {
      isChanged = true;
      this.isMaster = isMaster;
      this.hostName = hostName;
      if (rtspPort == 0)
      {
        this.rtspPort = DEFAULT_RTSP_PORT;
        this.isChanged = true;
      }
      else
      {
        this.rtspPort = rtspPort;
      }
    }

    /// <summary> 
    /// Create an object from an existing row of data. This will be used by Gentle to 
    /// construct objects from retrieved rows. 
    /// </summary> 
    public Server(int idServer, bool isMaster, string hostName, int rtspPort)
    {
      this.idServer = idServer;
      this.isMaster = isMaster;
      this.hostName = hostName;
      this.rtspPort = rtspPort;
      if (rtspPort == 0)
      {
        this.rtspPort = DEFAULT_RTSP_PORT;
        this.isChanged = true;
      }
      else
      {
        this.rtspPort = rtspPort;
      }
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
    public int IdServer
    {
      get { return idServer; }
    }

    /// <summary>
    /// Property relating to database column isMaster
    /// </summary>
    public bool IsMaster
    {
      get { return isMaster; }
      set
      {
        isChanged |= isMaster != value;
        isMaster = value;
      }
    }

    /// <summary>
    /// Property relating to database column hostName
    /// </summary>
    public string HostName
    {
      get { return hostName; }
      set
      {
        isChanged |= hostName != value;
        hostName = value;
      }
    }

    /// <summary>
    /// Property relating to database column rtspPort
    /// </summary>
    public int RtspPort
    {
      get { return rtspPort; }
      set
      {
        isChanged |= rtspPort != value;
        rtspPort = value == 0 ? DEFAULT_RTSP_PORT : value;
      }
    }

    #endregion

    #region Storage and Retrieval

    /// <summary>
    /// Static method to retrieve all instances that are stored in the database in one call
    /// </summary>
    public static IList<Server> ListAll()
    {
      return Broker.RetrieveList<Server>();
    }

    /// <summary>
    /// Retrieves an entity given it's id.
    /// </summary>
    public static Server Retrieve(int id)
    {
      // Return null if id is smaller than seed and/or increment for autokey
      if (id < 1)
      {
        return null;
      }
      Key key = new Key(typeof (Server), true, "idServer", id);
      return Broker.RetrieveInstance<Server>(key);
    }

    /// <summary>
    /// Retrieves an entity given it's id, using Gentle.Framework.Key class.
    /// This allows retrieval based on multi-column keys.
    /// </summary>
    public static Server Retrieve(Key key)
    {
      return Broker.RetrieveInstance<Server>(key);
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
          Log.Error("Exception in Server.Persist() with Message {0}", ex.Message);
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
      sb.AddConstraint(Operator.Equals, "idServer", idServer);

      // passing true indicates that we'd like a list of elements, i.e. that no primary key
      // constraints from the type being retrieved should be added to the statement
      SqlStatement stmt = sb.GetStatement(true);

      // execute the statement/query and create a collection of User instances from the result set
      return ObjectFactory.GetCollection<Card>(stmt.Execute());

      // TODO In the end, a GentleList should be returned instead of an arraylist
      //return new GentleList( typeof(Card), this );
    }

    /// <summary>
    /// Get a list of Recording referring to the current entity.
    /// </summary>
    public IList<Recording> ReferringRecording()
    {
      //select * from 'foreigntable'
      SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof (Recording));

      // where foreigntable.foreignkey = ourprimarykey
      sb.AddConstraint(Operator.Equals, "idServer", idServer);

      // passing true indicates that we'd like a list of elements, i.e. that no primary key
      // constraints from the type being retrieved should be added to the statement
      SqlStatement stmt = sb.GetStatement(true);

      // execute the statement/query and create a collection of User instances from the result set
      return ObjectFactory.GetCollection<Recording>(stmt.Execute());

      // TODO In the end, a GentleList should be returned instead of an arraylist
      //return new GentleList( typeof(Recording), this );
    }

    #endregion

    public void Delete()
    {
      IList<Card> list = ReferringCard();
      foreach (Card card in list)
      {
        card.Delete();
      }
      IList<Recording> listRecordings = ReferringRecording();
      foreach (Recording recording in listRecordings)
      {
        recording.Delete();
      }

      Remove();
    }
  }
}