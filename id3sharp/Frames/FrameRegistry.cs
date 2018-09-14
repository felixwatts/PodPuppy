#region CVS Log
/*
 * Version:
 *   $Id: FrameRegistry.cs,v 1.4 2004/11/16 07:08:14 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: FrameRegistry.cs,v $
 *   Revision 1.4  2004/11/16 07:08:14  cwoodbury
 *   Changed accessibility modifiers for some methods to internal or
 *   protected internal where appropriate.
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
using System.Collections.Generic;

namespace ID3Sharp.Frames
{
	/// <summary>
	/// A registry of frame-type data. This class is also responsible for creating
	/// new ID3v2Frame objects.
	/// </summary>
	internal static class FrameRegistry
	{
		#region Private Fields
		/// <summary>
		/// A dictionary, itself containing dictionaries for each ID3v2 minor version;
		/// each of these dictionaries has FrameTypes as keys and frame-ID strings as values.
		/// </summary>
		private static Dictionary<ID3Versions,Dictionary<FrameType,string>> idRegistry;
		/// <summary>
		/// A dictionary, itself containing dictionaries for each ID3v2 minor version;
		/// each of these dictionaries has frame-ID strings as keys and FrameTypes as values.
		/// </summary>
		private static Dictionary<ID3Versions, Dictionary<string, FrameType>> typeRegistry;
		/// <summary>
		/// A dictionary with FrameTypes as keys and prototype ID3v2Frame objects as values.
		/// </summary>
		private static Dictionary<FrameType, ID3v2Frame> prototypeRegistry;
		#endregion

		/// <summary>
		/// A static constructor that initializes the FrameRegistry.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline" )]
		static FrameRegistry()
		{
			InitializeRegistry();
		}

		#region Initialization Helpers
		/// <summary>
		/// Initializes the internal registries.
		/// </summary>
		private static void InitializeRegistry()
		{
			idRegistry = new Dictionary<ID3Versions, Dictionary<FrameType, string>>();
			idRegistry.Add( ID3Versions.V2_2, new Dictionary<FrameType, string>() );
			idRegistry.Add( ID3Versions.V2_3, new Dictionary<FrameType, string>() );
			idRegistry.Add( ID3Versions.V2_4, new Dictionary<FrameType, string>() );

			typeRegistry = new Dictionary<ID3Versions, Dictionary<string, FrameType>>();
			typeRegistry.Add( ID3Versions.V2_2, new Dictionary<string, FrameType>() );
			typeRegistry.Add( ID3Versions.V2_3, new Dictionary<string, FrameType>() );
			typeRegistry.Add( ID3Versions.V2_4, new Dictionary<string, FrameType>() );

			InitializeVersionIDRegistry( ID3Versions.V2_2 );
			InitializeVersionIDRegistry( ID3Versions.V2_3 );
			InitializeVersionIDRegistry( ID3Versions.V2_4 );

			prototypeRegistry = new Dictionary<FrameType, ID3v2Frame>();
			InitializePrototypeRegistry();
		}

		/// <summary>
		/// Initializes version-specific registries.
		/// </summary>
		/// <param name="version">The version associated with the registry to initialize.</param>
		private static void InitializeVersionIDRegistry( ID3Versions version )
		{
			Dictionary<FrameType, string> versionIDRegistry = idRegistry[ version ];

			if ( version == ID3Versions.V2_3 || version == ID3Versions.V2_4 )
			{
				#region FrameIDs shared between v2.3 and v2.4
				versionIDRegistry.Add( FrameType.AudioEncryption, "AENC" );
				versionIDRegistry.Add( FrameType.AttachedPicture, "APIC" );
				versionIDRegistry.Add( FrameType.Comments, "COMM" );
				versionIDRegistry.Add( FrameType.Commercial, "COMR" );
				versionIDRegistry.Add( FrameType.EncryptionMethodRegistraion, "ENCR" );
				versionIDRegistry.Add( FrameType.EventTimingCodes, "ETCO" );
				versionIDRegistry.Add( FrameType.GeneralEncapsulatedObject, "GEOB" );
				versionIDRegistry.Add( FrameType.GroupIdentificationRegistration, "GRID" );
				versionIDRegistry.Add( FrameType.LinkedInformation, "LINK" );
				versionIDRegistry.Add( FrameType.MusicCDIdentifier, "MCDI" );
				versionIDRegistry.Add( FrameType.MpegLocationLookupTable, "MLLT" );
				versionIDRegistry.Add( FrameType.Ownership, "OWNE" );
				versionIDRegistry.Add( FrameType.PlayCounter, "PCNT" );
				versionIDRegistry.Add( FrameType.Popularimeter, "POPM" );
				versionIDRegistry.Add( FrameType.PositionSynchronization, "POSS" );
				versionIDRegistry.Add( FrameType.Private, "PRIV" );
				versionIDRegistry.Add( FrameType.RecommendedBufferSize, "RBUF" );
				versionIDRegistry.Add( FrameType.Reverb, "RVRB" );
				versionIDRegistry.Add( FrameType.SynchronizedLyrics, "SYLT" );
				versionIDRegistry.Add( FrameType.SynchronizedTempoCodes, "SYTC" );
				versionIDRegistry.Add( FrameType.AlbumTitle, "TALB" );
				versionIDRegistry.Add( FrameType.BeatsPerMinute, "TBPM" );
				versionIDRegistry.Add( FrameType.Composer, "TCOM" );
				versionIDRegistry.Add( FrameType.ContentType, "TCON" );
				versionIDRegistry.Add( FrameType.CopyrightMessage, "TCOP" );
				versionIDRegistry.Add( FrameType.PlaylistDelay, "TDLY" );
				versionIDRegistry.Add( FrameType.EncodedBy, "TENC" );
				versionIDRegistry.Add( FrameType.Lyricist, "TEXT" );
				versionIDRegistry.Add( FrameType.ContentGroupDescription, "TIT1" );
				versionIDRegistry.Add( FrameType.Title, "TIT2" );
				versionIDRegistry.Add( FrameType.Subtitle, "TIT3" );
				versionIDRegistry.Add( FrameType.InitialKey, "TKEY" );
				versionIDRegistry.Add( FrameType.FileType, "TFLT" );
				versionIDRegistry.Add( FrameType.Language, "TLAN" );
				versionIDRegistry.Add( FrameType.Length, "TLEN" );
				versionIDRegistry.Add( FrameType.MediaType, "TMED" );
				versionIDRegistry.Add( FrameType.OriginalAlbumTitle, "TOAL" );
				versionIDRegistry.Add( FrameType.OriginalFilename, "TOFN" );
				versionIDRegistry.Add( FrameType.OriginalLyricist, "TOLY" );
				versionIDRegistry.Add( FrameType.OriginalArtist, "TOPE" );
				versionIDRegistry.Add( FrameType.FileOwner, "TOWN" );
				versionIDRegistry.Add( FrameType.LeadArtist, "TPE1" );
				versionIDRegistry.Add( FrameType.Band, "TPE2" );
				versionIDRegistry.Add( FrameType.Conductor, "TPE3" );
				versionIDRegistry.Add( FrameType.ModifiedBy, "TPE4" );
				versionIDRegistry.Add( FrameType.PartOfASet, "TPOS" );
				versionIDRegistry.Add( FrameType.Publisher, "TPUB" );
				versionIDRegistry.Add( FrameType.TrackNumber, "TRCK" );
				versionIDRegistry.Add( FrameType.InternetRadioStationName, "TRSN" );
				versionIDRegistry.Add( FrameType.InternetRadioStationOwner, "TRSO" );
				versionIDRegistry.Add( FrameType.SoftwareHardwareAndSettingsUsedForEncoding, "TSSE" );
				versionIDRegistry.Add( FrameType.InternationalStandardRecordingCode, "TSRC" );
				versionIDRegistry.Add( FrameType.UserDefinedText, "TXXX" );
				versionIDRegistry.Add( FrameType.UniqueFileIdentifier, "UFID" );
				versionIDRegistry.Add( FrameType.TermsOfUse, "USER" );
				versionIDRegistry.Add( FrameType.UnsynchronizedLyrics, "USLT" );
				versionIDRegistry.Add( FrameType.CommercialInformation, "WCOM" );
				versionIDRegistry.Add( FrameType.CopyrightLegalInformation, "WCOP" );
				versionIDRegistry.Add( FrameType.OfficialAudioFileWebpage, "WOAF" );
				versionIDRegistry.Add( FrameType.OfficialArtistWebpage, "WOAR" );
				versionIDRegistry.Add( FrameType.OfficialAudioSourceWebpage, "WOAS" );
				versionIDRegistry.Add( FrameType.OfficialInternetRadioStationHomepage, "WORS" );
				versionIDRegistry.Add( FrameType.Payment, "WPAY" );
				versionIDRegistry.Add( FrameType.PublishersOfficialWebpage, "WPUB" );
				versionIDRegistry.Add( FrameType.UserDefinedUrlLink, "WXXX" );
				#endregion

				if ( version == ID3Versions.V2_3 )
				{
					#region v2.3 FrameIDs
					versionIDRegistry.Add( FrameType.Equalization, "EQUA" );
					versionIDRegistry.Add( FrameType.InvolvedPeopleList, "IPLS" );
					versionIDRegistry.Add( FrameType.RelativeVolumeAdjustment, "RVAD" );
					versionIDRegistry.Add( FrameType.Date, "TDAT" );
					versionIDRegistry.Add( FrameType.Time, "TIME" );
					versionIDRegistry.Add( FrameType.OriginalReleaseYear, "TORY" );
					versionIDRegistry.Add( FrameType.RecordingDates, "TRDA" );
					versionIDRegistry.Add( FrameType.Size, "TSIZ" );
					versionIDRegistry.Add( FrameType.Year, "TYER" );
					#endregion
				}
				else if ( version == ID3Versions.V2_4 )
				{
					#region v2.4 FrameIDs
					versionIDRegistry.Add( FrameType.AudioSeekPointIndex, "ASPI" );
					versionIDRegistry.Add( FrameType.Equalization2, "EQU2" );
					versionIDRegistry.Add( FrameType.RelativeVolumeAdjustment2, "RVA2" );
					versionIDRegistry.Add( FrameType.Seek, "SEEK" );
					versionIDRegistry.Add( FrameType.Signature, "SIGN" );
					versionIDRegistry.Add( FrameType.EncodingTime, "TDEN" );
					versionIDRegistry.Add( FrameType.OriginalReleaseTime, "TDOR" );
					versionIDRegistry.Add( FrameType.RecordingTime, "TDRC" );
					versionIDRegistry.Add( FrameType.ReleaseTime, "TDRL" );
					versionIDRegistry.Add( FrameType.TaggingTime, "TDTG" );
					versionIDRegistry.Add( FrameType.InvolvedPeopleList2, "TIPL" );
					versionIDRegistry.Add( FrameType.MusicianCreditsList, "TMCL" );
					versionIDRegistry.Add( FrameType.Mood, "TMOO" );
					versionIDRegistry.Add( FrameType.ProducedNotice, "TPRO" );
					versionIDRegistry.Add( FrameType.AlbumSortOrder, "TSOA" );
					versionIDRegistry.Add( FrameType.PerformerSortOrder, "TSOP" );
					versionIDRegistry.Add( FrameType.TitleSortOrder, "TSOT" );
					versionIDRegistry.Add( FrameType.SetSubtitle, "TSST" );
					#endregion
				}
			}
			if ( version == ID3Versions.V2_2 )
			{
				#region v2.2 FrameIDs
				versionIDRegistry.Add( FrameType.RecommendedBufferSize, "BUF" );
				versionIDRegistry.Add( FrameType.PlayCounter, "CNT" );
				versionIDRegistry.Add( FrameType.Comments, "COM" );
				versionIDRegistry.Add( FrameType.AudioEncryption, "CRA" );
				versionIDRegistry.Add( FrameType.EncryptedMetaFrame, "CRM" );
				versionIDRegistry.Add( FrameType.EventTimingCodes, "ETC" );
				versionIDRegistry.Add( FrameType.Equalization, "EQU" );
				versionIDRegistry.Add( FrameType.GeneralEncapsulatedObject, "GEO" );
				versionIDRegistry.Add( FrameType.InvolvedPeopleList, "IPL" );
				versionIDRegistry.Add( FrameType.LinkedInformation, "LNK" );
				versionIDRegistry.Add( FrameType.MusicCDIdentifier, "MCI" );
				versionIDRegistry.Add( FrameType.MpegLocationLookupTable, "MLL" );
				versionIDRegistry.Add( FrameType.AttachedPicture, "PIC" );
				versionIDRegistry.Add( FrameType.Popularimeter, "POP" );
				versionIDRegistry.Add( FrameType.Reverb, "REV" );
				versionIDRegistry.Add( FrameType.RelativeVolumeAdjustment, "RVA" );
				versionIDRegistry.Add( FrameType.SynchronizedLyrics, "SLT" );
				versionIDRegistry.Add( FrameType.SynchronizedTempoCodes, "STC" );
				versionIDRegistry.Add( FrameType.AlbumTitle, "TAL" );
				versionIDRegistry.Add( FrameType.BeatsPerMinute, "TBP" );
				versionIDRegistry.Add( FrameType.Composer, "TCM" );
				versionIDRegistry.Add( FrameType.ContentType, "TCO" );
				versionIDRegistry.Add( FrameType.CopyrightMessage, "TCR" );
				versionIDRegistry.Add( FrameType.Date, "TDA" );
				versionIDRegistry.Add( FrameType.PlaylistDelay, "TDY" );
				versionIDRegistry.Add( FrameType.EncodedBy, "TEN" );
				versionIDRegistry.Add( FrameType.FileType, "TFT" );
				versionIDRegistry.Add( FrameType.Time, "TIM" );
				versionIDRegistry.Add( FrameType.InitialKey, "TKE" );
				versionIDRegistry.Add( FrameType.Language, "TLA" );
				versionIDRegistry.Add( FrameType.Length, "TLE" );
				versionIDRegistry.Add( FrameType.MediaType, "TMT" );
				versionIDRegistry.Add( FrameType.OriginalArtist, "TOA" );
				versionIDRegistry.Add( FrameType.OriginalFilename, "TOF" );
				versionIDRegistry.Add( FrameType.OriginalLyricist, "TOL" );
				versionIDRegistry.Add( FrameType.OriginalReleaseYear, "TOR" );
				versionIDRegistry.Add( FrameType.OriginalAlbumTitle, "TOT" );
				versionIDRegistry.Add( FrameType.LeadArtist, "TP1" );
				versionIDRegistry.Add( FrameType.Band, "TP2" );
				versionIDRegistry.Add( FrameType.Conductor, "TP3" );
				versionIDRegistry.Add( FrameType.ModifiedBy, "TP4" );
				versionIDRegistry.Add( FrameType.PartOfASet, "TPA" );
				versionIDRegistry.Add( FrameType.Publisher, "TPB" );
				versionIDRegistry.Add( FrameType.InternationalStandardRecordingCode, "TRC" );
				versionIDRegistry.Add( FrameType.RecordingDates, "TRD" );
				versionIDRegistry.Add( FrameType.TrackNumber, "TRK" );
				versionIDRegistry.Add( FrameType.Size, "TSI" );
				versionIDRegistry.Add( FrameType.SoftwareHardwareAndSettingsUsedForEncoding, "TSS" );
				versionIDRegistry.Add( FrameType.ContentGroupDescription, "TT1" );
				versionIDRegistry.Add( FrameType.Title, "TT2" );
				versionIDRegistry.Add( FrameType.Subtitle, "TT3" );
				versionIDRegistry.Add( FrameType.Lyricist, "TXT" );
				versionIDRegistry.Add( FrameType.UserDefinedText, "TXX" );
				versionIDRegistry.Add( FrameType.Year, "TYE" );
				versionIDRegistry.Add( FrameType.UniqueFileIdentifier, "UFI" );
				versionIDRegistry.Add( FrameType.OfficialAudioFileWebpage, "WAF" );
				versionIDRegistry.Add( FrameType.OfficialArtistWebpage, "WAR" );
				versionIDRegistry.Add( FrameType.OfficialAudioSourceWebpage, "WAS" );
				versionIDRegistry.Add( FrameType.CommercialInformation, "WCM" );
				versionIDRegistry.Add( FrameType.CopyrightLegalInformation, "WCP" );
				versionIDRegistry.Add( FrameType.PublishersOfficialWebpage, "WPB" );
				versionIDRegistry.Add( FrameType.UserDefinedUrlLink, "WXX" );
				#endregion
			}

			Dictionary<string, FrameType> versionTypeRegistry = typeRegistry[ version ];
			foreach ( FrameType frameType in versionIDRegistry.Keys )
			{
				versionTypeRegistry.Add( versionIDRegistry[ frameType ], frameType );
			}
		}

		/// <summary>
		/// Initializes the prototype registry.
		/// </summary>
		private static void InitializePrototypeRegistry()
		{
			ID3v2Frame commPrototype = new COMMFrame();
			ID3v2Frame geobPrototype = new GEOBFrame();
			ID3v2Frame privPrototype = new PRIVFrame();
			ID3v2Frame pcntPrototype = new PCNTFrame();
			ID3v2Frame textPrototype = new TextInformationFrame();
			ID3v2Frame tconPrototype = new TCONFrame();
			ID3v2Frame trckPrototype = new TRCKFrame();
			ID3v2Frame txxxPrototype = new TXXXFrame();
			ID3v2Frame ufidPrototype = new UFIDFrame();
			ID3v2Frame urlPrototype = new URLLinkFrame();
			ID3v2Frame wxxxPrototype = new WXXXFrame();
			ID3v2Frame unimplementedPrototype = new UnimplementedFrame();

			prototypeRegistry.Add( FrameType.UniqueFileIdentifier, ufidPrototype );
			prototypeRegistry.Add( FrameType.ContentGroupDescription, textPrototype );
			prototypeRegistry.Add( FrameType.Title, textPrototype );
			prototypeRegistry.Add( FrameType.Subtitle, textPrototype );
			prototypeRegistry.Add( FrameType.AlbumTitle, textPrototype );
			prototypeRegistry.Add( FrameType.OriginalAlbumTitle, textPrototype );
			prototypeRegistry.Add( FrameType.TrackNumber, trckPrototype );
			prototypeRegistry.Add( FrameType.PartOfASet, textPrototype );
			prototypeRegistry.Add( FrameType.SetSubtitle, textPrototype );
			prototypeRegistry.Add( FrameType.InternationalStandardRecordingCode, textPrototype );
			prototypeRegistry.Add( FrameType.LeadArtist, textPrototype );
			prototypeRegistry.Add( FrameType.Band, textPrototype );
			prototypeRegistry.Add( FrameType.Conductor, textPrototype );
			prototypeRegistry.Add( FrameType.ModifiedBy, textPrototype );
			prototypeRegistry.Add( FrameType.OriginalArtist, textPrototype );
			prototypeRegistry.Add( FrameType.Lyricist, textPrototype );
			prototypeRegistry.Add( FrameType.OriginalLyricist, textPrototype );
			prototypeRegistry.Add( FrameType.Composer, textPrototype );
			prototypeRegistry.Add( FrameType.MusicianCreditsList, textPrototype );
			prototypeRegistry.Add( FrameType.InvolvedPeopleList2, textPrototype );
			prototypeRegistry.Add( FrameType.EncodedBy, textPrototype );
			prototypeRegistry.Add( FrameType.BeatsPerMinute, textPrototype );
			prototypeRegistry.Add( FrameType.Length, textPrototype );
			prototypeRegistry.Add( FrameType.InitialKey, textPrototype );
			prototypeRegistry.Add( FrameType.Language, textPrototype );
			prototypeRegistry.Add( FrameType.ContentType, tconPrototype );
			prototypeRegistry.Add( FrameType.FileType, textPrototype );
			prototypeRegistry.Add( FrameType.MediaType, textPrototype );
			prototypeRegistry.Add( FrameType.Mood, textPrototype );
			prototypeRegistry.Add( FrameType.CopyrightMessage, textPrototype );
			prototypeRegistry.Add( FrameType.ProducedNotice, textPrototype );
			prototypeRegistry.Add( FrameType.Publisher, textPrototype );
			prototypeRegistry.Add( FrameType.FileOwner, textPrototype );
			prototypeRegistry.Add( FrameType.InternetRadioStationName, textPrototype );
			prototypeRegistry.Add( FrameType.InternetRadioStationOwner, textPrototype );
			prototypeRegistry.Add( FrameType.OriginalFilename, textPrototype );
			prototypeRegistry.Add( FrameType.PlaylistDelay, textPrototype );
			prototypeRegistry.Add( FrameType.EncodingTime, textPrototype );
			prototypeRegistry.Add( FrameType.OriginalReleaseTime, textPrototype );
			prototypeRegistry.Add( FrameType.RecordingTime, textPrototype );
			prototypeRegistry.Add( FrameType.ReleaseTime, textPrototype );
			prototypeRegistry.Add( FrameType.TaggingTime, textPrototype );
			prototypeRegistry.Add( FrameType.SoftwareHardwareAndSettingsUsedForEncoding, textPrototype );
			prototypeRegistry.Add( FrameType.AlbumSortOrder, textPrototype );
			prototypeRegistry.Add( FrameType.PerformerSortOrder, textPrototype );
			prototypeRegistry.Add( FrameType.TitleSortOrder, textPrototype );
			prototypeRegistry.Add( FrameType.UserDefinedText, txxxPrototype );
			prototypeRegistry.Add( FrameType.CommercialInformation, urlPrototype );
			prototypeRegistry.Add( FrameType.CopyrightLegalInformation, urlPrototype );
			prototypeRegistry.Add( FrameType.OfficialAudioFileWebpage, urlPrototype );
			prototypeRegistry.Add( FrameType.OfficialArtistWebpage, urlPrototype );
			prototypeRegistry.Add( FrameType.OfficialAudioSourceWebpage, urlPrototype );
			prototypeRegistry.Add( FrameType.OfficialInternetRadioStationHomepage, urlPrototype );
			prototypeRegistry.Add( FrameType.Payment, urlPrototype );
			prototypeRegistry.Add( FrameType.PublishersOfficialWebpage, urlPrototype );
			prototypeRegistry.Add( FrameType.UserDefinedUrlLink, wxxxPrototype );
			prototypeRegistry.Add( FrameType.MusicCDIdentifier, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.EventTimingCodes, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.MpegLocationLookupTable, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.SynchronizedTempoCodes, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.UnsynchronizedLyrics, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.SynchronizedLyrics, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.Comments, commPrototype );
			prototypeRegistry.Add( FrameType.RelativeVolumeAdjustment2, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.Equalization2, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.Reverb, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.AttachedPicture, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.GeneralEncapsulatedObject, geobPrototype );
			prototypeRegistry.Add( FrameType.PlayCounter, pcntPrototype );
			prototypeRegistry.Add( FrameType.Popularimeter, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.RecommendedBufferSize, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.AudioEncryption, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.LinkedInformation, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.PositionSynchronization, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.TermsOfUse, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.Ownership, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.Commercial, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.EncryptionMethodRegistraion, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.GroupIdentificationRegistration, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.Private, privPrototype );
			prototypeRegistry.Add( FrameType.Signature, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.Seek, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.AudioSeekPointIndex, unimplementedPrototype );

			prototypeRegistry.Add( FrameType.Equalization, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.InvolvedPeopleList, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.RelativeVolumeAdjustment, unimplementedPrototype );
			prototypeRegistry.Add( FrameType.Date, textPrototype );
			prototypeRegistry.Add( FrameType.Time, textPrototype );
			prototypeRegistry.Add( FrameType.OriginalReleaseYear, textPrototype );
			prototypeRegistry.Add( FrameType.RecordingDates, textPrototype );
			prototypeRegistry.Add( FrameType.Size, textPrototype );
			prototypeRegistry.Add( FrameType.Year, textPrototype );
			
			prototypeRegistry.Add( FrameType.EncryptedMetaFrame, unimplementedPrototype );

			prototypeRegistry.Add( FrameType.Unknown, unimplementedPrototype );
		}
		#endregion

		#region Registry Accessors
		/// <summary>
		/// Returns the frame ID for the given FrameType and version.
		/// </summary>
		/// <param name="frameType">The FrameType to look up.</param>
		/// <param name="version">The ID3v2 version to use.</param>
		/// <returns>The frame ID for the given FrameType and version.</returns>
		internal static string GetFrameId( FrameType frameType, ID3Versions version )
		{
			if ( version != ID3Versions.V2_2 && version != ID3Versions.V2_3 &&
				version != ID3Versions.V2_4 )
			{
				throw new UnsupportedVersionException( "This method must be called " +
					"with a single, specific ID3v2 version" );
			}

			string frameID = null;
			Dictionary<FrameType, string> versionRegistry = idRegistry[ version ];

			if ( versionRegistry.ContainsKey( frameType ) )
			{
				frameID = versionRegistry[ frameType ];
			}
			return frameID;
		}

		/// <summary>
		/// Returns the FrameType for the given frame ID and version.
		/// </summary>
		/// <param name="frameID">The frame ID string to look up.</param>
		/// <param name="version">The ID3v2 version to use.</param>
		/// <returns>The frame ID for the given FrameType and version.</returns>
		internal static FrameType GetFrameType( string frameID, ID3Versions version )
		{
			if ( version != ID3Versions.V2_2 && version != ID3Versions.V2_3 &&
				version != ID3Versions.V2_4 )
			{
				throw new UnsupportedVersionException( "This method must be called " +
					"with a single, specific ID3v2 version" );
			}

			Dictionary<string, FrameType> versionRegistry = typeRegistry[ version ];
			FrameType frameType;
			if ( versionRegistry.ContainsKey( frameID ) )
			{
				frameType = versionRegistry[ frameID ];
			}
			else
			{
				frameType = FrameType.Unknown;
			}

			return frameType;
		}

		/// <summary>
		/// Returns a new frame of the given type.
		/// </summary>
		/// <param name="frameType">The type of frame to return.</param>
		/// <returns>A new frame of the given type.</returns>
		internal static ID3v2Frame GetNewFrame( FrameType frameType )
		{
			if ( !prototypeRegistry.ContainsKey( frameType ) )
			{
				throw new FrameTypeNotRegisteredException( frameType );
			}

			ID3v2Frame prototypeFrame = prototypeRegistry[ frameType ];
			ID3v2Frame newFrame = prototypeFrame.Copy();
			newFrame.Type = frameType;

			return newFrame;
		}
		#endregion
	}
}
