#region CVS Log
/*
 * Version:
 *   $Id: COMMFrame.cs,v 1.12 2004/12/10 04:49:08 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: COMMFrame.cs,v $
 *   Revision 1.12  2004/12/10 04:49:08  cwoodbury
 *   Made changes to EncodedString and to how it is used to push it down to
 *   just being involved with frame I/O and not otherwise being used in frames.
 *
 *   Revision 1.11  2004/11/20 23:44:58  cwoodbury
 *   Removed unnecesary code.
 *
 *   Revision 1.10  2004/11/20 23:12:12  cwoodbury
 *   Removed TextEncodingType.ASCII type; replaced with ISO_8859_1 type
 *   or default type for EncodedString.
 *
 *   Revision 1.9  2004/11/19 18:38:47  cwoodbury
 *   Added code to catch and handle malformed data.
 *
 *   Revision 1.8  2004/11/16 07:08:14  cwoodbury
 *   Changed accessibility modifiers for some methods to internal or
 *   protected internal where appropriate.
 *
 *   Revision 1.7  2004/11/16 06:43:39  cwoodbury
 *   Fixed bug #1066848: EncodedStrings.CreateEncodedStrings() corrupted
 *   data in the optional leftover bytes.
 *
 *   Revision 1.6  2004/11/10 07:32:29  cwoodbury
 *   Factored out ParseFrameData() into ID3v2Frame.
 *
 *   Revision 1.5  2004/11/10 06:51:55  cwoodbury
 *   Hid CVS log messages away in #region
 *
 *   Revision 1.4  2004/11/10 04:44:16  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.3  2004/11/03 07:44:27  cwoodbury
 *   Added validity checks to parsing code.
 *
 *   Revision 1.2  2004/11/03 07:01:04  cwoodbury
 *   Updated known bug information.
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
using System.Collections.Generic;

using ID3Sharp.IO;

namespace ID3Sharp.Frames
{
	/// <summary>
	/// A Comments frame.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased" )]
	public class COMMFrame : TextInformationFrame
	{
		/// <summary>
		/// The default language code. "If the language is not known, the
		/// string 'XXX' should be used." - ID3 spec.
		/// </summary>
		public const string DefaultLanguageCode = "XXX";
		// ISO-639-2 (referenced by ID3v2.2 and v2.3 and the ID3v2.4 spec
		// specify 3-character language codes.
		private const int LanguageFieldLength = 3;
		private const TextEncodingType LanguageEncodingType = TextEncodingType.ISO_8859_1;

		#region Fields
		/// <summary>
		/// The contents of the language code field.
		/// </summary>
		private EncodedString language = new EncodedString();
		/// <summary>
		/// The contents of the description field.
		/// </summary>
		private EncodedString description = new EncodedString();
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new COMMFrame.
		/// </summary>
		protected internal COMMFrame()
		{
			language = new EncodedString( LanguageEncodingType, COMMFrame.DefaultLanguageCode );
			SetEncodedStringSettings();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="frame">Frame to copy.</param>
		protected internal COMMFrame( COMMFrame frame )
			: base( frame )
		{
			this.language = frame.language.Copy();
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
			if ( frameData.Length >= 5 )
			{
				// Encoding type
				TextEncodingType encType = (TextEncodingType) frameData[0];

				// Language
				char[] languageBytes = new char[ COMMFrame.LanguageFieldLength ];
				Array.Copy( frameData, 1, languageBytes, 0, languageBytes.Length );
				string languageStr = (new String( languageBytes )).ToUpper();
				language = new EncodedString( LanguageEncodingType, languageStr );
			
				// Text
				byte[] textBytes = new byte[ frameData.Length - 4 ];
				Array.Copy( frameData, 4, textBytes, 0, textBytes.Length );
				IList<EncodedString> strings = EncodedString.CreateStrings( encType, textBytes );
				if ( strings.Count >= 2 )
				{
					description = strings[ 0 ];
					base.EncodedText = strings[ 1 ];
					SetEncodedStringSettings();
				}
			}
		}

		private void SetEncodedStringSettings()
		{
			language.HasEncodingTypePrepended = false;
			language.IsTerminated = false;
			description.HasEncodingTypePrepended = false;
			description.IsTerminated = true;
			base.EncodedText.HasEncodingTypePrepended = false;
			base.EncodedText.IsTerminated = false;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets and sets the contents of the frame's language field.
		/// </summary>
		public string Language
		{
			get
			{
				return language.String;
			}
			set
			{
				if ( value == null )
				{
					language.String = COMMFrame.DefaultLanguageCode;
				}
				else if ( value.Length == 3 )
				{
					if ( value.Equals( COMMFrame.DefaultLanguageCode ) )
					{
						language.String = value;
					}
					else
					{
						language.String = value.ToLower();
					}
				}
				else
				{
					language.String = COMMFrame.DefaultLanguageCode;
				}
			}
		}

		/// <summary>
		/// Gets and sets the contents of the frame's description field.
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
		/// Gets the size (in bytes) of the frame (not including header).
		/// </summary>
		public override int Size
		{
			get
			{
				// encoding byte + language + description + text
				return 1 + language.Size + description.Size + base.EncodedText.Size;
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
			return new COMMFrame( this );
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
			stream.WriteByte( (byte) base.EncodedText.TextEncodingType );
			language.WriteToStream( stream );
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
			else if ( language.TextEncodingType != COMMFrame.LanguageEncodingType )
			{
				innerException = new InvalidTextEncodingTypeException(
					"URL not set with required TextEncodingType.",
					language.TextEncodingType );
			}
			else if ( description.TextEncodingType != base.EncodedText.TextEncodingType )
			{
				innerException = new InvalidTextEncodingTypeException(
					"Description and Text not set with same TextEncodingType.",
					description.TextEncodingType );
			}
			else
			{
				try
				{
					language.Validate( version );
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
