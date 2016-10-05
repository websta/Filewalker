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
        public List<VideoStream> VideoStream { get; set; }
        public List<AudioStream> AudioStream { get; set; }
        public List<TextStream> TextStream { get; set; }

        public Movie()
        {
            VideoStream = new List<VideoStream>();
            AudioStream = new List<AudioStream>();
            TextStream = new List<TextStream>();
        }
    }
}
