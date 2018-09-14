#region CVS Log
/*
 * Version:
 *   $Id: EncodedInteger.cs,v 1.3 2004/11/10 06:51:56 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: EncodedInteger.cs,v $
 *   Revision 1.3  2004/11/10 06:51:56  cwoodbury
 *   Hid CVS log messages away in #region
 *
 *   Revision 1.2  2004/11/10 06:31:14  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.1  2004/11/03 01:18:26  cwoodbury
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

namespace ID3Sharp.IO
{
	/// <summary>
	/// An integer encoded as a byte array.
	/// </summary>
	public static class EncodedInteger
	{
		#region Conversions
		/// <summary>
		/// Returns a byte array for the given integer.
		/// </summary>
		/// <param name="integer">The integer to convert.</param>
		/// <returns>A byte array for the given integer.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "0#" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1718:AvoidLanguageSpecificTypeNamesInParameters", MessageId = "0#" )]
		public static byte[] ToBytes( int integer )
		{
			return ToBytes( integer, 4 );
		}

		/// <summary>
		/// Returns a byte array for the given integer.
		/// </summary>
		/// <param name="integer">The integer to convert.</param>
		/// <param name="width">The number of bytes in the resulting byte array.
		/// (Must be between 1 and 4, inclusive.)</param>
		/// <returns>A byte array for the given integer.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "0#" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1718:AvoidLanguageSpecificTypeNamesInParameters", MessageId = "0#" )]
		public static byte[] ToBytes( int integer, int width )
		{
			if ( width > 4 || width < 1 )
			{
				throw new ArgumentException( "Desired byte array must be between one and four bytes long.",  "width" );
			}
			else
			{
				byte[] bytes = new byte[width];

				for ( int byteItr = 0; byteItr < width; byteItr++ )
				{
					int shiftAmount = 8 * (3 - byteItr);
					bytes[byteItr] = (byte) (integer >> shiftAmount);
				}

				return bytes;
			}
		}

		/// <summary>
		/// Returns an int for the given byte array.
		/// </summary>
		/// <param name="integerBytes">The byte array to convert.
		/// (Must be between 1 and 4 bytes long, inclusive.)</param>
		/// <returns>An int for the given byte array.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "0#" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1718:AvoidLanguageSpecificTypeNamesInParameters", MessageId = "0#" )]
		public static int ToInt( byte[] integerBytes )
		{
			if ( integerBytes.Length > 4 || integerBytes.Length < 1 )
			{
				throw new ArgumentException( "Byte array must be between one and four bytes long.",  "integerBytes" );
			}
			else
			{
				int retVal = 0;
				
				for ( int byteItr = 0; byteItr < integerBytes.Length; byteItr++ )
				{
					int shiftAmount = 8 * ((integerBytes.Length - 1) - byteItr);
					retVal += (int) integerBytes[byteItr] << shiftAmount;
				}

				return retVal;
			}
		}
		#endregion
	}
}
