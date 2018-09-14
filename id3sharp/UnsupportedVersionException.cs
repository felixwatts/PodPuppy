#region CVS Log
/*
 * Version:
 *   $Id: UnsupportedVersionException.cs,v 1.3 2004/11/10 06:51:55 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: UnsupportedVersionException.cs,v $
 *   Revision 1.3  2004/11/10 06:51:55  cwoodbury
 *   Hid CVS log messages away in #region
 *
 *   Revision 1.2  2004/11/03 08:19:33  cwoodbury
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
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ID3Sharp
{
	/// <summary>
	/// Represents an error that occurs when an operation is attempted for an ID3
	/// version that is not supported.
	/// </summary>
	[Serializable]
	public class UnsupportedVersionException : Exception
	{
		/// <summary>
		/// The unsupported version.
		/// </summary>
		private ID3Versions version;

		#region Constructors
		/// <summary>
		/// Creates a new UnsupportedVersionException.
		/// </summary>
		public UnsupportedVersionException()
		{
		}

		/// <summary>
		/// Creates a new UnsupportedVersionException.
		/// </summary>
		/// <param name="message">The error message that explains the reason
		/// for the exception.</param>
		public UnsupportedVersionException( string message )
			: base( message )
		{
		}

		/// <summary>
		/// Creates a new UnsupportedVersionException.
		/// </summary>
		/// <param name="version">The unsupported version.</param>
		public UnsupportedVersionException( ID3Versions version )
		{
			this.version = version;
		}

		/// <summary>
		/// Creates a new UnsupportedVersionException.
		/// </summary>
		/// <param name="message">The error message that explains the reason
		/// for the exception.</param>
		/// <param name="version">The unsupported version.</param>
		public UnsupportedVersionException( string message, ID3Versions version )
			: base( message )
		{
			this.version = version;
		}

		public UnsupportedVersionException( string message, Exception inner )
			: base( message, inner )
		{
		}

		protected UnsupportedVersionException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
			if ( info == null )
			{
				throw new ArgumentNullException( "info" );
			}

			version = (ID3Versions) info.GetInt32( "version" );
		}
		#endregion


		/// <summary>
		/// The unsupported version.
		/// </summary>
		public ID3Versions Version
		{
			get
			{
				return version;
			}
		}

		[SecurityPermission( SecurityAction.Demand, SerializationFormatter = true )]
		public override void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			if ( info == null )
			{
				throw new ArgumentNullException( "info" );
			}

			info.AddValue( "version", version );
			base.GetObjectData( info, context );
		}
	}
}
