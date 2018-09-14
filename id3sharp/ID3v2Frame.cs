#region CVS Log
/*
 * Version:
 *   $Id: ID3v2Frame.cs,v 1.10 2004/11/20 23:12:13 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: ID3v2Frame.cs,v $
 *   Revision 1.10  2004/11/20 23:12:13  cwoodbury
 *   Removed TextEncodingType.ASCII type; replaced with ISO_8859_1 type
 *   or default type for EncodedString.
 *
 *   Revision 1.9  2004/11/16 07:09:49  cwoodbury
 *   Added wrappers to FrameRegistry's wrappers to simplify usage for clients.
 *
 *   Revision 1.8  2004/11/11 09:40:06  cwoodbury
 *   Fixed bug in frame-parsing.
 *
 *   Revision 1.7  2004/11/10 07:32:29  cwoodbury
 *   Factored out ParseFrameData() into ID3v2Frame.
 *
 *   Revision 1.6  2004/11/10 06:51:55  cwoodbury
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
 *   Revision 1.2  2004/11/03 06:49:17  cwoodbury
 *   Fixed bug with size
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
using System.Diagnostics.CodeAnalysis;

using ID3Sharp.Frames;
using ID3Sharp.IO;

namespace ID3Sharp
{
	/// <summary>
	/// An abstract ID3v2 frame.
	/// </summary>
	public abstract class ID3v2Frame
	{
		#region Constants
		/// <summary>A constant for the length (in bytes) of an ID3v2.2 frame header.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
		protected const int ID3V2_2FrameHeaderLength = 6;
		/// <summary>A constant for the length (in bytes) of an ID3v2.3 frame header.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		protected const int ID3V2_3FrameHeaderLength = 10;
		/// <summary>A constant for the length (in bytes) of an ID3v2.4 frame header.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		protected const int ID3V2_4FrameHeaderLength = 10;

		/// <summary>A constant for the length (in bytes) of an ID3v2.2 frame ID.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		protected const int ID3V2_2FrameIdFieldLength = 3;
		/// <summary>A constant for the length (in bytes) of an ID3v2.3 frame ID.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		protected const int ID3V2_3FrameIdFieldLength = 4;
		/// <summary>A constant for the length (in bytes) of an ID3v2.4 frame ID.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		protected const int ID3V2_4FrameIdFieldLength = 4;

		/// <summary>A constant for the length (in bytes) of an ID3v2.2 frame size field.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		protected const int ID3V2_2FrameSizeFieldLength = 3;
		/// <summary>A constant for the length (in bytes) of an ID3v2.3 frame size field.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		protected const int ID3V2_3FrameSizeFieldLength = 4;
		/// <summary>A constant for the length (in bytes) of an ID3v2.4 frame size field.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		protected const int ID3V2_4FrameSizeFieldLength = 4;

		/// <summary>A constant for the length (in bytes) of an ID3v2.2 flags field.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		protected const int ID3V2_2FrameFlagsField = 0;
		/// <summary>A constant for the length (in bytes) of an ID3v2.3 flags field.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		protected const int ID3V2_3FrameFlagsField = 2;
		/// <summary>A constant for the length (in bytes) of an ID3v2.4 flags field.</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
		protected const int ID3V2_4FrameFlagsField = 2;
		#endregion

		#region Fields
		/// <summary>
		/// The type of this frame.
		/// </summary>
		private FrameType frameType;
		/// <summary>
		/// The header flags for this frame.
		/// </summary>
		private FrameHeaderFlagsV2_4 headerFlags;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new ID3v2Frame with unknown frame type.
		/// </summary>
		protected ID3v2Frame()
		{
			frameType = FrameType.Unknown;
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="frame">Frame to copy.</param>
		protected ID3v2Frame( ID3v2Frame frame )
		{
			this.frameType = frame.frameType;
			this.headerFlags = frame.headerFlags;
		}
		#endregion

		#region Constructor/Initialize helper methods
		/// <summary>
		/// Parses the frame header flags.
		/// </summary>
		/// <param name="flagBytes">The frame header flags.</param>
		/// <param name="version">The ID3 version of the tag being parsed.</param>
		private void ParseFlags( byte[] flagBytes, ID3Versions version )
		{
			int frameStatus;
			int frameFormat;
			switch ( version )
			{
				case ID3Versions.V2_2:
					// do nothing - no flags in v2.2
					break;
				case ID3Versions.V2_3:
					frameStatus = (int) flagBytes[0];
					frameFormat = (int) flagBytes[1];
					frameStatus = frameStatus << 8;
					headerFlags = ConvertFlags( (FrameHeaderFlagsV2_3) frameStatus + frameFormat );
					break;
				case ID3Versions.V2_4:
					frameStatus = (int) flagBytes[0];
					frameFormat = (int) flagBytes[1];
					frameStatus = frameStatus << 8;
					headerFlags = (FrameHeaderFlagsV2_4) frameStatus + frameFormat;
					break;
				default:
					throw new UnsupportedVersionException( version );
			}
		}
		
		/// <summary>
		/// Parses the raw frame data.
		/// </summary>
		/// <param name="frameData">The raw frame data.</param>
		/// <param name="version">The ID3 version of the tag being parsed.</param>
		protected abstract void ParseFrameData( byte[] frameData, ID3Versions version );
		#endregion

		#region Factory Methods
		/// <summary>
		/// Returns the frame ID for the given FrameType and version.
		/// </summary>
		/// <param name="frameType">The FrameType to look up.</param>
		/// <param name="version">The ID3v2 version to use.</param>
		/// <returns>The frame ID for the given FrameType and version.</returns>
		public static string GetFrameId( FrameType frameType, ID3Versions version )
		{
			return FrameRegistry.GetFrameId( frameType, version );
		}

		/// <summary>
		/// Returns the FrameType for the given frame ID and version.
		/// </summary>
		/// <param name="frameId">The frame ID string to look up.</param>
		/// <param name="version">The ID3v2 version to use.</param>
		/// <returns>The frame ID for the given FrameType and version.</returns>
		public static FrameType GetFrameType( string frameId, ID3Versions version )
		{
			return FrameRegistry.GetFrameType( frameId, version );
		}
		
		/// <summary>
		/// Returns a new frame of the given type.
		/// </summary>
		/// <param name="frameType">The type of frame to return.</param>
		/// <returns>A new frame of the given type.</returns>
		public static ID3v2Frame GetNewFrame( FrameType frameType )
		{
			return FrameRegistry.GetNewFrame( frameType );
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets and sets the FrameType of this frame.
		/// </summary>
		public FrameType Type
		{
			get
			{
				return frameType;
			}
			set
			{
				frameType = value;
			}
		}

		/// <summary>
		/// Gets the size (in bytes, not including header) of the frame.
		/// </summary>
		public abstract int Size
		{
			get;
		}

		/// <summary>
		/// The header flags for this frame.
		/// </summary>
		public FrameHeaderFlagsV2_4 HeaderFlags
		{
			get
			{
				return headerFlags;
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Gets the total size (in bytes, including header) of this frame for the given
		/// ID3v2 version.
		/// </summary>
		/// <param name="version">The format to be used in determing the frame size.</param>
		/// <returns>The total size (including header) of this frame for the given
		/// ID3v2 version.</returns>
		public virtual int GetTotalSize( ID3Versions version )
		{
			int frameHeaderLength = 0;
			switch ( version )
			{
				case ID3Versions.V2_2:
					frameHeaderLength = ID3V2_2FrameHeaderLength;
					break;
				case ID3Versions.V2_3:
					frameHeaderLength = ID3V2_3FrameHeaderLength;
					break;
				case ID3Versions.V2_4:
					frameHeaderLength = ID3V2_4FrameHeaderLength;
					break;
			}

			return this.Size + frameHeaderLength;
		}

		/// <summary>
		/// Returns a copy of this frame. Supports the prototype design pattern.
		/// </summary>
		/// <returns>A copy of this frame.</returns>
		public abstract ID3v2Frame Copy();

		/// <summary>
		/// Initializes the frame. Supports the prototype design pattern.
		/// </summary>
		/// <param name="flags">The frame header flags for this frame.</param>
		/// <param name="frameData">The raw data in the frame's data section.</param>
		/// <param name="version">The ID3v2 version of the tag being parsed.</param>
		public virtual void Initialize( byte[] flags, byte[] frameData, ID3Versions version)
		{
			ParseFlags( flags, version );
			ParseFrameData( frameData, version );
		}
		#endregion

		#region Protected Methods
		public abstract void Validate( ID3Versions version );
		#endregion

		#region Frame-Writing Methods
		/// <summary>
		/// Writes the frame to a stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="version">The ID3v2 version to use in writing the frame.</param>
		public abstract void WriteToStream( Stream stream, ID3Versions version );

		/// <summary>
		/// Writes the header for this frame to a stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="version">The ID3v2 version to use in writing the frame.</param>
		protected void WriteHeaderToStream( Stream stream, ID3Versions version )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			if ( (version & ID3Versions.V2) == ID3Versions.V2 )
			{
				string frameIDString = FrameRegistry.GetFrameId( this.frameType, version );
				if ( frameIDString == null )
				{
					throw new UnsupportedVersionException( version );
				}

				EncodedString frameID = new EncodedString( TextEncodingType.ISO_8859_1, frameIDString );
				frameID.IsTerminated = false;
				frameID.HasEncodingTypePrepended = false;
				frameID.WriteToStream( stream );
				WriteFrameSize( stream, version );
				WriteFlags( stream, version );
				stream.Flush();
			}
			else
			{
				throw new UnsupportedVersionException( version );
			}
		}

		/// <summary>
		/// Writes the header flags for this frame to a stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="version">The ID3v2 version to use in writing the frame.</param>
		protected void WriteFlags( Stream stream, ID3Versions version )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			byte formatByte;
			byte statusByte;
			switch ( version )
			{
				case ID3Versions.V2_2:
					break;

				case ID3Versions.V2_3:
					FrameHeaderFlagsV2_3 flags2_3 = ConvertFlags( this.HeaderFlags );
					formatByte = (byte) flags2_3;
					statusByte = (byte) ( ((int) flags2_3) >> 8 );
					stream.WriteByte( statusByte );
					stream.WriteByte( formatByte );
					break;

				case ID3Versions.V2_4:
					formatByte = (byte) this.HeaderFlags;
					statusByte = (byte) ( ((int) this.HeaderFlags) >> 8 );
					stream.WriteByte( statusByte );
					stream.WriteByte( formatByte );
					break;

				default:
					throw new UnsupportedVersionException( version );
			}
		}

		/// <summary>
		/// Writes the size field for this frame to a stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="version">The ID3v2 version to use in writing the frame.</param>
		protected void WriteFrameSize( Stream stream, ID3Versions version )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			switch ( version )
			{
				case ID3Versions.V2_2:
					stream.Write( EncodedInteger.ToBytes( this.Size, ID3V2_2FrameSizeFieldLength ), 0, ID3V2_2FrameSizeFieldLength );
					break;

				case ID3Versions.V2_3:
					stream.Write( EncodedInteger.ToBytes( this.Size, ID3V2_3FrameSizeFieldLength ), 0, ID3V2_3FrameSizeFieldLength );
					break;

				case ID3Versions.V2_4:
					stream.Write( SynchsafeInteger.Synchsafe( this.Size ), 0, ID3V2_4FrameSizeFieldLength );
					break;

				default:
					throw new UnsupportedVersionException( version );
			}
		}
		#endregion

		#region Frame-Reading Methods
		/// <summary>
		/// Reads and returns a frame from a stream.
		/// </summary>
		/// <param name="stream">The stream to read from.</param>
		/// <param name="version">The ID3v2 version of the tag being parsed.</param>
		/// <returns>The frame read from the stream.</returns>
		public static ID3v2Frame ReadFrame( Stream stream, ID3Versions version )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			ID3v2Frame frame = null;
			FrameParameters parameters = new FrameParameters( version );

			byte[] header = new byte[ parameters.HeaderLength ];
			char[] idChars = new char[ parameters.IDLength ];
			byte[] sizeBytes = new byte[ parameters.SizeLength ];
			byte[] flags = new byte[ parameters.FlagsLength ];
			byte[] frameData;
			string frameID;
			int size;

			stream.Read( header, 0, header.Length );

			Array.Copy( header, 0, idChars, 0, idChars.Length );
			Array.Copy( header, parameters.IDLength, sizeBytes, 0, sizeBytes.Length );
			Array.Copy( header, parameters.IDLength + parameters.SizeLength, flags, 0, flags.Length );

			if ( idChars[0] != 0x0 )
			{
				frameID = new String( idChars );
				if ( parameters.SizeIsSynchSafe )
				{
					size = SynchsafeInteger.UnSynchsafe( sizeBytes );
				}
				else
				{
					size = EncodedInteger.ToInt( sizeBytes );
				}

				frameData = new byte[ size ];
				stream.Read( frameData, 0, frameData.Length );

				FrameType frameType = FrameRegistry.GetFrameType( frameID, version );
				frame = FrameRegistry.GetNewFrame( frameType );
			
				frame.Initialize( flags, frameData, version );
			}
			else
			{
				frame = null;
			}

			return frame;
		}
		#endregion

		#region Reading/Writing Helper Methods
		/// <summary>
		/// Converts ID3v2.4 frame header flags to ID3v2.3 frame header flags.
		/// WARNING: This may cause a loss of data.
		/// </summary>
		/// <param name="flags2_4">ID3v2.4 frame header flags to convert.</param>
		/// <returns>Converted ID3v2.3 frame header flags.</returns>
		private FrameHeaderFlagsV2_3 ConvertFlags( FrameHeaderFlagsV2_4 flags2_4 )
		{
			FrameHeaderFlagsV2_3 flags2_3 = FrameHeaderFlagsV2_3.None;

			if ( HasFlag( flags2_4, FrameHeaderFlagsV2_4.TagAlterPreservation ) )
			{
				flags2_3 |= FrameHeaderFlagsV2_3.TagAlterPreservation;
			}
			if ( HasFlag( flags2_4, FrameHeaderFlagsV2_4.FileAlterPreservation ) )
			{
				flags2_3 |= FrameHeaderFlagsV2_3.FileAlterPreservation;
			}
			if ( HasFlag( flags2_4, FrameHeaderFlagsV2_4.ReadOnly ) )
			{
				flags2_3 |= FrameHeaderFlagsV2_3.ReadOnly;
			}
			if ( HasFlag( flags2_4, FrameHeaderFlagsV2_4.Compression ) )
			{
				flags2_3 |= FrameHeaderFlagsV2_3.Compression;
			}
			if ( HasFlag( flags2_4, FrameHeaderFlagsV2_4.Encryption ) )
			{
				flags2_3 |= FrameHeaderFlagsV2_3.Encryption;
			}

			return flags2_3;
		}

		/// <summary>
		/// Converts ID3v2.3 frame header flags to ID3v2.4 frame header flags.
		/// </summary>
		/// <param name="flags2_3">ID3v2.3 frame header flags to convert.</param>
		/// <returns>Converted ID3v2.4 frame header flags.</returns>
		private FrameHeaderFlagsV2_4 ConvertFlags( FrameHeaderFlagsV2_3 flags2_3 )
		{
			FrameHeaderFlagsV2_4 flags2_4 = FrameHeaderFlagsV2_4.None;

			if ( HasFlag( flags2_3, FrameHeaderFlagsV2_3.TagAlterPreservation ) )
			{
				flags2_4 |= FrameHeaderFlagsV2_4.TagAlterPreservation;
			}
			if ( HasFlag( flags2_3, FrameHeaderFlagsV2_3.FileAlterPreservation ) )
			{
				flags2_4 |= FrameHeaderFlagsV2_4.FileAlterPreservation;
			}
			if ( HasFlag( flags2_3, FrameHeaderFlagsV2_3.ReadOnly ) )
			{
				flags2_4 |= FrameHeaderFlagsV2_4.ReadOnly;
			}
			if ( HasFlag( flags2_3, FrameHeaderFlagsV2_3.Compression ) )
			{
				flags2_4 |= FrameHeaderFlagsV2_4.Compression;
			}
			if ( HasFlag( flags2_3, FrameHeaderFlagsV2_3.Encryption ) )
			{
				flags2_4 |= FrameHeaderFlagsV2_4.Encryption;
			}

			return flags2_4;
		}

		/// <summary>
		/// Returns true if flagSet contains flagToCheck; false otherwise.
		/// </summary>
		/// <param name="flagSet">A set of flags.</param>
		/// <param name="flagToCheck">A flag to be checked for.</param>
		/// <returns>True if flagSet contains flagToCheck; false otherwise.</returns>
		private bool HasFlag( FrameHeaderFlagsV2_4 flagSet, FrameHeaderFlagsV2_4 flagToCheck )
		{
			return ( (flagSet & flagToCheck) == flagToCheck );
		}

		/// <summary>
		/// Returns true if flagSet contains flagToCheck; false otherwise.
		/// </summary>
		/// <param name="flagSet">A set of flags.</param>
		/// <param name="flagToCheck">A flag to be checked for.</param>
		/// <returns>True if flagSet contains flagToCheck; false otherwise.</returns>
		private bool HasFlag( FrameHeaderFlagsV2_3 flagSet, FrameHeaderFlagsV2_3 flagToCheck )
		{
			return ( (flagSet & flagToCheck) == flagToCheck );
		}
		#endregion

		private class FrameParameters
		{
			public int HeaderLength;
			public int IDLength;
			public int SizeLength;
			public int FlagsLength;
			public bool SizeIsSynchSafe;

			[SuppressMessage( "Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily" )]
			public FrameParameters( ID3Versions version )
			{
				switch ( version )
				{
					case ID3Versions.V2_2:
						HeaderLength = ID3V2_2FrameHeaderLength;
						IDLength = ID3V2_2FrameIdFieldLength;
						SizeLength = ID3V2_2FrameSizeFieldLength;
						FlagsLength = ID3V2_2FrameFlagsField;
						SizeIsSynchSafe = false;
						break;

					case ID3Versions.V2_3:
						HeaderLength = ID3V2_3FrameHeaderLength;
						IDLength = ID3V2_3FrameIdFieldLength;
						SizeLength = ID3V2_3FrameSizeFieldLength;
						FlagsLength = ID3V2_3FrameFlagsField;
						SizeIsSynchSafe = false;
						break;

					case ID3Versions.V2_4:
						HeaderLength = ID3V2_4FrameHeaderLength;
						IDLength = ID3V2_4FrameIdFieldLength;
						SizeLength = ID3V2_4FrameSizeFieldLength;
						FlagsLength = ID3V2_4FrameFlagsField;
						SizeIsSynchSafe = true;
						break;

					default:
						throw new UnsupportedVersionException( version );
				}
			}
		}
	}

	#region Frame Header Flag enums
	/// <summary>
	/// The set of ID3v2.3 frame header flags.
	/// </summary>
	[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
	[Flags]
	public enum FrameHeaderFlagsV2_3 : int
	{ 
		/// <summary>Frame status byte, bit 7</summary>
		TagAlterPreservation = 32768,
		/// <summary>Frame status byte, bit 6</summary>
		FileAlterPreservation = 16384,
		/// <summary>Frame status byte, bit 5</summary>
		ReadOnly = 8192,
		/// <summary>Frame format byte, bit 7</summary>
		Compression = 128,
		/// <summary>Frame format byte, bit 6</summary>
		Encryption = 64,
		/// <summary>Frame format byte, bit 5</summary>
		GroupingIdentity = 32,
		/// <summary>No flags set.</summary>
		None = 0
	};

	/// <summary>
	/// The set of ID3v2.4 frame header flags.
	/// </summary>
	[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
	[Flags]
	public enum FrameHeaderFlagsV2_4 : int
	{ 
		// Frame status flags
		/// <summary>Frame status byte, bit 6</summary>
		TagAlterPreservation = 16384,
		/// <summary>Frame status byte, bit 5</summary>
		FileAlterPreservation = 8192,
		/// <summary>Frame status byte, bit 4</summary>
		ReadOnly = 4096,
		// Frame format flags
		/// <summary>Frame format byte, bit 6</summary>
		GroupingIdentity = 64,
		/// <summary>Frame format byte, bit 3</summary>
		Compression = 8,
		/// <summary>Frame format byte, bit 2</summary>
		Encryption = 4,
		/// <summary>Frame format byte, bit 1</summary>
		Unsynchronization = 2,
		/// <summary>Frame format byte, bit 0</summary>
		DataLengthIndicator = 1,
		/// <summary>No flags set.</summary>
		None = 0
	};
	#endregion
}
