using System;
using System.Runtime.Serialization;

namespace ID3Sharp.Frames
{
	[Serializable]
	public class FramesException : Exception
	{
		public FramesException() { }
		public FramesException( string message ) : base( message ) { }
		public FramesException( string message, Exception inner ) : base( message, inner ) { }
		protected FramesException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
		}
	}
}
