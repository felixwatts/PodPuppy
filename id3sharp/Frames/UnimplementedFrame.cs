#region CVS Log
/*
 * Version:
 *   $Id: UnimplementedFrame.cs,v 1.5 2004/11/16 07:08:14 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: UnimplementedFrame.cs,v $
 *   Revision 1.5  2004/11/16 07:08:14  cwoodbury
 *   Changed accessibility modifiers for some methods to internal or
 *   protected internal where appropriate.
 *
 *   Revision 1.4  2004/11/10 07:32:29  cwoodbury
 *   Factored out ParseFrameData() into ID3v2Frame.
 *
 *   Revision 1.3  2004/11/10 06:51:55  cwoodbury
 *   Hid CVS log messages away in #region
 *
 *   Revision 1.2  2004/11/10 04:44:16  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.1  2004/11/03 01:18:51  cwoodbury
 *   Added to ID3Sharp
 *
 */
#endregion

/* 
 * Author(s):
 *   Chris Woodbury 
 * 
 * Project Location:
 *	 http://id3sharp.sourceforge.net
 * 
 * License:
 *   Licensed under the Open Software License version 2.0
 */

using System;
using System.IO;

namespace ID3Sharp.Frames
{
	/// <summary>
	/// A frame without a specific implementation.
	/// </summary>
	public class UnimplementedFrame : ID3v2Frame
	{
		#region Fields
		/// <summary>
		/// The frame's raw data.
		/// </summary>
		private byte[] frameData;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Creates a new UnimplementedFrame with no frame data.
		/// </summary>
		protected internal UnimplementedFrame()
		{
			frameData = new byte[] {};
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="frame">Frame to copy.</param>
		protected internal UnimplementedFrame( UnimplementedFrame frame )
			: base( frame )
		{
			this.frameData = frame.frameData;
		}
		#endregion

		#region Constructor/Initialize helpers
		/// <summary>
		/// Parses the raw frame data.
		/// </summary>
		/// <param name="rawFrameData">The raw frame data.</param>
		/// <param name="version">The ID3v2 version of the tag being parsed.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "frameData" )]
		protected override void ParseFrameData( byte[] frameData, ID3Versions version )
		{
			this.frameData = frameData;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the size (in bytes) of the frame (not including header).
		/// </summary>
		public override int Size
		{
			get
			{
				return frameData.Length;
			}
		}

		/// <summary>
		/// Gets the frame's raw data.
		/// </summary>
		public byte[] Bytes
		{
			get
			{
				return frameData;
			}
		}
		#endregion

		/// <summary>
		/// Returns a copy of this frame. Supports the prototype design pattern.
		/// </summary>
		/// <returns>A copy of this frame.</returns>
		public override ID3v2Frame Copy()
		{
			return new UnimplementedFrame( this );
		}

		/// <summary>
		/// Writes the frame to a stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="version">The ID3v2 version to use in writing the frame.</param>
		public override void WriteToStream( Stream stream, ID3Versions version )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			WriteHeaderToStream( stream, version );
			stream.Write( frameData, 0, frameData.Length );
			stream.Flush();
		}

		public override void Validate( ID3Versions version )
		{
		}
	}
}
