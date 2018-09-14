#region CVS Log
/*
 * Version:
 *   $Id: ID3Genre.cs,v 1.2 2004/11/10 06:51:55 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: ID3Genre.cs,v $
 *   Revision 1.2  2004/11/10 06:51:55  cwoodbury
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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ID3Sharp
{
	/// <summary>
	/// This class contains a mapping between the integers for ID3v1 genres
	/// and their string values.
	/// </summary>
	public static class ID3Genre
	{
		#region Genres
		/// <summary>
		/// A collection of all of the genre string values in which the key of
		/// the genre its integer value for ID3v1 encoding.
		/// </summary>
		private static List<string> genres =
			new List<string>( new string[] { "Blues",					// 0
											 "Classic Rock",
											 "Country",
											 "Dance",
											 "Disco",
											 "Funk",
											 "Grunge",
											 "Hip-Hop",
											 "Jazz",
											 "Metal",
											 "New Age",					// 10
											 "Oldies",
											 "Other",
											 "Pop",
											 "R&B",
											 "Rap",
											 "Reggae",
											 "Rock",
											 "Techno",
											 "Industrial",
											 "Alternative",				// 20
											 "Ska",
											 "Death Metal",
											 "Pranks",
											 "Soundtrack",
											 "Euro-Techno",
											 "Ambient",
											 "Trip-Hop",
											 "Vocal",
											 "Jazz+Funk",
											 "Fusion",					// 30
											 "Trance",
											 "Classical",
											 "Instrumental",
											 "Acid",
											 "House",
											 "Game",
											 "Sound Clip",
											 "Gospel",
											 "Noise",
											 "AlternRock",				// 40
											 "Bass",
											 "Soul",
											 "Punk",
											 "Space",
											 "Meditative",
											 "Instrumental Pop",
											 "Instrumental Rock",
											 "Ethnic",
											 "Gothic",
											 "Darkwave",				// 50
											 "Techno-Industrial",
											 "Electronic",
											 "Pop-Folk",
											 "Eurodance",
											 "Dream",
											 "Southern Rock",
											 "Comedy",
											 "Cult",
											 "Gangsta",
											 "Top 40",					// 60
											 "Christian Rap",
											 @"Pop/Funk",
											 "Jungle",
											 "Native American",
											 "Cabaret",
											 "New Wave",
											 "Psychadelic",
											 "Rave",
											 "Showtunes",
											 "Trailer",					// 70
											 "Lo-Fi",
											 "Tribal",
											 "Acid Punk",
											 "Acid Jazz",
											 "Polka",
											 "Retro",
											 "Musical",
											 "Rock & Roll",
											 "Hard Rock",
											 "Folk",					// 80
											 "Folk-Rock",
											 "National Folk",
											 "Swing",
											 "Fast Fusion",
											 "Bebob",
											 "Latin",
											 "Revival",
											 "Celtic",
											 "Bluegrass",
											 "Avantgarde",				// 90
											 "Gothic Rock",
											 "Progressive Rock",
											 "Psychedelic Rock",
											 "Symphonic Rock",
											 "Slow Rock",
											 "Big Band",
											 "Chorus",
											 "Easy Listening",
											 "Acoustic",
											 "Humour",					// 100
											 "Speech",
											 "Chanson",
											 "Opera",
											 "Chamber Music",
											 "Sonata",
											 "Symphony",
											 "Booty Bass",
											 "Primus",
											 "Porn Groove",
											 "Satire",					// 110
											 "Slow Jam",
											 "Club",
											 "Tango",
											 "Samba",
											 "Folklore",
											 "Ballad",
											 "Power Ballad",
											 "Rhythmic Soul",
											 "Freestyle",
											 "Duet",					// 120
											 "Punk Rock",
											 "Drum Solo",
											 "Acapella",
											 "Euro-House",
											 "Dance Hall",
											 "Goa",
											 "Drum & Bass",
											 "Club-House",
											 "Hardcore",
											 "Terror",					// 130
											 "Indie",
											 "BritPop",
											 "Negerpunk",
											 "Polsk Punk",
											 "Beat",
											 "Christian Gangsta Rap",
											 "Heavy Metal",
											 "Black Metal",
											 "Crossover",
											 "Contemporary Christian",	// 140
											 "Christian Rock",
											 "Merengue",
											 "Salsa",
											 "Thrash Metal",
											 "Anime",
											 "JPop",
											 "Synthpop" } );			// 147
		#endregion

		/// <summary>
		/// A collection of the ID3v1 genre strings.
		/// </summary>
		public static ReadOnlyCollection<string> Genres
		{
			get
			{
				return new ReadOnlyCollection<string>( genres );
			}
		}
  
		/// <summary>
		/// Given an genre integer value, returns the corresponding string.
		/// If no genre exists for that integer, null is returned.
		/// </summary>
		/// <param name="numericGenre">The integer value for a genre.</param>
		/// <returns>The string corresponding to the specified integer,
		/// or null if there is no match.</returns>
		public static string GetGenre( int numericGenre ) 
		{
			string genre = null;
			if ( numericGenre >= 0 && numericGenre < genres.Count )
			{
				genre = genres[ numericGenre ];
			}

			return genre;
		}
		
		/// <summary>
		/// Given an genre string value, returns the corresponding ID3v1 integer value.
		/// If no genre exists for that string, -1 is returned.
		/// </summary>
		/// <param name="genre">The string value of a genre,
		/// case-insensitive.</param>
		/// <returns>The integer value corresponding to the specifiec string
		/// , or -1 if there is no match.</returns>
		public static int GetGenre( string genre ) 
		{
			Predicate<string> predicate =
				new Predicate<string>(delegate( string other)
					{
						return other.Equals( genre, StringComparison.InvariantCultureIgnoreCase );
					}
				);
			return genres.FindIndex( predicate );
		}
	}
}
