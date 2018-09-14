/*
 * Version:
 *   $Id: APICFrame.cs,v 1.1 2004/11/03 01:18:50 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: APICFrame.cs,v $
 *   Revision 1.1  2004/11/03 01:18:50  cwoodbury
 *   Added to ID3Sharp
 *
 *
 * 
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
	/// 
	/// </summary>
	public class APICFrame
	{
		/*
		#region Fields
		private EncodedString mimeType;
		private APICPictureType pictureType;
		private EncodedString description;
		private byte[] picture;
		#endregion

		#region Constructors
		public APICFrame( string frameID, byte[] flags, byte[] frameData, ID3Versions version )
			: base( frameID, flags, version )
		{
			ParseFrameData( frameData );
		}
		#endregion

		#region Constructor helpers
		private void ParseFrameData( byte[] frameData )
		{
			// Encoding type
			TextEncodingType encType = (TextEncodingType) frameData[0];

			// MIME type
			int mimeEnd = FindSingleByteNull( frameData, 1 );
			byte[] mimeBytes = new byte[ mimeEnd - 1 ];
			Array.Copy( frameData, 1, mimeBytes, 0, mimeBytes.Length );
			mimeType = new EncodedString( TextEncodingType.ISO_8859_1, mimeBytes );

			// Picture type
			pictureType = (APICPictureType) frameData[ mimeEnd + 1 ];

			// Desciption
			int descEnd;
			
			if ( encType == TextEncodingType.ISO_8859_1 || 
				encType == TextEncodingType.UTF_8 )
			{
				descEnd = FindSingleByteNull( frameData, mimeEnd + 2 );
			}
			else
			{
				descEnd = FindDoubleByteNull( frameData, mimeEnd + 2 );
			}

			byte[] descBytes = new byte[ (descEnd - mimeEnd) - 1 ];
			Array.Copy( frameData, mimeEnd + 2, descBytes, 0, descBytes.Length );
			description = new EncodedString( encType, descBytes );

			//Picture data
			picture = new byte[ frameData.Length - descEnd ];
			Array.Copy( frameData, descEnd + 1, picture, 0, picture.Length );
		}

		private static int FindSingleByteNull( byte[] data, int currentPosition )
		{
			for( ; currentPosition < data.Length; currentPosition++ )
			{
				if ( data[ currentPosition ] == 0x0 )
				{
					break;
				}
			}

			return currentPosition;
		}

		private static int FindDoubleByteNull( byte[] data, int currentPosition )
		{
			for( ; currentPosition < data.Length - 1; currentPosition++ )
			{
				if ( data[ currentPosition ] == 0x0 && 
					data[ currentPosition + 1 ] == 0x0 )
				{
					break;
				}
			}

			return currentPosition;
		}
		#endregion

		#region Properties
		public string MIMEType
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

		public APICPictureType PictureType
		{
			get
			{
				return pictureType;
			}
			set
			{
				pictureType = value;
			}
		}

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

		public byte[] Picture
		{
			get
			{
				return picture;
			}
			set
			{
				picture = value;
			}
		}

		public override int Size
		{
			get
			{
				// encoding description byte + mimeType + picture type byte + ...
				return 1 + mimeType.Size + 1 + description.Size;
			}
		}
		#endregion

		public override void WriteToStream( Stream stream, ID3Versions version )
		{
			throw new UnsupportedVersionException( version );
		}
		*/
	}
}
