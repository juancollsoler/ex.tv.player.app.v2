/*
 * Test cases
 * Copyright (C) 2008 Morten Mertner
 * 
 * This library is free software; you can redistribute it and/or modify it 
 * under the terms of the GNU Lesser General Public License 2.1 or later, as
 * published by the Free Software Foundation. See the included License.txt
 * or http://www.gnu.org/copyleft/lesser.html for details.
 *
 * $Id: PHNullableType.cs 1233 2008-03-14 06:29:09Z mm $
 */
using System;
using System.Collections.Generic;

namespace Gentle.Framework.Tests
{
	/// <summary>
	/// This type is used to test for handling enums as strings.
	/// </summary>
	[TableName( "PropertyHolder" )]
	public class PHNullableType : Persistent
	{
		[TableColumn( "ph_Id", NotNull = true ), PrimaryKey( AutoGenerated = true ), SequenceName( "PROPERTYHOLDER_SEQ" )]
		protected int id;
		[TableColumn( "ph_Name", NotNull = true, HandleEnumAsString = true )] // text column
		protected string text;
		[TableColumn( "TDateTime", NotNull = false )]
		protected DateTime? nullableDate;
		[TableColumn( "TDateTimeNN", NotNull = true )]
		protected DateTime date = DateTime.Now;
		
		/// <summary>
		/// Construct an instance for an existing database record.
		/// </summary>
		/// <param name="id">The id of the list</param>
		public static PHNullableType Retrieve( int id )
		{
			Key key = new Key( typeof(PHNullableType), true, "id", id );
			return Broker.RetrieveInstance<PHNullableType>( key );
		}

		/// <summary>
		/// Construct an instance for an existing database record (all fields specified).
		/// </summary>
		public PHNullableType( int id, string text, DateTime? nullableDate )
		{
			this.id = id;
			this.text = text;
			this.nullableDate = nullableDate;
		}

		#region Properties
		public int Id
		{
			get { return id; }
		}

		public string Text
		{
			get { return text; }
		}

		public DateTime? NullableDate
		{
			get { return nullableDate; }
			set { nullableDate = value; }
		}
		#endregion

		/// <summary>
		/// Example list retrieval of all objects (no constraints applied).
		/// </summary>
		public static IList<PHNullableType> ListAll
		{
			get { return Broker.RetrieveList<PHNullableType>(); }
		}
	}
}