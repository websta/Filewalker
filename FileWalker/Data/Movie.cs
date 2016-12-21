using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileWalker.Data;

namespace FileWalker
{
    class Movie
    {
        public List<VideoStream> VideoStream;
        public List<AudioStream> AudioStream;
        public List<TextStream> TextStream;
        public string FileSize;
        public int AudioStreamCount;
        public int VideoStreamCount;
        public int TextStreamCount;
        public string FileNameWithoutExtension;
        public string FileName;
        public string FilePath;
        public string DurationMs;
        public string Duration;
        public string Format;
        public string EncodedApplication;
        public string EncodedLibrary;
        public string FormatVersion;
        public string BitRateOverall;
        public string DateEncoded;
        /// <summary>
        /// Init Movie
        /// </summary>
        public Movie()
        {
            VideoStream = new List<VideoStream>();
            AudioStream = new List<AudioStream>();
            TextStream = new List<TextStream>();
        }
    }
}
