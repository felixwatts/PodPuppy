#region CVS Log
/*
 * Version:
 *   $Id: SynchsafeInteger.cs,v 1.3 2004/11/10 06:51:56 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: SynchsafeInteger.cs,v $
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
	/// An integer following the synchsafe specifications of ID3v2.
	/// </summary>
	public static class SynchsafeInteger
	{
		/// <summary>
		/// Returns the synchsafe byte array for the given integer.
		/// </summary>
		/// <param name="integer">The integer to synchsafe.</param>
		/// <returns>The synchsafe byte array for the given integer.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "0#" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1718:AvoidLanguageSpecificTypeNamesInParameters", MessageId = "0#" )]
		public static byte[] Synchsafe( int integer )
		{
			byte[] bytes = new byte[4];

			bytes[3] = (byte) (integer & 0x7F);
			bytes[2] = (byte) ((integer >> 7) & 0x7F);
			bytes[1] = (byte) ((integer >> 14) & 0x7F);
			bytes[0] = (byte) ((integer >> 21) & 0x7F);

			return bytes;
		}

		/// <summary>
		/// Returns the int for the given synchsafe byte array.
		/// </summary>
		/// <param name="integer">The syncsafe byte array.</param>
		/// <returns>The int for the given synchsafe byte array.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "0#" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1718:AvoidLanguageSpecificTypeNamesInParameters", MessageId = "0#" )]
		public static int UnSynchsafe( byte[] integer )
		{
			int retVal = 0;
			retVal += ((int) integer[0]) << 21;
			retVal += ((int) integer[1]) << 14;
			retVal += ((int) integer[2]) << 7;
			retVal += (int) integer[3];

			return retVal;
		}
	}
}

