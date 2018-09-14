using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ID3Sharp.Frames
{
	[Serializable]
	public class FrameValidationException : FramesException
	{
		private ID3v2Frame invalidFrame;

		#region Constructors
		public FrameValidationException() { }
		public FrameValidationException( string message ) : base( message ) { }
		public FrameValidationException( string message, Exception inner ) : base( message, inner ) { }

		public FrameValidationException( ID3v2Frame invalidFrame )
		{
			this.invalidFrame = invalidFrame;
		}
		public FrameValidationException( string message, ID3v2Frame invalidFrame )
			: base( message )
		{
			this.invalidFrame = invalidFrame;
		}
		public FrameValidationException( string message, ID3v2Frame invalidFrame, Exception inner )
			: base( message, inner )
		{
			this.invalidFrame = invalidFrame;
		}

		protected FrameValidationException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
			if ( info == null )
			{
				throw new ArgumentNullException( "info" );
			}

			invalidFrame = (ID3v2Frame) info.GetValue( "invalidFrame", typeof( ID3v2Frame ) );
		}
		#endregion

		public ID3v2Frame InvalidFrame
		{
			get { return invalidFrame; }
			set { invalidFrame = value; }
		}

		[SecurityPermission( SecurityAction.Demand, SerializationFormatter = true )]
		public override void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			if ( info == null )
			{
				throw new ArgumentNullException( "info" );
			}

			info.AddValue( "invalidFrame", invalidFrame );
			base.GetObjectData( info, context );
		}
	}
}
