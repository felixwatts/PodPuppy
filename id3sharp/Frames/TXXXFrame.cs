#region CVS Log
/*
 * Version:
 *   $Id: TXXXFrame.cs,v 1.9 2004/12/10 04:49:08 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: TXXXFrame.cs,v $
 *   Revision 1.9  2004/12/10 04:49:08  cwoodbury
 *   Made changes to EncodedString and to how it is used to push it down to
 *   just being involved with frame I/O and not otherwise being used in frames.
 *
 *   Revision 1.8  2004/11/20 23:12:12  cwoodbury
 *   Removed TextEncodingType.ASCII type; replaced with ISO_8859_1 type
 *   or default type for EncodedString.
 *
 *   Revision 1.7  2004/11/16 07:08:14  cwoodbury
 *   Changed accessibility modifiers for some methods to internal or
 *   protected internal where appropriate.
 *
 *   Revision 1.6  2004/11/16 06:43:38  cwoodbury
 *   Fixed bug #1066848: EncodedStrings.CreateEncodedStrings() corrupted
 *   data in the optional leftover bytes.
 *
 *   Revision 1.5  2004/11/10 07:32:29  cwoodbury
 *   Factored out ParseFrameData() into ID3v2Frame.
 *
 *   Revision 1.4  2004/11/10 06:51:55  cwoodbury
 *   Hid CVS log messages away in #region
 *
 *   Revision 1.3  2004/11/10 04:44:16  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.2  2004/11/03 07:44:27  cwoodbury
 *   Added validity checks to parsing code.
 *
 *   Revision 1.1  2004/11/03 01:18:50  cwoodbury
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
using System.Collections.Generic;

using ID3Sharp.IO;

namespace ID3Sharp.Frames
{
	/// <summary>
	/// A User-Defined Text Information frame.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased" )]
	public class TXXXFrame : TextInformationFrame
	{
		#region Fields
		/// <summary>
		/// The contents of the Description field.
		/// </summary>
		private EncodedString description = new EncodedString();
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new TXXXFrame.
		/// </summary>
		protected internal TXXXFrame()
		{
			SetEncodedStringSettings();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="frame">Frame to copy.</param>
		protected internal TXXXFrame( TXXXFrame frame )
			: base( frame )
		{
			this.description = frame.description.Copy();
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
			IList<EncodedString> strings = EncodedString.CreateStrings( frameData );
			if ( strings.Count >= 2 )
			{
				description = strings[ 0 ];
				base.EncodedText = strings[ 1 ];
				SetEncodedStringSettings();
			}
		}

		private void SetEncodedStringSettings()
		{
			description.HasEncodingTypePrepended = true;
			description.IsTerminated = true;
			base.EncodedText.HasEncodingTypePrepended = false;
			base.EncodedText.IsTerminated = false;
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

				// encoding byte + description + text
				return description.Size + base.EncodedText.Size;
			}
		}
		
		/// <summary>
		/// Gets and sets the contents of the Description field.
		/// </summary>
		public string Description
		{
			get
			{
				return description.String;
			}
			set
			{
				description.String = value;
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
			return new TXXXFrame( this );
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

			Validate( version );

			WriteHeaderToStream( stream, version );
			description.WriteToStream( stream );
			base.EncodedText.WriteToStream( stream );
			stream.Flush();
		}

		public override void Validate( ID3Versions version )
		{
			base.Validate( version );

			Exception innerException = null;
			if ( ( version & ID3Versions.V2 ) != ID3Versions.V2 )
			{
				innerException = new UnsupportedVersionException( version );
			}
			else if ( description.TextEncodingType != base.EncodedText.TextEncodingType )
			{
				innerException = new InvalidTextEncodingTypeException(
					"URL not set with required TextEncodingType.",
					description.TextEncodingType );
			}
			else 
			{
				try
				{
					description.Validate( version );
				}
				catch ( IOValidationException ex )
				{
					innerException = ex;
				}
			}

			if ( innerException != null )
			{
				throw new FrameValidationException( "Validation failed.", this, innerException );
			}
		}
		#endregion
	}
}
