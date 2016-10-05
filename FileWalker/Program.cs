using System;
using System.IO;
using FileWalker.Data;
using MediaInfoLib;
using Newtonsoft.Json;

namespace FileWalker
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            var mi = new MediaInfo();
            int audioStreamCount, videoStreamCount, textStreamCount;

            var files = Directory.GetFiles("C:\\Users\\websta\\Videos");
            Console.Write(String.Join("\r\n", files));

            mi.Open("C:\\Users\\websta\\Videos\\batman1.mkv");
            var toDisplay = program.ExtractGeneralFileInformation(mi, out audioStreamCount, out videoStreamCount, out textStreamCount);
            var movie = new Movie();

            for (var i = 0; i < audioStreamCount; i++)
            {
                movie.AudioStream.Add(program.ExtractAudioStreamInformation(mi, i));
            }

            for (int i = 0; i < videoStreamCount; i++)
            {
                movie.VideoStream.Add(program.ExtractVideoStreamInformation(mi, i));
            }

            for (int i = 0; i < textStreamCount; i++)
            {
                movie.TextStream.Add(program.ExtractTextStreamInformation(mi, i));
            }
            Console.Write(JsonConvert.SerializeObject(movie).ToString());
            Console.Read();
        }

        private AudioStream ExtractAudioStreamInformation(MediaInfo mi, int i)
        {
            //ToDisplay += "\r\n\r\nInformation from AudioStream " + i + "\r\n";
            //ToDisplay += MI.Get(MediaInfoLib.StreamKind.Audio, i, "Inform");
            var toDisplay = "\r\nFormat\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "Format");
            toDisplay += "\r\nCodec ID\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "CodecID");
            toDisplay += "\r\nDuration\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "DURATION");
            toDisplay += "\r\nBit rate mode\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "BitRate_Mode/String");
            toDisplay += "\r\nBit rate\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "BitRate");
            toDisplay += "\r\nSampling rate\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "SamplingRate");
            toDisplay += "\r\nChannel(s)\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "Channel(s)");
            toDisplay += "\r\nTitle\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "Title");
            toDisplay += "\r\nLanguage\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "Language");
            toDisplay += "\r\nNUMBER_OF_BYTES\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "NUMBER_OF_BYTES");
            toDisplay += "\r\nDefault\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "Default");
            toDisplay += "\r\nForced\t";
            toDisplay += mi.Get(StreamKind.Audio, i, "Forced");

            var audioStream = new AudioStream {Name = mi.Get(StreamKind.Audio, i, "Title")};

            return audioStream;
        }

        private VideoStream ExtractVideoStreamInformation(MediaInfo mi, int i)
        {
            //ToDisplay += "\r\n\r\nInformation from VideoStream " + i + "\r\n";
            //ToDisplay += MI.Get(MediaInfoLib.StreamKind.Video, i, "Inform");
            var toDisplay = "\r\nFormat\t";
            toDisplay += mi.Get(StreamKind.Video, i, "Format");
            toDisplay += "\r\nCodec ID\t";
            toDisplay += mi.Get(StreamKind.Video, i, "CodecID");
            toDisplay += "\r\nBit rate\t";
            toDisplay += mi.Get(StreamKind.Video, i, "BitRate");
            toDisplay += "\r\nWidth\t";
            toDisplay += mi.Get(StreamKind.Video, i, "Width");
            toDisplay += "\r\nHeight\t";
            toDisplay += mi.Get(StreamKind.Video, i, "Height");
            toDisplay += "\r\nDisplayAspectRatio\t";
            toDisplay += mi.Get(StreamKind.Video, i, "DisplayAspectRatio");
            toDisplay += "\r\nNUMBER_OF_BYTES\t";
            toDisplay += mi.Get(StreamKind.Video, i, "NUMBER_OF_BYTES");
            toDisplay += "\r\nDuration\t";
            toDisplay += mi.Get(StreamKind.Video, i, "DURATION");
            var stream = new VideoStream { Name = mi.Get(StreamKind.Video, i, "Title") };

            return stream;
        }

        private TextStream ExtractTextStreamInformation(MediaInfo mi, int i)
        {
            //ToDisplay += "\r\n\r\nInformation from TextStream " + i + "\r\n";
            //ToDisplay += MI.Get(MediaInfoLib.StreamKind.Text, i, "Inform");
            var toDisplay = "\r\nFormat\t";
            toDisplay += mi.Get(StreamKind.Text, i, "Format");
            toDisplay += "\r\nCodec ID\t";
            toDisplay += mi.Get(StreamKind.Text, i, "CodecID");
            toDisplay += "\r\nLanguage\t";
            toDisplay += mi.Get(StreamKind.Text, i, "Language");
            toDisplay += "\r\nDefault\t";
            toDisplay += mi.Get(StreamKind.Text, i, "Default");
            toDisplay += "\r\nForced\t";
            toDisplay += mi.Get(StreamKind.Text, i, "Forced");
            toDisplay += "\r\nNUMBER_OF_BYTES\t";
            toDisplay += mi.Get(StreamKind.Text, i, "NUMBER_OF_BYTES");
            var stream = new TextStream { Name = mi.Get(StreamKind.Text, i, "Title") };

            return stream;
        }

        private String ExtractGeneralFileInformation(MediaInfo mi, out int audioStreamCount, out int videoStreamCount, out int textStreamCount)
        {
            var fileSize = mi.Get(0, 0, "FileSize");
            var toDisplay = "\r\n\r\nGet with Stream=General and Parameter='FileSize'\r\n";
            toDisplay += fileSize;

            var generalStreamCount = mi.Count_Get(StreamKind.General);
            toDisplay += "\r\n\r\nCount_Get with StreamKind=Stream_General\r\n";
            toDisplay += generalStreamCount;

            audioStreamCount = mi.Count_Get(StreamKind.Audio);
            toDisplay += "\r\n\r\nCount_Get with StreamKind=Stream_Audio\r\n";
            toDisplay += audioStreamCount;

            videoStreamCount = mi.Count_Get(StreamKind.Video);
            toDisplay += "\r\n\r\nCount_Get with StreamKind=Stream_Video\r\n";
            toDisplay += videoStreamCount;

            textStreamCount = mi.Count_Get(StreamKind.Text);
            toDisplay += "\r\n\r\nCount_Get with StreamKind=Stream_Text\r\n";
            toDisplay += textStreamCount;

            //ToDisplay += "\r\n\r\nGet with Stream=General and Parameter='AudioCount'\r\n";
            //ToDisplay += MI.Get(StreamKind.General, 0, "AudioCount");

            //ToDisplay += "\r\n\r\nGet Video Format\r\n";
            //ToDisplay += MI.Get(MediaInfoLib.StreamKind.Video, 0, "Inform");

            //ToDisplay += "\r\n\r\nWidth\r\n";
            //ToDisplay += MI.Get(MediaInfoLib.StreamKind.Video, 0, "Width");

            //ToDisplay += "\r\n\r\nHeight\r\n";
            //ToDisplay += MI.Get(MediaInfoLib.StreamKind.Video, 0, "Height");


            //ToDisplay += MI.Get(MediaInfoLib.StreamKind.General, 0, "Format");

            return toDisplay;
        }
    }
}
