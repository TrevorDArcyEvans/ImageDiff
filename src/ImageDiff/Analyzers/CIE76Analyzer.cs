using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageDiff.Analyzers
{
  internal class CIE76Analyzer : IBitmapAnalyzer
  {
    private double JustNoticeableDifference { get; set; }

    public CIE76Analyzer(double justNoticeableDifference)
    {
      JustNoticeableDifference = justNoticeableDifference;
    }

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
          var firstLab = CIELab.FromRGB(firstPx[x, y]);
          var secondLab = CIELab.FromRGB(secondPx[x, y]);

          var score = Math.Sqrt(
            Math.Pow(secondLab.L - firstLab.L, 2) +
            Math.Pow(secondLab.a - firstLab.a, 2) +
            Math.Pow(secondLab.b - firstLab.b, 2));

          diff[x, y] = (score >= JustNoticeableDifference);
        }
      }
      return diff;
    }
  }
}
