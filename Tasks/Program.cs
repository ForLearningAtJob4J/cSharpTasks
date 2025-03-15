using System.Diagnostics;

class Program
{
    static async Task Main(string[] args)
    {
        string folderPath;

        if (args.Length != 1)
        {
            folderPath = "TestFiles";
        }
        else
        {
            folderPath = args[0];
        }

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine($"Папка {folderPath} не существует!");
            return;
        }

        string[] files = Directory.GetFiles(folderPath);

        var sw = Stopwatch.StartNew();
        sw.Start();

        int totalCount = 0;
        Task[] tasks = Array.ConvertAll(files, filePath => Task.Run(async () =>
        {
            using (StreamReader reader = new(filePath))
            {
                string? line;
                int spaceCount = 0;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    spaceCount += line.Count(s => s == ' ');
                }

                totalCount += spaceCount;
                Console.WriteLine($"Файл {Path.GetFileName(filePath)} содержит {spaceCount} пробелов.");
            }
        }));

        await Task.WhenAll(tasks);
        Console.WriteLine($"Все файлы содержит {totalCount} пробелов.");
        sw.Stop();
        Console.WriteLine($"Времени понадобилось: " + sw.Elapsed);
    }
}