#region CVS Log
/*
 * Version:
 *   $Id: Demo.cs,v 1.1 2004/11/16 08:40:53 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: Demo.cs,v $
 *   Revision 1.1  2004/11/16 08:40:53  cwoodbury
 *   Added Demo.cs
 *
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

using ID3Sharp;
using ID3Sharp.Frames;

namespace Demo
{
	[SuppressMessage( "Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces" )]
	public class Demo
	{
		public Demo()
		{
		}

		[SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args" )]
		[SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
		[STAThread]
		static void Main(string[] args)
		{
			Demo demo = new Demo();
			demo.DemoBasicUsage();
			demo.DemoAdvancedUsage();
			
			Console.Write( "Press Enter to exit" );
			Console.Read();
		}

		/// <summary>
		/// Demonstrates basic usage of the library.
		/// Creates a file with an ID3v1 and ID3v2 tag, each with some fields populated.
		/// You can open this output file up with an ID3 editor, like Winamp, (or a hex
		/// editor if you're into the gory details) and see the results.
		/// </summary>
		public void DemoBasicUsage()
		{
			DemoBasicUsageForID3v1();
			DemoBasicUsageForID3v2();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		public void DemoBasicUsageForID3v1()
		{
			// Create a new ID3v1 tag.
			ID3v1Tag v1Tag = new ID3v1Tag();
			v1Tag.Album = "My Track Album";
			v1Tag.Artist = "My Track Artist";
			v1Tag.Comments = "My Track Comments";
			v1Tag.Genre = "Rock"; // this has to be a genre from the ID3v1 genre list (see ID3Genre.cs)
			v1Tag.Title = "My Track Title";
			v1Tag.TrackNumber = 1;
			v1Tag.Year = "2004";

			// Write our new tag out to a file as ID3v1.1 (for example; v1.0 would work too)
			v1Tag.WriteTag( "basic.tag", ID3Versions.V1_1 );
			
			// Read the tag back from the file.
			v1Tag = null;
			if ( ! ID3v1Tag.HasTag( "basic.tag" ) )
			{
				Console.WriteLine( "Hmmmm....something didn't go right here." );
			}
			else
			{
				v1Tag = ID3v1Tag.ReadTag( "basic.tag" );

				// Some usage possibilities:
				Console.WriteLine( "Album: " + v1Tag.Album );
				if ( v1Tag.HasArtist ) // checks to see if Artist isn't null or empty
				{
					Console.WriteLine( "Artist: " + v1Tag.Artist );
				}

				// Make a change and write it back out to the file.
				v1Tag.Comments = null;
				v1Tag.WriteTag( "basic.tag", ID3Versions.V1_1 );

				// Show that the change worked.
				v1Tag = ID3v1Tag.ReadTag( "basic.tag" );
				if ( v1Tag.HasComments )
				{
					Console.WriteLine( "Hmmmm....something didn't go right here." );
				}
				else
				{
					Console.WriteLine( "See! This library really does work." );
				}
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		public void DemoBasicUsageForID3v2()
		{
			// Create a new ID3v2 tag. ID3v2 works mostly like ID3v1 from this
			// perspective, although there are a few enhancements we can use.
			ID3v2Tag v2Tag = new ID3v2Tag();
			// no length restrictions with v2
			v2Tag.Album = "An album with a really long name that wouldn't fit into a ID3v1 tag";
			v2Tag.Genre = "A genre of my own creation!"; // not restricted to pre-defined genres with v2
			
			// Write our new tag out to a file as ID3v2.3 (for example; v2.2
			// and v2.4 would work too, although ID3v2.4 support is scare out
			// in the real world)
			v2Tag.WriteTag( "basic.tag", ID3Versions.V2_3 );
			
			// Read the tag back from the file.
			v2Tag = null;
			if ( ! ID3v2Tag.HasTag( "basic.tag" ) )
			{
				Console.WriteLine( "Hmmmm....something didn't go right here." );
			}
			else
			{
				v2Tag = ID3v2Tag.ReadTag( "basic.tag" );

				// Some usage possibilities:
				Console.WriteLine( "Album: " + v2Tag.Album );

				// Make a change and write it back out to the file.
				v2Tag.Comments = "New comments";
				v2Tag.WriteTag( "basic.tag", ID3Versions.V2_3 );

				// Show that the change worked.
				v2Tag = ID3v2Tag.ReadTag( "basic.tag" );
				if ( v2Tag.HasComments )
				{
					Console.WriteLine( "The comments we just added: " + v2Tag.Comments );
				}
				else
				{
					Console.WriteLine( "Hmmmm....something didn't go right here." );
				}

				// Some other stuff:
				ID3Versions version = ID3v2Tag.LookForTag( "basic.tag" );
				Console.WriteLine( "ID3 tag found, version: " + version.ToString() );
			}
		}

		
		/// <summary>
		/// Demos advanced usage of the library - handling the ID3v2 frames themselves.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		public void DemoAdvancedUsage()
		{
			// ID3v2 Track Number frames allow you to specify not only the track number,
			// but the total number of tracks as well. Use a TRCKFrame to get to this
			// functionality. (The frame classes are named after their ID3v2.4 frame IDs,
			// e.g. TRCK for Track Number and PRIV for Private Use.)
			ID3v2Tag tag = new ID3v2Tag();
			
			// This somewhat awkward method for creating frames does have some benefits
			// in other areas.
			TRCKFrame trackNumberFrame = (TRCKFrame) ID3v2Frame.GetNewFrame( FrameType.TrackNumber );
			
			// Add the track number data.
			trackNumberFrame.TrackNumber = 1;
			trackNumberFrame.TotalTracks = 10;
			
			// Add the frame to the tag.
			tag.AddFrame( trackNumberFrame );

			// Most of the usual frames are simple implemented as TextInformationFrame's.
			TextInformationFrame artistFrame = (TextInformationFrame) ID3v2Frame.GetNewFrame( FrameType.LeadArtist );
			artistFrame.Text = "My Artist";
			tag.AddFrame( artistFrame );

			// Now that the artist frame is there, the Artist property will work
			Console.WriteLine( "Artist: " + tag.Artist );
			// but the others still don't.
			if ( tag.HasAlbum )
			{
				Console.WriteLine( "Hmmmm....something didn't go right here." );
			}
			else
			{
				Console.WriteLine( "Album (still null): " + tag.Album );
			}
			// While we're on the topic of the properties, TrackNumber works, but only returns
			// the track number, not the total tracks (can't fit both into one int, of course)
			Console.WriteLine( "Track Number: " + tag.TrackNumber );

			// Use a Private frame (PRIVFrame) to hold some private data
			PRIVFrame privateFrame = (PRIVFrame) ID3v2Frame.GetNewFrame( FrameType.Private );
			privateFrame.OwnerIdentifier = "ID3Sharp demo";
			privateFrame.PrivateData = new byte[] { 0x49, 0x44, 0x33, 0x53, 0x68, 0x61, 0x72, 0x70 };
			tag.AddFrame( privateFrame );

			// Write the tag out to a file
			tag.WriteTag( "advanced.tag", ID3Versions.V2_3 );
			// and read it back.
			tag = ID3v2Tag.ReadTag( "advanced.tag" );

			// Verify the contents
			privateFrame = (PRIVFrame) tag[ FrameType.Private ];
			Console.Write( "Private Data: " );
			foreach ( byte dataByte in privateFrame.PrivateData )
			{
				Console.Write( dataByte );
				Console.Write( " (" + (char) dataByte + ") " );
			}
			Console.WriteLine();

			// Remove the frames, with a few (for demonstration) different methods
			tag.RemoveFrame( privateFrame );
			tag.RemoveFrame( FrameType.LeadArtist );
			tag[ FrameType.TrackNumber ] = null;

			tag.WriteTag( "empty.tag", ID3Versions.V2_3 );

			// Questions? Problems? Suggestions? Post to a forum on the project site
			// or send me an email.
		}
	}
}
