using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageDiff
{
  public class Result
  {
    public static Result Create(Image<Rgba32> image, IEnumerable<Rectangle> boundingBoxes)
    {
      return new Result()
      {
        Image = image,
        BoundingBoxes = boundingBoxes
      };
    }

    public Image<Rgba32> Image { get; set; }

    public IEnumerable<Rectangle> BoundingBoxes { get; set; }

    public bool IsSimilar
    {
      get
      {
        return BoundingBoxes.Count() == 0;
      }
    }

    /// <summary>
    /// For more options, use Image.Save
    /// </summary>
    /// <param name="filename"></param>
    public void Save(string filename)
    {
      Image.SaveAsPng(filename);
    }
  }
}
