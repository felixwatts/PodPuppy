/*
 * Version:
 *   $Id: USERFrame.cs,v 1.1 2004/11/03 01:18:50 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: USERFrame.cs,v $
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
	/// Summary description for USERFrame.
	/// </summary>
	public class USERFrame
	{
		/*
		#region Fields
		private string language;
		private EncodedString text;
		#endregion

		#region Constructors
		public USERFrame( string frameID, byte[] flags, byte[] frameData, ID3Versions version )
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

			// Language
			char[] languageBytes = new char[ 3 ];
			Array.Copy( frameData, 1, languageBytes, 0, languageBytes.Length );
			language = (new String( languageBytes )).ToUpper();
			
			// Text
			byte[] textBytes = new byte[ frameData.Length - 4 ];
			Array.Copy( frameData, 4, textBytes, 0, textBytes.Length );
			text = new EncodedString( encType, textBytes );
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

		public string Text
		{
			get
			{
				return text.String;
			}
			set
			{
				text.String = value;
			}
		}

		public override int Size
		{
			get
			{
				// encoding description byte + 3 language bytes + ...
				return 1 + 3 + text.Size;
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
