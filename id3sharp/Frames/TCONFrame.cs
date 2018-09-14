#region CVS Log
/*
 * Version:
 *   $Id: TCONFrame.cs,v 1.5 2004/12/10 04:49:08 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: TCONFrame.cs,v $
 *   Revision 1.5  2004/12/10 04:49:08  cwoodbury
 *   Made changes to EncodedString and to how it is used to push it down to
 *   just being involved with frame I/O and not otherwise being used in frames.
 *
 *   Revision 1.4  2004/11/20 22:27:33  cwoodbury
 *   Fixed bug #1070224: TCONFrame does not properly handle data.
 *
 *   Revision 1.3  2004/11/10 06:51:55  cwoodbury
 *   Hid CVS log messages away in #region
 *
 *   Revision 1.2  2004/11/10 04:44:16  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.1  2004/11/03 01:18:51  cwoodbury
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
using System.Text.RegularExpressions;

using ID3Sharp.IO;

namespace ID3Sharp.Frames
{
	/// <summary>
	/// A Content Type frame.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased" )]
	public class TCONFrame : TextInformationFrame
	{
		private const string GENRE_REGEX_PATTERN = @"(\((?<int>[0-9]+)\))?(?<genre>.+)?";

		#region Constructors
		/// <summary>
		/// Creates a new TCONFrame.
		/// </summary>
		protected internal TCONFrame()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="frame">Frame to copy.</param>
		protected internal TCONFrame( TCONFrame frame ) 
			: base( frame )
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets and sets the contents of the Content Type field.
		/// </summary>
		public string ContentType
		{
			get
			{
				string genre = null;
				if ( base.Text != null )
				{
					Match match = Regex.Match( base.Text, GENRE_REGEX_PATTERN );
					if ( match.Success )
					{
						if ( match.Groups[ "genre" ].Success )
						{
							genre = match.Groups[ "genre" ].Value;
						}
						else if ( match.Groups[ "int" ].Success )
						{
							genre = ID3Genre.GetGenre( Int32.Parse( match.Groups[ "int" ].Value ) );
						}
					}
				}

				return genre;
			}
			set
			{
				string genre = value;
				int genreInt = ID3Genre.GetGenre( value );

				if ( genreInt >= 0 )
				{
					base.Text = "(" + genreInt + ")" + genre;
				}
				else
				{
					base.Text = genre;
				}
			}
		}

		/// <summary>
		/// Gets and sets the integer value of the Content Type field. This is
		/// an optional value, using the original ID3v1 Genre values.
		/// </summary>
		public int Int
		{
			get
			{
				int genreInt = -1;
				if ( base.Text != null )
				{
					Match match = Regex.Match( base.Text, GENRE_REGEX_PATTERN );
					if ( match.Success )
					{
						if ( match.Groups[ "int" ].Success )
						{
							genreInt = Int32.Parse( match.Groups[ "int" ].Value );
						}
						else if ( match.Groups[ "genre" ].Success )
						{
							genreInt = ID3Genre.GetGenre( match.Groups[ "genre" ].Value );
						}
					}
				}

				return genreInt;
			}
			set
			{
				string genreString = ID3Genre.GetGenre( value );
				if ( genreString != null )
				{
					base.Text = "(" + value + ")" + genreString;
				}
				else
				{
					base.Text = "";
				}
			}
		}
		#endregion
		
		/// <summary>
		/// Returns a copy of this frame. Supports the prototype design pattern.
		/// </summary>
		/// <returns>A copy of this frame.</returns>
		public override ID3v2Frame Copy()
		{
			return new TCONFrame( this );
		}

		/// <summary>
		/// Parses the raw frame data.
		/// </summary>
		/// <param name="frameData">The raw frame data.</param>
		/// <param name="version">The ID3 version of the tag being parsed.</param>
		protected override void ParseFrameData( byte[] frameData, ID3Versions version )
		{
			base.ParseFrameData( frameData, version );
			// "If the refinement should begin with a '(' character, it should be
			// replaced with '(('" [ID3 Spec.]
			if ( this.ContentType != null && this.ContentType.StartsWith( "((" ) )
			{
				this.ContentType = this.ContentType.Remove( 0, 1 );
			}
		}

		/// <summary>
		/// Writes the frame to a stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="version">The ID3v2 version to use in writing the frame.</param>
		public override void WriteToStream( System.IO.Stream stream, ID3Versions version )
		{
			// "If the refinement should begin with a '(' character, it should be
			// replaced with '(('" [ID3 Spec.]
			if ( this.ContentType != null && this.ContentType.StartsWith( "(" ) )
			{
				this.ContentType = this.ContentType.Insert( 0, "(" );
			}
			base.WriteToStream( stream, version );
		}
	}
}
