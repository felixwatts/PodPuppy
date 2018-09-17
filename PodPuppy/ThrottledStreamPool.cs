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
using System.Threading;

namespace PodPuppy
{
    /// <summary>
    /// Allows you to limit the combined speed of a group of streams. Streams may be added
    /// or removed from the group at any time. Used for bandwidth limiting the entire application.
    /// </summary>
    public class ThrottledStreamPool
    {
        public const long Unlimited = 0;

        private long _maximumBytesPerSecond = 0;
        private long _byteCount = 0;
        private long _start = 0;

        public ThrottledStreamPool()
        {
        }

        public long MaximumBytesPerSecond
        {
            get { return _maximumBytesPerSecond; }
            set 
            { 
                _maximumBytesPerSecond = value;
                Reset();
            }
        }

#if DEBUG
        public int ActualKBPerSec
        {
            get
            {
                long elapsedMilliseconds = CurrentMilliseconds - _start;

                if (elapsedMilliseconds > 0)
                {
                    // Calculate the current bps.
                    long bps = _byteCount * 1000L / elapsedMilliseconds;
                    return (int)(bps / 1024);
                }
                else return 0;
            }
        }
#endif

        /// <summary>
        /// Gets the current milliseconds.
        /// </summary>
        /// <value>The current milliseconds.</value>
        protected static long CurrentMilliseconds
        {
            get
            {
                return Environment.TickCount;
            }
        }

        /// <summary>
        /// Wraps the given stream returning a stream who's read/write speed is managed
        /// by the ThrottledStreamPool.
        /// </summary>
        /// <param name="strm"></param>
        /// <returns></returns>
        public Stream AddStream(Stream strm)
        {
            return new ThrottledStream(strm, this);
        }

        /// <summary>
        /// Given the number of bytes that have just been read from/written to one of the streams in
        /// the pool, returns the number of milisecods that the stream should wait before requesting/writing
        /// more. This time is calculated so that the average combined speed of all streams matches
        /// the MaximumBytesPerSecond property of the pool.
        /// </summary>
        /// <param name="bufferSizeInBytes">How many bytes were just read from this stream.</param>
        /// <returns>The number of millisecods to wait before requesting/writing more data.</returns>
        private long Throttle(long bufferSizeInBytes)
        {
            lock (this)
            {

                // Make sure the buffer isn't empty.
                if (_maximumBytesPerSecond <= 0 || bufferSizeInBytes <= 0)
                {
                    return 0;
                }

                _byteCount += bufferSizeInBytes;
                long elapsedMilliseconds = CurrentMilliseconds - _start;

                if (elapsedMilliseconds > 0)
                {
                    // Calculate the current bps.
                    long bps = _byteCount * 1000L / elapsedMilliseconds;

                    // If the bps are more then the maximum bps, try to throttle.
                    if (bps > _maximumBytesPerSecond)
                    {
                        // Calculate the time to sleep.
                        long wakeElapsed = _byteCount * 1000L / _maximumBytesPerSecond;
                        long toSleep = wakeElapsed - elapsedMilliseconds;

                        if (toSleep > 1)
                        {
                            // A sleep has been done, reset.
                            Reset();

                            // add a small random wait
                            //toSleep += ((CurrentMilliseconds & 8) << 5);

                            return toSleep;
                        }
                    }
                }

                return 0;
            }
        }        

        /// <summary>
        /// Will reset the bytecount to 0 and reset the start time to the current time.
        /// </summary>
        protected void Reset()
        {
            lock (this)
            {
                long difference = CurrentMilliseconds - _start;

                // Only reset counters when a known history is available of more then 1 second.
                if (difference > 1000)
                {
                    _byteCount = 0;
                    _start = CurrentMilliseconds;
                }
            }
        }

        /// <summary>
        /// Wraps a stream to provide a stream that belongs to a ThrottledStreamPool and throttles
        /// itself accordingly.
        /// </summary>
        private class ThrottledStream : Stream
        {
            /// <summary>
            /// The base stream.
            /// </summary>
            private Stream _baseStream;

            private ThrottledStreamPool _pool;

            public ThrottledStream(Stream baseStream, ThrottledStreamPool pool)
            {
                _baseStream = baseStream;
                _pool = pool;
            }

            /// <summary>
            /// Gets a value indicating whether the current stream supports reading.
            /// </summary>
            /// <returns>true if the stream supports reading; otherwise, false.</returns>
            public override bool CanRead
            {
                get
                {
                    return _baseStream.CanRead;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the current stream supports seeking.
            /// </summary>
            /// <value></value>
            /// <returns>true if the stream supports seeking; otherwise, false.</returns>
            public override bool CanSeek
            {
                get
                {
                    return _baseStream.CanSeek;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the current stream supports writing.
            /// </summary>
            /// <value></value>
            /// <returns>true if the stream supports writing; otherwise, false.</returns>
            public override bool CanWrite
            {
                get
                {
                    return _baseStream.CanWrite;
                }
            }

            /// <summary>
            /// Gets the length in bytes of the stream.
            /// </summary>
            /// <value></value>
            /// <returns>A long value representing the length of the stream in bytes.</returns>
            /// <exception cref="T:System.NotSupportedException">The base stream does not support seeking. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            public override long Length
            {
                get
                {
                    return _baseStream.Length;
                }
            }

            /// <summary>
            /// Gets or sets the position within the current stream.
            /// </summary>
            /// <value></value>
            /// <returns>The current position within the stream.</returns>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.NotSupportedException">The base stream does not support seeking. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            public override long Position
            {
                get
                {
                    return _baseStream.Position;
                }
                set
                {
                    _baseStream.Position = value;
                }
            }

            /// <summary>
            /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
            /// </summary>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
            public override void Flush()
            {
                _baseStream.Flush();
            }

            /// <summary>
            /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
            /// </summary>
            /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
            /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
            /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
            /// <returns>
            /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">The sum of offset and count is larger than the buffer length. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            /// <exception cref="T:System.NotSupportedException">The base stream does not support reading. </exception>
            /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
            public override int Read(byte[] buffer, int offset, int count)
            {
                long sleepFor = _pool.Throttle(count);
                if (sleepFor > 0)
                {
                    try
                    {
                        Thread.Sleep((int)sleepFor);
                    }
                    catch (ThreadAbortException) { }
                }

                return _baseStream.Read(buffer, offset, count);
            }

            /// <summary>
            /// Sets the position within the current stream.
            /// </summary>
            /// <param name="offset">A byte offset relative to the origin parameter.</param>
            /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"></see> indicating the reference point used to obtain the new position.</param>
            /// <returns>
            /// The new position within the current stream.
            /// </returns>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.NotSupportedException">The base stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            public override long Seek(long offset, SeekOrigin origin)
            {
                return _baseStream.Seek(offset, origin);
            }

            /// <summary>
            /// Sets the length of the current stream.
            /// </summary>
            /// <param name="value">The desired length of the current stream in bytes.</param>
            /// <exception cref="T:System.NotSupportedException">The base stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            public override void SetLength(long value)
            {
                _baseStream.SetLength(value);
            }

            /// <summary>
            /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
            /// </summary>
            /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
            /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
            /// <param name="count">The number of bytes to be written to the current stream.</param>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.NotSupportedException">The base stream does not support writing. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
            /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
            /// <exception cref="T:System.ArgumentException">The sum of offset and count is greater than the buffer length. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
            public override void Write(byte[] buffer, int offset, int count)
            {
                long sleepFor = _pool.Throttle(count);

                _baseStream.Write(buffer, offset, count);
            }

            public override void Close()
            {
                _baseStream.Close();
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return _baseStream.ToString();
            }
        }
    }
}
