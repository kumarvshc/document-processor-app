// See https://aka.ms/new-console-template for more information
using DocumentProcessor.Console.Models;
using DocumentProcessor.Console.Serialization;
using DocumentProcessor.Constants;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;


var jsonOptions = new JsonSerializerOptions
{
    TypeInfoResolver = AppJsonContext.Default
};


Console.WriteLine("=== Document Processor Console Client ===\n\n");

Console.Write("Please enter the directory path to scan the text files for processing: ");

var directoryToScan = Console.ReadLine();

bool isInValidDirectoryPath = false;

do
{
    if (!Directory.Exists(directoryToScan))
    {
        isInValidDirectoryPath = true;

        Console.WriteLine($"Error: Directory '{directoryToScan}' does not exist.\n");

        Console.Write("Please enter the valid directory path to scan the text files for processing: ");

        directoryToScan = Console.ReadLine();
    }
    else
    {
        isInValidDirectoryPath = false;
    }
} while (isInValidDirectoryPath);

var apiBaseUrl = Constants.CONST_DOC_PROCESSOR_API_BASEURL;

Console.WriteLine($"\nAPI URL: {apiBaseUrl}");

using var httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };

int successCount = 0, failCount = 0;

bool continueProcessing = false;

do
{
    Console.WriteLine($"Scanning directory {directoryToScan}");

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

            var request = new AddDocumentRequest
            {
                FileName = fileName,
                Content = content,
                Metadata = new Dictionary<string, string>
            {
                {"SourcePath" , filePath},
                {"CreatedDateTime", fileInfo.CreationTime.ToShortDateString() },
                {"LastAccessDateTime", fileInfo.LastAccessTime.ToShortDateString() }
            }
            };

            var response = await httpClient.PostAsJsonAsync(Constants.CONST_DOC_PROCESSOR_API_ADD_DOCUMENT_POST_URL, request, AppJsonContext.Default.AddDocumentRequest);

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

    successCount = failCount = 0;

    Console.Write("\nDo you want to process files again? (Y/N): ");
    var input = Console.ReadLine();
    continueProcessing = input?.Trim().ToUpper() == "Y";

} while (continueProcessing);

   

Console.WriteLine("\nBye!");