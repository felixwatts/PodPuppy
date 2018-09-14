using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ID3Sharp.Frames
{
	[Serializable]
	public class FrameTypeNotRegisteredException : FramesException
	{
		private FrameType unregisteredFrameType;

		#region Constructors
		public FrameTypeNotRegisteredException() { }
		public FrameTypeNotRegisteredException( string message ) : base( message ) { }
		public FrameTypeNotRegisteredException( string message, Exception inner )
			: base( message, inner ) { }

		public FrameTypeNotRegisteredException( FrameType unregisteredFrameType )
		{
			this.unregisteredFrameType = unregisteredFrameType;
		}

		public FrameTypeNotRegisteredException( FrameType unregisteredFrameType, string message )
			: base( message )
		{
			this.unregisteredFrameType = unregisteredFrameType;
		}

		public FrameTypeNotRegisteredException( FrameType unregisteredFrameType,
			string message, Exception inner )
			: base( message, inner )
		{
			this.unregisteredFrameType = unregisteredFrameType;
		}

		protected FrameTypeNotRegisteredException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
			if ( info == null )
			{
				throw new ArgumentNullException( "info" );
			}

			unregisteredFrameType = (FrameType) info.GetInt32( "unregisteredFrameType" );
		}
		#endregion

		public FrameType FrameType
		{
			get
			{
				return unregisteredFrameType;
			}
		}

		[SecurityPermission( SecurityAction.Demand, SerializationFormatter = true )]
		public override void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			if ( info == null )
			{
				throw new ArgumentNullException( "info" );
			}

			info.AddValue( "unregisteredFrameType", unregisteredFrameType );
			base.GetObjectData( info, context );
		}
	}
}
