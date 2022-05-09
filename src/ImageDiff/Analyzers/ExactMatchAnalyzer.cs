using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageDiff.Analyzers
{
  internal class ExactMatchAnalyzer : IBitmapAnalyzer
  {
    public bool[,] Analyze(Image<Rgba32> first, Image<Rgba32> second)
    {
      var firstPx = new Rgba32[first.Width, first.Height];
      first.ProcessPixelRows(acc =>
      {
        for (var y = 0; y < acc.Height; y++)
        {
          var pxRow = acc.GetRowSpan(y);
          for (var x = 0; x < pxRow.Length - 1; x++)
          {
            ref var px = ref pxRow[x];
            firstPx[x, y] = px;
          }
        }
      });

      var secondPx = new Rgba32[second.Width, second.Height];
      second.ProcessPixelRows(acc =>
      {
        for (var y = 0; y < acc.Height; y++)
        {
          var pxRow = acc.GetRowSpan(y);
          for (var x = 0; x < pxRow.Length - 1; x++)
          {
            ref var px = ref pxRow[x];
            secondPx[x, y] = px;
          }
        }
      });
      var diff = new bool[first.Width, first.Height];
      for (var x = 0; x < first.Width; x++)
      {
        for (var y = 0; y < first.Height; y++)
        {
          var firstPixel = firstPx[x, y];
          var secondPixel = secondPx[x, y];
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
