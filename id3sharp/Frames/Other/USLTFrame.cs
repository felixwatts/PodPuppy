/*
 * Version:
 *   $Id: USLTFrame.cs,v 1.1 2004/11/03 01:18:50 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: USLTFrame.cs,v $
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
	public class USLTFrame
	{
		/*
		#region Fields
		private string language;
		private EncodedString description;
		private EncodedString lyrics;
		#endregion

		#region Constructors
		public USLTFrame( string frameID, byte[] flags, byte[] frameData, ID3Versions version )
			: base( frameID, flags, version )
		{
			ParseFrameData( frameData );
		}
		#endregion

		#region Constructor helpers
		private void ParseFrameData( byte[] frameData )
		{
			TextEncodingType encType = (TextEncodingType) frameData[0];

			char[] languageBytes = new char[ 3 ];
			Array.Copy( frameData, 1, languageBytes, 0, languageBytes.Length );
			language = (new String( languageBytes )).ToUpper();
			
			byte[] textBytes = new byte[ frameData.Length - 4 ];
			Array.Copy( frameData, 4, textBytes, 0, textBytes.Length );
			EncodedString[] strings  = EncodedString.CreateEncodedStrings( encType, textBytes );
			description = strings[0];
			lyrics = strings[1];
		}
		#endregion

		#region Properties
		public string Language
		{
			get
			{
				return language;
			}
			set
			{
				if ( value.Length == 3 )
				{
					if ( value.Equals( "XXX" ) )
					{
						language = value;
					}
					else
					{
						language = value.ToLower();
					}
				}
				else
				{
					language = "XXX";
				}
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

		public string Lyrics
		{
			get
			{
				return lyrics.String;
			}
			set
			{
				lyrics.String = value;
			}
		}

		public override int Size
		{
			get
			{
				// encoding description byte + 3 language bytes + ...
				return 1 + 3 + description.Size + lyrics.Size;
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
