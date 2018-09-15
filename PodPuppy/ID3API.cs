using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace ID3API
{
	/// <summary>
	/// Models a single ID3v2 frame. Provides methods to serialize, deserlialise, get- and set-contents.
	/// </summary>
	public class ID3Frame
    {
        #region Private Fields

        private string _id;

        private int
            _sizeInBytes,
            _dataLengthInBytes;

        private bool
            _tagAlterPreservation,
            _fileAlterPreservation,
            _readOnly,

            _hasGroup,
            _compressed,
            _encrypted,
            _unsynced,
            _hasLengthIndicator;

        private byte
            _groupID,
            _encryptMethod;

        private byte[]
            _data;

        #endregion

        #region Public Properties

        /// <summary>
		/// Returns the ID (name) of the type of this frame.
		/// </summary>
		public string ID
		{
			get
			{
				return _id;
			}
		}

		/// <summary>
		/// Returns the size on disk of this frame in its current state.
		/// </summary>
		public int TotalSizeInBytes
		{
			get
			{
				return _sizeInBytes + 10;
			}
        }

        /// <summary>
        /// True if this frame should be preserved when its tag is altered.
        /// </summary>
        public bool TagAlterPreservation
        {
            get
            {
                return !_tagAlterPreservation;
            }
        }

        /// <summary>
        /// True if this frame should be preserved when its file (MP3 contents) is altered.
        /// </summary>
        public bool FileAlterPreservation
        {
            get
            {
                return !_fileAlterPreservation;
            }
        }

        #endregion

        #region Construction

        /// <summary>
		/// Contructs an empty frame.
		/// </summary>
		public ID3Frame()
		{
			_id = "";
			_data = null;
			_sizeInBytes = 0;
			_dataLengthInBytes = 0; // not used
			_tagAlterPreservation = true;
			_fileAlterPreservation = true;
			_readOnly = false;
			_hasGroup = false;
			_compressed = false;
			_encrypted = false;
			_unsynced = false;
			_hasLengthIndicator = false;
			_groupID = 0x00; // not used
			_encryptMethod = 0x00; // not used
		}

		/// <summary>
		/// Contructs a new frame with the specified type and contents.
		/// </summary>
		/// <param name="id">Type</param>
		/// <param name="str">Contents</param>
		public ID3Frame(string id, string str)
		{
	
			if(id.StartsWith("T") && id != "TXXX")
			{
				// ordinary text frame
				this._id = id; 
				Encoding e = Encoding.GetEncoding("ISO-8859-1");
				_data = new byte[e.GetByteCount(str) + 1];
				_data[0] = 0x00; // encoding byte
				e.GetBytes(str, 0, str.Length, _data, 1); // encoded string
			}
			else if(id == "COMM")
			{
				// comment frame

				// [todo] we do not enter a 'short description' of the comment
				this._id = id; 
				Encoding e = Encoding.GetEncoding("ISO-8859-1");
				_data = new byte[e.GetByteCount(str) + 5];

				_data[0] = 0x00; // encoding byte
				_data[1] = (byte)'e'; // language [todo] assume english to simplify UI.
				_data[2] = (byte)'n';
				_data[3] = (byte)'g';
				_data[4] = 0x00; // end of short description
				e.GetBytes(str, 0, str.Length, _data, 5); // encoded string
			}
			else if(id.StartsWith("W") && id != "WXXX")
			{
				// url frame
			}
			else
			{
				throw new ArgumentException("Cannot handle frames of type " + id);
			}

			_sizeInBytes = _data.Length;
			_dataLengthInBytes = 0; // not used
			_tagAlterPreservation = false;
			_fileAlterPreservation = false;
			_readOnly = false;
			_hasGroup = false;
			_compressed = false;
			_encrypted = false;
			_unsynced = false;
			_hasLengthIndicator = false;
			_groupID = 0x00; // not used
			_encryptMethod = 0x00; // not used

        }

        #endregion

        #region Public Methods

        /// <summary>
		/// Reads in the ID3 frame that begins at the current stream position. Returns the
		/// frame and advances the stream position to the begginning of the next frame.
		/// </summary>
		/// <returns>True on success. Failure means no frame was found (This can signify there
		/// are no more frames in the tag.)</returns>
		public bool FromStream(Stream strm)
		{
			try
			{
				byte[] bytes = new byte[4];

				// id..
				strm.Read(bytes, 0, 4);
				_id = Encoding.ASCII.GetString(bytes, 0, 4);

				// have we encountered the begginning of the padding?
				// if so return false.
				if(
					_id[0] == 0x00 &&
					_id[1] == 0x00 &&
					_id[2] == 0x00 &&
					_id[3] == 0x00
					) return false;
				
				// size..
				strm.Read(bytes, 0, 4);
				// size data is in syncsafe int form. decode to int.
				try
				{
                    //if (_id == "APIC")
                    //    _sizeInBytes = (bytes[0] << 24) + (bytes[1] << 16) + (bytes[2] << 8) + bytes[3];
					//else 
                    _sizeInBytes = ID3Helper.SyncSafeToInt(bytes, 0, 4);
				}
				catch(ArgumentException)
				{
                    System.Diagnostics.Trace.TraceWarning("Encountered badly formatted ID3 frame size: {0} in frame type {1}", 
                        BitConverter.ToString(bytes), ID);
		
					return false;
				}

				// flags..
				byte[] flags = new byte[2];
				strm.Read(flags, 0, 2);

				// status flags
				_tagAlterPreservation = (flags[0] & (int)ID3FrameStatusFlags.TagAlterPreservation) != 0;
				_fileAlterPreservation = (flags[0] & (int)ID3FrameStatusFlags.FileAlterPreservation) != 0;
				_readOnly = (flags[0] & (int)ID3FrameStatusFlags.ReadOnly) != 0;

				// format flags
				_hasGroup = (flags[1] & (int)ID3FrameFormatFlags.HasGroup) != 0;
				_compressed = (flags[1] & (int)ID3FrameFormatFlags.Compressed) != 0;
				_encrypted = (flags[1] & (int)ID3FrameFormatFlags.Encrypted) != 0;
				_unsynced = (flags[1] & (int)ID3FrameFormatFlags.Unsynced) != 0;
				_hasLengthIndicator = (flags[1] & (int)ID3FrameFormatFlags.HasLengthIndicator) != 0;

				// [todo] resyncing
				if(_unsynced)
				{
                    System.Diagnostics.Trace.TraceWarning("Encountered unsynced ID3 frame of type {0}. Data will not be extracted.", ID);
					return false;
				}

				// end of main header. 
				// Copy remaining info to a new stream so we can edit it if neccesary.
				MemoryStream ms;
				if(_unsynced)
				{
					// frame data is unsynced. resync it.
                    System.Diagnostics.Trace.TraceWarning("Resyncing ID3 frame of type {0}", ID);
					ms = ID3Helper.ResyncData(strm, _sizeInBytes);
				}
				else
				{
					byte[] buffer = new byte[_sizeInBytes];
					strm.Read(buffer, 0, _sizeInBytes);
					ms = new MemoryStream(buffer, 0, _sizeInBytes, false);
				}

				ms.Position = 0;
	
				// extended header info..

				// groupID
				if(_hasGroup)
					_groupID = (byte)ms.ReadByte();

				// encryption method
				if(_encrypted)
					_encryptMethod = (byte)ms.ReadByte();

				// decoded data length
				if(_hasLengthIndicator)
				{
					ms.Read(bytes, 0, 4);
					// size data is in syncsafe int form. decode to int.
					try
					{
						_dataLengthInBytes = ID3Helper.SyncSafeToInt(bytes, 0, 4);
					}
					catch(Exception)
					{
						// ignore.. we dont use the length indicator anyway.
						// [todo] maybe should abort (return false)
					}
				}

				// all the rest is the actual data.

				// [todo] decryption
				if(_encrypted)
				{
                    System.Diagnostics.Trace.TraceWarning("Encountered encrypted ID3 frame of type {0}. Data will not be extracted.", ID);

					ms.Close();
					return false;
				}

				// [todo] decompression
				if(_compressed)
				{
                    System.Diagnostics.Trace.TraceWarning("Encountered compressed ID3 frame of type {0}. Data will not be extracted.", ID);

					ms.Close();
					return false;
				}

				// store data
				_data = new byte[ms.Length - ms.Position];
				ms.Read(_data, 0, (int)ms.Length - (int)ms.Position);

				ms.Close();
			}
			catch(Exception e)
			{
                System.Diagnostics.Trace.TraceError("Error reading ID3 Frame Header : {0}.", e.Message);
				throw new ArgumentException("Error reading ID3 Frame Header : " + e.Message);
			}

			return true;
		}

		//--------------------------------------------

		/// <summary>
		/// Serializes this frame in ID3v2.3 format onto the specified stream.
		/// </summary>
		/// <returns>True on success.</returns>
		public bool ToStream(Stream strm)
		{
			// id
			strm.Write(Encoding.ASCII.GetBytes(_id), 0, 4);

			// size (as sysncsafe int)
			strm.Write(ID3Helper.IntToSyncSafe(_data.Length), 0, 4);

			// status flags
			byte flags = 0x00;
			if(_tagAlterPreservation)
				flags += (byte)ID3FrameStatusFlags.TagAlterPreservation;
			if(_fileAlterPreservation)
				flags += (byte)ID3FrameStatusFlags.FileAlterPreservation;
			if(_readOnly)
				flags += (byte)ID3FrameStatusFlags.ReadOnly;

			strm.WriteByte(flags);

			// format flags
			flags = 0x00;
			if(_compressed)
				flags += (byte)ID3FrameFormatFlags.Compressed;
			if(_encrypted)
				flags += (byte)ID3FrameFormatFlags.Encrypted;
			if(_hasGroup)
				flags += (byte)ID3FrameFormatFlags.HasGroup;
			if(_hasLengthIndicator)
				flags += (byte)ID3FrameFormatFlags.HasLengthIndicator;
			if(_unsynced)
				flags += (byte)ID3FrameFormatFlags.Unsynced;

			strm.WriteByte(flags);

			// header over. Write data.

			strm.Write(_data, 0, _data.Length);

			return true;
		}

		//--------------------------------------------

		/// <summary>
		/// Returns contents of this frame as a string.
		/// </summary>
		public string GetDataAsString()
		{
			// Some text type frames do not begin with T. e.g. COMM
			// if this is not a text frame type throw exception
			//if(!id.StartsWith("T"))
			//	throw new InvalidOperationException("ERROR: Attempt to get data of " + id + 
			//		" type frame as a string. (This is not a text type frame.)");

			if(_id.StartsWith("T") && _id != "TXXX")
			{
				// normal text frame
				// get encoding
				System.Text.Encoding e;
				byte encoding = _data[0];
				switch(encoding)
				{
					case 0x00:
					default:
						e = Encoding.GetEncoding("ISO-8859-1");
						break;
					case 0x01:
						e = Encoding.GetEncoding("UTF-16");
						break;
					case 0x02:
						e = Encoding.GetEncoding("UTF-16BE");
						break;
					case 0x03:
						e = Encoding.GetEncoding("UTF-8");
						break;
				}

				string result = e.GetString(_data, 1, _data.Length-1);
				return result;
			}
			else if(_id == "COMM")
			{
				// comment frame
				// get encoding
				System.Text.Encoding e;
				byte encoding = _data[0];
				switch(encoding)
				{
					case 0x00:
					default:
						e = Encoding.GetEncoding("ISO-8859-1");
						break;
					case 0x01:
						e = Encoding.GetEncoding("UTF-16");
						break;
					case 0x02:
						e = Encoding.GetEncoding("UTF-16BE");
						break;
					case 0x03:
						e = Encoding.GetEncoding("UTF-8");
						break;
				}

				// find beginning of actual comment

				int i;
				for(i = 4; i < _data.Length-1; i++)
				{
					if(_data[i] == 0x00)
						break;
				}
				i++;

				string result = e.GetString(_data, i, _data.Length-i);
				return result;

			}
			else if(_id == "WXXX")
			{
				// user defined URL. Contains 2 strings - description
				// and URL. ignore the description.

				// find beginning of actual comment

				int i;
				for(i = 0; i < _data.Length-1; i++)
				{
					if(_data[i] == 0x00)
						break;
				}
				i++;

				string result = Encoding.ASCII.GetString(_data, i, _data.Length-i);
				return result;
			}
			else if(_id.StartsWith("W"))
			{
				// ordinary URL frame
				return Encoding.ASCII.GetString(_data, 0, _data.Length);
			}
			else
			{
				throw new InvalidOperationException("Cannot get contents of frame type " + _id + " as a string.");
			}


        }

        #endregion
    }

	/// <summary>
	/// Provides methods to load and save ID3 tags (versions 1 and 2) and to get and set frame contents.
	/// </summary>
	public class ID3Tag
    {
        #region Private Fields

        private bool _hasChanged;
        private string _filename;
        private float _version;
        private int _sizeInBytes;
        private Hashtable _frames;

        //
        // Original Tag header info.
        // All these fields refer to the tag as it was when it was
        // loaded. All these fields are likely to change in the saved
        // tag - since tags are always saved in ID3v2.4 format without
        // unsyncing or an extended header. Also the size of the saved 
        // tag may be greater.
        //
        private bool
            _unsynced,
            _hasExtendedHeader,
            _experimental,
            _hasFooter;		

        #endregion

        #region Public Properties

        /// <summary>
		/// True if the tag has changed since it was last saved/loaded.
		/// </summary>
		public bool HasChanged
		{
			get
			{
				return _hasChanged;
			}
        }

        public string Filename
        {
            get
            {
                return _filename;
            }
        }

        /// <summary>
        /// Gets the size in bytes that the tag would occupy on disc - given its current state.
        /// </summary>
        public int Measure
        {
            get
            {
                int totalBytes = 10;
                foreach (ID3Frame frame in _frames.Values)
                {
                    if (!_hasChanged || frame.TagAlterPreservation)
                        totalBytes += frame.TotalSizeInBytes;
                }
                return totalBytes;
            }
        }

        #endregion

        #region Public Methods

        public bool HasFrame(ID3v2Frames frame)
        {
            return HasFrame(ID3Helper.GetFrameName(frame));
        }

        public bool HasFrame(string frameName)
        {
            return _frames.ContainsKey(frameName);
        }

        /// <summary>
        /// Opens the specified file and attempts to locate an ID3 v2 or v1 tag. If tag is found all
        /// information will be extracted. File is then released. Returns true if tag is found.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool Load(string filename)
        {
            _frames = new Hashtable();

            // try to open file
            FileStream strm = null;
            try
            {
                try
                {
                    strm = new FileStream(filename, FileMode.Open, FileAccess.Read);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.TraceError("Error opening file for ID3 reading. File: {0}. Error: {1}", filename, e.Message);
                    return false;
                }

                this._filename = filename;

                // look for ID3v2 tag..               

                // read in the first 10 bytes which form the header (if its there)
                byte[] bytes = new byte[10];
                int result = strm.Read(bytes, 0, 10);

                // does it fit the pattern for an ID3v2 header?
                if (
                    bytes[0] == 0x49 &&
                    bytes[1] == 0x44 &&
                    bytes[2] == 0x33 &&
                    bytes[3] < 0xFF &&

                    bytes[6] < 0x80 &&
                    bytes[7] < 0x80 &&
                    bytes[8] < 0x80 &&
                    bytes[9] < 0x80
                    )
                {
                    // file has an ID3v2 tag.

                    // extract header info

                    // version
                    _version = 2f + (((float)bytes[3]) / 10f) + (((float)bytes[4]) / 100f);

                    // flags...
                    _unsynced = (bytes[5] & (int)ID3TagFormatFlags.Unsynced) != 0;
                    _hasExtendedHeader = (bytes[5] & (int)ID3TagFormatFlags.HasExtendedHeader) != 0;
                    _experimental = (bytes[5] & (int)ID3TagFormatFlags.Experimental) != 0;
                    _hasFooter = (bytes[5] & (int)ID3TagFormatFlags.HasFooter) != 0;

                    // tag size
                    // size data is in syncsafe int form. decode to int.
                    _sizeInBytes = ID3Helper.SyncSafeToInt(bytes, 6, 4);

                    // if there is an extended header ignore it.
                    // [todo] record data in extended header.
                    int exSize = 0;
                    if (_hasExtendedHeader)
                    {
                        byte[] exBytes = new byte[4];
                        // [todo] check read is succesful.
                        strm.Read(exBytes, 0, 4);

                        // size data is in syncsafe int form. decode to int.
                        exSize = ID3Helper.SyncSafeToInt(bytes, 0, 4);

                        strm.Position += exSize - 4;
                    }

                    // We now have the header info filled in and are at the begginning of the
                    // frames.
                    _frames = new Hashtable();

                    int tagEnd = _sizeInBytes + 10;

                    //byte[] bs = new byte[_sizeInBytes];
                    //strm.Read(bs, 0, _sizeInBytes);
                    //string str = Encoding.ASCII.GetString(bs);
                    //int a = str.IndexOf("APIC");
                    //int b = str.IndexOf("TYER");

                    ID3Frame lastFrame = null;

                    while (strm.Position < tagEnd)
                    {
                        ID3Frame frame = new ID3Frame();
                        if (!frame.FromStream(strm))
                        {
                            byte[] b = new byte[strm.Length - strm.Position];
                            strm.Read(b, 0, (int)(strm.Length - strm.Position));
                            string str = Encoding.ASCII.GetString(b);
                            System.Diagnostics.Trace.TraceError("Error while reading ID3 tag for file {0}. At least one frame has been lost.", _filename);
                            break;
                        }

                        lastFrame = frame;

                        // ignore private frames of other apps.
                        if (frame.ID == "PRIV")
                            continue;
                        // [todo] temp fix.. need way of storing multiple frames of same id
                        if (!_frames.ContainsKey(frame.ID))
                            _frames.Add(frame.ID, frame);
                        else System.Diagnostics.Trace.TraceWarning("Encountered multiple ID3 frames of type {0}. All but one will be discarded.", frame.ID);
                    }

                    strm.Close();

                    _hasChanged = false;

                    return true;
                }
                else
                {
                    // file does not have am ID3v2 tag. Look for a ID3v1 tag..

                    strm.Position = strm.Length - 128;

                    byte[] header = new byte[3];
                    strm.Read(header, 0, 3);
                    byte[] shouldBe = Encoding.ASCII.GetBytes("TAG");
                    if (
                        header[0] == shouldBe[0] &&
                        header[1] == shouldBe[1] &&
                        header[2] == shouldBe[2]
                        )
                    {
                        // ID3v1 tag found

                        _version = 1.0f;
                        _unsynced = false;
                        _hasExtendedHeader = false;
                        _experimental = false;
                        _hasFooter = false;
                        _sizeInBytes = 0; // otherwise save process will overwrite some music

                        byte[] field = new byte[30];

                        // title..
                        strm.Read(field, 0, 30);
                        int strLen = Array.IndexOf(field, (byte)0x00);
                        if (strLen == -1)
                            strLen = 30;
                        if (strLen > 0)
                        {
                            string title = Encoding.ASCII.GetString(field, 0, strLen);
                            _frames.Add("TIT2", new ID3Frame("TIT2", title));
                        }

                        // artist..
                        strm.Read(field, 0, 30);
                        strLen = Array.IndexOf(field, (byte)0x00);
                        if (strLen == -1)
                            strLen = 30;
                        if (strLen > 0)
                        {
                            string artist = Encoding.ASCII.GetString(field, 0, strLen);
                            _frames.Add("TPE1", new ID3Frame("TPE1", artist));
                        }

                        // album..
                        strm.Read(field, 0, 30);
                        strLen = Array.IndexOf(field, (byte)0x00);
                        if (strLen == -1)
                            strLen = 30;
                        if (strLen > 0)
                        {
                            string album = Encoding.ASCII.GetString(field, 0, strLen);
                            _frames.Add("TALB", new ID3Frame("TALB", album));
                        }

                        // year..
                        byte[] year = new byte[4];
                        strm.Read(year, 0, 4);
                        string yearStr = "";
                        foreach (byte b in year)
                            yearStr += ((int)b).ToString();
                        _frames.Add("TDOR", new ID3Frame("TDOR", yearStr));

                        // comment..
                        strm.Read(field, 0, 30);
                        strLen = Array.IndexOf(field, (byte)0x00);
                        if (strLen == -1)
                            strLen = 30;
                        if (strLen > 0)
                        {
                            string comment = Encoding.ASCII.GetString(field, 0, strLen);
                            _frames.Add("COMM", new ID3Frame("COMM", comment));
                        }

                        // track number (optional)
                        if (field[28] == 0x00)
                        {
                            string track = ((int)field[29]).ToString();
                            _frames.Add("TRCK", new ID3Frame("TRCK", track));
                            _version = 1.1f;
                        }

                        // genre
                        byte genreCode = (byte)strm.ReadByte();
                        string genre = ID3Helper.ID3V1GenreToString(genreCode);
                        _frames.Add("TCON", new ID3Frame("TCON", genre));

                        strm.Close();

                        _hasChanged = false;

                        return true;
                    }
                    else
                    {
                        // no tag found
                        _sizeInBytes = 0;
                        _version = 2.4f;

                        // flags...
                        _unsynced = true;
                        _hasExtendedHeader = false;
                        _experimental = false;
                        _hasFooter = false;
                    }
                }
            }
            finally
            {
                if(strm != null)
                    strm.Close();
            }            

            _hasChanged = false;

            return false;
        }

        public void SetDataFromString(ID3v2Frames frame, string data)
        {
            SetDataFromString(ID3Helper.GetFrameName(frame), data);
        }

        /// <summary>
        /// Sets the data of the named frame type to the given string. If the frame does not
        /// exist it will be created.
        /// </summary>
        /// <param name="frameName">frame type name</param>
        /// <param name="data">data as string</param>
        public void SetDataFromString(string frameName, string data)
        {
            if (data == "")
                if (_frames.ContainsKey(frameName))
                {
                    _frames.Remove(frameName);
                    return;
                }

            ID3Frame frame = new ID3Frame(frameName, data);
            if (_frames.ContainsKey(frameName))
                _frames.Remove(frameName);
            _frames.Add(frameName, frame);
            _hasChanged = true;
        }

        public string GetDataAsString(ID3v2Frames frame)
        {
            return GetDataAsString(ID3Helper.GetFrameName(frame));
        }

        /// <summary>
        /// Returns the data of the named frame type as a string.
        /// </summary>
        /// <param name="frameName">frame name type</param>
        /// <returns>frame contents or "" if frame does not exist.</returns>
        public string GetDataAsString(string frameName)
        {
            // Some text type frames do not begin with T. e.g. COMM
            //
            //if(!frameName.StartsWith("T"))
            //	throw new ArgumentException(frameName + " is not a text type frame.", frameName);

            if (!_frames.ContainsKey(frameName))
                return "";
            else return ((ID3Frame)_frames[frameName]).GetDataAsString();
        }

        /// <summary>
        /// Saves changes to the tag and moves the MP3 to the specified path.
        /// </summary>
        /// <param name="filename">New name and location of MP3 file</param>
        /// <returns>True on success.</returns>
        public bool SaveAs(string filename)
        {
            // [todo] stuff could go wrong (files with same name, dir doesnt exist)
            bool success = Save();
            if (success)
            {
                if (File.Exists(filename))
                {
                    if (MessageBox.Show("File exists: " + filename + " overwrite?", "Save As", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        return success;
                }

                File.Move(this._filename, filename);
                this._filename = filename;
            }
            return success;
        }

        /// <summary>
        /// Saves the changes to the tag.
        /// </summary>
        /// <returns>True on success.</returns>
        public bool Save()
        {

            if (!_hasChanged)
                return true;

                int newSize = Measure;
                if (newSize <= _sizeInBytes)
                {
                    // tag has not outgrown its space
                    FileStream fs = null;
                    try
                    {
                        fs = File.OpenWrite(_filename);

                        // write header

                        // "ID3"
                        fs.Write(new byte[] { 0x49, 0x44, 0x33 }, 0, 3);

                        // version (2.3)
                        fs.Write(new byte[] { 0x03, 0x00 }, 0, 2);

                        // flags [todo] for the moment we will ignore previous flags
                        byte flags = 0x00;
                        fs.WriteByte(flags);

                        // tag size (as syncsafe int)
                        fs.Write(ID3Helper.IntToSyncSafe(_sizeInBytes), 0, 4);

                        // [todo] write extended header

                        // write frames

                        foreach (ID3Frame frame in _frames.Values)
                        {
                            if (frame.TagAlterPreservation)
                                frame.ToStream(fs);

                            else
                                System.Diagnostics.Trace.TraceWarning("Discarding ID3 frame of type {0} because it is marked as discard on tag change.", frame.ID);
                        }

                        // fill the remaining space with padding
                        int padSize = _sizeInBytes - newSize;
                        byte[] padding = new byte[padSize];
                        padding.Initialize();
                        fs.Write(padding, 0, padSize);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("Error saving ID3 tag changes. File: {0}. Error: {1}", _filename, ex.Message);
                        return false;
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Flush();
                            fs.Close();
                        }
                    }

                }
                else
                {
                    // tag has outgrown is previous space.. we have to rewrite the whole file

                    // begin constructing the file in memory
                    MemoryStream ms = new MemoryStream();

                    // "ID3"
                    ms.Write(new byte[] { 0x49, 0x44, 0x33 }, 0, 3);

                    // version (2.4)
                    ms.Write(new byte[] { 0x03, 0x00 }, 0, 2);

                    // flags [todo] for the moment we will ignore previous flags
                    byte flags = 0x00;
                    ms.WriteByte(flags);

                    // tag size (as sysnsafe int)
                    // add some padding
                    ms.Write(ID3Helper.IntToSyncSafe(newSize + 2000), 0, 4);

                    // [todo] write extended header

                    // write frames

                    foreach (ID3Frame frame in _frames.Values)
                    {
                        if (frame.TagAlterPreservation)
                            frame.ToStream(ms);
                        else
                            System.Diagnostics.Trace.TraceWarning("Discarding ID3 frame of type {0} because it is marked as discard on tag change.", frame.ID);
                    }

                    // Add some padding
                    int padSize = 2000;
                    byte[] padding = new byte[padSize];
                    padding.Initialize();
                    ms.Write(padding, 0, padSize);

                    FileStream fs = null;
                    byte[] music;
                    try
                    {
                        fs = File.OpenRead(_filename);                        
                        if (_version >= 2)
                        {
                            // tag at front
                            int musicLength = (int)(fs.Length - _sizeInBytes);
                            music = new byte[musicLength];
                            fs.Read(music, 0, musicLength);
                        }
                        else
                        {
                            // tag at end.
                            // keep old tag
                            music = new byte[(int)fs.Length];
                            fs.Read(music, 0, (int)fs.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("Error opening file {0} for ID3 reading. Changes to this file will not be saved. Error: {1}", _filename, ex.Message);
                        return false;
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                            fs = null;
                        }
                    }

                    ms.Write(music, 0, music.Length);

                    try
                    {
                        fs = File.OpenWrite(_filename);

                        ms.WriteTo(fs);
                        ms.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("Error saving ID3 tag changes. File: {0}. Error: {1}", _filename, ex.Message);
                        return false;
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Flush();
                            fs.Close();
                        }
                    }
                }                       

            _hasChanged = false;
            return true;
        }        

        #endregion       
	}

	public class ID3Helper
    {
        #region Private Fields

        private static ArrayList _ID3v1Genres;

        #endregion

        #region Construction

        static ID3Helper()
		{
			_ID3v1Genres = new ArrayList();

			_ID3v1Genres.Add("Blues");
			_ID3v1Genres.Add("Classic Rock");
			_ID3v1Genres.Add("Country");
			_ID3v1Genres.Add("Dance");
			_ID3v1Genres.Add("Disco");
			_ID3v1Genres.Add("Funk");
			_ID3v1Genres.Add("Grunge");
			_ID3v1Genres.Add("Hip-Hop");
			_ID3v1Genres.Add("Jazz");
			_ID3v1Genres.Add("Metal");
			_ID3v1Genres.Add("New Age");
			_ID3v1Genres.Add("Oldies");
			_ID3v1Genres.Add("Other");
			_ID3v1Genres.Add("Pop");
			_ID3v1Genres.Add("R&B");
			_ID3v1Genres.Add("Rap");
			_ID3v1Genres.Add("Reggae");
			_ID3v1Genres.Add("Rock");
			_ID3v1Genres.Add("Techno");
			_ID3v1Genres.Add("Industrial");
			_ID3v1Genres.Add("Alternative");
			_ID3v1Genres.Add("Ska");
			_ID3v1Genres.Add("Death Metal");
			_ID3v1Genres.Add("Pranks");
			_ID3v1Genres.Add("Soundtrack");
			_ID3v1Genres.Add("Euro-Techno");
			_ID3v1Genres.Add("Ambient");
			_ID3v1Genres.Add("Trip-Hop");
			_ID3v1Genres.Add("Vocal");
			_ID3v1Genres.Add(" Jazz+Funk");
			_ID3v1Genres.Add("Fusion");
			_ID3v1Genres.Add("Trance");
			_ID3v1Genres.Add("Classical");
			_ID3v1Genres.Add("Instrumental");
			_ID3v1Genres.Add("Acid");
			_ID3v1Genres.Add("House");
			_ID3v1Genres.Add("Game");
			_ID3v1Genres.Add("Sound Clip");
			_ID3v1Genres.Add("Gospel");
			_ID3v1Genres.Add("Noise");
			_ID3v1Genres.Add("AlternRock");
			_ID3v1Genres.Add("Bass");
			_ID3v1Genres.Add("Soul");
			_ID3v1Genres.Add("Punk");
			_ID3v1Genres.Add("Space");
			_ID3v1Genres.Add("Meditative");
			_ID3v1Genres.Add("Instrumental Pop");
			_ID3v1Genres.Add("Instrumental Rock");
			_ID3v1Genres.Add("Ethnic");
			_ID3v1Genres.Add("Gothic");
			_ID3v1Genres.Add("Darkwave");
			_ID3v1Genres.Add("Techno-Industrial");
			_ID3v1Genres.Add("Electronic");
			_ID3v1Genres.Add("Pop-Folk");
			_ID3v1Genres.Add("Eurodance");
			_ID3v1Genres.Add("Dream");
			_ID3v1Genres.Add("Southern Rock");
			_ID3v1Genres.Add("Comedy");
			_ID3v1Genres.Add("Cult");
			_ID3v1Genres.Add("Gangsta");
			_ID3v1Genres.Add("Top 40");
			_ID3v1Genres.Add("Christian Rap");
			_ID3v1Genres.Add("Pop/Funk");
			_ID3v1Genres.Add("Jungle");
			_ID3v1Genres.Add("Native American");
			_ID3v1Genres.Add("Cabaret");
			_ID3v1Genres.Add("New Wave");
			_ID3v1Genres.Add("Psychadelic");
			_ID3v1Genres.Add("Rave");
			_ID3v1Genres.Add("Showtunes");
			_ID3v1Genres.Add("Trailer");
			_ID3v1Genres.Add("Lo-Fi");
			_ID3v1Genres.Add("Tribal");
			_ID3v1Genres.Add("Acid Punk");
			_ID3v1Genres.Add("Acid Jazz");
			_ID3v1Genres.Add("Polka");
			_ID3v1Genres.Add("Retro");
			_ID3v1Genres.Add("Musical");
			_ID3v1Genres.Add("Rock & Roll");
			_ID3v1Genres.Add("Hard Rock");
			_ID3v1Genres.Add("Folk");
			_ID3v1Genres.Add("Folk-Rock");
			_ID3v1Genres.Add("National Folk");
			_ID3v1Genres.Add("Swing");
			_ID3v1Genres.Add("Fast Fusion");
			_ID3v1Genres.Add("Bebob");
			_ID3v1Genres.Add("Latin");
			_ID3v1Genres.Add("Revival");
			_ID3v1Genres.Add("Celtic");
			_ID3v1Genres.Add("Bluegrass");
			_ID3v1Genres.Add("Avantgarde");
			_ID3v1Genres.Add("Gothic Rock");
			_ID3v1Genres.Add("Progressive Rock");
			_ID3v1Genres.Add("Psychedelic Rock");
			_ID3v1Genres.Add("Symphonic Rock");
			_ID3v1Genres.Add("Slow Rock");
			_ID3v1Genres.Add("Big Band");
			_ID3v1Genres.Add("Chorus");
			_ID3v1Genres.Add("Easy Listening");
			_ID3v1Genres.Add("Acoustic");
			_ID3v1Genres.Add("Humour");
			_ID3v1Genres.Add("Speech");
			_ID3v1Genres.Add("Chanson");
			_ID3v1Genres.Add("Opera");
			_ID3v1Genres.Add("Chamber Music");
			_ID3v1Genres.Add("Sonata");
			_ID3v1Genres.Add("Symphony");
			_ID3v1Genres.Add("Booty Brass");
			_ID3v1Genres.Add("Primus");
			_ID3v1Genres.Add("Porn Groove");
			_ID3v1Genres.Add("Satire");
			_ID3v1Genres.Add("Slow Jam");
			_ID3v1Genres.Add("Club");
			_ID3v1Genres.Add("Tango");
			_ID3v1Genres.Add("Samba");
			_ID3v1Genres.Add("Folklore");
			_ID3v1Genres.Add("Ballad");
			_ID3v1Genres.Add("Power Ballad");
			_ID3v1Genres.Add("Rhytmic Soul");
			_ID3v1Genres.Add("Freestyle");
			_ID3v1Genres.Add("Duet");
			_ID3v1Genres.Add("Punk Rock");
			_ID3v1Genres.Add("Drum Solo");
			_ID3v1Genres.Add("A Capela");
			_ID3v1Genres.Add("Euro-House");
			_ID3v1Genres.Add("Dance Hall");

        }

        #endregion

        #region Public Methods

        /// <summary>
		/// Returns the name of the genre with the given ID3v1 genre code.
		/// </summary>
		public static string ID3V1GenreToString(byte code)
		{
			int index = (int)code;
			if(index < _ID3v1Genres.Count)
				return (string)_ID3v1Genres[(int)code];
			else return "";
		}
	
		/// <summary>
		/// Converts syncsafe int to its 32bit int representation. (See description of
		/// syncsafe ints in ID3v2 specification.)
		/// </summary>
		public static int SyncSafeToInt(byte[] ssd, int offset, int count)
		{
			for(int i = offset; i < offset + count; i++)
			{
				if(ssd[i] >= 0x80)
					throw new ArgumentException("Given data is not valid syncsafe data. Data is : "
						+ BitConverter.ToString(ssd, offset, count));
			}

			int result = 0;
			for(int i = offset; i < offset + count; i++)
			{
				result |= ssd[i];
				if(i < offset + count - 1)
					result = result << 7;
			}
			return result;
		}

		/// <summary>
		/// Converts a 32bit integer to its syncsafe representation. (See description of
		/// syncsafe ints in ID3v2 specification.)
		/// </summary>
		public static byte[] IntToSyncSafe(int num)
		{
			// [todo] check input can be fitted into 28 bits
			byte[] result = new byte[4];
			result[3] = (byte)(num & 0xFFFFFF7F);
			result[2] = (byte)((num >> 7) & 0xFFFFFF7F);
			result[1] = (byte)((num >> 14) & 0xFFFFFF7F);
			result[0] = (byte)((num >> 21) & 0xFFFFFF7F);

			return result;
		}

		/// <summary>
		/// Returns a Stream containing a resynced copy of the data in data and advances the
		/// input stream by size bytes. (See description of the desyncing scheme in ID3v2 specification.)
		/// </summary>
		/// <param name="data"></param>
		/// <param name="size">number of bytes to decode.</param>
		/// <returns>Resynced data.</returns>
		public static MemoryStream ResyncData(Stream data, int size)
		{
			MemoryStream ms = new MemoryStream();

			byte prevByte, thisByte, nextByte;
			thisByte = (byte)data.ReadByte();
			ms.WriteByte(thisByte);
			for(int b = 1; b < size-1; b++)
			{
				prevByte = thisByte;
				thisByte = (byte)data.ReadByte();
				nextByte = (byte)data.ReadByte();

				if(!(
					prevByte == 0xFF &&
					thisByte == 0x00 &&
					nextByte >= 224
					))
				{
					// this byte is not a desync byte. Copy it.
					ms.WriteByte(thisByte);
				}

				data.Position --;
			}

			// write out the last byte in the input
			ms.WriteByte((byte)data.ReadByte());

			ms.Position = 0;
			return ms;
		}

        public static string GetFrameName(ID3v2Frames frame)
        {
            switch (frame)
            {

                case ID3v2Frames.Genre:
                    return "TCON";
                case ID3v2Frames.Album:
                    return "TALB";
                case ID3v2Frames.Artist:
                    return "TPE1";
                case ID3v2Frames.Title:
                    return "TIT2";
                case ID3v2Frames.TrackNumber:
                    return "TRCK";

                case ID3v2Frames.AudioEncryption:
                    return "AENC";
                case ID3v2Frames.AttachedPicture:
                    return "APIC";
                case ID3v2Frames.AudioSeekPointIndex:
                    return "ASPI";
                case ID3v2Frames.Comments:
                    return "COMM";
                case ID3v2Frames.CommercialFrame:
                    return "COMR";
                case ID3v2Frames.EncryptionMethodRegistration:
                    return "ENCR";
                case ID3v2Frames.Equalisation2:
                    return "EQU2";
                case ID3v2Frames.EventTimingCodes:
                    return "ETCO";
                case ID3v2Frames.GeneralEncapsulatedObject:
                    return "GEOB";
                case ID3v2Frames.GroupIdentificationRegistration:
                    return "GRID";
                case ID3v2Frames.LinkedInformation:
                    return "LINK";
                case ID3v2Frames.MusicCDIdentifier:
                    return "MCDI";
                case ID3v2Frames.MPEGLocationLookupTable:
                    return "MLLT";
                case ID3v2Frames.OwnershipFrame:
                    return "OWNE";
                case ID3v2Frames.PrivateFrame:
                    return "PRIV";
                case ID3v2Frames.PlayCounter:
                    return "PCNT";
                case ID3v2Frames.Popularimeter:
                    return "POPM";
                case ID3v2Frames.PositionSynchronisationFrame:
                    return "POSS";
                case ID3v2Frames.RecommendedBufferSize:
                    return "RBUF";
                case ID3v2Frames.RelativeVolumeAdjustment2:
                    return "RVA2";
                case ID3v2Frames.Reverb:
                    return "RVRB";
                case ID3v2Frames.SeekFrame:
                    return "SEEK";
                case ID3v2Frames.SignatureFrame:
                    return "SIGN";
                case ID3v2Frames.SynchronisedLyricText:
                    return "SYLT";
                case ID3v2Frames.SynchronisedTempoCodes:
                    return "SYTC";
                case ID3v2Frames.AlbumMovieShowTitle:
                    return "TALB";
                case ID3v2Frames.BPMBeatsPerMinute:
                    return "TBPM";
                case ID3v2Frames.Composer:
                    return "TCOM";
                case ID3v2Frames.ContentType:
                    return "TCON";
                case ID3v2Frames.CopyrightMessage:
                    return "TCOP";
                case ID3v2Frames.EncodingTime:
                    return "TDEN";
                case ID3v2Frames.PlaylistDelay:
                    return "TDLY";
                case ID3v2Frames.OriginalReleaseTime:
                    return "TDOR";
                case ID3v2Frames.RecordingTime:
                    return "TDRC";
                case ID3v2Frames.ReleaseTime:
                    return "TDRL";
                case ID3v2Frames.TaggingTime:
                    return "TDTG";
                case ID3v2Frames.EncodedBy:
                    return "TENC";
                case ID3v2Frames.LyricistTextWriter:
                    return "TEXT";
                case ID3v2Frames.FileType:
                    return "TFLT";
                case ID3v2Frames.InvolvedPeopleList:
                    return "TIPL";
                case ID3v2Frames.ContentGroupDescription:
                    return "TIT1";
                case ID3v2Frames.TitleSongnameContentDescription:
                    return "TIT2";
                case ID3v2Frames.SubtitleDescriptionRefinement:
                    return "TIT3";
                case ID3v2Frames.InitialKey:
                    return "TKEY";
                case ID3v2Frames.LanguageS:
                    return "TLAN";
                case ID3v2Frames.Length:
                    return "TLEN";
                case ID3v2Frames.MusicianCreditsList:
                    return "TMCL";
                case ID3v2Frames.MediaType:
                    return "TMED";
                case ID3v2Frames.Mood:
                    return "TMOO";
                case ID3v2Frames.OriginalAlbumMovieShowTitle:
                    return "TOAL";
                case ID3v2Frames.OriginalFilename:
                    return "TOFN";
                case ID3v2Frames.OriginalLyricistSTextWriterS:
                    return "TOLY";
                case ID3v2Frames.OriginalArtistSPerformerS:
                    return "TOPE";
                case ID3v2Frames.FileOwnerLicensee:
                    return "TOWN";
                case ID3v2Frames.LeadPerformerSSoloistS:
                    return "TPE1";
                case ID3v2Frames.BandOrchestraAccompaniment:
                    return "TPE2";
                case ID3v2Frames.ConductorPerformerRefinement:
                    return "TPE3";
                case ID3v2Frames.InterpretedRemixedOrOtherwiseModifiedBy:
                    return "TPE4";
                case ID3v2Frames.PartOfASet:
                    return "TPOS";
                case ID3v2Frames.ProducedNotice:
                    return "TPRO";
                case ID3v2Frames.Publisher:
                    return "TPUB";
                case ID3v2Frames.TrackNumberPositionInSet:
                    return "TRCK";
                case ID3v2Frames.InternetRadioStationName:
                    return "TRSN";
                case ID3v2Frames.InternetRadioStationOwner:
                    return "TRSO";
                case ID3v2Frames.AlbumSortOrder:
                    return "TSOA";
                case ID3v2Frames.PerformerSortOrder:
                    return "TSOP";
                case ID3v2Frames.TitleSortOrder:
                    return "TSOT";
                case ID3v2Frames.ISRCInternationalStandardRecordingCode:
                    return "TSRC";
                case ID3v2Frames.SoftwareHardwareAndSettingsUsedForEncoding:
                    return "TSSE";
                case ID3v2Frames.SetSubtitle:
                    return "TSST";
                case ID3v2Frames.UserDefinedTextInformationFrame:
                    return "TXXX";
                case ID3v2Frames.UniqueFileIdentifier:
                    return "UFID";
                case ID3v2Frames.TermsOfUse:
                    return "USER";
                case ID3v2Frames.UnsynchronisedLyricTextTranscription:
                    return "USLT";
                case ID3v2Frames.CommercialInformationURL:
                    return "WCOM";
                case ID3v2Frames.CopyrightLegalInformationURL:
                    return "WCOP";
                case ID3v2Frames.OfficialAudioFileWebpage:
                    return "WOAF";
                case ID3v2Frames.OfficialArtistPerformerWebpage:
                    return "WOAR";
                case ID3v2Frames.OfficialAudioSourceWebpage:
                    return "WOAS";
                case ID3v2Frames.OfficialInternetRadioStationHomepage:
                    return "WORS";
                case ID3v2Frames.PaymentURL:
                    return "WPAY";
                case ID3v2Frames.PublishersOfficialWebpage:
                    return "WPUB";
                case ID3v2Frames.UserDefinedURLLinkFrame:
                    return "WXXX";
                case ID3v2Frames.Year:
                    return "TYER";
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }    
	
	/// <summary>
	/// Converts ID3 Frame codes (like "TIT2") to their descriptions from the ID3v2.4
	/// specification (like "Title/songname/content description") and vice versa.
	/// Can also be used to get a complete list of the ccore frame names.
	/// </summary>
	public class FrameNameConverter
    {
        #region Private Fields

        private static Hashtable 
			nameToDesc,
			descToName;

        #endregion

        #region Construction

        static FrameNameConverter()
		{
			nameToDesc = new Hashtable();

			nameToDesc.Add("AENC", "Audio encryption");
			nameToDesc.Add("APIC", "Attached picture");
			nameToDesc.Add("ASPI", "Audio seek point index");
			nameToDesc.Add("COMM", "Comments");
			nameToDesc.Add("COMR", "Commercial frame");
			nameToDesc.Add("ENCR", "Encryption method registration");
			nameToDesc.Add("EQU2", "Equalisation (2)");
			nameToDesc.Add("ETCO", "Event timing codes");
			nameToDesc.Add("GEOB", "General encapsulated object");
			nameToDesc.Add("GRID", "Group identification registration");
			nameToDesc.Add("LINK", "Linked information");
			nameToDesc.Add("MCDI", "Music CD identifier");
			nameToDesc.Add("MLLT", "MPEG location lookup table");
			nameToDesc.Add("OWNE", "Ownership frame");
			nameToDesc.Add("PRIV", "Private frame");
			nameToDesc.Add("PCNT", "Play counter");
			nameToDesc.Add("POPM", "Popularimeter");
			nameToDesc.Add("POSS", "Position synchronisation frame");
			nameToDesc.Add("RBUF", "Recommended buffer size");
			nameToDesc.Add("RVA2", "Relative volume adjustment (2)");
			nameToDesc.Add("RVRB", "Reverb");
			nameToDesc.Add("SEEK", "Seek frame");
			nameToDesc.Add("SIGN", "Signature frame");
			nameToDesc.Add("SYLT", "Synchronised lyric/text");
			nameToDesc.Add("SYTC", "Synchronised tempo codes");
			nameToDesc.Add("TALB", "Album/Movie/Show title");
			nameToDesc.Add("TBPM", "BPM (beats per minute)");
			nameToDesc.Add("TCOM", "Composer");
			nameToDesc.Add("TCON", "Content type");
			nameToDesc.Add("TCOP", "Copyright message");
			nameToDesc.Add("TDEN", "Encoding time");
			nameToDesc.Add("TDLY", "Playlist delay");
			nameToDesc.Add("TDOR", "Original release time");
			nameToDesc.Add("TDRC", "Recording time");
			nameToDesc.Add("TDRL", "Release time");
			nameToDesc.Add("TDTG", "Tagging time");
			nameToDesc.Add("TENC", "Encoded by");
			nameToDesc.Add("TEXT", "Lyricist/Text writer");
			nameToDesc.Add("TFLT", "File type");
			nameToDesc.Add("TIPL", "Involved people list");
			nameToDesc.Add("TIT1", "Content group description");
			nameToDesc.Add("TIT2", "Title/songname/content description");
			nameToDesc.Add("TIT3", "Subtitle/Description refinement");
			nameToDesc.Add("TKEY", "Initial key");
			nameToDesc.Add("TLAN", "Language(s)");
			nameToDesc.Add("TLEN", "Length");
			nameToDesc.Add("TMCL", "Musician credits list");
			nameToDesc.Add("TMED", "Media type");
			nameToDesc.Add("TMOO", "Mood");
			nameToDesc.Add("TOAL", "Original album/movie/show title");
			nameToDesc.Add("TOFN", "Original filename");
			nameToDesc.Add("TOLY", "Original lyricist(s)/text writer(s)");
			nameToDesc.Add("TOPE", "Original artist(s)/performer(s)");
			nameToDesc.Add("TOWN", "File owner/licensee");
			nameToDesc.Add("TPE1", "Lead performer(s)/Soloist(s)");
			nameToDesc.Add("TPE2", "Band/orchestra/accompaniment");
			nameToDesc.Add("TPE3", "Conductor/performer refinement");
			nameToDesc.Add("TPE4", "Interpreted, remixed, or otherwise modified by");
			nameToDesc.Add("TPOS", "Part of a set");
			nameToDesc.Add("TPRO", "Produced notice");
			nameToDesc.Add("TPUB", "Publisher");
			nameToDesc.Add("TRCK", "Track number/Position in set");
			nameToDesc.Add("TRSN", "Internet radio station name");
			nameToDesc.Add("TRSO", "Internet radio station owner");
			nameToDesc.Add("TSOA", "Album sort order");
			nameToDesc.Add("TSOP", "Performer sort order");
			nameToDesc.Add("TSOT", "Title sort order");
			nameToDesc.Add("TSRC", "ISRC (international standard recording code)");
			nameToDesc.Add("TSSE", "Software/Hardware and settings used for encoding");
			nameToDesc.Add("TSST", "Set subtitle");
			nameToDesc.Add("TXXX", "User defined text information frame");
			nameToDesc.Add("UFID", "Unique file identifier");
			nameToDesc.Add("USER", "Terms of use");
			nameToDesc.Add("USLT", "Unsynchronised lyric/text transcription");
			nameToDesc.Add("WCOM", "Commercial information URL");
			nameToDesc.Add("WCOP", "Copyright/Legal information URL");
			nameToDesc.Add("WOAF", "Official audio file webpage");
			nameToDesc.Add("WOAR", "Official artist/performer webpage");
			nameToDesc.Add("WOAS", "Official audio source webpage");
			nameToDesc.Add("WORS", "Official Internet radio station homepage");
			nameToDesc.Add("WPAY", "Payment URL");
			nameToDesc.Add("WPUB", "Publishers official webpage");
			nameToDesc.Add("WXXX", "User defined URL link frame");

			nameToDesc.Add("TYER", "Year"); // ID3v2.3 only
			
			descToName = new Hashtable();

			foreach(string key in nameToDesc.Keys)
			{
				descToName.Add(nameToDesc[key], key);
			}

        }

        #endregion

        #region Public Methods

        /// <summary>
		/// Returns the description of the named frame type.
		/// </summary>
		/// <param name="name">Name of frame type. This should be one of the four character codes
		/// described in the ID3v2 specification</param>
		/// <returns>Description of named frame type.</returns>
		public static string NameToDesc(string name)
		{
			if(!nameToDesc.ContainsKey(name))
				return "Unknown frame type";
			else return (string)nameToDesc[name];
		}

		/// <summary>
		/// Returns name (frame ID) that matches the given description. The description should
		/// be one of the ones produced by this class.
		/// </summary>
		/// <param name="desc"></param>
		/// <returns></returns>
		public static string DescToName(string desc)
		{
			if(!descToName.ContainsKey(desc))
				throw new ArgumentException("Unknown frame description : " + desc);
			else return (string)descToName[desc];
		}

		/// <summary>
		/// Returns a complete list of the frame type names.
		/// </summary>
		public static string[] Names
		{
			get
			{
				string[] result = new string[nameToDesc.Keys.Count];
				int i = 0;
				foreach(string name in nameToDesc.Keys)
				{
					result[i] = name;
					i++;
				}
				return result;
			}
		}

		/// <summary>
		/// Returns a complete list of the frame type descriptions.
		/// </summary>
		public static string[] Descriptions
		{
			get
			{
				string[] result = new string[descToName.Keys.Count];
				int i = 0;
				foreach(string desc in descToName.Keys)
				{
					result[i] = desc;
					i++;
				}
				return result;
			}
        }

        #endregion
    }

    /// <summary>
    /// See ID3v2 specification.
    /// </summary>
    public enum ID3TagFormatFlags
    {
        Unsynced = 128,
        HasExtendedHeader = 64,
        Experimental = 32,
        HasFooter = 16
    }

    /// <summary>
    /// See ID3v2 specification.
    /// </summary>
    public enum ID3FrameStatusFlags
    {
        TagAlterPreservation = 64,
        FileAlterPreservation = 32,
        ReadOnly = 16
    }

    /// <summary>
    /// See ID3v2 specification.
    /// </summary>
    public enum ID3FrameFormatFlags
    {
        HasGroup = 64,
        Compressed = 8,
        Encrypted = 4,
        Unsynced = 2,
        HasLengthIndicator = 1
    }

    public enum ID3v2Frames
    {

        // alternative names for common frames:

        Genre,
        Album,
        Artist,
        TrackNumber,
        Title,

        /// <summary>
        /// Audio encryption
        /// </summary>
        AudioEncryption,
        /// <summary>
        /// Attached picture
        /// </summary>
        AttachedPicture,
        /// <summary>
        /// Audio seek point index
        /// </summary>
        AudioSeekPointIndex,
        /// <summary>
        /// Comments
        /// </summary>
        Comments,
        /// <summary>
        /// Commercial frame
        /// </summary>
        CommercialFrame,
        /// <summary>
        /// Encryption method registration
        /// </summary>
        EncryptionMethodRegistration,
        /// <summary>
        /// Equalisation (2)
        /// </summary>
        Equalisation2,
        /// <summary>
        /// Event timing codes
        /// </summary>
        EventTimingCodes,
        /// <summary>
        /// General encapsulated object
        /// </summary>
        GeneralEncapsulatedObject,
        /// <summary>
        /// Group identification registration
        /// </summary>
        GroupIdentificationRegistration,
        /// <summary>
        /// Linked information
        /// </summary>
        LinkedInformation,
        /// <summary>
        /// Music CD identifier
        /// </summary>
        MusicCDIdentifier,
        /// <summary>
        /// MPEG location lookup table
        /// </summary>
        MPEGLocationLookupTable,
        /// <summary>
        /// Ownership frame
        /// </summary>
        OwnershipFrame,
        /// <summary>
        /// Private frame
        /// </summary>
        PrivateFrame,
        /// <summary>
        /// Play counter
        /// </summary>
        PlayCounter,
        /// <summary>
        /// Popularimeter
        /// </summary>
        Popularimeter,
        /// <summary>
        /// Position synchronisation frame
        /// </summary>
        PositionSynchronisationFrame,
        /// <summary>
        /// Recommended buffer size
        /// </summary>
        RecommendedBufferSize,
        /// <summary>
        /// Relative volume adjustment (2)
        /// </summary>
        RelativeVolumeAdjustment2,
        /// <summary>
        /// Reverb
        /// </summary>
        Reverb,
        /// <summary>
        /// Seek frame
        /// </summary>
        SeekFrame,
        /// <summary>
        /// Signature frame
        /// </summary>
        SignatureFrame,
        /// <summary>
        /// Synchronised lyric/text
        /// </summary>
        SynchronisedLyricText,
        /// <summary>
        /// Synchronised tempo codes
        /// </summary>
        SynchronisedTempoCodes,
        /// <summary>
        /// Album/Movie/Show title
        /// </summary>
        AlbumMovieShowTitle,
        /// <summary>
        /// BPM (beats per minute)
        /// </summary>
        BPMBeatsPerMinute,
        /// <summary>
        /// Composer
        /// </summary>
        Composer,
        /// <summary>
        /// Content type
        /// </summary>
        ContentType,
        /// <summary>
        /// Copyright message
        /// </summary>
        CopyrightMessage,
        /// <summary>
        /// Encoding time
        /// </summary>
        EncodingTime,
        /// <summary>
        /// Playlist delay
        /// </summary>
        PlaylistDelay,
        /// <summary>
        /// Original release time
        /// </summary>
        OriginalReleaseTime,
        /// <summary>
        /// Recording time
        /// </summary>
        RecordingTime,
        /// <summary>
        /// Release time
        /// </summary>
        ReleaseTime,
        /// <summary>
        /// Tagging time
        /// </summary>
        TaggingTime,
        /// <summary>
        /// Encoded by
        /// </summary>
        EncodedBy,
        /// <summary>
        /// Lyricist/Text writer
        /// </summary>
        LyricistTextWriter,
        /// <summary>
        /// File type
        /// </summary>
        FileType,
        /// <summary>
        /// Involved people list
        /// </summary>
        InvolvedPeopleList,
        /// <summary>
        /// Content group description
        /// </summary>
        ContentGroupDescription,
        /// <summary>
        /// Title/songname/content description
        /// </summary>
        TitleSongnameContentDescription,
        /// <summary>
        /// Subtitle/Description refinement
        /// </summary>
        SubtitleDescriptionRefinement,
        /// <summary>
        /// Initial key
        /// </summary>
        InitialKey,
        /// <summary>
        /// Language(s)
        /// </summary>
        LanguageS,
        /// <summary>
        /// Length
        /// </summary>
        Length,
        /// <summary>
        /// Musician credits list
        /// </summary>
        MusicianCreditsList,
        /// <summary>
        /// Media type
        /// </summary>
        MediaType,
        /// <summary>
        /// Mood
        /// </summary>
        Mood,
        /// <summary>
        /// Original album/movie/show title
        /// </summary>
        OriginalAlbumMovieShowTitle,
        /// <summary>
        /// Original filename
        /// </summary>
        OriginalFilename,
        /// <summary>
        /// Original lyricist(s)/text writer(s)
        /// </summary>
        OriginalLyricistSTextWriterS,
        /// <summary>
        /// Original artist(s)/performer(s)
        /// </summary>
        OriginalArtistSPerformerS,
        /// <summary>
        /// File owner/licensee
        /// </summary>
        FileOwnerLicensee,
        /// <summary>
        /// Lead performer(s)/Soloist(s)
        /// </summary>
        LeadPerformerSSoloistS,
        /// <summary>
        /// Band/orchestra/accompaniment
        /// </summary>
        BandOrchestraAccompaniment,
        /// <summary>
        /// Conductor/performer refinement
        /// </summary>
        ConductorPerformerRefinement,
        /// <summary>
        /// Interpreted, remixed, or otherwise modified by
        /// </summary>
        InterpretedRemixedOrOtherwiseModifiedBy,
        /// <summary>
        /// Part of a set
        /// </summary>
        PartOfASet,
        /// <summary>
        /// Produced notice
        /// </summary>
        ProducedNotice,
        /// <summary>
        /// Publisher
        /// </summary>
        Publisher,
        /// <summary>
        /// Track number/Position in set
        /// </summary>
        TrackNumberPositionInSet,
        /// <summary>
        /// Internet radio station name
        /// </summary>
        InternetRadioStationName,
        /// <summary>
        /// Internet radio station owner
        /// </summary>
        InternetRadioStationOwner,
        /// <summary>
        /// Album sort order
        /// </summary>
        AlbumSortOrder,
        /// <summary>
        /// Performer sort order
        /// </summary>
        PerformerSortOrder,
        /// <summary>
        /// Title sort order
        /// </summary>
        TitleSortOrder,
        /// <summary>
        /// ISRC (international standard recording code)
        /// </summary>
        ISRCInternationalStandardRecordingCode,
        /// <summary>
        /// Software/Hardware and settings used for encoding
        /// </summary>
        SoftwareHardwareAndSettingsUsedForEncoding,
        /// <summary>
        /// Set subtitle
        /// </summary>
        SetSubtitle,
        /// <summary>
        /// User defined text information frame
        /// </summary>
        UserDefinedTextInformationFrame,
        /// <summary>
        /// Unique file identifier
        /// </summary>
        UniqueFileIdentifier,
        /// <summary>
        /// Terms of use
        /// </summary>
        TermsOfUse,
        /// <summary>
        /// Unsynchronised lyric/text transcription
        /// </summary>
        UnsynchronisedLyricTextTranscription,
        /// <summary>
        /// Commercial information URL
        /// </summary>
        CommercialInformationURL,
        /// <summary>
        /// Copyright/Legal information URL
        /// </summary>
        CopyrightLegalInformationURL,
        /// <summary>
        /// Official audio file webpage
        /// </summary>
        OfficialAudioFileWebpage,
        /// <summary>
        /// Official artist/performer webpage
        /// </summary>
        OfficialArtistPerformerWebpage,
        /// <summary>
        /// Official audio source webpage
        /// </summary>
        OfficialAudioSourceWebpage,
        /// <summary>
        /// Official Internet radio station homepage
        /// </summary>
        OfficialInternetRadioStationHomepage,
        /// <summary>
        /// Payment URL
        /// </summary>
        PaymentURL,
        /// <summary>
        /// Publishers official webpage
        /// </summary>
        PublishersOfficialWebpage,
        /// <summary>
        /// User defined URL link frame
        /// </summary>
        UserDefinedURLLinkFrame,
        /// <summary>
        /// Year
        /// </summary>
        Year,
    }  
}
