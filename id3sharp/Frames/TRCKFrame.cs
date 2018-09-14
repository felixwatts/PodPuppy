#region CVS Log
/*
 * Version:
 *   $Id: TRCKFrame.cs,v 1.5 2004/12/10 04:49:08 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: TRCKFrame.cs,v $
 *   Revision 1.5  2004/12/10 04:49:08  cwoodbury
 *   Made changes to EncodedString and to how it is used to push it down to
 *   just being involved with frame I/O and not otherwise being used in frames.
 *
 *   Revision 1.4  2004/11/21 19:22:18  cwoodbury
 *   Added exception handling to string-to-int conversion.
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

using ID3Sharp.IO;

namespace ID3Sharp.Frames
{
	/// <summary>
	/// A Track Number/Position in Set frame.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased" )]
	public class TRCKFrame : TextInformationFrame
	{
		#region Constructors
		/// <summary>
		/// Creates a new TRCKFrame.
		/// </summary>
		protected internal TRCKFrame()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="frame">Frame to copy.</param>
		protected internal TRCKFrame( TRCKFrame frame ) 
			: base( frame )
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets and sets the contents of the Track Number field.
		/// </summary>
		public int TrackNumber
		{
			get
			{
				int trackNumber = -1;
				if ( ! String.IsNullOrEmpty( base.Text ) )
				{
					string[] parts = base.Text.Split( '\\', '/' );
					try 
					{
						trackNumber = Int32.Parse( parts[0] );
					}
					catch ( FormatException )
					{
						// Improperly formatted integer; not a big problem - we'll just return -1.
					}
				}

				return trackNumber;
			}
			set
			{
				if ( value < 1 )
				{
					base.Text = null;
				}
				else
				{
					if ( this.TotalTracks > 0 )
					{
						base.Text = value + "//" + this.TotalTracks;
					}
					else
					{
						base.Text = value.ToString();
					}
				}
			}
		}

		/// <summary>
		/// Gets and sets the (optional) total number of tracks/elements in the
		/// original recording.
		/// </summary>
		public int TotalTracks
		{
			get
			{
				int totalTracks = -1;
				if ( ! String.IsNullOrEmpty( base.Text ) )
				{
					string[] parts = base.Text.Split( '\\', '/' );
					if ( parts.Length > 1 )
					{
						try 
						{
							totalTracks = Int32.Parse( parts[1] );
						}
						catch ( FormatException )
						{
							// Improperly formatted integer; not a big problem - we'll just return -1.
						}
					}
				}

				return totalTracks;
			}
			set
			{
				if ( value >= this.TrackNumber && this.TrackNumber > 0 )
				{
					base.Text = this.TrackNumber + "/" + value;
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
			return new TRCKFrame( this );
		}
	}
}
