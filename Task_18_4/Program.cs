using System;
using System.Threading.Tasks;
using YoutubeExplode;
//using YoutubeExplode.DemoConsole.Utils;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

/// <summary>
/// 
/// </summary>
namespace Task_18_4     
{
    class Program
    {
        static async Task Main()
        {
            string action = "3";
            string videoName = "https://www.youtube.com/watch?v=A7MNA-qMOMM";

            //while (true)
            //{
            //    Console.Write("Укажите действие:\n 1 - получить информацию\n2 - загрузить\n3 - завершщить работу\nВаш выбор: ");

            //}

            //Receiver receiver = new();
            Rcvr receiver = new();
            GetInfoCommand infoCommand = new(receiver, videoName);

            Sender sender = new();
            sender.SetCommand(infoCommand);
            await sender.Execute();

            Console.WriteLine("\nDownloading part\n");
            DownloadCommand downloadComand = new(receiver, videoName);
            sender.SetCommand(downloadComand);
            await sender.Execute();
        }

        internal class Sender
        {
            ICommand command;
            public void SetCommand(ICommand command)
            {
                this.command = command;
            }
            public async Task Execute()
            {
                var x = await command.Execute();
            }
        }

        public interface ICommand
        {
            public Task<int> Execute();
        }

        public class GetInfoCommand : ICommand
        {
            Rcvr receiver;
            public GetInfoCommand(Rcvr receiver, string videoName)
            {
                this.receiver = receiver;
                this.receiver.VideoName = videoName;
            }

            public async Task<int> Execute()
            {
                receiver.GetVideoInfo();
                return 0;
            }
        }

        public class DownloadCommand : ICommand
        {
            Rcvr receiver;
            string videoName;
            public DownloadCommand(Rcvr receiver, string videoName)
            {
                this.receiver = receiver;
                this.receiver.VideoName = videoName;
            }

            public async Task<int> Execute()
            {
                var x = await receiver.Download();
                return 0;
            }
        }

        internal class Rcvr
        {
            YoutubeClient youtube = new();
            public string VideoName { get; set; }
            VideoId videoId;

            bool ValidateVideo()
            {
                try
                {
                    this.videoId = VideoId.Parse(this.VideoName);
                    Console.WriteLine($"Found video: {videoId}");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"No video \"{this.VideoName}\" found");
                    return false;
                }
            }
            public void GetVideoInfo()
            {
                if (!ValidateVideo()) return;

                var info = youtube.Videos.GetAsync(videoId);
                Console.WriteLine(info.Result.Author);
            }
            public async Task<int> Download()
            {
                //var youtube = new YoutubeClient();
                if (!ValidateVideo()) return 1;

                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
                var streamInfo = streamManifest.GetMuxedStreams().TryGetWithHighestVideoQuality();
                if (streamInfo is null)
                {
                    Console.Error.WriteLine("This video has no muxed streams.");
                    return 1;
                }

                Console.Write($"Downloading stream: {streamInfo.Container.Name}");
                var fileName = $"{videoId}.{streamInfo.Container.Name}";
                await youtube.Videos.Streams.DownloadAsync(streamInfo, fileName);
                Console.WriteLine($"Video saved to '{fileName}'");
                return 0;
            }
        }
    }
}