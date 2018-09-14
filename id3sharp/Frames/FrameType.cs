#region CVS Log
/*
 * Version:
 *   $Id: FrameType.cs,v 1.3 2004/11/10 06:51:55 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: FrameType.cs,v $
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

namespace ID3Sharp.Frames
{
	/// <summary>
	/// Enumeration of ID3v2 frame types.
	/// </summary>
	public enum FrameType
	{
		/// <summary>Unknown frame type.</summary>
		Unknown,

		// v2.4 Frames
		
		/// <summary>UFID (2.3, 2.4), UFI (2.2). Unique File ID.</summary>
		UniqueFileIdentifier,
		/// <summary>TIT1 (2.3, 2.4), TT1 (2.2).</summary>
		ContentGroupDescription,
		/// <summary>TIT2 (2.3, 2.4), TT2 (2.2). "Title" frame.</summary>
		Title,
		/// <summary>TIT3 (2.3, 2.4), TT3 (2.2).</summary>
		Subtitle,
		/// <summary>TALB (2.3, 2.4), TAL (2.2). "Album" frame.</summary>
		AlbumTitle,
		/// <summary>TOAL (2.3, 2.4), TOT (2.2).</summary>
		OriginalAlbumTitle,
		/// <summary>TRCK (2.3, 2.4), TRK (2.2). "Track Number" frame.</summary>
		TrackNumber,
		/// <summary>TPOS (2.3, 2.4), TPA (2.2).</summary>
		PartOfASet,
		/// <summary>TSST (2.4).</summary>
		SetSubtitle,
		/// <summary>TSRC (2.3, 2.4), TRC (2.2).</summary>
		InternationalStandardRecordingCode,
		/// <summary>TPE1 (2.3, 2.4), TP1 (2.2). "Artist" frame.</summary>
		LeadArtist,
		/// <summary>TPE2 (2.3, 2.4), TP2 (2.2).</summary>
		Band,
		/// <summary>TPE3 (2.3, 2.4), TP3 (2.2).</summary>
		Conductor,
		/// <summary>TPE4 (2.3, 2.4), TP4 (2.2).</summary>
		ModifiedBy,
		/// <summary>TOPE (2.3, 2.4), TOA (2.2).</summary>
		OriginalArtist,
		/// <summary>TEXT (2.3, 2.4), TXT (2.2). "Original Artist" frame.</summary>
		Lyricist,
		/// <summary>TOLY (2.3, 2.4), TOL (2.2).</summary>
		OriginalLyricist,
		/// <summary>TCOM (2.3, 2.4), TCM (2.2). "Composer" frame.</summary>
		Composer,
		/// <summary>TMCL (2.4).</summary>
		MusicianCreditsList,
		/// <summary>TIPL (2.4).</summary>
		InvolvedPeopleList2,
		/// <summary>TENC (2.3, 2.4), TEN (2.2). "Encoded By" frame.</summary>
		EncodedBy,
		/// <summary>TBPM (2.3, 2.4), TBP (2.2).</summary>
		BeatsPerMinute,
		/// <summary>TLEN (2.3, 2.4), TLE (2.2).</summary>
		Length,
		/// <summary>TKEY (2.3, 2.4), TKE (2.2).</summary>
		InitialKey,
		/// <summary>TLAN (2.3, 2.4), TLA (2.2).</summary>
		Language,
		/// <summary>TCON (2.3, 2.4), TCO (2.2). "Genre" frame.</summary>
		ContentType,
		/// <summary>TFLT (2.3, 2.4), TFT (2.2).</summary>
		FileType,
		/// <summary>TMED (2.3, 2.4), TMT (2.2).</summary>
		MediaType,
		/// <summary>TMOO (2.4).</summary>
		Mood,
		/// <summary>TCOP (2.3, 2.4), TCR (2.2).</summary>
		CopyrightMessage,
		/// <summary>TPRO (2.4).</summary>
		ProducedNotice,
		/// <summary>TPUB (2.3, 2.4), TPB (2.2).</summary>
		Publisher,
		/// <summary>TOWN (2.3, 2.4).</summary>
		FileOwner,
		/// <summary>TRSN (2.3, 2.4).</summary>
		InternetRadioStationName,
		/// <summary>TRSO (2.3, 2.4).</summary>
		InternetRadioStationOwner,
		/// <summary>TOFN (2.3, 2.4), TOF (2.2).</summary>
		OriginalFilename,
		/// <summary>TDLY (2.3, 2.4), TDY (2.2).</summary>
		PlaylistDelay,
		/// <summary>TDEN (2.4)).</summary>
		EncodingTime,
		/// <summary>TDOR (2.4).</summary>
		OriginalReleaseTime,
		/// <summary>TDRC (2.4).</summary>
		RecordingTime,
		/// <summary>TDRL (2.4).</summary>
		ReleaseTime,
		/// <summary>TDTG (2.4).</summary>
		TaggingTime,
		/// <summary>TSSE (2.3, 2.4), TSS (2.2).</summary>
		SoftwareHardwareAndSettingsUsedForEncoding,
		/// <summary>TSOA (2.4).</summary>
		AlbumSortOrder,
		/// <summary>TSOP (2.4).</summary>
		PerformerSortOrder,
		/// <summary>TSOT (2.4).</summary>
		TitleSortOrder,
		/// <summary>TXXX (2.3, 2.4), TXX (2.2).</summary>
		UserDefinedText,
		/// <summary>WCOM (2.3, 2.4), WCM (2.2).</summary>
		CommercialInformation,
		/// <summary>WCOP (2.3, 2.4), WCP (2.2).</summary>
		CopyrightLegalInformation,
		/// <summary>WOAF (2.3, 2.4), WAF (2.2).</summary>
		OfficialAudioFileWebpage,
		/// <summary>WOAR (2.3, 2.4), WAR (2.2).</summary>
		OfficialArtistWebpage,
		/// <summary>WOAS (2.3, 2.4), WAS (2.2).</summary>
		OfficialAudioSourceWebpage,
		/// <summary>WORS (2.3, 2.4).</summary>
		OfficialInternetRadioStationHomepage,
		/// <summary>WPAY (2.3, 2.4).</summary>
		Payment,
		/// <summary>WPUB (2.3, 2.4), WPB (2.2).</summary>
		PublishersOfficialWebpage,
		/// <summary>WXXX (2.3, 2.4), WXX (2.2).</summary>
		UserDefinedUrlLink,
		/// <summary>MCDI (2.3, 2.4), MCI (2.2).</summary>
		MusicCDIdentifier,
		/// <summary>ETCO (2.3, 2.4), ETC (2.2).</summary>
		EventTimingCodes,
		/// <summary>MLLT (2.3, 2.4), MLL (2.2).</summary>
		MpegLocationLookupTable,
		/// <summary>SYTC (2.3, 2.4), STC (2.2).</summary>
		SynchronizedTempoCodes,
		/// <summary>USLT (2.3, 2.4), ULT (2.2).</summary>
		UnsynchronizedLyrics,
		/// <summary>SYLT (2.3, 2.4), SLT (2.2).</summary>
		SynchronizedLyrics,
		/// <summary>COMM (2.3, 2.4), COM (2.2). "Comment" frame.</summary>
		Comments,
		/// <summary>RVA2 (2.3, 2.4).</summary>
		RelativeVolumeAdjustment2,
		/// <summary>EQU2 (2.4).</summary>
		Equalization2,
		/// <summary>RVRB (2.3, 2.4), REV (2.2).</summary>
		Reverb,
		/// <summary>APIC (2.3, 2.4), PIC (2.2).</summary>
		AttachedPicture,
		/// <summary>GEOB (2.3, 2.4), GEO (2.2). Encapsulated object.</summary>
		GeneralEncapsulatedObject,
		/// <summary>PCNT (2.3, 2.4), CNT (2.2).</summary>
		PlayCounter,
		/// <summary>POPM (2.3, 2.4), POP (2.2).</summary>
		Popularimeter,
		/// <summary>RBUF (2.3, 2.4), BUF (2.2).</summary>
		RecommendedBufferSize,
		/// <summary>AENC (2.3, 2.4), CRA (2.2).</summary>
		AudioEncryption,
		/// <summary>LINK (2.3, 2.4), LNK (2.2).</summary>
		LinkedInformation,
		/// <summary>POSS (2.3, 2.4).</summary>
		PositionSynchronization,
		/// <summary>USER (2.3, 2.4).</summary>
		TermsOfUse,
		/// <summary>OWNE (2.3, 2.4).</summary>
		Ownership,
		/// <summary>COMR (2.3, 2.4).</summary>
		Commercial,
		/// <summary>ENCR (2.3, 2.4).</summary>
		EncryptionMethodRegistraion,
		/// <summary>GRID (2.3, 2.4).</summary>
		GroupIdentificationRegistration,
		/// <summary>PRIV (2.3, 2.4). Private data.</summary>
		Private,
		/// <summary>SIGN (2.4).</summary>
		Signature,
		/// <summary>SEEK (2.4).</summary>
		Seek,
		/// <summary>ASPI (2.4).</summary>
		AudioSeekPointIndex,

		//v2.3 Frames

		/// <summary>EQUA (2.3), EQU (2.2).</summary>
		Equalization,
		/// <summary>IPLS (2.3), IPL (2.2).</summary>
		InvolvedPeopleList,
		/// <summary>RVAD (2.3), RVA (2.2).</summary>
		RelativeVolumeAdjustment,
		/// <summary>TDAT (2.3), TDA (2.2).</summary>
		Date,
		/// <summary>TIME (2.3), TIM (2.2).</summary>
		Time,
		/// <summary>TORY (2.3), TOR (2.2).</summary>
		OriginalReleaseYear,
		/// <summary>TRDA (2.3), TRD (2.2).</summary>
		RecordingDates,
		/// <summary>TSIZ (2.3), TSI (2.2).</summary>
		Size,
		/// <summary>TYER (2.3), TYE (2.2). "Year" frame.</summary>
		Year,

		//v2.2 Frames

		/// <summary>CRM (2.2).</summary>
		EncryptedMetaFrame
	}
}
