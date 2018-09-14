#region CVS Log
/*
 * Version:
 *   $Id: ID3v1Tag.cs,v 1.4 2004/11/16 08:41:25 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: ID3v1Tag.cs,v $
 *   Revision 1.4  2004/11/16 08:41:25  cwoodbury
 *   Renamed Comment property to Comments for consistency.
 *
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
using System.IO;
using System.Text;
using ID3Sharp.IO;

namespace ID3Sharp
{
	/// <summary>
	/// This class represents an ID3v1 tag.
	/// </summary>
	public class ID3v1Tag : ID3Tag
	{
		#region Fields
		/// <summary>The contents of the Album field.</summary>
		private string album = "";
		/// <summary>The contents of the Artist field.</summary>
		private string artist = "";
		/// <summary>The contents of the Comments field.</summary>
		private string comments = "";
		/// <summary>The contents of the Genre field.</summary>
		private int genre = -1;
		/// <summary>The contents of the Title field.</summary>
		private string title = "";
		/// <summary>The contents of the Year field.</summary>
		private string year = "";
		/// <summary>The contents of the Track Number field.</summary>
		private byte trackNumber;
		#endregion

		#region Constants
		/// <summary>
		/// The length of the ID3v1 tag.
		/// </summary>
		public const int TagLenth = 128;
		/// <summary>
		/// The length of the title field.
		/// </summary>
		public const int TitleFieldLength = 30;
		/// <summary>
		/// The length of the artist field.
		/// </summary>
		public const int ArtistFieldLength = 30;
		/// <summary>
		/// The length of the album field.
		/// </summary>
		public const int AlbumFieldLength = 30;
		/// <summary>
		/// The length of the year field.
		/// </summary>
		public const int YearFieldLength = 4;
		/// <summary>
		/// The length of the ID3v1 comments field.
		/// </summary>
		public const int ID3v1CommentsFieldLength = 30;
		/// <summary>
		/// The length of the ID3v1.1 comments field.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member" )]
		public const int ID3v1_1CommentsFieldLength = 28;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new ID3v1Tag.
		/// </summary>
		public ID3v1Tag()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="tag">The tag to copy.</param>
		public ID3v1Tag( ID3Tag tag )
			: base( tag )
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the value of the album field.
		/// </summary>
		public override string Album
		{
			get
			{
				return album;
			}
			set
			{
				album = ValidateString( value, AlbumFieldLength );
			}
		}

		/// <summary>
		/// Gets or sets the value of the artist field.
		/// </summary>
		public override string Artist
		{
			get
			{
				return artist;
			}
			set
			{
				artist = ValidateString( value, ArtistFieldLength );
			}
		}

		/// <summary>
		/// Gets or sets the value of the comments field.
		/// </summary>
		public override string Comments
		{
			get
			{
				return comments;
			}
			set
			{
				comments = ValidateString( value, ID3v1CommentsFieldLength );
			}
		}

		/// <summary>
		/// Gets or sets the value of the string value of the genre field.
		/// </summary>
		public override string Genre
		{
			get
			{
				return ID3Genre.GetGenre( this.GenreAsInt );
			}
			set
			{
				if ( value == null )
				{
					this.GenreAsInt = -1;
				}
				else
				{
					if ( ID3Genre.GetGenre( value ) != -1 )
					{
						this.GenreAsInt = ID3Genre.GetGenre( value );
					}
					else
					{
						this.GenreAsInt = ID3Genre.GetGenre( "Other" );
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the integer value of the genre field.
		/// </summary>
		public override int GenreAsInt
		{
			get
			{
				return genre;
			}
			set
			{
				if ( ID3Genre.GetGenre( value ) != null )
				{
					genre = value;
				}
				else
				{
					genre = -1;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the title field.
		/// </summary>
		public override string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = ValidateString( value, TitleFieldLength );
			}
		}

		/// <summary>
		/// Gets or sets the value of the year field.
		/// </summary>
		public override string Year
		{
			get
			{
				return year;
			}
			set
			{
				year = ValidateString( value, YearFieldLength );
			}
		}

		/// <summary>
		/// Gets or sets the value of the track number field. This field is
		/// only available in ID3v1.1.
		/// </summary>
		public override int TrackNumber
		{
			get
			{
				return (int) trackNumber;
			}
			set
			{
				if ( value >= byte.MinValue && value <= byte.MaxValue )
				{
					trackNumber = (byte) value;
				}
				else
				{
					trackNumber = 0;
				}
			}
		}
		#endregion

		/// <summary>
		/// Returns a string that is not null and is not longer than the specifed
		/// length.
		/// </summary>
		/// <param name="str">The string to validate.</param>
		/// <param name="length">The maximum length of the string.</param>
		/// <returns>A string that is not null and is not longer than the specifed
		/// length.</returns>
		protected string ValidateString( string str, int length )
		{
			// no nulls allowed
			if ( str == null )
			{
				str = "";
			}

			// trim to length, if necessary
			if ( str.Length > length )
			{
				str = str.Substring( 0, length );
			}

			return str;
		}

		#region Tag-Reading Methods
		/// <summary>
		/// Searches for an ID3v1 tag in a file. Returns true if the file
		/// contains an ID3v1 tag; false otherwise.
		/// </summary>
		/// <param name="filename">The name of a file to search.</param>
		/// <returns>True if the file contains an ID3v1 tag; false otherwise.</returns>
		public static bool HasTag( string filename )
		{
			using ( Stream stream = new FileStream( filename, FileMode.Open, FileAccess.Read ) )
			{
				return HasTag( stream );
			}
		}

		/// <summary>
		/// Searches for an ID3v1 tag in a stream. Returns true if the stream
		/// contains an ID3v1 tag; false otherwise.
		/// </summary>
		/// <param name="stream">A Stream to search.</param>
		/// <returns>True if stream contains an ID3v1 tag; false otherwise.</returns>
		public static bool HasTag( Stream stream )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			bool hasTag = false;
			if ( stream.Length >= ID3v1Tag.TagLenth )
			{
				stream.Seek( ( -ID3v1Tag.TagLenth ), SeekOrigin.End );

				byte[] buf = new byte[3];
				stream.Read( buf, 0, buf.Length );

				string sBuf = BytesToString( buf );
				hasTag = sBuf.Equals( "TAG" );
			}

			return hasTag;
		}

		/// <summary>
		/// Reads an ID3v1 tag from a file. Returns null if no ID3v1 tag can be found
		/// in the file.
		/// </summary>
		/// <param name="filename">The name of the file to read.</param>
		/// <returns>The tag read from the file, or null if no ID3v1 tag was present.</returns>
		public static ID3v1Tag ReadTag( string filename )
		{
			using ( Stream stream = new FileStream( filename, FileMode.Open, FileAccess.Read ) )
			{
				return ReadTag( stream );
			}
		}

		/// <summary>
		/// Reads an ID3v1 tag from a stream. Returns null if no ID3v1 tag can be found
		/// in the stream.
		/// </summary>
		/// <param name="stream">The stream to read.</param>
		/// <returns>The tag read from the stream, or null if no ID3v1 tag can be found.</returns>
		public static ID3v1Tag ReadTag( Stream stream )
		{
			ID3v1Tag tag = null;

			if ( HasTag( stream ) )
			{
				tag = new ID3v1Tag();
				byte[] bytes;

				bytes = ReadFromFile( ID3v1Tag.TitleFieldLength, stream );
				tag.Title = BytesToString( bytes );

				bytes = ReadFromFile( ID3v1Tag.ArtistFieldLength, stream );
				tag.Artist = BytesToString( bytes );

				bytes = ReadFromFile( ID3v1Tag.AlbumFieldLength, stream );
				tag.Album = BytesToString( bytes );

				bytes = ReadFromFile( ID3v1Tag.YearFieldLength, stream );
				tag.Year = BytesToString( bytes );

				bytes = ReadFromFile( ID3v1Tag.ID3v1_1CommentsFieldLength, stream );
				tag.Comments = BytesToString( bytes );

				// Track Number
				bytes = ReadFromFile( 2, stream );
				if ( bytes[0] == 0x0 )
				{
					tag.TrackNumber = (int) bytes[1];
				}
				else
				{
					tag.Comments += BytesToString( bytes );
				}

				// Genre
				bytes = ReadFromFile( 1, stream );
				if ( (sbyte) bytes[0] == -1 )
				{
					tag.GenreAsInt = -1;
				}
				else
				{
					tag.GenreAsInt = (int) bytes[0];
				}
			}

			return tag;
		}

		/// <summary>
		/// Reads a number of bytes, starting at the given position from the
		/// given FileStream. The bytes read are returned as a byte array.
		/// </summary>
		/// <param name="startLoc">The position at which to begin
		/// reading, relative to the start of the tag.</param>
		/// <param name="length">The number of bytes to read.</param>
		/// <param name="stream">The FileStream from which to read.</param>
		/// <returns>An array of the bytes read.</returns>
		protected static byte[] ReadFromFile( long startLoc, int length, Stream stream )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			stream.Seek( startLoc, SeekOrigin.Begin );

			byte[] buf = new byte[ length ];
			stream.Read( buf, 0, length );

			return buf;
		}

		/// <summary>
		/// Reads a number of bytes, starting at the current position from the
		/// given FileStream. The bytes read are returned as a byte array.
		/// </summary>
		/// <param name="length">The number of bytes to read.</param>
		/// <param name="stream">The FileStream from which to read.</param>
		/// <returns>An array of the bytes read.</returns>
		protected static byte[] ReadFromFile( int length, Stream stream )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			return ReadFromFile( stream.Position, length, stream );
		}

		/// <summary>
		/// Converts an array of bytes to a string.
		/// </summary>
		/// <param name="bytes">The byte array to convert.</param>
		/// <returns>The string equivalent to the specified
		/// array of bytes.</returns>
		protected static string BytesToString( byte[] bytes )
		{
			// copy to char[] for the string constructor
			char[] chars = new char[ bytes.Length ];
			Array.Copy( bytes, chars, bytes.Length );

			// make the string
			string str = new string( chars );

			// handle null-termination
			int stringEnd = str.IndexOf( (char) 0x0 );
			if ( stringEnd == -1 )
			{
				stringEnd = str.Length;
			}
			str = str.Substring( 0, stringEnd );
			str = str.Trim();

			return str;
		}
		#endregion

		#region Tag-Writing Methods
		/// <summary>
		/// Writes the tag to a stream using the format specified by a particular ID3 version.
		/// Only V1_0 and V1_1 are supported.
		/// </summary>
		/// <param name="stream">The stream to write the tag to.</param>
		/// <param name="version">The version to use. Only V1_0 and V1_1 are supported.</param>
		public override void WriteTag( Stream stream, ID3Versions version )
		{
			if ( (version & ID3Versions.V1) != ID3Versions.V1 )
			{
				throw new UnsupportedVersionException( "Only versions 1.x are supported by this method.", version );
			}

			EnsureSpace( stream );
			string tagTitle = FormatString( this.Title, TitleFieldLength );
			string tagArtist = FormatString( this.Artist, ArtistFieldLength );
			string tagAlbum = FormatString( this.Album, AlbumFieldLength );
			string tagYear = FormatString( this.Year, YearFieldLength );

			stream.Seek( - TagLenth, SeekOrigin.End );
			stream.Write( StringToBytes( "TAG" ), 0, 3 );
			stream.Write( StringToBytes( tagTitle ), 0, TitleFieldLength );
			stream.Write( StringToBytes( tagArtist ), 0, ArtistFieldLength );
			stream.Write( StringToBytes( tagAlbum ), 0, AlbumFieldLength );
			stream.Write( StringToBytes( tagYear ), 0, YearFieldLength );

			if ( version == ID3Versions.V1_0 )
			{
				string tagComment = FormatString( this.Comments, ID3v1CommentsFieldLength );
				stream.Write( StringToBytes( tagComment ), 0, ID3v1CommentsFieldLength );
			}
			else
			{
				string tagComment = FormatString( this.Comments, ID3v1_1CommentsFieldLength );
				stream.Write( StringToBytes( tagComment ), 0, ID3v1_1CommentsFieldLength );
				stream.WriteByte( (byte) 0x0 );
				stream.WriteByte( (byte) this.TrackNumber );
			}
			stream.WriteByte( (byte) this.GenreAsInt );
		}

		/// <summary>
		/// Ensures that there is space for the tag at the end of the stream.
		/// </summary>
		/// <param name="stream">The stream to check.</param>
		private void EnsureSpace( Stream stream )
		{
			if ( ! ID3v1Tag.HasTag( stream ) )
			{
				stream.SetLength( stream.Length + TagLenth );
			}
		}

		/// <summary>
		/// Returns a string that is not null and is a certain length,
		/// truncated or padded with nulls if necessary.
		/// </summary>
		/// <param name="str">The string to format.</param>
		/// <param name="length">The desired length of the string.</param>
		/// <returns>The string, truncated or padded to the specified length.</returns>
		protected string FormatString( string str, int length )
		{
			if ( str == null )
			{
				str = "";
			}

			if ( str.Length > length )
			{
				// truncate
				str = str.Substring( 0, length );
			}
			else
			{
				// pad with NULLs
				str = str.PadRight( length, (char) 0x0 );
			}

			return str;
		}
		#endregion

		#region Tag Removal Methods
		/// <summary>
		/// Removes any ID3v1 tag from a file.
		/// </summary>
		/// <param name="filename">The name of the file whose ID3v1 tag will be removed.</param>
		public static void RemoveTag( string filename )
		{
			using ( FileStream stream = new FileStream( filename, FileMode.Open, FileAccess.ReadWrite ) )
			{
				RemoveTag( stream );
			}
		}

		/// <summary>
		/// Removes any ID3v1 tag from a stream.
		/// </summary>
		/// <param name="stream">The stream whose ID3v1 tag will be removed.</param>
		public static void RemoveTag( Stream stream )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			if ( HasTag( stream ) )
			{
				stream.SetLength( stream.Length - TagLenth );
			}
			else
			{
				throw new TagNotFoundException( "No tag found" );
			}
		}
		#endregion
	}
}
