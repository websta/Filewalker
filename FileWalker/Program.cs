using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using FileWalker.Data;
using MediaInfoLib;
using Newtonsoft.Json;

namespace FileWalker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var program = new Program();

            var mi = new MediaInfo();
            var startDirectory = "D:\\video-wihsy";


            var files = program.ExtractFiles(startDirectory);
            files.ForEach(fileName =>
            {
                mi.Open(fileName);
                //Console.WriteLine(mi.Inform());
                var movie = program.ExtractGeneralFileInformation(mi);
                movie = program.ExtractAudioStreamInformation(movie, mi);
                movie = program.ExtractVideoStreamInformation(movie, mi);
                movie = program.ExtractTextStreamInformation(movie, mi);
                movie = program.ExtractFileNameAndFilePath(movie, fileName);
                //Console.Write(JsonConvert.SerializeObject(movie).ToString());
                //Console.Write("-----------------------------------------------");
                // serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(movie.FilePath + "\\info.json"))
                {
                    file.Write(JsonConvert.SerializeObject(movie, Formatting.Indented));
                }
                var responseCode = program.PostObject("http://localhost:9200/movies/movie", JsonConvert.SerializeObject(movie));
                Console.WriteLine(responseCode);
            });
            Console.ReadLine();
        }


        private string GetMovieDatabaseInformation(string title, string year)
        {
            var result = "";



            return result;
        }

        private string PostObject(string postUrl, string payload)
        {
            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            if (payload != null)
            {
                request.ContentLength = payload.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(Encoding.UTF8.GetBytes(payload), 0, payload.Length);
                dataStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.StatusCode.ToString();
        }
        
        private List<string> ExtractFiles(string startDirectory)
        {
            var result = Directory.GetFileSystemEntries(startDirectory).ToList();
            // handle directories with recursive call
            var directories = result.Where(file => File.GetAttributes(file).HasFlag(FileAttributes.Directory)).ToList(); // extract all directories from list
            directories = directories.Where(directory => !directory.ToLower().Contains("additional")).ToList(); // skip all folders containing ADDITIONAL
            directories.ForEach(directory => result.AddRange(ExtractFiles(directory))); // RECURSIVE CALL FOR DIRECTORIES !
            // filter files
            result = result.Where(file => !File.GetAttributes(file).HasFlag(FileAttributes.Directory)).ToList(); // remove all directories
            result = result.Where(file => file.ToLower().EndsWith(".mkv")).ToList(); // skip all files not ending with MKV
            result = result.Where(file => !file.ToLower().Contains("additional")).ToList(); // skip all files containing ADDITIONAL
            // return files
            return result;
        }

        private Movie ExtractFileNameAndFilePath(Movie movie, string fileName)
        {
            movie.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            movie.FileName = Path.GetFileName(fileName);
            movie.FilePath = Path.GetDirectoryName(fileName);
            return movie;
        }

        private Movie ExtractAudioStreamInformation(Movie movie, MediaInfo mi)
        {
            for (var i = 0; i < movie.AudioStreamCount; i++)
                movie.AudioStream.Add(new AudioStream
                {
                    Format = mi.Get(StreamKind.Audio, i, "Format"),
                    Codec = mi.Get(StreamKind.Audio, i, "CodecID"),
                    Duration = mi.Get(StreamKind.Audio, i, "Duration"),
                    BitRateMode = mi.Get(StreamKind.Audio, i, "BitRate_Mode/String"),
                    BitRate = mi.Get(StreamKind.Audio, i, "BitRate"),
                    SamplingRate = mi.Get(StreamKind.Audio, i, "SamplingRate"),
                    Channels = mi.Get(StreamKind.Audio, i, "Channel(s)"),
                    Title = mi.Get(StreamKind.Audio, i, "Title"),
                    Language = mi.Get(StreamKind.Audio, i, "Language"),
                    Bytes = mi.Get(StreamKind.Audio, i, "StreamSize"),
                    Default = mi.Get(StreamKind.Audio, i, "Default"),
                    Forced = mi.Get(StreamKind.Audio, i, "Forced")
                });

            return movie;
        }

        private Movie ExtractVideoStreamInformation(Movie movie, MediaInfo mi)
        {
            //Console.WriteLine("ExtractVideo, BitRate Nominal: " + mi.Get(StreamKind.Video, 0, "BitRate_Nominal"));
            //Console.WriteLine("ExtractVideo, Format/Info: " + mi.Get(StreamKind.Video, 0, "Format/Info"));
            //Console.WriteLine("ExtractVideo, MuxingMode: " + mi.Get(StreamKind.Video, 0, "MuxingMode"));
            for (var i = 0; i < movie.VideoStreamCount; i++)
            {
                var videoStream = new VideoStream
                {
                    Format = mi.Get(StreamKind.Video, i, "Format"),
                    Codec = mi.Get(StreamKind.Video, i, "CodecID"),
                    //BitRate = mi.Get(StreamKind.Video, i, "BitRate"),
                    Width = mi.Get(StreamKind.Video, i, "Width"),
                    Height = mi.Get(StreamKind.Video, i, "Height"),
                    DisplayAspectRatio = mi.Get(StreamKind.Video, i, "DisplayAspectRatio"),
                    //Bytes = mi.Get(StreamKind.Video, i, "StreamSize/String3"),
                    Bytes = mi.Get(StreamKind.Video, i, "StreamSize"),
                    Duration = mi.Get(StreamKind.Video, i, "Duration"),
                    Title = mi.Get(StreamKind.Video, i, "Title"),
                    BitrateNominal = mi.Get(StreamKind.Video, i, "BitRate_Nominal"),
                };


                movie.VideoStream.Add(videoStream);
            }
            return movie;
        }

        private Movie ExtractTextStreamInformation(Movie movie, MediaInfo mi)
        {

            for (var i = 0; i < movie.TextStreamCount; i++)
            {
                movie.TextStream.Add(new TextStream
                {
                    Title = mi.Get(StreamKind.Text, i, "Title"),
                    Format = mi.Get(StreamKind.Text, i, "Format"),
                    Codec = mi.Get(StreamKind.Text, i, "CodecID"),
                    Language = mi.Get(StreamKind.Text, i, "Language"),
                    Default = mi.Get(StreamKind.Text, i, "Default"),
                    Forced = mi.Get(StreamKind.Text, i, "Forced"),
                    Bytes = mi.Get(StreamKind.Text, i, "NUMBER_OF_BYTES")
                });


            }

            return movie;
        }

        private Movie ExtractGeneralFileInformation(MediaInfo mi)
        {
            //var fileSize = mi.Get(0, 0, "FileSize");
            //var toDisplay = "\r\n\r\nGet with Stream=General and Parameter='FileSize'\r\n";
            //toDisplay += fileSize;

            //var generalStreamCount = mi.Count_Get(StreamKind.General);
            //toDisplay += "\r\n\r\nCount_Get with StreamKind=Stream_General\r\n";
            //toDisplay += generalStreamCount;

            //audioStreamCount = mi.Count_Get(StreamKind.Audio);
            //toDisplay += "\r\n\r\nCount_Get with StreamKind=Stream_Audio\r\n";
            //toDisplay += audioStreamCount;

            //videoStreamCount = mi.Count_Get(StreamKind.Video);
            //toDisplay += "\r\n\r\nCount_Get with StreamKind=Stream_Video\r\n";
            //toDisplay += videoStreamCount;

            //textStreamCount = mi.Count_Get(StreamKind.Text);
            //toDisplay += "\r\n\r\nCount_Get with StreamKind=Stream_Text\r\n";
            //toDisplay += textStreamCount;
            //Console.WriteLine("General, FrameRate: ", mi.Get(StreamKind.General, 0, "FrameRate"));
            //Console.WriteLine("Video, BitRate: ", mi.Get(StreamKind.Video, 0, "BitRate"));
            //Console.WriteLine("Audio, StreamSize: ", mi.Get(StreamKind.Audio, 0, "StreamSize"));
            //Console.WriteLine("Audio, StreamSize: ", mi.Get(StreamKind.Audio, 1, "StreamSize"));
            //Console.WriteLine("Audio, StreamSize: ", mi.Get(StreamKind.Audio, 2, "StreamSize"));
            var movie = new Movie
            {
                FileSize = mi.Get(StreamKind.General, 0, "FileSize"),
                AudioStreamCount = mi.Count_Get(StreamKind.Audio),
                VideoStreamCount = mi.Count_Get(StreamKind.Video),
                TextStreamCount = mi.Count_Get(StreamKind.Text),
                DurationMs = mi.Get(StreamKind.General, 0, "Duration"),
                Duration = mi.Get(StreamKind.General, 0, "Duration/String3"),
                Format = mi.Get(StreamKind.General, 0, "Format"),
                FormatVersion = mi.Get(StreamKind.General, 0, "Format_Version"),
                EncodedApplication = mi.Get(StreamKind.General, 0, "Encoded_Application"),
                EncodedLibrary = mi.Get(StreamKind.General, 0, "Encoded_Library"),
                BitRateOverall = mi.Get(StreamKind.General, 0, "OverallBitRate"),
                DateEncoded = mi.Get(StreamKind.General, 0, "Encoded_Date"),

            };

            return movie;
        }
    }
}
