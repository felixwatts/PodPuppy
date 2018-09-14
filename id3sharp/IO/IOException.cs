using System;
using System.Collections.Generic;
using System.Text;

namespace ID3Sharp.IO
{
	[Serializable]
	public class IOException : Exception
	{
		public IOException() { }
		public IOException( string message ) : base( message ) { }
		public IOException( string message, Exception inner ) : base( message, inner ) { }
		protected IOException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context )
			: base( info, context ) { }
	}
}
