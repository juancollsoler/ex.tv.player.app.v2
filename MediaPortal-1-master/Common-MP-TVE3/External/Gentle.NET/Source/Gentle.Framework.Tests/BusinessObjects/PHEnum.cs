/*
 * Test cases
 * Copyright (C) 2004 Morten Mertner
 * 
 * This library is free software; you can redistribute it and/or modify it 
 * under the terms of the GNU Lesser General Public License 2.1 or later, as
 * published by the Free Software Foundation. See the included License.txt
 * or http://www.gnu.org/copyleft/lesser.html for details.
 *
 * $Id: PHEnum.cs 1232 2008-03-14 05:36:00Z mm $
 */
using System;
using System.Collections;

namespace Gentle.Framework.Tests
{
	/// <summary>
	/// This type is used to test for handling enums as strings.
	/// </summary>
	[TableName( "PropertyHolder" )]
	public class PHEnum : Persistent
	{
		[TableColumn( "ph_Id", NotNull = true ), PrimaryKey( AutoGenerated = true ), SequenceName( "PROPERTYHOLDER_SEQ" )]
		protected int id;
		[TableColumn( "ph_Name", NotNull = true, HandleEnumAsString = true )] // text column
			protected DayOfWeek asText;
		[TableColumn( "TNVarChar", NotNull = false, HandleEnumAsString = true )] // nvarchar column
			protected DayOfWeek asNVarChar;
		[TableColumn( "TNText", NotNull = false, HandleEnumAsString = true )] // ntext column
			protected DayOfWeek asNText;
		[TableColumn( "TDateTimeNN", NotNull = true )]
		protected DateTime dtnn = DateTime.Now;

		/// <summary>
		/// Construct a new instance (no database record).
		/// </summary>
		public PHEnum()
		{
		}

		/// <summary>
		/// Construct an instance for an existing database record.
		/// </summary>
		/// <param name="id">The id of the list</param>
		public static PHEnum Retrieve( int id )
		{
			Key key = new Key( typeof(PHEnum), true, "id", id );
			return Broker.RetrieveInstance( typeof(PHEnum), key ) as PHEnum;
		}

		/// <summary>
		/// Construct an instance for an existing database record (all fields specified).
		/// </summary>
		public PHEnum( int id, DayOfWeek asText, DayOfWeek asNVarChar, DayOfWeek asNText )
			// string name, string _nvarchar, string _ntext )
		{
			this.id = id;
			this.asText = asText;
			this.asNVarChar = asNVarChar;
			this.asNText = asNText;
		}

		#region PHEnum Properties
		public int Id
		{
			get { return id; }
		}

		public DayOfWeek AsText
		{
			get { return asText; }
		}

		public DayOfWeek AsNVarChar
		{
			get { return asNVarChar; }
		}

		public DayOfWeek AsNText
		{
			get { return asNText; }
		}
		#endregion

		public void SetEnum( DayOfWeek dow )
		{
			asText = dow;
			asNVarChar = dow;
			asNText = dow;
		}

		/// <summary>
		/// Example list retrieval of all objects (no constraints applied).
		/// </summary>
		public static IList ListAll
		{
			get { return Broker.RetrieveList( typeof(PHEnum) ); }
		}
	}
}