#region CVS Log
/*
 * Version:
 *   $Id: FrameComposite.cs,v 1.7 2004/11/16 07:08:14 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: FrameComposite.cs,v $
 *   Revision 1.7  2004/11/16 07:08:14  cwoodbury
 *   Changed accessibility modifiers for some methods to internal or
 *   protected internal where appropriate.
 *
 *   Revision 1.6  2004/11/10 07:32:22  cwoodbury
 *   Factored out ParseFrameData() into ID3v2Frame.
 *
 *   Revision 1.5  2004/11/10 06:51:55  cwoodbury
 *   Hid CVS log messages away in #region
 *
 *   Revision 1.4  2004/11/10 04:54:03  cwoodbury
 *   Added error checks to AddFrame and RemoveFrame.
 *
 *   Revision 1.3  2004/11/10 04:44:16  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.2  2004/11/03 06:52:02  cwoodbury
 *   Fixed bug with size
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
using System.IO;
using System.Collections.Generic;

namespace ID3Sharp.Frames
{
	/// <summary>
	/// A group of frames with the same frame type.
	/// </summary>
	public class FrameComposite : ID3v2Frame
	{
		#region Fields
		/// <summary>
		/// The frames contained in this composite.
		/// </summary>
		private List<ID3v2Frame> frames;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new FrameComposite for a given frame type.
		/// </summary>
		/// <param name="frameType"></param>
		protected internal FrameComposite( FrameType frameType )
		{
			this.Type = frameType;
			frames = new List<ID3v2Frame>();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="frame">Frame to copy.</param>
		protected internal FrameComposite( FrameComposite frame )
			: base( frame )
		{
			this.frames = new List<ID3v2Frame>();
			foreach ( ID3v2Frame childFrame in frame.frames )
			{
				this.frames.Add( childFrame.Copy() );
			}
		}
		#endregion

		/// <summary>
		/// Throws an exception. ParseFrameData cannot be called on objects of type FrameComposite.
		/// </summary>
		/// <param name="frameData">Invalid.</param>
		/// <param name="version">Invalid.</param>
		protected override void ParseFrameData( byte[] frameData, ID3Versions version )
		{
			throw new FrameCompositeException( "ParseFrameData cannot be called on " +
				"objects of type FrameComposite." );
		}

		#region Properties
		/// <summary>
		/// Gets the size (in bytes, not including headers) of all the frames
		/// contained in this composite.
		/// </summary>
		public override int Size
		{
			get
			{
				int size = 0;
				foreach ( ID3v2Frame frame in frames )
				{
					size += frame.Size;
				}
				return size;
			}
		}

		/// <summary>
		/// Gets the number of frames contained in this composite.
		/// </summary>
		public virtual int FrameCount
		{
			get
			{
				return frames.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual ID3v2Frame this[ int index ]
		{
			get
			{
				return frames[ index ];
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Gets the total size (in bytes, including headers) of all the frames contained in
		/// this composite.
		/// </summary>
		/// <param name="version">The format to be used in determing the frame size.</param>
		/// <returns>The total size (including headers) of all the frames contained in
		/// this composite.</returns>
		public override int GetTotalSize( ID3Versions version )
		{
			int size = 0;
			foreach ( ID3v2Frame frame in frames )
			{
				size += frame.GetTotalSize( version );
			}
			return size;
		}

		/// <summary>
		/// Adds a frame to the composite.
		/// </summary>
		/// <param name="newFrame">The frame to be added.</param>
		public virtual void AddFrame( ID3v2Frame newFrame )
		{
			if ( newFrame == null )
			{
				throw new ArgumentNullException( "newFrame" );
			}

			if ( newFrame.Type == this.Type )
			{
				frames.Add( newFrame );
			}
			else
			{
				throw new FrameCompositeException( "FrameType of frame to be " +
					"added does not match FrameType of composite" );
			}
		}

		/// <summary>
		/// Removes a frame from the composite.
		/// </summary>
		/// <param name="frame">The frame to be removed.</param>
		public virtual void RemoveFrame( ID3v2Frame frame )
		{
			if ( frames.Contains( frame ) )
			{
				frames.Remove( frame );
			}
			else
			{
				throw new FrameCompositeException( "Composite does not contain " +
					"frame to be removed" );
			}
		}


		/// <summary>
		/// Returns a deep copy of this composite frame. Supports the prototype design pattern.
		/// </summary>
		/// <returns>A copy of this frame, with copies of all composited frames.</returns>
		public override ID3v2Frame Copy()
		{
			return new FrameComposite( this );
		}

		/// <summary>
		/// Throws an exception. Objects of type FrameComposite cannot be initialized.
		/// </summary>
		/// <param name="flags">Invalid.</param>
		/// <param name="frameData">Invalid.</param>
		/// <param name="version">Invalid.</param>
		public override void Initialize( byte[] flags, byte[] frameData, ID3Versions version )
		{
			throw new FrameCompositeException( "Objects of type FrameComposite " +
				"cannot be initialized." );
		}

		/// <summary>
		/// Writes the composited frames to a stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="version">The ID3v2 version to use in writing the frame.</param>
		public override void WriteToStream( Stream stream, ID3Versions version )
		{
			foreach ( ID3v2Frame frame in frames )
			{
				frame.WriteToStream( stream, version );
			}
		}

		public override void Validate( ID3Versions version )
		{
			foreach ( ID3v2Frame frame in frames )
			{
				frame.Validate( version );
			}
		}
		#endregion
	}
}
