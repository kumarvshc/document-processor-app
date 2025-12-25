// See https://aka.ms/new-console-template for more information
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

Console.WriteLine("=== Document Processor Console Client ===\n\n");

await Task.Delay(15000);

var apiBaseUrl = "https://localhost:7088";
var directoryToScan = @"C:\DocProcessConsoleTest";

Console.WriteLine($"API URL: {apiBaseUrl}");
Console.WriteLine($"Scanning directory {directoryToScan}");

if(!Directory.Exists(directoryToScan))
{
    Console.WriteLine($"Error: Directory '{directoryToScan}' does not exist.");
    return;
}


using var httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };

var successCount = 0;
var failCount = 0;

bool continueProcessing = false;

do
{
    var txtFiles = Directory.GetFiles(directoryToScan, "*.txt", SearchOption.TopDirectoryOnly);

    if (txtFiles.Length == 0)
    {
        Console.WriteLine("No .txt files found in the directory.");
        return;
    }

    Console.WriteLine($"Found {txtFiles.Length} .txt file(s)\n");


    foreach (var filePath in txtFiles)
    {
        var fileName = Path.GetFileName(filePath);
        Console.WriteLine($"Processing: {fileName}");

        try
        {
            var content = await System.IO.File.ReadAllTextAsync(filePath);

            if (content.Length > 1024)
            {
                content = content[..1024];
                Console.WriteLine("WARNING ::: Content truncated to 1024 characters");
            }

            var fileInfo = new FileInfo(filePath);

            var request = new
            {
                FileName = fileName,
                Content = content,
                MaxContentSize = 1024,
                Metadata = new Dictionary<string, string>
            {
                {"SourcePath" , filePath},
                {"CreatedDateTime", fileInfo.CreationTime.ToShortDateString() },
                {"LastAccessDateTime", fileInfo.LastAccessTime.ToShortDateString() }
            }
            };

            var response = await httpClient.PostAsJsonAsync("/api/document", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"SUCCESS: {response.StatusCode}");
                Console.WriteLine($"Response: {result}");
                successCount++;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"FAILED: {response.StatusCode}");
                Console.WriteLine($"Error: {error}");
                failCount++;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            failCount++;
        }
        Console.WriteLine();
    }

    Console.WriteLine("=== Summary ===");
    Console.WriteLine($"Total files: {txtFiles.Length}");
    Console.WriteLine($"Successful: {successCount}");
    Console.WriteLine($"Failed: {failCount}");

    Console.WriteLine("\nDo you want to process files again? (Y/N): ");
    var input = Console.ReadLine();
    continueProcessing = input?.Trim().ToUpper() == "Y";

} while (continueProcessing);

   

Console.WriteLine("\nBye!");