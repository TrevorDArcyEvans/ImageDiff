using System.Collections.Generic;
using SixLabors.ImageSharp;

namespace ImageDiff.BoundingBoxes
{
  internal interface IBoundingBoxIdentifier
  {
    IEnumerable<Rectangle> CreateBoundingBoxes(int[,] labelMap);
  }
}
