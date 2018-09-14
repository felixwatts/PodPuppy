#region CVS Log
/*
 * Version:
 *   $Id: ID3Tag.cs,v 1.3 2004/11/16 08:41:25 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: ID3Tag.cs,v $
 *   Revision 1.3  2004/11/16 08:41:25  cwoodbury
 *   Renamed Comment property to Comments for consistency.
 *
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
using System.IO;

namespace ID3Sharp
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class ID3Tag
	{
		#region Properties
		/// <summary>
		/// Gets and sets the album title of the file this tag represents.
		/// </summary>
		public abstract string Album
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the album title of the file this tag represents.
		/// </summary>
		public abstract string Artist
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the comments of the file this tag represents.
		/// </summary>
		public abstract string Comments
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the string value of the genre of the file this tag represents.
		/// </summary>
		public abstract string Genre
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the integer value of the genre of the file this tag represents.
		/// </summary>
		public virtual int GenreAsInt
		{
			get
			{
				return ID3Genre.GetGenre( this.Genre );
			}
			set
			{
				this.Genre = ID3Genre.GetGenre( value );
			}
		}

		/// <summary>
		/// Gets and sets the title of the file this tag represents.
		/// </summary>
		public abstract string Title
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the release year of the file this tag represents.
		/// </summary>
		public abstract string Year
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the track number of the file this tag represents.
		/// </summary>
		public abstract int TrackNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a value indicating whether the Artist property is not empty or null.
		/// </summary>
		public bool HasArtist
		{
			get
			{
				return ! String.IsNullOrEmpty( Artist );
			}
		}

		/// <summary>
		/// Gets a value indicating whether the Title property is not empty or null.
		/// </summary>
		public bool HasTitle
		{
			get
			{
				return ! String.IsNullOrEmpty( Title );
			}
		}

		/// <summary>
		/// Gets a value indicating whether the Album property is not empty or null.
		/// </summary>
		public bool HasAlbum
		{
			get
			{
				return ! String.IsNullOrEmpty( Album );
			}
		}

		/// <summary>
		/// Gets a value indicating whether the Comments property is not empty or null.
		/// </summary>
		public bool HasComments
		{
			get
			{
				return ! String.IsNullOrEmpty( Comments );
			}
		}

		/// <summary>
		/// Gets a value indicating whether the Genre property is not empty or null.
		/// </summary>
		public bool HasGenre
		{
			get
			{
				return ! String.IsNullOrEmpty( Genre );
			}
		}

		/// <summary>
		/// Gets a value indicating whether the Year property is not empty or null.
		/// </summary>
		public bool HasYear
		{
			get
			{
				return ! String.IsNullOrEmpty( Year );
			}
		}

		/// <summary>
		/// Gets a value indicating whether the TrackNumber property is greater than 0.
		/// </summary>
		public bool HasTrackNumber
		{
			get
			{
				return (TrackNumber > 0);
			}
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Creates a new ID3Tag.
		/// </summary>
		protected ID3Tag()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="tag">The tag to copy.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors" )]
		protected ID3Tag( ID3Tag tag )
		{
			if ( tag == null )
			{
				throw new ArgumentNullException( "tag" );
			}

			this.Album = tag.Album;
			this.Artist = tag.Artist;
			this.Comments = tag.Comments;
			this.GenreAsInt = tag.GenreAsInt;
			this.Title = tag.Title;
			this.TrackNumber = tag.TrackNumber;
			this.Year = tag.Year;
		}
		#endregion

		/// <summary>
		/// Returns a string representation of this tag object.
		/// </summary>
		/// <returns>A string representation of this tag object.</returns>
		public override string ToString()
		{			
			string toString = String.Format( "{0} - {1}", Artist, Title );
			if ( HasAlbum )
			{
				toString += String.Format( " ({0})", Album );
			}

			return toString;
		}

		/// <summary>
		/// Writes the tag to a stream using the format specified by a particular ID3 version.
		/// </summary>
		/// <param name="stream">The stream to write the tag to.</param>
		/// <param name="version">The version to use.</param>
		public abstract void WriteTag( Stream stream, ID3Versions version );

		/// <summary>
		/// Writes the tag to a file using the format specified by a particular ID3 version.
		/// </summary>
		/// <param name="filename">The name of the file to write the tag to.</param>
		/// <param name="version">The version to use.</param>
		public virtual void WriteTag( string filename, ID3Versions version )
		{
			using ( Stream stream = new FileStream( filename, FileMode.OpenOrCreate, FileAccess.ReadWrite ) )
			{
				WriteTag( stream, version );
			}
		}

		/// <summary>
		/// Converts a string to an array of bytes.
		/// </summary>
		/// <param name="str">The string to convert.</param>
		/// <returns>The array of bytes equivalent to the
		/// specified string.</returns>
		protected byte[] StringToBytes( string str )
		{
			if ( str == null )
			{
				throw new ArgumentNullException( "str" );
			}

			byte[] bytes = new byte[ str.Length ];

			for ( int itr = 0; itr < bytes.Length; itr++ )
			{
				bytes[ itr ] = (byte) str[ itr ];
			}

			return bytes;
		}
	}
}
