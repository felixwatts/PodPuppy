#region CVS Log
/*
 * Version:
 *   $Id: TextEncodingType.cs,v 1.4 2004/11/20 23:07:47 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: TextEncodingType.cs,v $
 *   Revision 1.4  2004/11/20 23:07:47  cwoodbury
 *   Removed ASCII type.
 *
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
using System.Diagnostics.CodeAnalysis;

namespace ID3Sharp.IO
{
	/// <summary>
	/// Text encodings for ID3 frames.
	/// </summary>
	// This is a byte field defined by the ID3v2.4 specification; so keep it a byte.
	[SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
	public enum TextEncodingType : byte
	{
		/// <summary>ISO-8859-1 encoding.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		ISO_8859_1 = 0,
		/// <summary>UTF-16 encoding.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		UTF_16 = 1,
		/// <summary>Big-endian UTF-16 encoding.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		UTF_16BE = 2,
		/// <summary>UTF-8 encoding.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		UTF_8 = 3
	}
}
