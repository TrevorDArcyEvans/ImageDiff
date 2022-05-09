using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageDiff
{
  public class CompareOptions
  {
    public AnalyzerTypes AnalyzerType { get; set; }
    public LabelerTypes Labeler { get; set; }
    public double JustNoticeableDifference { get; set; }
    public int DetectionPadding { get; set; }
    public int BoundingBoxPadding { get; set; }
    public int BoundingBoxThickness { get; set; }
    public Rgba32 BoundingBoxColor { get; set; }
    public BoundingBoxModes BoundingBoxMode { get; set; }

    public CompareOptions()
    {
      Labeler = LabelerTypes.Basic;
      JustNoticeableDifference = 2.3;
      DetectionPadding = 2;
      BoundingBoxPadding = 2;
      BoundingBoxColor = Color.Red;
      BoundingBoxMode = BoundingBoxModes.Single;
      AnalyzerType = AnalyzerTypes.ExactMatch;
      BoundingBoxThickness = 1;
    }
  }
}
