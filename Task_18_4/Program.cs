using System;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using System.Collections.Generic;

namespace Task_18_4     
{
    class Program
    {
        static async Task Main()
        {
            const string defaultVideoName = "https://www.youtube.com/watch?v=A7MNA-qMOMM";
            string videoName = defaultVideoName;
            Receiver receiver = new();
            Sender sender = new();

            while (true)
            {
                Console.Write("\nВведите ссылку на видео на youtibe.\nЕсли нажмете Enter, получите ссылку по умолчанию (A7MNA-qMOMM): ");
                videoName = Console.ReadLine();
                if (string.IsNullOrEmpty(videoName)) 
                    videoName = defaultVideoName; 

                Console.Write("Укажите действие:\n1 - получить информацию о видео\n2 - загрузить видео\nВаш выбор: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        GetInfoCommand infoCommand = new(receiver, videoName);
                        sender.SetCommand(infoCommand);
                        await sender.Execute();
                        break;

                    case "2":
                        DownloadCommand downloadComand = new(receiver, videoName);
                        sender.SetCommand(downloadComand);
                        await sender.Execute();
                        break;

                    default: return;
                }
            }
        }

        internal class Sender
        {
            ICommand command;
            public void SetCommand(ICommand command) => this.command = command;
            public async Task Execute() => await command.Execute();
        }

        public interface ICommand
        {
            public Task Execute();
        }

        public class GetInfoCommand : ICommand
        {
            Receiver receiver;
            public GetInfoCommand(Receiver receiver, string videoName)
            {
                this.receiver = receiver;
                this.receiver.VideoName = videoName;
            }

            public async Task Execute() => receiver.GetVideoInfo();
        }

        public class DownloadCommand : ICommand
        {
            Receiver receiver;
            public DownloadCommand(Receiver receiver, string videoName)
            {
                this.receiver = receiver;
                this.receiver.VideoName = videoName;
            }

            public async Task Execute() => await receiver.Download();
        }

        internal class Receiver
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
                    Console.WriteLine($"No video \"{this.VideoName}\" found. Exception Message: {ex.Message}");
                    return false;
                }
            }
            public void GetVideoInfo()
            {
                if (!ValidateVideo()) return;

                var info = youtube.Videos.GetAsync(videoId);
                Console.WriteLine(info.Result.Description);
            }
            public async Task Download()
            {
                if (!ValidateVideo()) return;

                try
                {
                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
                    var streamInfo = streamManifest.GetMuxedStreams().TryGetWithHighestVideoQuality();
                    if (streamInfo is null)
                    {
                        Console.Error.WriteLine("This video has no muxed streams.");
                        return;
                    }

                    Console.Write($"Downloading stream: {streamInfo.Container.Name}");
                    var fileName = $"{videoId}.{streamInfo.Container.Name}";
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, fileName);
                    Console.WriteLine($"Video saved to '{fileName}'");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occured while downloading: {ex.Message}");
                }
            }
        }
    }
}