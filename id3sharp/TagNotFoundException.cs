#region CVS Log
/*
 * Version:
 *   $Id: TagNotFoundException.cs,v 1.2 2004/11/10 06:51:56 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: TagNotFoundException.cs,v $
 *   Revision 1.2  2004/11/10 06:51:56  cwoodbury
 *   Hid CVS log messages away in #region
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

namespace ID3Sharp
{
	/// <summary>
	/// Represents an error that occurs when an expected ID3 tag cannot be found
	/// in a stream or file.
	/// </summary>
	[Serializable]
	public class TagNotFoundException : Exception
	{
		/// <summary>
		/// Creates a new TagNotFoundException.
		/// </summary>
		public TagNotFoundException()
		{
		}

		/// <summary>
		/// Creates a new TagNotFoundException.
		/// </summary>
		/// <param name="message">The error message that explains the reason
		/// for the exception.</param>
		public TagNotFoundException( string message )
			: base( message )
		{
		}

		public TagNotFoundException( string message, Exception inner )
			: base( message, inner )
		{
		}

		protected TagNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context )
			: base( info, context )
		{
		}
	}
}
