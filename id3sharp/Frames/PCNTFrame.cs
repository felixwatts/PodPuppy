#region CVS Log
/*
 * Version:
 *   $Id: PCNTFrame.cs,v 1.5 2004/11/16 07:08:14 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: PCNTFrame.cs,v $
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

using ID3Sharp.IO;

namespace ID3Sharp.Frames
{
	/// <summary>
	/// A Play Count frame.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased" )]
	public class PCNTFrame : ID3v2Frame
	{
		#region Fields
		/// <summary>
		/// The contents of the Counter field.
		/// </summary>
		private int counter;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new Play Count frame.
		/// </summary>
		protected internal PCNTFrame()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="frame">Frame to copy.</param>
		protected internal PCNTFrame( PCNTFrame frame )
			: base( frame )
		{
			this.counter = frame.counter;
		}
		#endregion

		#region Constructor/Initialize helpers
		/// <summary>
		/// Parses the raw frame data.
		/// </summary>
		/// <param name="frameData">The raw frame data.</param>
		/// <param name="version">The ID3v2 version of the tag being parsed.</param>
		protected override void ParseFrameData( byte[] frameData, ID3Versions version )
		{
			counter = EncodedInteger.ToInt( frameData );
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
				return 4;
			}
		}

		/// <summary>
		/// Gets and sets the contents of the Counter field.
		/// </summary>
		public int Counter
		{
			get
			{
				return counter;
			}
			set
			{
				counter = value;
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns a copy of this frame. Supports the prototype design pattern.
		/// </summary>
		/// <returns>A copy of this frame.</returns>
		public override ID3v2Frame Copy()
		{
			return new PCNTFrame( this );
		}

		/// <summary>
		/// Writes the frame to a stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="version">The ID3v2 version to use in writing the frame.</param>
		public override void WriteToStream( Stream stream, ID3Versions version )
		{
			throw new UnsupportedVersionException( version );
		}

		public override void Validate( ID3Versions version )
		{
		}
		#endregion
	}
}
