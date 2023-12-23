using Serilog;
using SilexxFileDownloader;
using System.Globalization;

namespace SilexxFileDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            AppSettings appSettings = new AppSettings();
            var SourcePath = appSettings.SourcePath;
            var DestinationPath = appSettings.DestinationPath;
            var LoggingPath = appSettings.LoggingPath;

            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File(Path.Combine(LoggingPath, "log-.txt"), rollingInterval: RollingInterval.Day).CreateLogger();
            Log.Information("Started the application {0}", typeof(Program).Namespace);
            Console.WriteLine("Started the application {0}", typeof(Program).Namespace);

            try
            {
                var dir = DateTime.UtcNow.ToString("yyyyMMdd", CultureInfo.CreateSpecificCulture("en-US"));
                Directory.CreateDirectory(DestinationPath);

                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(Path.Combine(SourcePath, dir)))
                    {
                        var directory = new DirectoryInfo(Path.Combine(SourcePath, dir));
                        var optionFile = directory.GetFiles().OrderByDescending(f => f.LastAccessTimeUtc).ToList();
                        int count = 0;
                        int FileCount = 1;
                        List<string> optionFileLatest = new List<string>();
                        foreach (var option in optionFile)
                        {
                            var opt = option;
                            if (opt.Name.ToLower().Contains("churchill") && opt.Name.EndsWith(".csv") && opt.Name.ToLower().Contains("options"))
                            {
                                Console.WriteLine(FileCount.ToString() + " file is Options file.");
                                Log.Information(FileCount.ToString() + " file is Options file.");

                                if (count < 1)
                                {
                                    optionFileLatest.Add(opt.Name);
                                }
                                else
                                {
                                    Console.WriteLine("Took the latest Options file.");
                                    Log.Information("Took the latest Options file");
                                    break;
                                }
                                count++;
                            }
                            else
                            {
                                Console.WriteLine(FileCount.ToString() + " file isn't Options file.");
                                Log.Information(FileCount.ToString() + " file isn't Options file.");
                            }
                            FileCount++;
                        }

                        if (!File.Exists(Path.Combine(DestinationPath, optionFileLatest[0])))
                        {
                            File.Copy(Path.Combine(SourcePath, dir, optionFileLatest[0]), Path.Combine(DestinationPath, optionFileLatest[0]), true);
                            Log.Information("File transferred into the destination succussfully.");
                            Console.WriteLine("File transferred into the destination succussfully.");
                        }
                        else
                        {
                            Log.Error("File didn't transfer, it already exists in the Destination directory.");
                            Console.WriteLine("File didn't transfer, it already exists in the Destination directory.");      
                        }
                    }
                    else
                    {
                        Console.WriteLine("Date file does not exist in the Source directory.");
                        Log.Error("Date file doesn't exists in the Source directory.");
                    }
                }
                else
                {
                    Log.Error("Source directory doesn't exists.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex}");
            }
            finally
            {
                Log.Information("End the application {0}", typeof(Program).Namespace);
                Console.WriteLine("End the application {0}", typeof(Program).Namespace);
                Log.CloseAndFlush();
                Environment.Exit(0);
            }
        }
    }
}
