using System;
using System.Collections.Generic;
using System.Text;

namespace ID3Sharp.IO
{
	[Serializable]
	public class IOValidationException : IOException
	{
		public IOValidationException() { }
		public IOValidationException( string message ) : base( message ) { }
		public IOValidationException( string message, Exception inner ) : base( message, inner ) { }
		protected IOValidationException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context )
			: base( info, context ) { }
	}
}
