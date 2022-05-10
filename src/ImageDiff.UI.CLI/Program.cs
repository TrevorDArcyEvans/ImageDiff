namespace ImageDiff.UI.CLI;

using CommandLine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

internal static class Program
{
  public static async Task Main(string[] args)
  {
    var result = await Parser.Default.ParseArguments<Options>(args)
      .WithParsedAsync(Run);
    await result.WithNotParsedAsync(HandleParseError);
  }

  private static async Task Run(Options opt)
  {
    var img1 = await Image.LoadAsync<Rgba32>(opt.ImageFile1Path);
    var img2 = await Image.LoadAsync<Rgba32>(opt.ImageFile2Path);
    var options = new CompareOptions
    {
      AnalyzerType = AnalyzerTypes.CIE76,
      BoundingBoxMode = BoundingBoxModes.Multiple,
      Labeler = LabelerTypes.ConnectedComponentLabeling

    };
    var comparer = new BitmapComparer(options);
    var diff = comparer.Compare(img1, img2);
    diff.Image.SaveAsPng(opt.DiffFilePath);

    Console.WriteLine($"ImageFile1    = {opt.ImageFile1Path}");
    Console.WriteLine($"ImageFile2    = {opt.ImageFile2Path}");
    Console.WriteLine($"DiffImageFile = {opt.DiffFilePath}");
    Console.WriteLine($"  BBoxes      = {diff.BoundingBoxes.Count()}");
  }

  private static Task HandleParseError(IEnumerable<Error> errs)
  {
    if (errs.IsVersion())
    {
      Console.WriteLine("Version Request");
      return Task.CompletedTask;
    }

    if (errs.IsHelp())
    {
      Console.WriteLine("Help Request");
      return Task.CompletedTask;
      ;
    }

    Console.WriteLine("Parser Fail");
    return Task.CompletedTask;
  }
}