#region CVS Log
/*
 * Version:
 *   $Id: ID3Versions.cs,v 1.3 2004/11/10 06:51:56 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: ID3Versions.cs,v $
 *   Revision 1.3  2004/11/10 06:51:56  cwoodbury
 *   Hid CVS log messages away in #region
 *
 *   Revision 1.2  2004/11/10 06:31:14  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.1  2004/11/03 01:18:07  cwoodbury
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
using System.Diagnostics.CodeAnalysis;

namespace ID3Sharp
{
	/// <summary>
	/// Specifies constants for the possible ID3 tag versions.
	/// </summary>
	[SuppressMessage( "Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags" )]
	[Flags]
	public enum ID3Versions
	{
		/// <summary>No ID3 version specified.</summary>
		None = 0x00,	// 00000000
		/// <summary>ID3v1.x</summary>
		V1 = 0x01,		// 00000001
		/// <summary>ID3v1.0</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		V1_0 = 0x03,	// 00000011
		/// <summary>ID3v1.1</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		V1_1 = 0x05,	// 00000101
		/// <summary>ID3v2.x</summary>
		V2 = 0x08,		// 00001000
		/// <summary>ID3v2.2</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		V2_2 = 0x18,	// 00011000
		/// <summary>ID3v2.3</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		V2_3 = 0x28,	// 00101000
		/// <summary>ID3v2.4</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		V2_4 = 0x48,	// 01001000
		/// <summary>Unknown ID3 version.</summary>
		Unknown = 0x80,	// 10000000
		/// <summary>All ID3 versions specified.</summary>
		All = 0xFF		// 11111111
	}
}
