/*
 * Test cases
 * Copyright (C) 2004 Morten Mertner
 * 
 * This library is free software; you can redistribute it and/or modify it 
 * under the terms of the GNU Lesser General Public License 2.1 or later, as
 * published by the Free Software Foundation. See the included License.txt
 * or http://www.gnu.org/copyleft/lesser.html for details.
 *
 * $Id: MemberCC.cs 1232 2008-03-14 05:36:00Z mm $
 */
using System.Collections;

namespace Gentle.Framework.Tests
{
	/// <summary>
	/// The Member class represents a MailingList subscriber.
	/// </summary>
	[TableName( "ListMember" )]
	public class MemberCC : Member
	{
		[TableColumn, Concurrency] //, SoftDelete]
			protected int databaseVersion; // for automatic support for optimistic offline locking of rows

		// construct new list instance (new record)
		public MemberCC( int listId, string name, string address ) :
			this( 1, 0, listId, name, address )
		{
		}

		// construct new list instance (existing database record)
		public MemberCC( int databaseVersion, int id, int listId, string name, string address ) :
			base( id, listId, name, address )
		{
			this.databaseVersion = databaseVersion;
		}

		#region Properties
		/// <summary>
		/// This property provides concurrency control when globally enabled, as defined by the
		/// value of the configiguration key "Gentle.Framework/Options/ConcurrencyControl". 
		/// </summary>
		public int DatabaseVersion
		{
			get { return databaseVersion; }
			set { databaseVersion = value; }
		}
		#endregion

		public new static MemberCC Retrieve( int id )
		{
			Key key = new Key( typeof(MemberCC), true, "Id", id );
			return Broker.RetrieveInstance( typeof(MemberCC), key ) as MemberCC;
		}

		public new static IList ListAll
		{
			get { return Broker.RetrieveList( typeof(MemberCC) ); }
		}
	}
}