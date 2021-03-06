/*
 * Test cases
 * Copyright (C) 2004 Morten Mertner
 * 
 * This library is free software; you can redistribute it and/or modify it 
 * under the terms of the GNU Lesser General Public License 2.1 or later, as
 * published by the Free Software Foundation. See the included License.txt
 * or http://www.gnu.org/copyleft/lesser.html for details.
 *
 * $Id: TestMisc.cs 1234 2008-03-14 11:41:44Z mm $
 */

using System;
using System.Data;
using Gentle.Common;
using NUnit.Framework;

namespace Gentle.Framework.Tests
{
	[TestFixture]
	public class TestMisc
	{
		[TableName( "PropertyHolder" )]
		private class BigNull : Persistent
		{
			[TableColumn( "ph_Id", NotNull = true ), PrimaryKey( AutoGenerated = true )]
			public long id = 0;
			[TableColumn( "TLong", NotNull = false, NullValue = 0 )]
			public long bigNull = 0;
		}

		[Test]
		public void TestNullValueForLong()
		{
			// get map to make sure object map is ok
			ObjectMap om = ObjectFactory.GetMap( null, typeof(BigNull) );
			FieldMap fm = om.GetFieldMap( "bigNull" );
			Assert.IsTrue( fm.NullValue != null, "NullValue not set." );
			Assert.IsTrue( fm.IsNullable, "Database column not marked as nullable." );
			Assert.IsTrue( fm.IsValueType, "Type not a value type (can hold null)." );
			Assert.IsFalse( fm.IsNullAssignable, "Type can hold null." );
			long x = 0;
			Assert.AreEqual( x, fm.NullValue, "Comparison 1 of object and long failed." );
			Assert.IsTrue( x.Equals( fm.NullValue ), "Comparison 2 of object and long failed." );
			Assert.IsTrue( fm.NullValue.Equals( x ), "Comparison 3 of object and long failed." );
			// create test statement with bigint parameter
			SqlBuilder sb = new SqlBuilder( StatementType.Insert, typeof(BigNull) );
			SqlStatement stmt = sb.GetStatement();
			// create object and set statement params
			BigNull bn = new BigNull();
			stmt.SetParameters( bn, true );
			// verify parameter values (assume bigNull is first and only param)
			IDbDataParameter param = stmt.Command.Parameters[ 0 ] as IDbDataParameter;
			Assert.AreEqual( DBNull.Value, param.Value, "Parameter 0 not converted to DBNull." );
		}

		[Test]
		public void TestHashCodeCalc()
		{
			int hash1 = GetFieldComboHashCode( new[] { "grass", "tree" } );
			int hash2 = GetFieldComboHashCode( new[] { "tree", "grass" } );
			Assert.IsTrue( hash1 != hash2, "Hash computation must not be transitive." );
		}

		// copy of private method from ObjectConstructor.cs
		public int GetFieldComboHashCode( string[] names )
		{
			int hash = 0;
			if( names == null || names.Length <= 0 )
			{
				return 0;
			}
			string x = "";
			foreach( string name in names )
			{
				x += name;
			}
			hash = x.ToLower().GetHashCode();
			return hash;
		}

		[Test]
		public void TestCheckIsCalledFrom()
		{
			try
			{
				Check.VerifyEquals( "A", "B", Error.Unspecified, "This check is meant to fail and provoke an error." );
			}
			catch( Exception e )
			{
				// verify that Check excludes itself
				Assert.IsFalse( Check.IsCalledFrom( "Gentle.Common", e ), "IsCalledFrom not working (1)" );
				Assert.IsFalse( Check.IsCalledFrom( "Check.IsCalledFrom", e ), "IsCalledFrom not working (2)" );
				// verify that the calling method is not excluded
				Assert.IsTrue( Check.IsCalledFrom( "Gentle.Framework", e ), "IsCalledFrom not working (A)" );
				Assert.IsTrue( Check.IsCalledFrom( "TestCheck", e ), "IsCalledFrom not working (B)" );
				Assert.IsTrue( Check.IsCalledFrom( "TestMisc.TestCheckIsCalledFrom", e ), "IsCalledFrom not working (C)" );
			}
		}
	}
}