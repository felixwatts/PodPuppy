using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ID3Sharp.IO
{
	[Serializable]
	public class InvalidTextEncodingTypeException : IOValidationException
	{
		private TextEncodingType invalidEncoding;

		#region Constructors
		public InvalidTextEncodingTypeException() { }
		public InvalidTextEncodingTypeException( string message )
			: base( message ) { }
		public InvalidTextEncodingTypeException( string message, Exception inner )
			: base( message, inner ) { }

		public InvalidTextEncodingTypeException( TextEncodingType invalidEncoding )
			: base()
		{
			this.invalidEncoding = invalidEncoding;
		}
		public InvalidTextEncodingTypeException( string message, TextEncodingType invalidEncoding )
			: base( message )
		{
			this.invalidEncoding = invalidEncoding;
		}
		public InvalidTextEncodingTypeException( string message, TextEncodingType invalidEncoding
			, Exception inner )
			: base( message, inner )
		{
			this.invalidEncoding = invalidEncoding;
		}

		protected InvalidTextEncodingTypeException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
			if ( info == null )
			{
				throw new ArgumentNullException( "info" );
			}

			invalidEncoding = (TextEncodingType) info.GetByte( "invalidEncoding" );
		}
		#endregion

		public TextEncodingType InvalidEncoding
		{
			get { return invalidEncoding; }
			set { invalidEncoding = value; }
		}

		[SecurityPermission( SecurityAction.Demand, SerializationFormatter = true )]
		public override void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			if ( info == null )
			{
				throw new ArgumentNullException( "info" );
			}

			info.AddValue( "invalidEncoding", invalidEncoding );
			base.GetObjectData( info, context );
		}
	}
}
