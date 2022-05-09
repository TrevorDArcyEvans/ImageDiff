using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageDiff.Analyzers
{
  internal class ExactMatchAnalyzer : IBitmapAnalyzer
  {
    public bool[,] Analyze(Image<Rgba32> first, Image<Rgba32> second)
    {
      var diff = new bool[first.Width, first.Height];
      for (var x = 0; x < first.Width; x++)
      {
        for (var y = 0; y < first.Height; y++)
        {
          var firstPixel = first.GetPixel(x, y);
          var secondPixel = second.GetPixel(x, y);
          if (firstPixel != secondPixel)
          {
            diff[x, y] = true;
          }
        }
      }
      return diff;
    }
  }
}