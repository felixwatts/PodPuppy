#region CVS Log
/*
 * Version:
 *   $Id: UFIDFrame.cs,v 1.10 2004/12/10 04:49:08 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: UFIDFrame.cs,v $
 *   Revision 1.10  2004/12/10 04:49:08  cwoodbury
 *   Made changes to EncodedString and to how it is used to push it down to
 *   just being involved with frame I/O and not otherwise being used in frames.
 *
 *   Revision 1.9  2004/11/20 23:51:04  cwoodbury
 *   Made default owner-identifier string explicity use ISO-8859-1, as per ID3 specs.
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
	/// A Unique File Identifier frame.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased" )]
	public class UFIDFrame : ID3v2Frame
	{
		#region Fields
		/// <summary>
		/// The contents of the Owner Identifier field.
		/// </summary>
		private EncodedString ownerIdentifier = new EncodedString();
		/// <summary>
		/// The contents of the Identifier field.
		/// </summary>
		private byte[] identifier;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new UFIDFrame.
		/// </summary>
		protected internal UFIDFrame()
		{
			identifier = new byte[] { };
			SetEncodedStringSettings();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="frame">Frame to copy.</param>
		protected internal UFIDFrame( UFIDFrame frame )
			: base( frame )
		{
			this.ownerIdentifier = frame.ownerIdentifier.Copy();
			this.identifier = new byte[ frame.identifier.Length ];
			frame.identifier.CopyTo( this.identifier, 0 );
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
			IList<EncodedString> strings =
				EncodedString.CreateStrings( TextEncodingType.ISO_8859_1, frameData, 1,
				delegate( byte[] leftoverBytes ) { Identifier = leftoverBytes; } );
			if ( strings.Count == 1 )
			{
				ownerIdentifier = strings[ 0 ];
				SetEncodedStringSettings();
			}
		}

		private void SetEncodedStringSettings()
		{
			ownerIdentifier.HasEncodingTypePrepended = true;
			ownerIdentifier.IsTerminated = true;
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
				// owner identifier + data
				return ownerIdentifier.Size + identifier.Length;
			}
		}

		/// <summary>
		/// Gets and sets the contents of the Owner Identifier field.
		/// </summary>
		public string OwnerIdentifier
		{
			get
			{
				return ownerIdentifier.String;
			}
			set
			{
				if ( String.IsNullOrEmpty( value ) )
				{
					throw new ArgumentException( "Owner identifier must be non-empty", "value" );
				}
				else
				{
					ownerIdentifier.String = value;
				}
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
				return ownerIdentifier.TextEncodingType;
			}
			set
			{
				ownerIdentifier.TextEncodingType = value;
			}
		}

		/// <summary>
		/// Gets and sets the contents of the Identifier field.
		/// </summary>
		public byte[] Identifier
		{
			get
			{
				return identifier;
			}
			set
			{
				if ( value.Length > 8 )
				{
					throw new ArgumentException( "Identifier may only be up to 64 bits", "value" );
				}
				else
				{
					identifier = value;
				}

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
			return new UFIDFrame( this );
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

			ownerIdentifier.WriteToStream( stream );

			stream.Write( identifier, 0, identifier.Length );
			stream.Flush();
		}

		public override void Validate( ID3Versions version )
		{
			Exception innerException = null;
			if ( ( version & ID3Versions.V2 ) != ID3Versions.V2 )
			{
				innerException = new UnsupportedVersionException( version );
			}
			else
			{
				try
				{
					ownerIdentifier.Validate( version );
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
