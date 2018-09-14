#region CVS Log
/*
 * Version:
 *   $Id: EncodedString.cs,v 1.9 2004/12/10 04:49:07 cwoodbury Exp $
 *
 * Revisions:
 *   $Log: EncodedString.cs,v $
 *   Revision 1.9  2004/12/10 04:49:07  cwoodbury
 *   Made changes to EncodedString and to how it is used to push it down to
 *   just being involved with frame I/O and not otherwise being used in frames.
 *
 *   Revision 1.8  2004/11/20 23:09:05  cwoodbury
 *   Fixed bug #1067703: ISO-8859-1 not correctly implemented.
 *   Added default constructor.
 *
 *   Revision 1.7  2004/11/19 18:38:47  cwoodbury
 *   Added code to catch and handle malformed data.
 *
 *   Revision 1.6  2004/11/16 06:44:22  cwoodbury
 *   Refactored code.
 *
 *   Revision 1.4  2004/11/10 06:51:56  cwoodbury
 *   Hid CVS log messages away in #region
 *
 *   Revision 1.3  2004/11/10 06:31:14  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.2  2004/11/10 04:44:16  cwoodbury
 *   Updated documentation.
 *
 *   Revision 1.1  2004/11/03 01:18:26  cwoodbury
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
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ID3Sharp.IO
{
	/// <summary>
	/// A string encoded in any of a number of encodings.
	/// </summary>
	public class EncodedString
	{
		/// <summary>
		/// A delegate that can be passed as a paramter to CreateStrings to retrieve
		/// any bytes leftover after string creation.
		/// </summary>
		/// <param name="leftoverBytes">The bytes leftover from the point at which
		/// CreateStrings was unable to parse any more strings from the data bytes given.</param>
		public delegate void CreateStringsLeftoverBytes( byte[] leftoverBytes );

		#region Static Encoding objects
		/// <summary>A static UnicodeEncoding instance for
		/// BOM-prefixed Unicode strings.</summary>
		private static Encoding utf16Encoding = new UnicodeEncoding( false, true );
		/// <summary>A static UnicodeEncoding instance for
		/// Big-Endian Unicode strings.</summary>
		private static Encoding utf16BEEncoding = new UnicodeEncoding( true, false );
		/// <summary>A static UTF8Encoding instance.</summary>
		private static Encoding utf8Encoding = new UTF8Encoding();
		#endregion

		#region Fields
		/// <summary>The TextEncodingType of the string.</summary>
		private TextEncodingType encodingType;
		/// <summary>The string.</summary>
		private string str;
		/// <summary>A value indicating whether this string is terminated
		/// when it is written.</summary>
		private bool isTerminated;
		/// <summary>A value indicating whether this string has the encoding
		/// type prepending when it is written.</summary>
		private bool hasEncodingTypePrepended;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new empty EncodedString with the default text encoding.
		/// </summary>
		public EncodedString()
		{
			this.TextEncodingType = TextEncodingType.ISO_8859_1;
			this.String = "";
		}

		public EncodedString( string str )
		{
			this.String = str;
		}

		/// <summary>
		/// Creates a new EncodedString.
		/// </summary>
		/// <param name="encodingType">The type of encoding to use.</param>
		/// <param name="str">The string from which to create the EncodedString.</param>
		public EncodedString( TextEncodingType encodingType, string str )
		{
			this.TextEncodingType = encodingType;
			this.String = str;
		}

		/// <summary>
		/// Creates a new EncodedString.
		/// </summary>
		/// <param name="encodingType">The type of encoding to use.</param>
		/// <param name="stringBytes">The byte array from which to create the EncodedString.</param>
		public EncodedString( TextEncodingType encodingType, byte[] stringBytes )
		{
			this.encodingType = encodingType;
			
			switch( encodingType )
			{
				case TextEncodingType.ISO_8859_1:
					this.SetISO_8859_1Bytes( stringBytes );
					break;
				case TextEncodingType.UTF_16:
					this.SetUTF_16Bytes( stringBytes );
					break;
				case TextEncodingType.UTF_16BE:
					this.SetUTF_16BEBytes( stringBytes );
					break;
				case TextEncodingType.UTF_8:
					this.SetUTF_8Bytes( stringBytes );
					break;
			}
		}
		#endregion

		#region Factory methods
		/// <summary>
		/// Parses a byte array and creates and returns one or more strings
		/// from the byte array. NULL bytes are interpreted as string terminators.
		/// </summary>
		/// <param name="stringDataWithEncoding">A byte-array string whose first byte
		/// is a text encoding field.</param>
		/// <returns>One or more strings created from the byte array.</returns>
		public static ReadOnlyCollection<EncodedString>
			CreateStrings( byte[] stringDataWithEncoding )
		{
			// .Length - 1: first byte of string is encoding, not string data
			return CreateStrings( stringDataWithEncoding, stringDataWithEncoding.Length - 1, null );
		}

		/// <summary>
		/// Parses a byte array and creates and returns one or more strings
		/// from the byte array. NULL bytes are interpreted as string terminators.
		/// </summary>
		/// <param name="encType">The text encoding of the byte-array string.</param>
		/// <param name="stringData">A byte-array string.</param>
		/// <returns>One or more strings created from the byte array.</returns>
		public static ReadOnlyCollection<EncodedString>
			CreateStrings( TextEncodingType encType, byte[] stringData )
		{
			return CreateStrings( encType, stringData, stringData.Length, null );
		}

		/// <summary>
		/// Parses a byte array and creates and returns one or more strings
		/// from the byte array. NULL bytes are interpreted as string terminators.
		/// Strings up to and including maxStrings are created and returned; the remainder
		/// of the byte array (if any) is passed back using the CreateStringsLeftoverBytes
		/// delegate.
		/// </summary>
		/// <param name="stringDataWithEncoding">A byte-array string whose first byte
		/// is a text encoding field.</param>
		/// <param name="maxStrings">The maximum number of strings to create.</param>
		/// <param name="leftoversDelegate">A delegate to be used to pass back any bytes
		/// left over from the string parsing.</param>
		/// <returns>One or more strings created from the byte array.</returns>
		public static ReadOnlyCollection<EncodedString> CreateStrings( byte[] stringDataWithEncoding,
			int maxStrings, CreateStringsLeftoverBytes leftoversDelegate )
		{
			TextEncodingType encType = (TextEncodingType) stringDataWithEncoding[0];
			byte[] textBytes = new byte[ stringDataWithEncoding.Length - 1 ];
			Array.Copy( stringDataWithEncoding, 1, textBytes, 0, textBytes.Length );

			ReadOnlyCollection<EncodedString> strings;
			if ( textBytes.Length > 0 )
			{
				strings = CreateStrings( encType, textBytes, maxStrings, leftoversDelegate );
			}
			else
			{
				List<EncodedString> stringsList = new List<EncodedString>();
				stringsList.Add( new EncodedString( encType, "" ) );
				strings = new ReadOnlyCollection<EncodedString>( stringsList );
			}
			
			return strings;
		}

		/// <summary>
		/// Parses a byte array and creates and returns one or more strings
		/// from the byte array. NULL bytes are interpreted as string terminators.
		/// Strings up to and including maxStrings are created and returned; the remainder
		/// of the byte array (if any) is passed back using the CreateStringsLeftoverBytes
		/// delegate.
		/// </summary>
		/// <param name="encType">The text encoding of the byte-array string.</param>
		/// <param name="stringData">A byte-array string.</param>
		/// <param name="maxStrings">The maximum number of strings to create.</param>
		/// <param name="leftoversDelegate">A delegate to be used to pass back any bytes
		/// left over from the string parsing.</param>
		/// <returns>One or more strings created from the byte array.</returns>
		public static ReadOnlyCollection<EncodedString> CreateStrings( TextEncodingType encType,
			 byte[] stringData, int maxStrings, CreateStringsLeftoverBytes leftoversDelegate )
		{
			List<EncodedString> strings = new List<EncodedString>();
			List<int> nullIndices;
			int incrementSize;
			if ( encType == TextEncodingType.ISO_8859_1 || 
				encType == TextEncodingType.UTF_8 )
			{
				nullIndices = FindSingleByteNulls( stringData, maxStrings );
				incrementSize = 1;
			}
			else
			{
				nullIndices = FindDoubleByteNulls( stringData, maxStrings );
				incrementSize = 2;
			}

			int start = 0;
			foreach( int nullIndex in nullIndices )
			{
				strings.Add( CreateString( start, nullIndex, stringData, encType ) );

				start = nullIndex + incrementSize;
			}

			if ( strings.Count < maxStrings )
			{
				// Make a string from the remaining bytes
				strings.Add( CreateString( start, stringData.Length, stringData, encType ) );
			}
			else
			{
				// Add the remaining bytes to the end
				byte[] remainingBytes = new byte[ stringData.Length - start ];
				Array.Copy( stringData, start, remainingBytes, 0, remainingBytes.Length );
				if ( leftoversDelegate != null )
				{
					leftoversDelegate( remainingBytes );
				}
			}

			return new ReadOnlyCollection<EncodedString>( strings );
		}
		#endregion

		#region Factory method helper methods
		/// <summary>
		/// Creates and returns a string from a portion of a byte array.
		/// </summary>
		/// <param name="startIndex">The index (in the byte array) at which to begin
		/// creating the string.</param>
		/// <param name="endIndex">The index (in the byte array) at which to finish
		/// creating the string.</param>
		/// <param name="stringData">The byte array from which to create the string.</param>
		/// <param name="encType">The TextEncodingType of the byte array.</param>
		/// <returns>A string from a portion of a byte array.</returns>
		private static EncodedString CreateString(
			int startIndex, int endIndex, byte[] stringData, TextEncodingType encType )
		{
			byte[] stringBytes = new byte[ endIndex - startIndex ];
			Array.Copy( stringData, startIndex, stringBytes, 0, stringBytes.Length );
			return new EncodedString( encType, stringBytes );
		}

		/// <summary>
		/// Returns a sorted List<int> containing indices of NULL bytes in the
		/// given byte array.
		/// </summary>
		/// <param name="data">The byte array to search.</param>
		/// <param name="maxNulls">The maximum number of NULL bytes to find.</param>
		/// <returns>A sorted List<int> containing indices of NULL bytes in the
		/// given byte array.</returns>
		private static List<int> FindSingleByteNulls( byte[] data, int maxNulls )
		{
			List<int> nulls = new List<int>();

			for ( int dataItr = 0; dataItr < data.Length && nulls.Count < maxNulls; dataItr++ )
			{
				if ( data[ dataItr ] == 0x0 )
				{
					nulls.Add( dataItr );
				}
			}

			return nulls;
		}

		/// <summary>
		/// Returns a sorted List<int> containing indices of NULL byte-pairs in the
		/// given byte array.
		/// </summary>
		/// <param name="data">The byte array to search.</param>
		/// <param name="maxNulls">The maximum number of NULL bytes to find.</param>
		/// <returns>A sorted List<int> containing indices of NULL byte-pairs in the
		/// given byte array.</returns>
		private static List<int> FindDoubleByteNulls( byte[] data, int maxNulls )
		{
			List<int> nulls = new List<int>();

			for ( int dataItr = 0; dataItr < data.Length - 1 && nulls.Count < maxNulls; dataItr += 2 )
			{
				if ( data[ dataItr ] == 0x0 && data[ dataItr + 1 ] == 0x0 )
				{
					nulls.Add( dataItr );
				}
			}

			return nulls;
		}
		#endregion

		public static void CommonalizeEncoding( params EncodedString[] strings )
		{
			TextEncodingType commonType = TextEncodingType.ISO_8859_1;
			foreach ( EncodedString encString in strings )
			{
				if ( (byte) encString.TextEncodingType > (byte) commonType )
				{
					commonType = encString.TextEncodingType;
				}
			}

			foreach ( EncodedString encString in strings )
			{
				encString.TextEncodingType = commonType;
			}
		}

		#region Properties
		/// <summary>
		/// Gets and sets the encoding of this string.
		/// </summary>
		public TextEncodingType TextEncodingType
		{
			get
			{
				return encodingType;
			}
			set
			{
				encodingType = value;
			}
		}

		/// <summary>
		/// Gets the length of the string (in bytes) according to the current encoding.
		/// </summary>
		public int Size
		{
			get
			{
				return this.ToBytes().Length;
			}
		}

		/// <summary>
		/// Gets and sets the String representation of the string.
		/// </summary>
		public string String
		{
			get
			{
				string returnValue = "";

				switch( encodingType )
				{
					case TextEncodingType.ISO_8859_1:
						returnValue = this.ToISO_8859_1String();
						break;
					case TextEncodingType.UTF_16:
						returnValue = this.ToUTF_16String();
						break;
					case TextEncodingType.UTF_16BE:
						returnValue = this.ToUTF_16BEString();
						break;
					case TextEncodingType.UTF_8:
						returnValue = this.ToUTF_8String();
						break;
				}

				return returnValue;
			}
			set
			{
				if ( value == null )
				{
					str = "";
				}
				else
				{
					str = value;
				}
			}
		}

		public bool IsTerminated
		{
			get { return isTerminated; }
			set { isTerminated = value; }
		}

		public bool HasEncodingTypePrepended
		{
			get { return hasEncodingTypePrepended; }
			set { hasEncodingTypePrepended = value; }
		}
	
		#endregion

		#region Public Methods
		/// <summary>
		/// Gets the byte-array representation of the string.
		/// </summary>
		public byte[] ToBytes()
		{
			byte[] bytes = null;

			switch ( encodingType )
			{
				case TextEncodingType.ISO_8859_1:
					bytes = this.ToISO_8859_1Bytes();
					break;
				case TextEncodingType.UTF_16:
					bytes = this.ToUTF_16Bytes();
					break;
				case TextEncodingType.UTF_16BE:
					bytes = this.ToUTF_16BEBytes();
					break;
				case TextEncodingType.UTF_8:
					bytes = this.ToUTF_8Bytes();
					break;
			}

			return bytes;
		}
		#endregion

		#region Conversions
		/// <summary>
		/// Gets the string as an ISO-8859-1-encoded byte array.
		/// </summary>
		private byte[] ToISO_8859_1Bytes()
		{
			int offset = 0;
			if ( hasEncodingTypePrepended ) { offset = 1; }

			byte[] stringBytes = PrepareBytes( TextEncodingType.ISO_8859_1 );
			for ( int charItr = 0; charItr < str.Length; charItr++ )
			{
				stringBytes[ charItr + offset ] = (byte) str[ charItr ];
			}
			return stringBytes;
		}

		/// <summary>
		/// Gets and sets the string as a UTF-16-encoded byte array.
		/// </summary>
		private byte[] ToUTF_16Bytes()
		{
			int offset = 0;
			if ( hasEncodingTypePrepended ) { offset = 1; }

			byte[] stringBytes = PrepareBytes( TextEncodingType.UTF_16 );

			byte[] bom = utf16Encoding.GetPreamble();
			Array.Copy( bom, 0, stringBytes, offset, bom.Length );
			utf16Encoding.GetBytes( str, 0, str.Length, stringBytes, bom.Length + offset );

			return stringBytes;
		}

		/// <summary>
		/// Gets the string as a big-endian, UTF-16-encoded byte array.
		/// </summary>
		private byte[] ToUTF_16BEBytes()
		{
			int offset = 0;
			if ( hasEncodingTypePrepended ) { offset = 1; }

			byte[] stringBytes = PrepareBytes( TextEncodingType.UTF_16BE );

			utf16BEEncoding.GetBytes( str, 0, str.Length, stringBytes, offset );

			return stringBytes;
		}

		/// <summary>
		/// Gets the string as a UTF-8-encoded byte array.
		/// </summary>
		private byte[] ToUTF_8Bytes()
		{
			int offset = 0;
			if ( hasEncodingTypePrepended ) { offset = 1; }

			byte[] stringBytes = PrepareBytes( TextEncodingType.UTF_8 );

			utf8Encoding.GetBytes( str, 0, str.Length, stringBytes, offset );

			return stringBytes;
		}

		/// <summary>
		/// Prepares a byte array for an EncodedString in byte form by prepending
		/// an encoding-type byte and adding termination, according to the settings.
		/// </summary>
		private byte[] PrepareBytes( TextEncodingType encType )
		{
			int encodingLength = 0;
			int terminationLength = 0;
			byte[] termination = this.GetTermination( encType );

			if ( hasEncodingTypePrepended ) { encodingLength = 1; }
			if ( isTerminated ) { terminationLength = termination.Length; }

			int length = 0;
			switch ( encType )
			{
				case TextEncodingType.ISO_8859_1:
					length = str.Length;
					break;
				case TextEncodingType.UTF_16:
					byte[] bom = utf16Encoding.GetPreamble();
					length = utf16Encoding.GetByteCount( str ) + bom.Length;
					break;
				case TextEncodingType.UTF_16BE:
					length = utf16BEEncoding.GetByteCount( str );
					break;
				case TextEncodingType.UTF_8:
					length = utf8Encoding.GetByteCount( str );
					break;
			}
			length += encodingLength + terminationLength;

			byte[] stringBytes = new byte[ length ];
			if ( hasEncodingTypePrepended )
			{
				stringBytes[ 0 ] = (byte) encType;
			}
			if ( isTerminated )
			{
				Array.Copy( termination, 0, stringBytes, length - terminationLength, terminationLength );
			}

			return stringBytes;
		}

		/// <summary>
		/// Gets the string as an ISO-8859-1-encoded String.
		/// </summary>
		private string ToISO_8859_1String()
		{
			StringBuilder isoStringBuilder = new StringBuilder();
			byte[] stringBytes =
				GetStringDataOnly( this.ToISO_8859_1Bytes(), TextEncodingType.ISO_8859_1 );
			foreach ( byte stringByte in stringBytes )
			{
				isoStringBuilder.Append( (char) stringByte );
			}
			return isoStringBuilder.ToString();
		}

		/// <summary>
		/// Gets the string as a UTF-16-encoded String.
		/// </summary>
		private string ToUTF_16String()
		{
			return utf16Encoding.GetString(
				GetStringDataOnly( ToUTF_16Bytes(), TextEncodingType.UTF_16 ) );
		}

		/// <summary>
		/// Gets the string as a big-endian, UTF-16-encoded String.
		/// </summary>
		private string ToUTF_16BEString()
		{
			return utf16BEEncoding.GetString(
				GetStringDataOnly( ToUTF_16BEBytes(), TextEncodingType.UTF_16BE ) );
		}

		/// <summary>
		/// Gets the string as a UTF-8-encoded String.
		/// </summary>
		private string ToUTF_8String()
		{
			return utf8Encoding.GetString(
				GetStringDataOnly( ToUTF_8Bytes(), TextEncodingType.UTF_8 ) );
		}

		/// <summary>
		/// Removes a byte order mark (BOM), prepended text encoding type byte
		/// and appended string termination, if present according to the
		/// settings of this EncodedString.
		/// </summary>
		/// <returns>A byte array with only the character data.</returns>
		private byte[] GetStringDataOnly( byte[] stringBytesWithExtras, TextEncodingType encType )
		{
			int startIndex = 0;
			int endIndex = 0;
			if ( hasEncodingTypePrepended ) { startIndex += 1; }
			if ( encType == TextEncodingType.UTF_16 ) { startIndex += 2; }
			if ( isTerminated ) { endIndex += this.GetTerminationLength( encType ); }
			int totalExtraLength = startIndex + endIndex;

			byte[] bytesWithoutExtras = new byte[ stringBytesWithExtras.Length - totalExtraLength ];
			Array.Copy( stringBytesWithExtras, startIndex, bytesWithoutExtras, 0, bytesWithoutExtras.Length );

			return bytesWithoutExtras;
		}

		/// <summary>
		/// Sets the string as an ISO-8859-1-encoded byte array.
		/// </summary>
		private void SetISO_8859_1Bytes( byte[] value )
		{
			StringBuilder isoStringBuilder = new StringBuilder();
			foreach ( byte stringByte in value )
			{
				isoStringBuilder.Append( (char) stringByte );
			}
			this.String = isoStringBuilder.ToString();
		}

		/// <summary>
		/// Gets and sets the string as a UTF-16-encoded byte array.
		/// </summary>
		private void SetUTF_16Bytes( byte[] value )
		{
			if ( value.Length >= 2 )
			{
				byte[] valueBom = new byte[ 2 ];
				valueBom[ 0 ] = value[ 0 ];
				valueBom[ 1 ] = value[ 1 ];
				byte[] littleEndianBom = ( new UnicodeEncoding( false, true ) ).GetPreamble();
				if ( valueBom[ 0 ] == littleEndianBom[ 0 ] && valueBom[ 1 ] == littleEndianBom[ 1 ] )
				{
					this.String = utf16Encoding.GetString(
						GetStringDataOnly( value, TextEncodingType.UTF_16 ) );
				}
				else if ( valueBom[ 1 ] == littleEndianBom[ 0 ] && valueBom[ 0 ] == littleEndianBom[ 1 ] )
				{
					this.String = ( new UnicodeEncoding( true, true ) ).GetString(
						GetStringDataOnly( value, TextEncodingType.UTF_16 ) );
				}
				else
				{
					this.String = utf16Encoding.GetString( value );
				}
			}
			else
			{
				this.String = "";
			}
		}

		/// <summary>
		/// Sets the string as a big-endian, UTF-16-encoded byte array.
		/// </summary>
		private void SetUTF_16BEBytes( byte[] value )
		{
			this.String = utf16BEEncoding.GetString( value );
		}

		/// <summary>
		/// Sets the string as a UTF-8-encoded byte array.
		/// </summary>
		private void SetUTF_8Bytes( byte[] value )
		{
			this.String = utf8Encoding.GetString( value );
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Gets the termination byte array under the given encoding.
		/// </summary>
		private byte[] GetTermination( TextEncodingType encType )
		{
			if ( encType == TextEncodingType.UTF_16 ||
				encType == TextEncodingType.UTF_16BE )
			{
				return new byte[] { 0x00, 0x00 };
			}
			else
			{
				return new byte[] { 0x00 };
			}
		}

		/// <summary>
		/// Gets the length of the termination byte array under the given encoding.
		/// </summary>
		private int GetTerminationLength( TextEncodingType encType )
		{
			if ( encType == TextEncodingType.UTF_16 ||
				encType == TextEncodingType.UTF_16BE )
			{
				return 2;
			}
			else
			{
				return 1;
			}
		}
		#endregion

		/// <summary>
		/// Creates a deep copy of this object.
		/// </summary>
		/// <returns>A deep copy of this object.</returns>
		public EncodedString Copy()
		{
			EncodedString copy = new EncodedString();
			copy.str = this.str;
			copy.encodingType = this.encodingType;
			copy.isTerminated = this.isTerminated;
			copy.hasEncodingTypePrepended = this.hasEncodingTypePrepended;

			return copy;
		}

		/// <summary>
		/// Writes the EncodedString to a stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		public void WriteToStream( Stream stream )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}
			stream.Write( this.ToBytes(), 0, this.Size );
		}

		public void Validate( ID3Versions version )
		{
			Exception innerException = null;

			bool isV2_2 = (version & ID3Versions.V2_2) == ID3Versions.V2_2;
			bool isV2_3 = (version & ID3Versions.V2_3) == ID3Versions.V2_3;
			bool isUtf16be = encodingType == TextEncodingType.UTF_16BE;
			bool isUtf8 = encodingType == TextEncodingType.UTF_8;

			if ( ( isV2_2 || isV2_3 ) && ( isUtf16be || isUtf8 ) )
			{
				string message = String.Format( "Text encoding type not valid for " +
					"given ID3 version.\nEncoding: {0}\nID3 version: {1}", encodingType, version );
				innerException = new InvalidTextEncodingTypeException( message,
					encodingType );
			}

			if ( innerException != null )
			{
				throw new IOValidationException( "Validation failed.", innerException );
			}
		}
	}
}
