//////////////////////////////////////////////////////////////////////////////////
//                                                                              //
// Title: PodPuppy                                                              //
// Author: Felix Watts                                                          //
// Contact: felix@fwatts.info                                                   //
// Website: http://fwatts.info                                                  //
//                                                                              //
// This file is distributed under the Creative Commons Attribution 2.5 Licence. //
// http://creativecommons.org/licenses/by/2.5/                                  //
//                                                                              //
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Security;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Threading;

namespace PodPuppy
{
    public enum ItemFileNamingSchemeEnum
    {
        TitleFromFeed,
        FilenameOnServer
    }

    public enum BalloonFunction
    {
        PlayFile,
        OpenFolder
    }

    public enum SearchMode
    {
        Google,
        GoogleWithFileType,
    }

    [Serializable]
    public class Config
    {
        private static XmlSerializer _serializer;

        [NonSerialized]
        public string ConfigFilename;

        public bool ShowInTaskbarWhenMinimized = false;

        public bool EnableScheduler = false;

        public SearchMode SearchMode = SearchMode.GoogleWithFileType;

        //[XmlIgnore]
        public List<string> SavedTitleTags;
        //[XmlIgnore]
        public List<string> SavedArtistTags;
        //[XmlIgnore]
        public List<string> SavedAlbumTags;
        //[XmlIgnore]
        public List<string> SavedGenreTags;

        public List<string> SavedFilenameTags;

        [XmlIgnore]
        public bool[,] Schedule = 
            {
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true },

            };

        public int MainSplitterLocation = 163;
        public int MainFormWidth = 733;
        public int MainFormHeight = 453;
        public int MainFormLeft = 0;
        public int MainFormTop = 0;

        static Config()
        {
            _serializer = new XmlSerializer(typeof(Config));            
        }

        public Config()
        {
            string pm = System.Configuration.ConfigurationManager.AppSettings["portableMode"];
            if (pm != null && pm == "true")
            {
                PortableMode = true;
                
                SettingsDir = Application.StartupPath + "\\";
                CompletedFilesBaseDirectory = Path.GetPathRoot(Application.StartupPath) + "Podcasts\\";                
            }
            else
            {
                PortableMode = false;
                SettingsDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\PodPuppy\\";

                CompletedFilesBaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            }

            ConfigFilename = SettingsDir + "PodPuppy.config";
            DownloadBaseDirectory = SettingsDir + "downloads";

            SavedAlbumTags = new List<string>(new string[]{ "%p" });
            SavedArtistTags = new List<string>();
            SavedGenreTags = new List<string>(new string[] { "Podcast" });
            SavedTitleTags = new List<string>(new string[] { "%t" });
            SavedFilenameTags = new List<string>(new string[] { "%t" });
        }

        [NonSerialized]
        public string SettingsDir;

        [NonSerialized]
        public bool PortableMode = false;
       
        [NonSerialized]
        public string DownloadBaseDirectory;

        public string CompletedFilesBaseDirectory;

        public int FeedViewSortColumn = 3;

        public int ItemViewSortColumn = 3;

        public bool FeedSortAscending = true;

        public bool ItemSortAscending = true;

        public bool WritePlaylists = true;

        public bool LaunchOnStartup
        {
            get
            {
                RegistryKey startupList = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                return startupList.GetValue("PodPuppy") != null;
            }

            [System.Security.Permissions.RegistryPermission(System.Security.Permissions.SecurityAction.Demand)]
            set
            {
                try
                {
                    RegistryKey startupList = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                    if (value)
                        startupList.SetValue("PodPuppy", Application.ExecutablePath);
                    else
                        startupList.DeleteValue("PodPuppy", false);

                    startupList.Close();
                }
                catch (SecurityException)
                {
                    MessageBox.Show("You do not have the required permissions to enable Launch on Startup.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public string GetDownloadDestination(Item item)
        {
            string result = DownloadBaseDirectory + "\\" 
                + SanitiseFileOrFolderName(item.Feed.Title) 
                + "\\" 
                + SanitiseFileOrFolderName(Path.GetFileName(item.URL))
                + ".INCOMPLETE";

            return Tools.EnsurePathIsNotTooLong(result);
        }

        public string GetCompleteDestination(Item item)
        {
            return GetCompleteDestination(item, item.Feed.ItemFilenamePattern);
        }

        public string GetCompleteDestination(Item item, string itemFilenamePattern)
        {
            string result = Path.Combine(item.Feed.Folder, SanitiseFileOrFolderName(item.ExpandTagString(itemFilenamePattern)));

            string ext = Path.GetExtension(item.URL);
            if (ext.Contains("?"))
                ext = ext.Substring(0, ext.IndexOf('?'));
            if (ext.Contains("%"))
                ext = ext.Substring(0, ext.IndexOf('%'));
            result += ext;

            result = Tools.EnsurePathIsNotTooLong(result);

            return result;

            // C:\Documents and Settings\Administrator\My Documents\My Music\Podcasts\Movie-Cast (creation podcasts)\Moviecast for week starting 1st June..mp3?d=17
        }

        public string GetDefaultCompleteDestination(Feed feed)
        {
            return Tools.EnsurePathIsNotTooLong(Path.Combine(CompletedFilesBaseDirectory, SanitiseFileOrFolderName(feed.Title)));
        }

        public string GetCompleteDestination(Feed feed)
        {
            return feed.Folder;
        }

        public string GetSyncedDestination(Feed feed)
        {
            if (!feed.Syncronised || SyncFolder == "")
                return null;
            return SyncFolder + "\\" + SanitiseFileOrFolderName(feed.Title);
        }

        public string GetSyncedDestination(Item item)
        {
            if (!item.Feed.Syncronised || SyncFolder == "")
                return null;

            string result = SyncFolder
                + "\\"
                + SanitiseFileOrFolderName(item.Feed.Title)
                + "\\";

            string ext = Path.GetExtension(item.URL);
            result += SanitiseFileOrFolderName(item.Title);
            if (ext.Contains("?"))
                ext = ext.Substring(0, ext.IndexOf('?'));
            result += ext;

            return result;
        }

        public string SanitiseFileOrFolderName(string name)
        {
            foreach (char forbidden in Path.GetInvalidFileNameChars())
                name = name.Replace(forbidden, ' ');
            name = name.Trim();          

            return name;
        }

        public int MaxDownloads = 4;

        public int CheckFeedInterval = 20;
      
        public bool ShowBalloons = false;

        public BalloonFunction BalloonFunction = BalloonFunction.PlayFile;

        //public bool ShowDeleteItemWarning = true;

        public bool StartMinimised = false;

        //public bool HideSkippedItems = false;

        public string SyncFolder = "";

        public string SyncVolumeLabel = "";

        public bool AutoSync = false;

        //public bool AddAlbumTag = false;

        //public bool AddGenreTag = false;

        public bool CheckForNewVersion = true;

        public int MaxBandwidthInBytes = 0;

        public string SyncedFileTypes = "mp3 wma ogg m3u";

        public string DynamicOPMLSource = "";

        public bool DynamicOPMLJustGetLatest = true;

        public bool DynamicOPMLDeleteFiles = true;

        public bool Save()
        {
            Stream fileStream = null;

            int retries = 3;
            while (retries > 0)
            {
                try
                {
                    string dir = Path.GetDirectoryName(ConfigFilename);
                    Directory.CreateDirectory(dir);
                    fileStream = File.Create(ConfigFilename);
                    _serializer.Serialize(fileStream, this);
                    fileStream.Flush();
                    fileStream.Close();
                    fileStream = null;

                    XmlDocument doc = new XmlDocument();
                    doc.Load(ConfigFilename);
                    XmlNode scheduleNode = doc.CreateElement("Schedule");
                    for (int day = 0; day < 7; day++)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int hour = 0; hour < 24; hour++)
                        {
                            sb.Append(Schedule[hour, day] ? '1' : '0');
                            sb.Append(' ');
                        }
                        sb.Length--;
                        XmlNode dayNode = doc.CreateElement("day");
                        dayNode.InnerText = sb.ToString();
                        scheduleNode.AppendChild(dayNode);
                    }
                    doc.DocumentElement.AppendChild(scheduleNode);

                    XmlNode tagNode = doc.CreateElement("TitleTags");
                    tagNode.InnerXml = SerializeStringList(SavedTitleTags);
                    doc.DocumentElement.AppendChild(tagNode);

                    tagNode = doc.CreateElement("AlbumTags");
                    tagNode.InnerXml = SerializeStringList(SavedAlbumTags);
                    doc.DocumentElement.AppendChild(tagNode);

                    tagNode = doc.CreateElement("ArtistTags");
                    tagNode.InnerXml = SerializeStringList(SavedArtistTags);
                    doc.DocumentElement.AppendChild(tagNode);

                    tagNode = doc.CreateElement("GenreTags");
                    tagNode.InnerXml = SerializeStringList(SavedGenreTags);
                    doc.DocumentElement.AppendChild(tagNode);

                    tagNode = doc.CreateElement("FilenameTags");
                    tagNode.InnerXml = SerializeStringList(SavedFilenameTags);
                    doc.DocumentElement.AppendChild(tagNode);

                    doc.Save(ConfigFilename);

                    return true;
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.TraceError("****** Error saving config: " + ex.Message);
                    System.Diagnostics.Trace.TraceError("****** Retry " + retries);
                    Thread.Sleep(250);
                    retries--;
                    continue;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("****** Error saving config: " + ex.Message);
                    return false;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Flush();
                        fileStream.Close();
                    }
                }
            }

            return false;
        }

        public static Config Load()
        {
            Config result = new Config();

            if (!File.Exists(result.ConfigFilename))
                return result;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(result.ConfigFilename);

                SetBoolFromNode(doc, "/Config/EnableScheduler", ref result.EnableScheduler);
                //SetStringFromNode(doc, "/Config/DownloadBaseDirectory", ref result.DownloadBaseDirectory);
                SetStringFromNode(doc, "/Config/CompletedFilesBaseDirectory", ref result.CompletedFilesBaseDirectory);
                SetIntFromNode(doc, "/Config/MaxDownloads", ref result.MaxDownloads);
                SetIntFromNode(doc, "/Config/CheckFeedInterval", ref result.CheckFeedInterval);               
                SetBoolFromNode(doc, "/Config/ShowBalloons", ref result.ShowBalloons);
                object tmp = GetEnumFromNode(doc, "/Config/BalloonFunction", typeof(BalloonFunction));
                if (tmp != null)
                    result.BalloonFunction = (BalloonFunction)tmp;
                //SetBoolFromNode(doc, "/Config/ShowDeleteItemWarning", ref result.ShowDeleteItemWarning);
                SetBoolFromNode(doc, "/Config/StartMinimised", ref result.StartMinimised);
                bool launchOnStartup = false;
                SetBoolFromNode(doc, "/Config/LaunchOnStartup", ref launchOnStartup);
                result.LaunchOnStartup = launchOnStartup;
                SetBoolFromNode(doc, "/Config/ShowInTaskbarWhenMinimized", ref result.ShowInTaskbarWhenMinimized);
                tmp = GetEnumFromNode(doc, "/Config/SearchMode", typeof(SearchMode));
                if (tmp != null)
                    result.SearchMode = (SearchMode)tmp;
                //SetBoolFromNode(doc, "/Config/HideSkippedItems", ref result.HideSkippedItems);
                SetStringFromNode(doc, "/Config/SyncFolder", ref result.SyncFolder);
                SetBoolFromNode(doc, "/Config/AutoSync", ref result.AutoSync);
                SetStringFromNode(doc, "/Config/SyncVolumeLabel", ref result.SyncVolumeLabel);
                //SetBoolFromNode(doc, "/Config/AddAlbumTag", ref result.AddAlbumTag);
                //SetBoolFromNode(doc, "/Config/AddGenreTag", ref result.AddGenreTag);
                SetBoolFromNode(doc, "/Config/CheckForNewVersion", ref result.CheckForNewVersion);
                SetIntFromNode(doc, "/Config/MaxBandwidthInBytes", ref result.MaxBandwidthInBytes);
                SetStringFromNode(doc, "/Config/SyncedFileTypes", ref result.SyncedFileTypes);
                SetStringFromNode(doc, "/Config/DynamicOPMLSource", ref result.DynamicOPMLSource);
                SetBoolFromNode(doc, "/Config/DynamicOPMLJustGetLatest", ref result.DynamicOPMLJustGetLatest);
                SetBoolFromNode(doc, "/Config/DynamicOPMLDeleteFiles", ref result.DynamicOPMLDeleteFiles);
                SetIntFromNode(doc, "/Config/FeedViewSortColumn", ref result.FeedViewSortColumn);
                SetIntFromNode(doc, "/Config/ItemViewSortColumn", ref result.ItemViewSortColumn);
                SetBoolFromNode(doc, "/Config/FeedSortAscending", ref result.FeedSortAscending);
                SetBoolFromNode(doc, "/Config/ItemSortAscending", ref result.ItemSortAscending);
                SetBoolFromNode(doc, "/Config/WritePlaylists", ref result.WritePlaylists);

                SetStringListFromNode(doc, "/Config/TitleTags", result.SavedTitleTags);
                SetStringListFromNode(doc, "/Config/AlbumTags", result.SavedAlbumTags);
                SetStringListFromNode(doc, "/Config/ArtistTags", result.SavedArtistTags);
                SetStringListFromNode(doc, "/Config/GenreTags", result.SavedGenreTags);
                SetStringListFromNode(doc, "/Config/FilenameTags", result.SavedFilenameTags);

                SetIntFromNode(doc, "/Config/MainSplitterLocation", ref result.MainSplitterLocation);
                SetIntFromNode(doc, "/Config/MainFormWidth", ref result.MainFormWidth);
                SetIntFromNode(doc, "/Config/MainFormHeight", ref result.MainFormHeight);
                SetIntFromNode(doc, "/Config/MainFormLeft", ref result.MainFormLeft);
                SetIntFromNode(doc, "/Config/MainFormTop", ref result.MainFormTop);  

                XmlNode scheduleNode = doc.SelectSingleNode("/Config/Schedule");
                if (scheduleNode != null)
                {
                    int day = 0;
                    foreach (XmlNode dayNode in scheduleNode.ChildNodes)
                    {
                        string dayStr = dayNode.InnerText;
                        string[] hours = dayStr.Split(' ');
                        if (hours.Length != 24)
                            throw new Exception();

                        for(int hour = 0; hour < 24; hour ++)
                            result.Schedule[hour, day] = hours[hour] == "1" ? true : false;

                        day++;
                    }
                }

                //
                // Adjust to current drive letter
                //

                if (result.PortableMode)
                {
                    string drive = Path.GetPathRoot(Application.StartupPath);

                    result.CompletedFilesBaseDirectory = drive + result.CompletedFilesBaseDirectory.Substring(3);
                    result.DownloadBaseDirectory = drive + result.DownloadBaseDirectory.Substring(3);
                }

                return result;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Exception in Config.Load: " + ex.Message);
                MessageBox.Show("The .config file appears to have become corrupted. As a result the configuration has been reset to the default settings. Please send a bug report by selecting 'Help->Report a Bug' from the main menu. We Aplogise for the inconvenience.", "Error Loading Configuration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return new Config();
            }
        }

        private static bool TryParseStringList(XmlNode listNode, List<string> list)
        {
            if (listNode != null)
            {
                list.Clear();

                foreach (XmlNode itemNode in listNode.ChildNodes)
                    list.Add(itemNode.InnerText);

                return true;
            }

            return false;
        }

        private static string SerializeStringList(IList<string> stringList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<list>");

            if (stringList != null)
            {
                foreach (string str in stringList)
                    sb.AppendFormat("<string>{0}</string>", str);
            }

            sb.Append("</list>");

            return sb.ToString();
        }

        private static bool? SetBoolFromNode(XmlDocument doc, string xpath, ref bool field)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            if (node == null)
                return null;

            if (bool.TryParse(node.InnerText, out field))
                return field;

            return null;
        }

        private static string SetStringFromNode(XmlDocument doc, string xpath, ref string field)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            if (node == null)
                return null;

            field = node.InnerText;

            return field;
        }

        private static int? SetIntFromNode(XmlDocument doc, string xpath, ref int field)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            if (node == null)
                return null;

            if (int.TryParse(node.InnerText, out field))
                return field;

            return null;
        }

        private static List<string> SetStringListFromNode(XmlDocument doc, string xpath, List<string> list)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            if (node == null)
                return null;

            if (node.ChildNodes.Count == 0)
                return null;            

            if (TryParseStringList(node.ChildNodes[0], list))
                return list;
            
            return null;
        }

        private static object GetEnumFromNode(XmlDocument doc, string xpath, Type type)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            if (node == null)
                return null;

            try
            {
                return Enum.Parse(type, node.InnerText);
            }
            catch(Exception ex) 
            {
                System.Diagnostics.Trace.TraceError("Exception in method GetEnumFromNode: " + ex.Message + ". text: " + node.InnerText + ". type: " + type.Name);
                return null;
            }
        }

    }
}
