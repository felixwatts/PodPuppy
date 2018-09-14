#region CVS Log
/*
 * Version:
 *   $Id: GEOBFrame.cs,v 1.9 2004/12/10 04:49:08 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: GEOBFrame.cs,v $
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
 *   Revision 1.6  2004/11/16 06:43:39  cwoodbury
 *   Fixed bug #1066848: EncodedStrings.CreateEncodedStrings() corrupted
 *   data in the optional leftover bytes.
 *
 *   Revision 1.5  2004/11/10 07:32:29  cwoodbury
 *   Factored out ParseFrameData() into ID3v2Frame.
 *
 *   Revision 1.4  2004/11/10 06:51:55  cwoodbury
 *   Hid CVS log messages away in #region
 *
 *   Revision 1.3  2004/11/10 06:31:14  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.2  2004/11/10 04:44:16  cwoodbury
 *   Updated documentation.
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
	/// An Encapsulated-Object frame.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased" )]
	public class GEOBFrame : ID3v2Frame
	{
		private const TextEncodingType MimeTypeEncodingType = TextEncodingType.ISO_8859_1;

		#region Fields
		/// <summary>
		/// The contents of the MIME-Type field.
		/// </summary>
		private EncodedString mimeType = new EncodedString();
		/// <summary>
		/// The contents of the Filename field.
		/// </summary>
		private EncodedString filename = new EncodedString();
		/// <summary>
		/// The contents of the Description field.
		/// </summary>
		private EncodedString description = new EncodedString();
		/// <summary>
		/// The contents of the Encapsulated Object field.
		/// </summary>
		private byte[] encapsulatedObject;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new GEOBFrame.
		/// </summary>
		protected internal GEOBFrame()
		{
			encapsulatedObject = new byte[] { };
			SetEncodedStringSettings();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="frame">Frame to copy.</param>
		protected internal GEOBFrame( GEOBFrame frame )
			: base( frame )
		{
			this.mimeType = frame.mimeType.Copy();
			this.filename = frame.filename.Copy();
			this.description = frame.description.Copy();
			this.encapsulatedObject = new byte[ frame.encapsulatedObject.Length ];
			frame.encapsulatedObject.CopyTo( this.encapsulatedObject, 0 );
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
			// Encoding type
			TextEncodingType encType = (TextEncodingType) frameData[ 0 ];

			// MIME type
			int mimeEnd = FindNull( frameData, 1 );
			byte[] mimeBytes = new byte[ mimeEnd - 1 ];
			Array.Copy( frameData, 1, mimeBytes, 0, mimeBytes.Length );
			mimeType = new EncodedString( MimeTypeEncodingType, mimeBytes );

			byte[] remainingBytes = new byte[ ( frameData.Length - mimeEnd ) - 1 ];
			Array.Copy( frameData, mimeEnd + 1, remainingBytes, 0, remainingBytes.Length );

			IList<EncodedString> strings = EncodedString.CreateStrings( encType, remainingBytes, 2,
				delegate( byte[] leftoverBytes ) { encapsulatedObject = leftoverBytes; } );
			filename = strings[ 0 ];
			description = strings[ 1 ];
			SetEncodedStringSettings();
		}

		private void SetEncodedStringSettings()
		{
			mimeType.HasEncodingTypePrepended = false;
			mimeType.IsTerminated = true;
			filename.HasEncodingTypePrepended = false;
			filename.IsTerminated = true;
			description.HasEncodingTypePrepended = false;
			description.IsTerminated = true;
		}

		/// <summary>
		/// Returns the index of the next null byte in a byte array.
		/// </summary>
		/// <param name="data">The byte array to search.</param>
		/// <param name="currentIndex">The index at which to start looking.</param>
		/// <returns>The index of the next null byte in a byte array.</returns>
		private static int FindNull( byte[] data, int currentIndex )
		{
			for( ; currentIndex < data.Length; currentIndex++ )
			{
				if ( data[ currentIndex ] == 0x0 )
				{
					break;
				}
			}

			return currentIndex;
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
				// encoding byte + MIME type + filename + description + object
				return 1 + mimeType.Size + filename.Size + description.Size +
					encapsulatedObject.Length;
			}
		}
		
		/// <summary>
		/// Gets and sets the contents of the MIME-Type field.
		/// </summary>
		public string MimeType
		{
			get
			{
				return mimeType.String;
			}
			set
			{
				mimeType.String = value;
			}
		}

		/// <summary>
		/// Gets and sets the contents of the Filename field.
		/// </summary>
		public string Filename
		{
			get
			{
				return filename.String;
			}
			set
			{
				filename.String = value;
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

		/// <summary>
		/// Gets and sets the method of encoding used when writing
		/// strings in the frame to a stream.
		/// </summary>
		public TextEncodingType EncodingType
		{
			get
			{
				return filename.TextEncodingType;
			}
			set
			{
				filename.TextEncodingType = value;
				description.TextEncodingType = value;
			}
		}
	
		/// <summary>
		/// Gets and sets the contents of the Encapsulated Object field.
		/// </summary>
		public byte[] EncapsulatedObject
		{
			get
			{
				return encapsulatedObject;
			}
			set
			{
				encapsulatedObject = value;
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
			return new GEOBFrame( this );
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
			stream.WriteByte( (byte) filename.TextEncodingType );

			mimeType.WriteToStream( stream );
			filename.WriteToStream( stream );
			description.WriteToStream( stream );

			stream.Write( encapsulatedObject, 0, encapsulatedObject.Length );
			stream.Flush();
		}

		public override void Validate( ID3Versions version )
		{
			Exception innerException = null;
			if ( ( version & ID3Versions.V2 ) != ID3Versions.V2 )
			{
				innerException = new UnsupportedVersionException( version );
			}
			else if ( mimeType.TextEncodingType != GEOBFrame.MimeTypeEncodingType )
			{
				innerException = new InvalidTextEncodingTypeException(
					"MIME type not set with required TextEncodingType.",
					mimeType.TextEncodingType );
			}
			else if ( description.TextEncodingType != filename.TextEncodingType )
			{
				innerException = new InvalidTextEncodingTypeException(
					"Description and Text not set with same TextEncodingType.",
					description.TextEncodingType );
			}
			else
			{
				try
				{
					mimeType.Validate( version );
					filename.Validate( version );
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
