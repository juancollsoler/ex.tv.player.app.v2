/*
 * Test classes used to test NULL translation for DateTime fields
 * Copyright (C) 2005 Morten Mertner
 * 
 * This library is free software; you can redistribute it and/or modify it 
 * under the terms of the GNU Lesser General Public License 2.1 or later, as
 * published by the Free Software Foundation. See the included License.txt
 * or http://www.gnu.org/copyleft/lesser.html for details.
 *
 * $Id: PHDateTimeNullValue.cs 1241 2008-04-21 14:49:35Z mm $
 */
using System;

namespace Gentle.Framework.Tests
{
	/// <summary>
	/// Abstract base class for test classes using the PropertyHolder table.
	/// </summary>
	[TableName( "PropertyHolder" )]
	public abstract class PHBase : Persistent
	{
		[TableColumn( "ph_Id", NotNull = true ), PrimaryKey( AutoGenerated = true ), SequenceName( "PROPERTYHOLDER_SEQ" )]
		protected int id;
		[TableColumn( "ph_Name", NotNull = true )]
		protected string name = "unused here";
		[TableColumn( "TDateTimeNN", NotNull = true )] // NULL not permitted
		protected DateTime dtnn;

		/// <summary>
		/// Construct an instance for an existing database record (all fields specified).
		/// </summary>
		protected PHBase( int id, DateTime dtnn )
		{
			this.id = id;
			this.dtnn = dtnn;
		}

		#region PHDateTimeNullValue Properties
		public int Id
		{
			get { return id; }
		}

		public DateTime DTNN
		{
			get { return dtnn; }
			set { dtnn = value; }
		}
		#endregion
	}

	/// <summary>
	/// Test class used to test NULL translation for DateTime fields using MinValue (the default).
	/// </summary>
	[TableName( "PropertyHolder" )]
	public class PHDateTimeNullValueMin : PHBase
	{
		[TableColumn( "TDateTime", NotNull = false )] // NULL permitted
		protected DateTime dt;

		/// <summary>
		/// Construct an instance for an existing database record.
		/// </summary>
		/// <param name="id">The id of the list</param>
		public static PHDateTimeNullValueMin Retrieve( int id )
		{
			Key key = new Key( typeof(PHDateTimeNullValueMin), true, "id", id );
			return Broker.RetrieveInstance( typeof(PHDateTimeNullValueMin), key ) as PHDateTimeNullValueMin;
		}

		/// <summary>
		/// Construct an instance for an existing database record (all fields specified).
		/// </summary>
		public PHDateTimeNullValueMin( int id, DateTime dtnn, DateTime dt ) : base( id, dtnn )
		{
			this.dt = dt;
		}

		/// <summary>
		/// Construct a new instance.
		/// </summary>
		public PHDateTimeNullValueMin( DateTime dtnn, DateTime dt ) : base( 0, dtnn )
		{
			this.dt = dt;
		}

		#region PHDateTimeNullValueMin Properties
		public DateTime DT
		{
			get { return dt; }
			set { dt = value; }
		}
		#endregion
	}

	/// <summary>
	/// Test class used to test NULL translation for DateTime fields using MaxValue.
	/// </summary>
	[TableName( "PropertyHolder" )]
	public class PHDateTimeNullValueMax : PHBase
	{
		[TableColumn( "TDateTime", NotNull = false, NullValue = NullOption.MaxValue )] // NULL permitted
			protected DateTime dt;

		/// <summary>
		/// Construct an instance for an existing database record.
		/// </summary>
		/// <param name="id">The id of the list</param>
		public static PHDateTimeNullValueMax Retrieve( int id )
		{
			Key key = new Key( typeof(PHDateTimeNullValueMax), true, "id", id );
			return Broker.RetrieveInstance( typeof(PHDateTimeNullValueMax), key ) as PHDateTimeNullValueMax;
		}

		/// <summary>
		/// Construct an instance for an existing database record (all fields specified).
		/// </summary>
		public PHDateTimeNullValueMax( int id, DateTime dtnn, DateTime dt ) : base( id, dtnn )
		{
			this.dt = dt;
		}

		/// <summary>
		/// Construct a new instance.
		/// </summary>
		public PHDateTimeNullValueMax( DateTime dtnn, DateTime dt ) : base( 0, dtnn )
		{
			this.dt = dt;
		}

		#region PHDateTimeNullValueMax Properties
		public DateTime DT
		{
			get { return dt; }
			set { dt = value; }
		}
		#endregion
	}
}