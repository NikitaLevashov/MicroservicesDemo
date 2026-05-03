using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;


namespace Functions.PurchaseWorkflow.Triggers;

public class ResizeImageFunction
{
    private readonly ILogger<ResizeImageFunction> log;

    private readonly BlobServiceClient _blobService;

    // В Function v3/v4 можешь заинжектить BlobServiceClient через DI (FunctionsStartup),
    // либо создать внутри по Connection String. Для простоты — берём из env:
    public ResizeImageFunction(ILogger<ResizeImageFunction> logger)
    {
        log = logger;
        // В проде лучше через IConfiguration/DI
        var conn = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        _blobService = new BlobServiceClient(conn);
    }


    //[Function(nameof(ResizeImageFunction))]
    //public async Task Run([BlobTrigger("input/{name}", Connection = "")] Stream stream, string name)
    //{
    //    using var blobStreamReader = new StreamReader(stream);
    //    var content = await blobStreamReader.ReadToEndAsync();
    //    _logger.LogInformation("C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}", name, content);
    //}



    [Function(nameof(ResizeImageFunction))]
    public async Task Run(
        [BlobTrigger("input/{name}", Connection = "AzureWebJobsStorage")] Stream inputBlob,
        string name)
    {
        var conn = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        var output = _blobService.GetBlobContainerClient("output");
        var targetName = AppendSuffixBeforeExtension(name, "_thumb.jpg"); // <-- .jpg
        var target = output.GetBlobClient(targetName);

        using var image = Image.Load(inputBlob);
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(300, 300),
            Mode = ResizeMode.Max
        }));

        using var ms = new MemoryStream();
        image.Save(ms, new JpegEncoder { Quality = 85 });
        ms.Position = 0;

        var headers = new BlobHttpHeaders
        {
            ContentType = "image/jpeg" // <-- правильный тип
        };

        await target.UploadAsync(ms, new BlobUploadOptions { HttpHeaders = headers });
        log.LogInformation("Saved {Target} with Content-Type image/jpeg", targetName);
    }

    static string AppendSuffixBeforeExtension(string fileName, string suffix)
    {
        var dot = fileName.LastIndexOf('.');
        if (dot <= 0) return fileName + suffix; // без расширения
        return fileName.Substring(0, dot) + suffix + fileName.Substring(dot);
    }

}