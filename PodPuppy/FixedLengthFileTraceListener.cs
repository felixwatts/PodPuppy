// PodPuppy - a simple podcast receiver for Windows
// Copyright (c) Felix Watts 2008 (felixwatts@gmail.com)
// https://github.com/felixwatts/PodPuppy
//
// This file is distributed under the Creative Commons Attribution-NonCommercial 4.0 International Licence
// https://creativecommons.org/licenses/by-nc/4.0/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace PodPuppy
{
    /// <summary>
    /// A trace listener that that just stores the last N trace messages. The trace
    /// messages are written to the specified file when the listener is closed.
    /// </summary>
    public class FixedLengthFileTraceListener : TraceListener, IDisposable
    {
        #region Private Fields

        private int _maxLines = 16;

        private bool _initialised = false;

        private bool _full = false;

        private int _numLines = 0;

        private Queue<string> _lines;

        private StringBuilder _currentLine;

        private string _filename;

        #endregion

        #region Construction

        public FixedLengthFileTraceListener(string filename)
        {
            _filename = Path.Combine(Statics.Config.SettingsDir, filename); 
            _lines = new Queue<string>();
            _currentLine = new StringBuilder();
        }

        #endregion

        #region Overrides

        public override void Write(string message)
        {
            _currentLine.Append(message);
        }

        public override void WriteLine(string message)
        {
            Initialise();

            if (_full)
                _lines.Dequeue();

            _currentLine.Append(message);

            _lines.Enqueue(_currentLine.ToString());

            _currentLine.Length = 0;

            if (!_full)
            {
                _numLines++;
                if (_numLines == _maxLines)
                    _full = true;
            }
        }

        public override void Close()
        {
            StreamWriter writer = new StreamWriter(_filename);
            foreach (string line in _lines)
                writer.WriteLine(line);

            writer.Flush();
            writer.Close();

            base.Close();
        }

        protected override string[] GetSupportedAttributes()
        {
            return new string[] { "initializeData", "maxLines" };
        }       

        #endregion

        #region Public Methods

        public string CurrentContent
        {
            get
            {
                Initialise();

                StringBuilder sb = new StringBuilder();
                foreach (string line in _lines)
                    sb.AppendLine(line);
                return sb.ToString();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Stuff that needs to be done once but after the Attributes
        /// property has been set (so can't be done during construction)
        /// </summary>
        private void Initialise()
        {
            if (_initialised)
                return;
            _initialised = true;

            if (Attributes.ContainsKey("maxLines"))
                int.TryParse(Attributes["maxLines"], out _maxLines);

            if (File.Exists(_filename))
            {
                StreamReader reader = null;
                try
                {
                    reader = new StreamReader(_filename);
                    while (!reader.EndOfStream)
                        WriteLine(reader.ReadLine());
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Close();
        }

        #endregion
    }
}
