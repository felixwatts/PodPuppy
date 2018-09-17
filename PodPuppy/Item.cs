// PodPuppy - a simple podcast receiver for Windows
// Copyright (c) Felix Watts 2008 (felixwatts@gmail.com)
// https://github.com/felixwatts/PodPuppy
//
// This file is distributed under the Creative Commons Attribution-NonCommercial 4.0 International Licence
// https://creativecommons.org/licenses/by-nc/4.0/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.Threading;
//using ID3Lib;
using ID3Sharp;

//using ID3API;

namespace PodPuppy
{
    public enum ItemStatus
    {
        /// <summary>
        /// The item is ready to begin or continue downloading.
        /// </summary>
        Pending,
        /// <summary>
        /// The item is currently being downloaded.
        /// </summary>
        Downloading,
        /// <summary>
        /// The item has already been downloaded.
        /// </summary>
        Complete,
        /// <summary>
        /// Don't download this item (at users request)
        /// </summary>
        Skip,
        /// <summary>
        /// An error has occurred while processing this item. See ErrorMessage property for more info.
        /// </summary>
        Error,
        //WorkerRemoved   // stop downloading this item and remove its worker from the pool
        /// <summary>
        /// Old item file has been deleted but keep info so we don't download again at next refresh.
        /// Note: this state is also used to hide older items where the user has requested to only get the 
        /// latest item in a new subscription.
        /// </summary>
        Deleted,
    }   

    public delegate void ItemUpdateHandler();
    public delegate void ItemDownloadFinishedHandler(Item item);
    public delegate void ItemStatusChangedHandler(Item item, ItemStatus previousStatus);

    public class Item : ListViewItem, IXmlSerializable
    {

        #region Private Members

        private string
            _title,
            _description,
            _url,
            _errorMessage;

        private DateTime _publicationDate;

        private DateTime? _downloadedDate = null;

        private Feed _feed;

        private ItemStatus _status;

        private int _prevPercentComplete = -1;

        #endregion   
    
        #region Public Properties

        private long _contentLength;
        public long ContentLengthBytes
        {
            get { return _contentLength; }
            set 
            { 
                _contentLength = value;

                RefreshToolTip();
            }
        }

        private long _bytesDownloaded;
        public long BytesDownloaded
        {
            get { return _bytesDownloaded; }
            set 
            { 
                _bytesDownloaded = value;

                int percentComplete = PercentComplete;
                if (percentComplete != _prevPercentComplete)
                {
                    RefreshStatusString();
                    if(Feed != null)
                        Feed.RefreshStatusString();
                }
                _prevPercentComplete = percentComplete;
            }
        }

        private string _downloadDestination = "";
        public string DownloadDestination
        {
            get 
            {
                if (_downloadDestination == "")
                    _downloadDestination = Statics.Config.GetDownloadDestination(this);
                return _downloadDestination; 
            }
            set { _downloadDestination = value; }
        }

        private string _completeDestination = "";
        public string CompleteDestination
        {
            get
            {
                if (_completeDestination == "")
                    _completeDestination = Statics.Config.GetCompleteDestination(this);
                return _completeDestination;
            }
            set { _completeDestination = value; }
        }

        public ItemStatus Status
        {
            get { return _status; }
        }

        public Feed Feed
        {
            get { return _feed; }
        }

        public string URL
        {
            get { return _url; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Description
        {
            get { return _description; }
        }

        public int PercentComplete
        {
            get
            {
                if (_contentLength == 0)
                    return 0;
                else return (int)((_bytesDownloaded * 100) / _contentLength);
            }
        }

        public DateTime PublicationDate
        {
            get { return _publicationDate; }
        }

        public DateTime? DownloadedDate
        {
            get { return _downloadedDate; }
        }

        public bool Highlighted
        {
            get { return object.Equals(Font, Statics.BoldFont); }
            set { Font = value ? Statics.BoldFont : Statics.NormalFont; }
        }

        private bool _tagsSet = false;

        private string _trackTitleTag = null;
        public string TrackTitleTag
        {
            get 
            {
                if (!_tagsSet)
                    ReadTagsFromFile();
                return _trackTitleTag; 
            }
            set { _trackTitleTag = value; }
        }

        private string _albumTag = null;
        public string AlbumTag
        {
            get
            {
                if (!_tagsSet)
                    ReadTagsFromFile(); 
                return _albumTag;
            }
            set { _albumTag = value; }
        }

        private string _artistTag = null;
        public string ArtistTag
        {
            get
            {
                if (!_tagsSet)
                    ReadTagsFromFile(); 
                return _artistTag;
            }
            set { _artistTag = value; }
        }

        private string _genreTag = null;
        public string GenreTag
        {
            get
            {
                if (!_tagsSet)
                    ReadTagsFromFile(); 
                return _genreTag;
            }
            set { _genreTag = value; }
        }
	
        #endregion

        #region Private Properties

        public void TryAgain()
        {
            if (Status != ItemStatus.Error)
                return;

            ChangeStatus(ItemStatus.Pending);
        }

        public string ToolTip
        {
            get
            {
                return Description == null || Description == "" ? "No description" : Tools.StripHTML(Description);

                //StringBuilder sb = new StringBuilder();
                //sb.Append("Title: ");
                //sb.Append(Title);
                //sb.Append('\n');
                //sb.Append("Size: ");
                //sb.Append(ContentLengthBytes == 0 ? "??" : (ContentLengthBytes / 1024).ToString());
                //sb.Append(" KB");
                //sb.Append('\n');
                //sb.Append("URL: ");
                //sb.Append(URL);

                //return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the string that is displayed in the UI representing the current status of the Item. 
        /// </summary>
        public string StatusString
        {
            get
            {
                switch (Status)
                {
                    case ItemStatus.Complete:
                        return "Complete";
                    case ItemStatus.Downloading:
                        return "Downloading (" + PercentComplete + "%)";
                    case ItemStatus.Error:
                        return _errorMessage;
                    case ItemStatus.Pending:
                        return Statics.Paused ? "Paused" : "Pending";
                    case ItemStatus.Skip:
                        return "Skipped";
                    default:
#if DEBUG
                        return Status.ToString();
#else
                        return "";
#endif
                }
            }
        }        

        #endregion

        #region Construction

        public Item() {} // for serialiser

        public Item(Feed feed, string title, string description, string url, DateTime publicationDate) : base()
        {
            _feed = feed;
            _title = title;
            _description = description;
            _url = url;
            _publicationDate = publicationDate;

            _status = ItemStatus.Pending;                        

            LinkUp(feed);
        }

        // just some stuff that needs to be done at some point in construction/deserialisation
        public void LinkUp(Feed feed)
        {
            _feed = feed;

            Text = _title;

            SubItems.Add(new ListViewItem.ListViewSubItem());
            SubItems[1].Text = StatusString;

            SubItems.Add(new ListViewItem.ListViewSubItem());
            SubItems[2].Text = _publicationDate.ToShortDateString();

            SubItems.Add(new ListViewItem.ListViewSubItem());
            SubItems[3].Text = _downloadedDate.HasValue ? _downloadedDate.Value.ToShortDateString() : "";

            ToolTipText = ToolTip;

            //Checked = Status != ItemStatus.Skip;
        }

        #endregion

        #region Public Methods

        public void Hide()
        {
            if (Status != ItemStatus.Pending)
            {
                System.Diagnostics.Trace.TraceError("Attempt to hide an item that is not pending. ItemStatus: " + Status);
                return;
            }
            else ChangeStatus(ItemStatus.Deleted);
        }

        public void DeleteOldItem()
        {
            if (Status != ItemStatus.Complete)
            {
                Hide();
                return;
            }

            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(CompleteDestination, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error deleting old item: " + ex.Message);
                // ignore for now                
            }
            ChangeStatus(ItemStatus.Deleted);
        }

        public void RecoverFromDirtyShutdown()
        {
            if (Status != ItemStatus.Complete)
            {
                // restart the download.
                RemoveIncompleteDownload();
            }
        }

        public void OnDownloadWorkerFinished()
        {
            switch (Status)
            {
                case ItemStatus.Complete:
                    OnDownloadComplete();
                    break;
                default:
                    OnDownloadStopped();
                    break;
            }
        }

        private void OnDownloadStopped()
        {
            if (DownloadStopped != null)
                DownloadStopped(this);
        }

        public bool Download()
        {
            if (Status != ItemStatus.Pending)
                return false;

            // check if already downloaded
            // (requested feature, for pasting previously
            // downlaoded files into correct dir
            // and then subscribing to feed.
            if (File.Exists(Statics.Config.GetCompleteDestination(this)))
            {
                if (ContentLengthBytes == 0)
                    ContentLengthBytes = new FileInfo(Statics.Config.GetCompleteDestination(this)).Length;

                ChangeStatus(ItemStatus.Complete);
                OnDownloadComplete();
                return true;
            }

            ChangeStatus(ItemStatus.Downloading);
            bool success = Statics.DownloadWorkerPool.Download(this);

            if (!success)
                ChangeStatus(ItemStatus.Pending);

            return success;
        }

        public void StopDownload()
        {
            if (_status != ItemStatus.Downloading)
                return;

            ChangeStatus(ItemStatus.Pending);

            //Statics.DownloadWorkerPool.Stop(this);
        }

        /// <summary>
        /// Sets this Items status to Error and sets its error message accordingly.
        /// </summary>
        public void Error(Exception ex)
        {
            Error(ex.Message);
        }

        /// <summary>
        /// Sets this Items status to Error and sets its error message accordingly.
        /// </summary>
        public void Error(string msg)
        {
            System.Diagnostics.Trace.TraceWarning("Item Error: " + msg + ". URL: " + (_url == null ? "null" : _url));
            _errorMessage = msg;
            RemoveIncompleteDownload();
            ChangeStatus(ItemStatus.Error);            
        }        

        public void Skip(bool yes)
        {
            switch (Status)
            {
                case ItemStatus.Pending:
                case ItemStatus.Downloading:
                case ItemStatus.Error:
                    if (yes)
                        ChangeStatus(ItemStatus.Skip);
                    break;
                case ItemStatus.Skip:
                    if (!yes)
                    {
                        if (CompleteDestination != "" && File.Exists(CompleteDestination))
                            ChangeStatus(ItemStatus.Complete);
                        else ChangeStatus(ItemStatus.Pending);
                    }
                    break;
                case ItemStatus.Complete:
                    if (yes)
                    {
                        if (Statics.DeleteCompletedItemsOnSkip)
                            RemoveCompletedDownload();                            
                        ChangeStatus(ItemStatus.Skip);
                        Feed.WritePlaylist();                        
                    }
                    break;
            }

            ForeColor = _status == ItemStatus.Skip ? System.Drawing.Color.Silver : System.Drawing.Color.Black;
        }

        public void OpenContainingFolder()
        {
            if (Status != ItemStatus.Complete)
                return;

            string dir = Path.GetDirectoryName(_completeDestination);

            try
            {
                Process.Start(dir);
            }
            catch (Exception) { }
        }

        public void Play()
        {
            if (Status != ItemStatus.Complete)
                return;

            if (!File.Exists(_completeDestination))
                return;

            try
            {
                Process.Start(_completeDestination);
            }
            catch (Exception) { }
        }

        public void RefreshStatusString()
        {
            if (ListView == null)
                return;

            if (ListView.InvokeRequired)
            {
                ItemUpdateHandler h = new ItemUpdateHandler(RefreshStatusString);
                ListView.Invoke(h, new object[] { });
            }
            else
            {
                SubItems[1].Text = StatusString;
            }
        }

        public void RefreshDateDownloadedString()
        {
            if (ListView == null)
                return;

            if (ListView.InvokeRequired)
            {
                ItemUpdateHandler h = new ItemUpdateHandler(RefreshStatusString);
                ListView.Invoke(h, new object[] { });
            }
            else
            {
                SubItems[3].Text = Tools.ToSensibleTimeString(_downloadedDate);
            }
        }

        public void RefreshToolTip()
        {
            if (ListView == null)
                return;

            if (ListView.InvokeRequired)
            {
                ItemUpdateHandler h = new ItemUpdateHandler(RefreshToolTip);
                ListView.Invoke(h, new object[] { });
            }
            else
            {
                ToolTipText = ToolTip;
            }
        }

        public void OnDownloadComplete()
        {
            _downloadedDate = DateTime.Now;
            RefreshDateDownloadedString();

            AutoAddTags();

            Highlighted = true;            

            if (DownloadComplete != null)
                DownloadComplete(this);
        }

        public void SetDownloadComplete()
        {
            ChangeStatus(ItemStatus.Complete);
        }

        private void SetTagContent(ID3Tag tag, bool overwriteExistingTags)
        {
            if (!tag.HasTitle || overwriteExistingTags)
                tag.Title = TrackTitleTag;

            if (!tag.HasAlbum || overwriteExistingTags)
                tag.Album = AlbumTag;

            if (!tag.HasArtist || overwriteExistingTags)
                tag.Artist = ArtistTag;

            if (!tag.HasGenre || overwriteExistingTags)
                tag.Genre = GenreTag;
        }

        public void WriteTagsToFile()
        {
            WriteTagsToFile(Feed.OverwriteExistingTags);
        }

        public bool WriteTagsToFile(bool overwriteExistingTags)
        {
            if (Status != ItemStatus.Complete)
                return false;

            if (!CompleteDestination.EndsWith(".mp3", StringComparison.CurrentCultureIgnoreCase))
                return false;

            try
            {

                ID3Tag tag = null;

                switch (ID3v2Tag.LookForTag(CompleteDestination))
                {
                    case ID3Versions.None:
                    case ID3Versions.Unknown:
                    default:

                        tag = new ID3v2Tag();
                        SetTagContent(tag, overwriteExistingTags);
                        tag.WriteTag(CompleteDestination, ID3Versions.V2_4);

                        break;

                    case ID3Versions.V1:
                    case ID3Versions.V1_0:
                    case ID3Versions.V1_1:

                        tag = ID3v1Tag.ReadTag(CompleteDestination);
                        SetTagContent(tag, overwriteExistingTags);
                        tag.WriteTag(CompleteDestination, ID3Versions.V1_1);

                        break;

                    case ID3Versions.V2:
                    case ID3Versions.V2_2:
                    case ID3Versions.V2_3:

                        tag = ID3v2Tag.ReadTag(CompleteDestination);
                        SetTagContent(tag, overwriteExistingTags);
                        tag.WriteTag(CompleteDestination, ID3Versions.V2_3);

                        break;

                    case ID3Versions.V2_4:

                        tag = ID3v2Tag.ReadTag(CompleteDestination);
                        SetTagContent(tag, overwriteExistingTags);
                        tag.WriteTag(CompleteDestination, ID3Versions.V2_4);

                        break;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error writing tags to: " + CompleteDestination + ". " + ex.Message);

                return false;
            }

            return true;
        }

        #region Item Comparison Methods

        public static int NameComparer(Item a, Item b)
        {
            return a.Title.CompareTo(b.Title);
        }

        public static int StatusComparer(Item a, Item b)
        {
            return a.StatusString.CompareTo(b.StatusString);
        }

        public static int PublicationDateComparer(Item a, Item b)
        {
            return -(a.PublicationDate.CompareTo(b.PublicationDate));
        }

        public static int DownloadDateComparer(Item a, Item b)
        {
            if (a.DownloadedDate == null && b.DownloadedDate == null)
                return 0;
            else if (a.DownloadedDate == null)
                return 1;
            else if (b.DownloadedDate == null)
                return -1;
            else return -(a.DownloadedDate.Value.CompareTo(b.DownloadedDate.Value));
        }

        #endregion

        #endregion

        #region Private Methods

        private void ReadTagsFromFile()
        {
            if (Status != ItemStatus.Complete)
                return;

            if (!CompleteDestination.EndsWith(".mp3", StringComparison.CurrentCultureIgnoreCase))
                return;

            try
            {
                ID3Tag tag = ID3v2Tag.ReadTag(CompleteDestination);
                
                if (tag == null)
                {
                    tag = ID3v1Tag.ReadTag(CompleteDestination);
                    if (tag == null)
                        tag = new ID3v2Tag();
                }

                TrackTitleTag = tag.Title;
                AlbumTag = tag.Album;
                ArtistTag = tag.Artist;
                GenreTag = tag.Genre;

                _tagsSet = true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: unable to read ID3 Tag for file: " + CompleteDestination + ". " + ex.Message);
            }            
        }

        private void RemoveIncompleteDownload()
        {

            if(File.Exists(DownloadDestination))
            {
                try
                {
                    File.Delete(DownloadDestination);                    
                }
                catch(Exception){}
            }

            BytesDownloaded = 0;
        }

        private void RemoveCompletedDownload()
        {
            if (File.Exists(CompleteDestination))
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(CompleteDestination, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);                    
                }
                catch (Exception) { }
            }

            BytesDownloaded = 0;
            _downloadedDate = null;
        }

        private void ChangeStatus(ItemStatus newStatus)
        {
            if (newStatus == _status)
                return; 
            
            ItemStatus oldStatus = _status;

            _status = newStatus;

            RefreshStatusString();            

            if (StatusChanged != null)
                StatusChanged(this, oldStatus);
        }

        private void AutoAddTags()
        {
            if (Status != ItemStatus.Complete)
                return;

            if (!CompleteDestination.EndsWith(".mp3", StringComparison.CurrentCultureIgnoreCase))
                return;

            if (!(Feed.ApplyAlbumTag || Feed.ApplyArtistTag || Feed.ApplyGenreTag || Feed.ApplyTrackTitleTag))
                return;

            if (Feed.ApplyArtistTag && Feed.ArtistTag != "")
                _artistTag = ExpandTagString(Feed.ArtistTag);

            if (Feed.ApplyAlbumTag && Feed.AlbumTag != "")
                _albumTag = ExpandTagString(Feed.AlbumTag);

            if (Feed.ApplyGenreTag && Feed.GenreTag != "")
                _genreTag = ExpandTagString(Feed.GenreTag);

            if (Feed.ApplyTrackTitleTag && Feed.TrackTitleTag != "")
                _trackTitleTag = ExpandTagString(Feed.TrackTitleTag);

            _tagsSet = true;

            Thread thread = new Thread(new ThreadStart(WriteTagsToFile));
            thread.Start();
            //WriteTagsToFile(Feed.OverwriteExistingTags);
        }

        public string ExpandTagString(string tagString)
        {
            tagString = tagString.Replace("%t", Title);
            tagString = tagString.Replace("%p", Feed.Title);
            tagString = tagString.Replace("%d", PublicationDate.ToShortDateString());

            tagString = tagString.Replace("%D", PublicationDate.Day.ToString("00"));
            tagString = tagString.Replace("%M", PublicationDate.Month.ToString("00"));
            tagString = tagString.Replace("%Y", PublicationDate.Year.ToString());

            int numComplete = 0;
            foreach(Item item in Feed.Items)
                if(item.Status == ItemStatus.Complete)
                    numComplete ++;
            tagString = tagString.Replace("%n", numComplete.ToString("000"));

            tagString = tagString.Replace("%N", (999 - numComplete).ToString("000"));

            return tagString;
        }

        private enum ID3TagFrames
        {
            Album,
            Artist,
            Genre,
            Title,
        }

        private string ID3TagFrameToString(ID3TagFrames frameType)
        {
            string frameID;
            switch (frameType)
            {
                case ID3TagFrames.Album:
                    frameID = "TALB";
                    break;
                case ID3TagFrames.Artist:
                    frameID = "TPE1";
                    break;
                case ID3TagFrames.Genre:
                    frameID = "TCON";
                    break;
                case ID3TagFrames.Title:
                    frameID = "TIT2";
                    break;
                default:
                    throw new System.NotImplementedException("Unknown ID3 Frame Type : " + frameType);
            }

            return frameID;
        }

        private string GetTagFrame(ID3TagFrames frameType)
        {
            // TODO
            return "";
        }

        private void SetTagFrame(ID3TagFrames frameType, string value, bool overwriteExisting)
        {
            // TODO
        }

        #endregion

        #region Events

        /// <summary>
        /// Could be due to completion, error, pause or skip.
        /// </summary>
        public event ItemDownloadFinishedHandler DownloadComplete;
        public event ItemDownloadFinishedHandler DownloadStopped;
        public event ItemStatusChangedHandler StatusChanged;

        #endregion

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement();

            _title = reader.ReadElementString("title");
            //Description = reader.ReadElementString("description"); // might cause problems if descriptn contains html
            _url = reader.ReadElementString("url"); 
           
            _status = (ItemStatus)Enum.Parse(typeof(ItemStatus), reader.ReadElementString("status"));
            ForeColor = _status == ItemStatus.Skip ? System.Drawing.Color.Silver : System.Drawing.Color.Black;

            BytesDownloaded = long.Parse(reader.ReadElementString("bytesDownloaded"));
            ContentLengthBytes = long.Parse(reader.ReadElementString("contentLength"));
            _errorMessage = reader.ReadElementString("errorMessage");
            DownloadDestination = reader.ReadElementString("downloadDestination");
            CompleteDestination = reader.ReadElementString("completeDestination");

            try
            {
                _publicationDate = DateTime.Parse(reader.ReadElementString("publishedDate"));
                string dt = reader.ReadElementString("downloadedDate");
                _downloadedDate = dt == "" ? null : (DateTime?)DateTime.Parse(dt);
                _description = reader.ReadElementString("description");
            }
            catch(System.Xml.XmlException)
            {
                _publicationDate = DateTime.Now;
                _downloadedDate = _status == ItemStatus.Complete ? (DateTime?)DateTime.Now : null;
                _description = "";
            }

            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("title", _title);
            writer.WriteElementString("url", _url);
            writer.WriteElementString("status", Status.ToString());
            writer.WriteElementString("bytesDownloaded", _bytesDownloaded.ToString());
            writer.WriteElementString("contentLength", _contentLength.ToString());
            writer.WriteElementString("errorMessage", _errorMessage);
            writer.WriteElementString("downloadDestination", _downloadDestination); // don't use property becuase when you access the property it sets it according to the current config - only want to do that when we actually start the download.
            writer.WriteElementString("completeDestination", _completeDestination); // don't use property becuase when you access the property it sets it according to the current config - only want to do that when we actually start the download.
            writer.WriteElementString("publishedDate", _publicationDate.ToString("u"));
            writer.WriteElementString("downloadedDate", _downloadedDate.HasValue ? _downloadedDate.Value.ToString("u") : "");
            writer.WriteElementString("description", Description);
        }

        #endregion
    }
}
