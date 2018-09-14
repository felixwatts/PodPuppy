/*
 * Version:
 *   $Id: SEEKFrame.cs,v 1.1 2004/11/03 01:18:50 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: SEEKFrame.cs,v $
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
	public class SEEKFrame
	{
		/*
		#region Fields
		protected EncodedInteger offset;
		#endregion

		#region Constructors
		public SEEKFrame( string frameID, byte[] flags, byte[] frameData, ID3Versions version ) 
			: base( frameID, flags, version )
		{
			ParseFrameData( frameData );
		}
		#endregion

		#region Constructor helpers
		private void ParseFrameData( byte[] frameData )
		{
			offset = new EncodedInteger( frameData );
		}
		#endregion

		#region Properties
		public override int Size
		{
			get
			{
				return 4;
			}
		}

		public int Offset
		{
			get
			{
				return offset.Integer;
			}
			set
			{
				offset.Integer = value;
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
