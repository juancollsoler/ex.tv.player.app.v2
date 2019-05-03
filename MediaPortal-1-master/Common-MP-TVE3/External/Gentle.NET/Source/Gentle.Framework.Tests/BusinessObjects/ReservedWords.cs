namespace Gentle.Framework.Tests
{
	/// <summary>
	/// Class to test out reserved words handling. Because reserved words are provider 
	/// specific, these tests are only executed with SQL Server.
	/// </summary>
	[TableName( "Order" )] // table name is also a reserved word
	public class ReservedWords : Persistent
	{
		protected int _identity;
		protected string _order;
		protected int _value;
		protected string _of;
		protected string _group;

		#region ReservedWords Properties      
		[TableColumn( "Identity", NotNull = true ), PrimaryKey( AutoGenerated = true )]
		public int Identity
		{
			get { return _identity; }
			set { _identity = value; }
		}

		[TableColumn( "Order" )]
		public string Order
		{
			get { return _order; }
			set { _order = value; }
		}

		[TableColumn( "Value" )]
		public int Value
		{
			get { return _value; }
			set { _value = value; }
		}

		[TableColumn( "Of" )]
		public string Of
		{
			get { return _of; }
			set { _of = value; }
		}

		[TableColumn( "Group" )]
		public string Group
		{
			get { return _group; }
			set { _group = value; }
		}
		#endregion

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="identity"></param>
		/// <param name="order"></param>
		/// <param name="value"></param>
		/// <param name="of"></param>
		/// <param name="group"></param>
		public ReservedWords( int identity, string order, int value, string of, string group )
		{
			_identity = identity;
			_order = order;
			_value = value;
			_of = of;
			_group = group;
		}

		/// <summary>
		/// Retrieve using the tables identity field
		/// </summary>
		/// <param name="identity"></param>
		/// <returns></returns>
		public static ReservedWords Retrieve( int identity )
		{
			Key key = new Key( typeof(ReservedWords), true, "Identity", identity );
			return Broker.RetrieveInstance( typeof(ReservedWords), key ) as ReservedWords;
		}

		/// <summary>
		/// Retrieve using the tables Group field
		/// </summary>
		/// <param name="group"></param>
		/// <returns></returns>
		public static ReservedWords Retrieve( string group )
		{
			Key key = new Key( typeof(ReservedWords), true, "Group", group );
			return Broker.RetrieveInstance( typeof(ReservedWords), key ) as ReservedWords;
		}
	}
}