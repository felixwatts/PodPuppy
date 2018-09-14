/*
 * Version:
 *   $Id: APICPictureType.cs,v 1.1 2004/11/03 01:18:50 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: APICPictureType.cs,v $
 *   Revision 1.1  2004/11/03 01:18:50  cwoodbury
 *   Added to ID3Sharp
 *
 *
 * 
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

namespace ID3Sharp.Frames
{
	public enum APICPictureType : byte
	{
		Other=0,
		PNGFileIcon=1,
		OtherFileIcon=2,
		FrontCover=3,
		BackCover=4,
		LeafletPage=5,
		Media=6,
		LeadArtist=7,
		Artist=8,
		Conductor=9,
		Band=10,
		Composer=11,
		Lyricist=12,
		RecordingLocation=13,
		DuringRecording=14,
		DuringPerformance=15,
		VideoScreenCapture=16,
		BrightColoredFish=17,
		Illustration=18,
		BandLogo=19,
		PublisherLogo=20
	}
}
