using System;
using System.Threading.Tasks;
//using YoutubeExplode.DemoConsole.Utils;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace YoutubeExplode.DemoConsole
{
    // This demo prompts for video ID and downloads one media stream.
    // It's intended to be very simple and straight to the point.
    // For a more involved example - check out the WPF demo.

    public static class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Start");

            T tt = new();

            var x = await tt.Meth();
            Console.WriteLine("Finish");
        }
        //public static async Task<int> Meth()

        public class T
        {
            public async Task<int> Meth()

            {

                var youtube = new YoutubeClient();
                var videoId = VideoId.Parse("https://www.youtube.com/watch?v=A7MNA-qMOMM");
                Console.WriteLine("getting manifest");
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
                Console.WriteLine("got manifest");
                var streamInfo = streamManifest.GetMuxedStreams().TryGetWithHighestVideoQuality();
                if (streamInfo is null)
                {
                    Console.Error.WriteLine("This video has no muxed streams.");
                    return 1;
                }

                Console.Write($"Downloading stream: {streamInfo.VideoQuality.Label} / {streamInfo.Container.Name}... ");

                var fileName = $"{videoId}.{streamInfo.Container.Name}";

                // using (var progress = new InlineProgress()) // display progress in console
                await youtube.Videos.Streams.DownloadAsync(streamInfo, fileName); //, progress);

                Console.WriteLine($"Video saved to '{fileName}'");

                return 0;
            }

        }

        internal class InlineProgress : IProgress<double>, IDisposable
        {
            private readonly int _posX;
            private readonly int _posY;

            public InlineProgress()
            {
                _posX = Console.CursorLeft;
                _posY = Console.CursorTop;
            }

            public void Report(double progress)
            {
                Console.SetCursorPosition(_posX, _posY);
                Console.WriteLine($"{progress:P1}");
            }

            public void Dispose()
            {
                Console.SetCursorPosition(_posX, _posY);
                Console.WriteLine("Completed ✓");
            }
        }
    }
}
