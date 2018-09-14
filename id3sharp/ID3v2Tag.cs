#region CVS Log
/*
 * Version:
 *   $Id: ID3v2Tag.cs,v 1.9 2004/11/20 22:17:54 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: ID3v2Tag.cs,v $
 *   Revision 1.9  2004/11/20 22:17:54  cwoodbury
 *   Fixed bug #1069845: ID3v2Tag.Genre does not properly handle data.
 *   Updated documentation.
 *
 *   Revision 1.8  2004/11/16 08:41:25  cwoodbury
 *   Renamed Comment property to Comments for consistency.
 *
 *   Revision 1.7  2004/11/11 09:39:51  cwoodbury
 *   Organized code.
 *
 *   Revision 1.6  2004/11/10 06:51:56  cwoodbury
 *   Hid CVS log messages away in #region
 *
 *   Revision 1.5  2004/11/10 06:31:14  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.4  2004/11/10 04:44:16  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.3  2004/11/03 08:19:33  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.2  2004/11/03 06:49:08  cwoodbury
 *   Fixed bug with size;
 *   added recognition of tag header flags
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using ID3Sharp.Frames;
using ID3Sharp.IO;

namespace ID3Sharp
{
	/// <summary>
	/// This class represents an ID3v2 tag.
	/// </summary>
	public class ID3v2Tag : ID3Tag
	{
		#region Constants
		/// <summary>
		/// The length of a tag header.
		/// </summary>
		private const int TAG_HEADER_LENGTH = 10;
		/// <summary>
		/// The length of a tag footer.
		/// </summary>
		private const int TAG_FOOTER_LENGTH = 10;
		/// <summary>
		/// The position in the tag header of the flags field.
		/// </summary>
		private const int TAG_FLAGS_POSITION = 5;
		/// <summary>
		/// The position in the tag header of the start of the size field.
		/// </summary>
		private const int TAG_SIZE_START = 6;
		/// <summary>
		/// The number of bytes in the tag header size field.
		/// </summary>
		private const int TAG_SIZE_LENGTH = 4;
		#endregion

		#region Fields
		/// <summary>
		/// The frames contained in this tag. The keys are the frame's FrameType
		/// and the value is the frame itself.
		/// </summary>
		private Dictionary<FrameType, ID3v2Frame> frames = new Dictionary<FrameType, ID3v2Frame>();
		/// <summary>
		/// The number of padding bytes in this tag.
		/// </summary>
		private int paddingSize;
		/// <summary>
		/// The header flags for this tag.
		/// </summary>
		private TagHeaderFlagsV2 headerFlags;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new ID3v2Tag.
		/// </summary>
		public ID3v2Tag()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="tag">The tag to copy.</param>
		public ID3v2Tag( ID3Tag tag )
			: base( tag )
		{
			ID3v2Tag v2Tag = tag as ID3v2Tag;
			if ( v2Tag != null )
			{
				this.paddingSize = v2Tag.paddingSize;
				this.headerFlags = v2Tag.headerFlags;

				this.frames = new Dictionary<FrameType, ID3v2Frame>( v2Tag.FrameCount );
				foreach ( ID3v2Frame frame in v2Tag.Frames )
				{
					this.AddFrame( frame.Copy() );
				}
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the number of padding bytes in this tag.
		/// </summary>
		public virtual int PaddingSize
		{
			get
			{
				return paddingSize;
			}
			set
			{
				paddingSize = value;
			}
		}

		/// <summary>
		/// Gets the number of frames in this tag.
		/// </summary>
		public virtual int FrameCount
		{
			get
			{
				int count = 0;
				foreach ( ID3v2Frame frame in frames.Values )
				{
					FrameComposite composite = frame as FrameComposite;
					if ( composite != null )
					{
						count += composite.FrameCount;
					}
					else
					{
						count++;
					}
				}
				return count;
			}
		}

		/// <summary>
		/// Gets or sets the ID3v2Frame for a particular FrameType.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1043:UseIntegralOrStringArgumentForIndexers" )]
		public virtual ID3v2Frame this[ FrameType frameType ]
		{
			get
			{
				ID3v2Frame frame = null;
				if ( frames.ContainsKey( frameType ) )
				{
					frame = frames[ frameType ];
				}

				return frame;
			}
			set
			{
				if ( value == null )
				{
					RemoveFrame( frameType );
				}
				else
				{
					AddFrame( value );
				}
			}
		}

		/// <summary>
		/// Gets the header flags for the tag.
		/// </summary>
		public TagHeaderFlagsV2 HeaderFlags
		{
			get
			{
				return headerFlags;
			}
		}
		#endregion
		
		#region Public Methods
		/// <summary>
		/// Gets the total size of the frames in this tag.
		/// </summary>
		public virtual int GetFramesSize( ID3Versions version )
		{
			int size = 0;
			foreach ( ID3v2Frame frame in frames.Values )
			{
				size += frame.GetTotalSize( version );
			}
			return size;
		}

		/// <summary>
		/// Gets the total size of this tag (header + frames + padding).
		/// </summary>
		public virtual int GetTotalSize( ID3Versions version )
		{
			return this.GetFramesSize( version ) + this.PaddingSize + TAG_HEADER_LENGTH;
		}
		#endregion

		#region ID3v1 Field Equivalent Properties
		/// <summary>
		/// Gets or sets the value of the album frame.
		/// </summary>
		public override string Album
		{
			get
			{
				return GetFrameTextValue( FrameType.AlbumTitle );
			}
			set
			{
				SetFrameTextValue( FrameType.AlbumTitle, value );
			}
		}

		/// <summary>
		/// Gets or sets the value of the artist frame.
		/// </summary>
		public override string Artist
		{
			get
			{
				return GetFrameTextValue( FrameType.LeadArtist );
			}
			set
			{
				SetFrameTextValue( FrameType.LeadArtist, value );
			}
		}

		/// <summary>
		/// Gets or sets the value of the comments frame.
		/// </summary>
		public override string Comments
		{
			get
			{
				return GetFrameTextValue( FrameType.Comments );
			}
			set
			{
				SetFrameTextValue( FrameType.Comments, value );
			}
		}

		/// <summary>
		/// Gets or sets the value of the genre frame.
		/// </summary>
		public override string Genre
		{
			get
			{
				string genreString = null;
				TCONFrame genreFrame = GetFirstFrame( FrameType.ContentType ) as TCONFrame;
				if ( genreFrame != null )
				{
					genreString = genreFrame.ContentType;
				}

				return genreString;
			}
			set
			{
				if ( value == null )
				{
					this.RemoveFrame( FrameType.ContentType );
				}
				else
				{
					TCONFrame genreFrame = GetFirstFrame( FrameType.ContentType ) as TCONFrame;
					if ( genreFrame == null )
					{
						genreFrame =
							(TCONFrame) FrameRegistry.GetNewFrame( FrameType.ContentType );
						this.AddFrame( genreFrame );
					}
					
					genreFrame.ContentType = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the title frame.
		/// </summary>
		public override string Title
		{
			get
			{
				return GetFrameTextValue( FrameType.Title );
			}
			set
			{
				SetFrameTextValue( FrameType.Title, value );
			}
		}

		/// <summary>
		/// Gets or sets the value of the year frame.
		/// </summary>
		public override string Year
		{
			get
			{
				return GetFrameTextValue( FrameType.Year );
			}
			set
			{
				SetFrameTextValue( FrameType.Year, value );
			}
		}

		/// <summary>
		/// Gets or sets the value of the track number frame. If no track
		/// number is present in the tag, -1 is returned.
		/// </summary>
		public override int TrackNumber
		{
			get
			{
				int trackNumber = -1;
				TRCKFrame trackFrame = GetFirstFrame( FrameType.TrackNumber ) as TRCKFrame;
				if ( trackFrame != null )
				{
					trackNumber = trackFrame.TrackNumber;
				}

				return trackNumber;
			}
			set
			{
				if ( value < 1 )
				{
					SetFrameTextValue( FrameType.TrackNumber, null );
				}
				else
				{
					TRCKFrame trackFrame = GetFirstFrame( FrameType.TrackNumber ) as TRCKFrame;
					if ( trackFrame == null )
					{
						trackFrame = (TRCKFrame) FrameRegistry.GetNewFrame( FrameType.TrackNumber );
						this.AddFrame( trackFrame );
					}
					trackFrame.TrackNumber = value;
				}
			}
		}
		#endregion

		#region Property Helpers
		/// <summary>
		/// Gets the value of a text frame. If the frame is a composite, the
		/// first frame is used; if no frames exist for the given FrameType,
		/// null is returned.
		/// </summary>
		/// <param name="frameType">The type of frame to get the value of.</param>
		/// <returns>The value of the indicated text frame, or null if there
		/// are no frames of the indicated type.</returns>
		private string GetFrameTextValue( FrameType frameType )
		{
			string textValue = null;

			TextInformationFrame textFrame = GetFirstFrame( frameType ) as TextInformationFrame;

			if ( textFrame != null )
			{
				textValue = textFrame.Text;
			}
			return textValue;
		}

		/// <summary>
		/// Sets the value of a text frame. If the frame is a composite, the
		/// first frame is used; if no frames exist for the given FrameType,
		/// a new frame is added; if null is passed as the value, any frames
		/// of the specified type are removed.
		/// </summary>
		/// <param name="frameType">The type of frame to set the value of.</param>
		/// <param name="textValue">The value to assign to the frame.</param>
		private void SetFrameTextValue( FrameType frameType, string textValue )
		{
			if ( textValue == null )
			{
				this.RemoveFrame( frameType );
			}
			else
			{
				TextInformationFrame textFrame = GetFirstFrame(frameType) as TextInformationFrame;

				if ( textFrame == null )
				{
					textFrame = (TextInformationFrame) FrameRegistry.GetNewFrame( frameType );
					this.AddFrame( textFrame );
				}
				textFrame.Text = textValue;
			}
		}

		private ID3v2Frame GetFirstFrame( FrameType frameType )
		{
			ID3v2Frame frame = null;
			if ( frames.ContainsKey( frameType ) )
			{
				frame = frames[ frameType ];
				
				FrameComposite composite = frame as FrameComposite;
				if ( composite != null )
				{
					frame = composite[ 0 ] as ID3v2Frame;
				}
			}

			return frame;
		}
		#endregion

		#region Frame Accessors
		/// <summary>
		/// Gets a copy of the frames in this tag.
		/// </summary>
		/// <returns>A copy of the frames in this tag.</returns>
		public ReadOnlyCollection<ID3v2Frame> Frames
		{
			get
			{
				ReadOnlyCollection<ID3v2Frame> frameSet =
					new ReadOnlyCollection<ID3v2Frame>( new List<ID3v2Frame>( frames.Values ) );
				return frameSet;
			}
		}

		/// <summary>
		/// Adds a frame to the tag.
		/// </summary>
		/// <param name="newFrame">The frame to add.</param>
		public void AddFrame( ID3v2Frame newFrame )
		{
			if ( newFrame == null )
			{
				throw new ArgumentNullException( "newFrame" );
			}

			if ( frames.ContainsKey( newFrame.Type ) )
			{
				ID3v2Frame existingFrame = frames[ newFrame.Type ];
				FrameComposite composite = existingFrame as FrameComposite;
				
				if ( composite == null )
				{
					composite = new FrameComposite( existingFrame.Type );
					composite.AddFrame( existingFrame );

					frames.Remove( newFrame.Type );
					frames.Add( composite.Type, composite );
				}

				composite.AddFrame( newFrame );
			}
			else
			{
				frames.Add( newFrame.Type, newFrame );
			}
		}

		/// <summary>
		/// Removes a given frame from the tag.
		/// </summary>
		/// <param name="frame">The frame to remove.</param>
		public void RemoveFrame( ID3v2Frame frame )
		{
			if ( frame == null )
			{
				throw new ArgumentNullException( "frame" );
			}

			if ( frames.ContainsKey( frame.Type ) )
			{
				ID3v2Frame existingFrame = frames[ frame.Type ];
				FrameComposite composite = existingFrame as FrameComposite;

				if ( composite != null && composite != frame )
				{
					composite.RemoveFrame( frame );
				}
				else
				{
					frames.Remove( frame.Type );
				}
			}
		}

		/// <summary>
		/// Removes any frames of a given type from the tag.
		/// </summary>
		/// <param name="frameType">The frame type to remove.</param>
		public void RemoveFrame( FrameType frameType )
		{
			frames.Remove( frameType );
		}

		/// <summary>
		/// Returns true if the tag has any frames of the given type; false
		/// otherwise.
		/// </summary>
		/// <param name="frameType">The type of frame to check for.</param>
		/// <returns>True if the tag has any frames of the given type; false
		/// otherwise.</returns>
		public bool HasFrameType( FrameType frameType )
		{
			return frames.ContainsKey( frameType );
		}
		#endregion

		#region Tag-Writing Methods
		/// <summary>
		/// Writes the tag to a stream using the format specified by a particular ID3 version.
		/// Only V2_2, V2_3 and V2_4 are supported.
		/// </summary>
		/// <param name="stream">The stream to write the tag to.</param>
		/// <param name="version">The version to use. Only V2_2, V2_3 and V2_4 are supported.</param>
		public override void WriteTag( Stream stream, ID3Versions version )
		{
			if ( (version & ID3Versions.V2) != ID3Versions.V2 )
			{
				throw new UnsupportedVersionException( "Only versions 2.x are supported by this method.", version );
			}

			EnsureSpace( stream, version );
			stream.Position = 0;
			WriteHeader( stream, version );

			foreach ( ID3v2Frame frame in frames.Values )
			{
				try
				{
					frame.WriteToStream( stream, version );
				}
				catch ( UnsupportedVersionException )
				{
					//***
				}
			}
			
			WritePadding( stream );
		}

		#region Sizing Methods
		/// <summary>
		/// Examines the stream to see if there is sufficient space for the tag to be written.
		/// If there is not enough space, sufficient space is made.
		/// </summary>
		/// <param name="stream">The stream to check for space.</param>
		/// <param name="writeVersion">The version to use in calculating space required.</param>
		private void EnsureSpace( Stream stream, ID3Versions writeVersion )
		{
			ID3Versions existingTagVersion = ID3v2Tag.LookForTag( stream );
			if ( (existingTagVersion & ID3Versions.V2) == ID3Versions.V2 )
			{
				int existingSize = ReadTagSize( stream );
				if ( existingSize < this.GetTotalSize( writeVersion ) )
				{
					SmartPadding( stream, existingSize, writeVersion );
				}
				else
				{
					this.PaddingSize += existingSize - this.GetTotalSize( writeVersion );
				}
			}
			else
			{
				SmartPadding( stream, 0, writeVersion );
			}
		}

		/// <summary>
		/// Reads and returns the tag size from a stream.
		/// </summary>
		/// <param name="stream">The stream containing the tag.</param>
		/// <returns>The tag size from a stream.</returns>
		private static int ReadTagSize( Stream stream )
		{
			stream.Seek( TAG_SIZE_START, SeekOrigin.Begin );
			byte[] tagSizeBytes = new byte[ TAG_SIZE_LENGTH ];
			stream.Read( tagSizeBytes, 0, tagSizeBytes.Length );
			int tagSize = SynchsafeInteger.UnSynchsafe( tagSizeBytes );

			return tagSize + TAG_HEADER_LENGTH;
		}

		/// <summary>
		/// Examines the tag to see if the space available is sufficient for the tag
		/// to be written. If there is not enough space, sufficient space is made, 
		/// possibly including empty padding space.
		/// </summary>
		/// <param name="stream">The stream to be expanded.</param>
		/// <param name="tagSpaceAvailable">The number of bytes available for the tag.</param>
		/// <param name="version">The version of the tag to be written.</param>
		private void SmartPadding( Stream stream, int tagSpaceAvailable, ID3Versions version )
		{
			int tagSpaceNeeded = this.GetTotalSize( version ) - this.PaddingSize;
			int tagDiff = tagSpaceNeeded - tagSpaceAvailable;
			
			// current padding isn't enough - expand the file
			if ( tagDiff > 0 )
			{
				long minNeeded = stream.Length + tagDiff;
				// round out the cluster (4096 bytes)
				long newLength = ((minNeeded / 4096) + 1) * 4096;
				int lengthDiff = (int) (newLength - stream.Length);

				this.PaddingSize = lengthDiff - tagDiff;
				AddHeadSpace( stream, lengthDiff );
			}
		}

		/// <summary>
		/// Adds a specified number of empty bytes to the head of the specified stream.
		/// </summary>
		/// <param name="stream">The stream to expand.</param>
		/// <param name="increase">The number of bytes to add.</param>
		private void AddHeadSpace( Stream stream, int increase )
		{
			int bufLength;
			long startPosition;
			byte[] buffer;

			stream.Seek( 0, SeekOrigin.End );
			stream.SetLength( stream.Length + increase );
			while( stream.Position > 0 )
			{
				bufLength = 10000;
				if ( stream.Position - bufLength < increase )
				{
					bufLength = (int) stream.Position;
				}

				stream.Seek( - bufLength, SeekOrigin.Current );
				startPosition = stream.Position;
				buffer = new byte[ bufLength ];

				stream.Read( buffer, 0, bufLength );
				stream.Position = startPosition + increase;
				stream.Write( buffer, 0, bufLength );
				stream.Position = startPosition;
			}
			stream.Flush();
		}
		
		/// <summary>
		/// Writes padding (null bytes) to the stream to fill the space
		/// allocated to the tag.
		/// </summary>
		/// <param name="stream">The stream to be written to.</param>
		private void WritePadding( Stream stream )
		{
			byte[] padding = new byte[ paddingSize ];
			stream.Write( padding, 0, paddingSize );
		}
		#endregion

		/// <summary>
		/// Writes the tag header to the specified stream.
		/// </summary>
		/// <param name="stream">The stream to be written to.</param>
		/// <param name="version">The ID3v2 version of the tag to be written.</param>
		private void WriteHeader( Stream stream, ID3Versions version )
		{
			if ( (version & ID3Versions.V2) == ID3Versions.V2 )
			{
				stream.Write( StringToBytes( "ID3" ), 0, 3 );
				WriteVersion( stream, version );
				WriteFlags( stream, version );
				stream.Write(
					SynchsafeInteger.Synchsafe( this.GetFramesSize( version ) + this.PaddingSize ), 0, 4 );
				stream.Flush();
			}
			else
			{
				throw new UnsupportedVersionException( version );
			}
		}

		/// <summary>
		/// Writes the specified version to the specified stream.
		/// </summary>
		/// <param name="stream">The stream to be written to.</param>
		/// <param name="version">The ID3v2 version to be written.</param>
		private void WriteVersion( Stream stream, ID3Versions version )
		{
			switch ( version )
			{
				case ID3Versions.V2_2:
					stream.WriteByte( 0x02 );
					stream.WriteByte( 0x00 );
					break;

				case ID3Versions.V2_3:
					stream.WriteByte( 0x03 );
					stream.WriteByte( 0x00 );
					break;

				case ID3Versions.V2_4:
					stream.WriteByte( 0x04 );
					stream.WriteByte( 0x00 );
					break;

				default:
					throw new UnsupportedVersionException( version );
			}
		}

		/// <summary>
		/// Writes the flags to the specified stream.
		/// *** Flags are currently unsupported. ***
		/// </summary>
		/// <param name="stream">The stream to be written to.</param>
		/// <param name="version">The ID3v2 version of the tag to be written.</param>
		private void WriteFlags( Stream stream, ID3Versions version )
		{
			if ( (version & ID3Versions.V2) == ID3Versions.V2 )
			{
				//*** Doesn't support flags
				stream.WriteByte( 0x00 );
			}
			else
			{
				throw new UnsupportedVersionException( version );
			}
		}
		#endregion

		#region Tag-Reading Methods
		/// <summary>
		/// Searches for an ID3v2 tag in a file. Returns true if the file
		/// contains an ID3v2 tag; false otherwise.
		/// </summary>
		/// <param name="filename">The name of a file to search.</param>
		/// <returns>True if the file contains an ID3v2 tag; false otherwise.</returns>
		public static bool HasTag( string filename )
		{
			ID3Versions version = LookForTag( filename );
			return ((version & ID3Versions.V2) == ID3Versions.V2);
		}

		/// <summary>
		/// Searches for an ID3v2 tag in a stream. Returns true if the stream
		/// contains an ID3v2 tag; false otherwise.
		/// </summary>
		/// <param name="stream">A Stream to search.</param>
		/// <returns>True if stream contains an ID3v2 tag; false otherwise.</returns>
		public static bool HasTag( Stream stream )
		{
			ID3Versions version = LookForTag( stream );
			return ((version & ID3Versions.V2) == ID3Versions.V2);
		}

		/// <summary>
		/// Returns the version of any ID3v2 tag found in a file.
		/// </summary>
		/// <param name="filename">The name of a file to search.</param>
		/// <returns>The version of any ID3v2 tag found in a file.</returns>
		public static ID3Versions LookForTag( string filename )
		{
			using( Stream stream = new FileStream( filename, FileMode.Open, FileAccess.Read ) )
			{
				return LookForTag( stream );
			}
		}

		/// <summary>
		/// Returns the version of any ID3v2 tag found in a stream.
		/// </summary>
		/// <param name="stream">The stream to search.</param>
		/// <returns>The version of any ID3v2 tag found in a stream.</returns>
		public static ID3Versions LookForTag( Stream stream )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			ID3Versions tagVersion;

			byte[] tagHeader = new byte[TAG_HEADER_LENGTH];
			// look at the start of the stream for the tag.
			stream.Seek( 0, SeekOrigin.Begin );
			stream.Read( tagHeader, 0, tagHeader.Length );

			// Pattern for finding ID3v2 tags, as defined in ID3v2 specs
			if ( tagHeader[0] == (byte) 'I' && tagHeader[1] == (byte) 'D' && tagHeader[2] == (byte) '3' &&
				tagHeader[3] < 0xFF && tagHeader[4] < 0xFF &&
				tagHeader[6] < 0x80 && tagHeader[7] < 0x80 && tagHeader[8] < 0x80 && tagHeader[9] < 0x80 )
			{
				switch ( tagHeader[3] )
				{
					case 0x2:
						tagVersion = ID3Versions.V2_2;
						break;
					case 0x3:
						tagVersion = ID3Versions.V2_3;
						break;
					case 0x4:
						tagVersion = ID3Versions.V2_4;
						break;
					default:
						tagVersion = ID3Versions.Unknown;
						break;
				}
			}
			else
			{
				tagVersion = ID3Versions.None;
			}

			return tagVersion;
		}

		/// <summary>
		/// Reads an ID3v2 tag from a file. Returns null if no ID3v2 tag can be found
		/// in the file.
		/// </summary>
		/// <param name="filename">The name of the file to read.</param>
		/// <returns>The tag read from the file, or null if no ID3v2 tag was present.</returns>
		public static ID3v2Tag ReadTag( string filename )
		{
			using( Stream stream = new FileStream( filename, FileMode.Open, FileAccess.Read ) )
			{
				return ReadTag( stream );
			}
		}

		/// <summary>
		/// Reads an ID3v2 tag from a stream. Returns null if no ID3v2 tag can be found
		/// in the stream.
		/// </summary>
		/// <param name="stream">The stream to read.</param>
		/// <returns>The tag read from the stream, or null if no ID3v2 tag can be found.</returns>
		public static ID3v2Tag ReadTag( Stream stream )
		{
			ID3Versions version = LookForTag( stream );
			ID3v2Tag tag = null;
			
			if ( (version & ID3Versions.V2) == ID3Versions.V2 )
			{
				tag = new ID3v2Tag();
				
				tag.headerFlags = ReadFlags( stream, version );
			
				int tagSize = ReadTagSize( stream );

				// Go to the first byte after the header
				stream.Seek( TAG_HEADER_LENGTH, SeekOrigin.Begin );

				while ( stream.Position < tagSize )
				{
					ID3v2Frame newFrame = ID3v2Frame.ReadFrame( stream, version );
					if ( newFrame != null )
					{
						tag.AddFrame( newFrame );
					}
					else
					{
						break;
					}
				}
			}

			return tag;
		}

		#region Flag-Reading Methods
		/// <summary>
		/// Reads and returns the tag header flags from a stream.
		/// </summary>
		/// <param name="stream">The stream to read.</param>
		/// <param name="version">The version of the ID3v2 tag being read.</param>
		/// <returns>The tag header flags read from the stream.</returns>
		private static TagHeaderFlagsV2 ReadFlags( Stream stream, ID3Versions version )
		{
			TagHeaderFlagsV2 flags = TagHeaderFlagsV2.None;

			stream.Seek( TAG_FLAGS_POSITION, SeekOrigin.Begin );
			switch ( version )
			{
				case ID3Versions.V2_2:
					flags = ConvertFlags( (TagHeaderFlagsV2_2) stream.ReadByte() );
					break;
				case ID3Versions.V2_3:
					flags = ConvertFlags( (TagHeaderFlagsV2_3) stream.ReadByte() );
					break;
				case ID3Versions.V2_4:
					flags = ConvertFlags( (TagHeaderFlagsV2_4) stream.ReadByte() );
					break;
				default:
					throw new UnsupportedVersionException( version );
			}

			return flags;
		}

		/// <summary>
		/// Converts a version-specific flag value to the general form.
		/// </summary>
		/// <param name="flags2_4">The ID3v2.4 flags to convert.</param>
		/// <returns>The converted TagHeaderFlagsV2 value.</returns>
		private static TagHeaderFlagsV2 ConvertFlags( TagHeaderFlagsV2_4 flags2_4 )
		{
			TagHeaderFlagsV2 flags = TagHeaderFlagsV2.None;

			if ( HasFlag( flags2_4, TagHeaderFlagsV2_4.Unsynchronization ) )
			{
				flags |= TagHeaderFlagsV2.Unsynchronization;
			}
			if ( HasFlag( flags2_4, TagHeaderFlagsV2_4.ExtendedHeader ) )
			{
				flags |= TagHeaderFlagsV2.ExtendedHeader;
			}
			if ( HasFlag( flags2_4, TagHeaderFlagsV2_4.ExperimentalIndicator ) )
			{
				flags |= TagHeaderFlagsV2.ExperimentalIndicator;
			}
			if ( HasFlag( flags2_4, TagHeaderFlagsV2_4.FooterPresent ) )
			{
				flags |= TagHeaderFlagsV2.FooterPresent;
			}

			return flags;
		}

		/// <summary>
		/// Converts a version-specific flag value to the general form.
		/// </summary>
		/// <param name="flags2_3">The ID3v2.3 flags to convert.</param>
		/// <returns>The converted TagHeaderFlagsV2 value.</returns>
		private static TagHeaderFlagsV2 ConvertFlags( TagHeaderFlagsV2_3 flags2_3 )
		{
			TagHeaderFlagsV2 flags = TagHeaderFlagsV2.None;

			if ( HasFlag( flags2_3, TagHeaderFlagsV2_3.Unsynchronization ) )
			{
				flags |= TagHeaderFlagsV2.Unsynchronization;
			}
			if ( HasFlag( flags2_3, TagHeaderFlagsV2_3.ExtendedHeader ) )
			{
				flags |= TagHeaderFlagsV2.ExtendedHeader;
			}
			if ( HasFlag( flags2_3, TagHeaderFlagsV2_3.ExperimentalIndicator ) )
			{
				flags |= TagHeaderFlagsV2.ExperimentalIndicator;
			}

			return flags;
		}

		/// <summary>
		/// Converts a version-specific flag value to the general form.
		/// </summary>
		/// <param name="flags2_2">The ID3v2.2 flags to convert.</param>
		/// <returns>The converted TagHeaderFlagsV2 value.</returns>
		private static TagHeaderFlagsV2 ConvertFlags( TagHeaderFlagsV2_2 flags2_2 )
		{
			TagHeaderFlagsV2 flags = TagHeaderFlagsV2.None;

			if ( HasFlag( flags2_2, TagHeaderFlagsV2_2.Unsynchronization ) )
			{
				flags |= TagHeaderFlagsV2.Unsynchronization;
			}
			if ( HasFlag( flags2_2, TagHeaderFlagsV2_2.Compression ) )
			{
				flags |= TagHeaderFlagsV2.Compression;
			}

			return flags;
		}

		/// <summary>
		/// Returns true if the flag set contains the specified flag; false otherwise.
		/// </summary>
		/// <param name="flagSet">The flag set to check.</param>
		/// <param name="flagToCheck">The desired flag.</param>
		/// <returns>True if the flag set contains the specified flag; false otherwise.</returns>
		private static bool HasFlag( TagHeaderFlagsV2_4 flagSet, TagHeaderFlagsV2_4 flagToCheck )
		{
			return ( (flagSet & flagToCheck) == flagToCheck );
		}

		/// <summary>
		/// Returns true if the flag set contains the specified flag; false otherwise.
		/// </summary>
		/// <param name="flagSet">The flag set to check.</param>
		/// <param name="flagToCheck">The desired flag.</param>
		/// <returns>True if the flag set contains the specified flag; false otherwise.</returns>
		private static bool HasFlag( TagHeaderFlagsV2_3 flagSet, TagHeaderFlagsV2_3 flagToCheck )
		{
			return ( (flagSet & flagToCheck) == flagToCheck );
		}

		/// <summary>
		/// Returns true if the flag set contains the specified flag; false otherwise.
		/// </summary>
		/// <param name="flagSet">The flag set to check.</param>
		/// <param name="flagToCheck">The desired flag.</param>
		/// <returns>True if the flag set contains the specified flag; false otherwise.</returns>
		private static bool HasFlag( TagHeaderFlagsV2_2 flagSet, TagHeaderFlagsV2_2 flagToCheck )
		{
			return ( (flagSet & flagToCheck) == flagToCheck );
		}
		#endregion
		#endregion

		#region Tag Removal Methods
		/// <summary>
		/// Removes any ID3v2 tag from a file.
		/// </summary>
		/// <param name="filename">The name of the file whose ID3v2 tag will be removed.</param>
		public static void RemoveTag( string filename )
		{
			using ( FileStream stream = new FileStream( filename, FileMode.Open, FileAccess.ReadWrite ) )
			{
				RemoveTag( stream );
			}
		}

		/// <summary>
		/// Removes any ID3v2 tag from a stream.
		/// </summary>
		/// <param name="stream">The stream whose ID3v2 tag will be removed.</param>
		public static void RemoveTag( Stream stream )
		{
			ID3Versions version = ID3v2Tag.LookForTag( stream );
			if ( (version & ID3Versions.V2) == ID3Versions.V2 )
			{
				int tagSize = ReadTagSize( stream );
				long newSize = stream.Length - tagSize;
				
				int bufLength;
				long startPosition;
				byte[] buffer;

				stream.Seek( tagSize, SeekOrigin.Begin );
				while( stream.Position < stream.Length )
				{
					bufLength = 10000;
					if ( stream.Position + bufLength > stream.Length )
					{
						bufLength = (int) (stream.Length - stream.Position);
					}

					buffer = new byte[ bufLength ];

					stream.Read( buffer, 0, bufLength );
					startPosition = stream.Position;
					stream.Position = startPosition - (tagSize + bufLength);
					stream.Write( buffer, 0, bufLength );
					stream.Position = startPosition;
				}
				stream.Flush();
				stream.SetLength( newSize );
			}
			else
			{
				throw new TagNotFoundException( "No tag found" );
			}
		}
		#endregion
	}

	#region Tag Header Flag enums
	/// <summary>
	/// The set of ID3v2.2 tag header flags.
	/// </summary>
	[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
	[SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
	[Flags]
	public enum TagHeaderFlagsV2_2 : byte
	{ 
		/// <summary>Bit 7</summary>
		Unsynchronization = 128,
		/// <summary>Bit 6</summary>
		Compression = 64,
		/// <summary>No flags set.</summary>
		None = 0
	};

	/// <summary>
	/// The set of ID3v2.3 tag header flags.
	/// </summary>
	[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
	[SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
	[Flags]
	public enum TagHeaderFlagsV2_3 : byte
	{ 
		/// <summary>Bit 7</summary>
		Unsynchronization = 128,
		/// <summary>Bit 6</summary>
		ExtendedHeader = 64,
		/// <summary>Bit 5</summary>
		ExperimentalIndicator = 32,
		/// <summary>No flags set.</summary>
		None = 0
	};

	/// <summary>
	/// The set of ID3v2.4 tag header flags.
	/// </summary>
	[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
	[SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )]
	[Flags]
	public enum TagHeaderFlagsV2_4 : byte
	{
		/// <summary>Bit 7</summary>
		Unsynchronization = 128,
		/// <summary>Bit 6</summary>
		ExtendedHeader = 64,
		/// <summary>Bit 5</summary>
		ExperimentalIndicator = 32,
		/// <summary>Bit 4</summary>
		FooterPresent = 16,
		/// <summary>No flags set.</summary>
		None = 0
	};

	/// <summary>
	/// The set of tag headers flags across all versions of ID3v2.
	/// </summary>
	[Flags]
	public enum TagHeaderFlagsV2 : int
	{
		/// <summary>Unsynchronization flag (2.2-2.4) set.</summary>
		Unsynchronization = 128,
		/// <summary>Extended Header flag (2.3-2.4) set.</summary>
		ExtendedHeader = 64,
		/// <summary>Experimental Indicator flag (2.3-2.4) set.</summary>
		ExperimentalIndicator = 32,
		/// <summary>Footer Present flag (2.4) set.</summary>
		FooterPresent = 16,
		/// <summary>Compression flag (2.2) set.</summary>
		Compression = 1,
		/// <summary>No flags set.</summary>
		None = 0
	};
	#endregion
}
