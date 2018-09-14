using System;
using System.Diagnostics.CodeAnalysis;

namespace ID3Sharp.Frames
{
	[Serializable]
	public class FrameCompositeException : FramesException
	{
		public FrameCompositeException() { }
		public FrameCompositeException( string message ) : base( message ) { }
		public FrameCompositeException( string message, Exception inner ) : base( message, inner ) { }
		protected FrameCompositeException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context )
			: base( info, context ) { }
	}
}
