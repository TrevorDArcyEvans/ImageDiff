using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageDiff.Analyzers
{
  internal interface IBitmapAnalyzer
  {
    bool[,] Analyze(Image<Rgba32> first, Image<Rgba32> second);
  }
}
