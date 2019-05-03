/*
 * Test cases
 * Copyright (C) 2004 Morten Mertner
 * 
 * This library is free software; you can redistribute it and/or modify it 
 * under the terms of the GNU Lesser General Public License 2.1 or later, as
 * published by the Free Software Foundation. See the included License.txt
 * or http://www.gnu.org/copyleft/lesser.html for details.
 *
 * $Id: User.cs 1232 2008-03-14 05:36:00Z mm $
 */

using System.Collections;

namespace Gentle.Framework.Tests
{
	/// <summary>
	/// This class exists for the purpose of testing automatic management of n:m
	/// relationships, graph operations (i.e. recursive processing of objects and
	/// all properties on it), and reserved word handling.
	/// </summary>
	[TableName( "Users" )]
	public class User : Persistent
	{
		protected int id;
		protected string firstName;
		protected string lastName;
		protected Roles primaryRole; // test of enum property
		protected GentleList roles; // container for storing the role objects

		// construct instance by retrieving entry from database
		public User( int userId )
		{
			// update key field
			id = userId;
			// retrieve instance and update all properties
			// WARNING this is much slower than using the static Retrieve method below
			if( userId > 0 )
			{
				Broker.Refresh( this );
			}
		}

		public static User Retrieve( int id )
		{
			Key key = new Key( typeof(User), true, "Id", id );
			return Broker.RetrieveInstance( typeof(User), key ) as User;
		}

		// construct new instance
		public User( string firstName, string lastName, Roles primaryRole ) :
			this( 0, firstName, lastName, primaryRole )
		{
		}

		/// <summary>
		/// Construct instance from existing data. This is the constructor invoked by 
		/// Gentle when creating new User instances from result sets.
		/// </summary>
		public User( int userId, string firstName, string lastName, Roles primaryRole )
		{
			id = userId;
			this.firstName = firstName;
			this.lastName = lastName;
			this.primaryRole = primaryRole;
		}

		#region Persistent Properties
		[TableColumn( "UserId" ), PrimaryKey( AutoGenerated = true ), SequenceName( "USERS_SEQ" )]
		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		[TableColumn]
		public virtual string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		[TableColumn]
		public virtual string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		[TableColumn]
		public virtual Roles PrimaryRole
		{
			get { return primaryRole; }
			set { primaryRole = value; }
		}
		#endregion

		public virtual GentleList Roles
		{
			get
			{
				if( roles == null )
				{
					roles = new GentleList( typeof(Role), this, typeof(UserRole) );
				}
				return roles;
			}
		}

		public virtual GentleList MemberRoles
		{
			get
			{
				if( roles == null )
				{
					roles = new GentleList( typeof(Role), this, typeof(UserRole), typeof(Member) );
				}
				return roles;
			}
		}

		public static IList List
		{
			get { return Broker.RetrieveList( typeof(User) ); }
		}
	}
}