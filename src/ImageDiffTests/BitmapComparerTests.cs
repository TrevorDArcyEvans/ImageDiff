using System;
using ImageDiff;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageDiffTests
{
  [TestFixture]
  public class BitmapComparerTests
  {
    protected const string TestImage1 = "./images/TestImage1.png";
    protected const string TestImage2 = "./images/TestImage2.png";
    protected const string OutputFormat = "output_{0}.png";
    protected Image<Rgba32> FirstImage { get; set; }
    protected Image<Rgba32> SecondImage { get; set; }

    [SetUp]
    public void Setup()
    {
      FirstImage = Image.Load<Rgba32>(TestImage1);
      SecondImage = Image.Load<Rgba32>(TestImage2);
    }

    [Test]
    public void CompareThrowsWhenFirstImageIsNull()
    {
      var target = new BitmapComparer(null);
      Assert.Throws<ArgumentNullException>(() => target.Compare(null, SecondImage));
    }

    [Test]
    public void CompareThrowsWhenSecondImageIsNull()
    {
      var target = new BitmapComparer(null);
      Assert.Throws<ArgumentNullException>(() => target.Compare(FirstImage, null));
    }

    [Test]
    public void CompareThrowsWhenImagesAreNotSameWidth()
    {
      var firstBitmap = new Image<Rgba32>(10, 10);
      var secondBitmap = new Image<Rgba32>(20, 10);

      var target = new BitmapComparer(null);
      Assert.Throws<ArgumentException>(() => target.Compare(firstBitmap, secondBitmap));
    }

    [Test]
    public void CompareThrowsWhenImagesAreNotSameHeight()
    {
      var firstBitmap = new Image<Rgba32>(10, 10);
      var secondBitmap = new Image<Rgba32>(10, 20);

      var target = new BitmapComparer(null);
      Assert.Throws<ArgumentException>(() => target.Compare(firstBitmap, secondBitmap));
    }

    [Test]
    public void ImageDiffThrowsWhenBoundingBoxPaddingIsLessThanZero()
    {
      Assert.Throws<ArgumentException>(() => new BitmapComparer(new CompareOptions
      {
        BoundingBoxColor = Color.Red,
        BoundingBoxMode = BoundingBoxModes.Single,
        AnalyzerType = AnalyzerTypes.ExactMatch,
        DetectionPadding = 2,
        BoundingBoxPadding = -2,
        Labeler = LabelerTypes.Basic
      }));
    }

    [Test]
    public void ImageDiffThrowsWhenDetectionPaddingIsLessThanZero()
    {
      Assert.Throws<ArgumentException>(() => new BitmapComparer(new CompareOptions
      {
        BoundingBoxColor = Color.Red,
        BoundingBoxMode = BoundingBoxModes.Single,
        AnalyzerType = AnalyzerTypes.ExactMatch,
        DetectionPadding = -2,
        BoundingBoxPadding = 2,
        Labeler = LabelerTypes.Basic
      }));
    }

    [Test]
    public void CompareWorksWithNoOptions()
    {
      var target = new BitmapComparer();
      var result = target.Compare(FirstImage, SecondImage);
      result.Save(string.Format(OutputFormat, "CompareWorksWithNullOptions"));
    }

    [Test]
    public void CompareWorksWithNullOptions()
    {
      var target = new BitmapComparer(null);
      var result = target.Compare(FirstImage, SecondImage);
      result.Save(string.Format(OutputFormat, "CompareWorksWithNullOptions"));
    }

    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    public void CompareWorksWithIdenticalImages(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
    {
      var target = new BitmapComparer(new CompareOptions
      {
        AnalyzerType = aType,
        BoundingBoxMode = bMode,
        Labeler = lType
      });
      var result = target.Compare(FirstImage, FirstImage);
      result.Save(string.Format(OutputFormat, string.Format("CompareWorksWithIdenticalImages_{0}_{1}_{2}", aType, bMode, lType)));
    }


    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    public void CompareWorksWithDifferentImages(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
    {
      var target = new BitmapComparer(new CompareOptions
      {
        BoundingBoxColor = Color.Red,
        BoundingBoxMode = bMode,
        AnalyzerType = aType,
        DetectionPadding = 2,
        BoundingBoxPadding = 2,
        Labeler = lType
      });
      var result = target.Compare(FirstImage, SecondImage);
      result.Save(string.Format(OutputFormat, string.Format("CompareWorksWithDifferentImages_{0}_{1}_{2}", aType, bMode, lType)));
    }

    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    public void EqualsReturnsTrueWithSameImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
    {
      var target = new BitmapComparer(new CompareOptions
      {
        BoundingBoxColor = Color.Red,
        BoundingBoxMode = bMode,
        AnalyzerType = aType,
        DetectionPadding = 2,
        BoundingBoxPadding = 2,
        Labeler = lType
      });
      var newInstanceOfFirstImage = Image.Load<Rgba32>(TestImage1);
      var result = target.Equals(FirstImage, newInstanceOfFirstImage);
      Assert.IsTrue(result);
    }

    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    public void EqualsReturnsFalseWithDifferentImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
    {
      var target = new BitmapComparer(new CompareOptions
      {
        BoundingBoxColor = Color.Red,
        BoundingBoxMode = bMode,
        AnalyzerType = aType,
        DetectionPadding = 2,
        BoundingBoxPadding = 2,
        Labeler = lType
      });
      var result = target.Equals(FirstImage, SecondImage);
      Assert.IsFalse(result);
    }

    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    public void EqualsReturnsTrueWithNullImages(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
    {
      var target = new BitmapComparer(new CompareOptions
      {
        BoundingBoxColor = Color.Red,
        BoundingBoxMode = bMode,
        AnalyzerType = aType,
        DetectionPadding = 2,
        BoundingBoxPadding = 2,
        Labeler = lType
      });
      var result = target.Equals(null, null);
      Assert.IsTrue(result);
    }

    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    public void EqualsReturnsFalseWithNullFirstImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
    {
      var target = new BitmapComparer(new CompareOptions
      {
        BoundingBoxColor = Color.Red,
        BoundingBoxMode = bMode,
        AnalyzerType = aType,
        DetectionPadding = 2,
        BoundingBoxPadding = 2,
        Labeler = lType
      });
      var result = target.Equals(null, SecondImage);
      Assert.IsFalse(result);
    }

    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
    [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
    public void EqualsReturnsFalseWithNullSecondImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
    {
      var target = new BitmapComparer(new CompareOptions
      {
        BoundingBoxColor = Color.Red,
        BoundingBoxMode = bMode,
        AnalyzerType = aType,
        DetectionPadding = 2,
        BoundingBoxPadding = 2,
        Labeler = lType
      });
      var result = target.Equals(FirstImage, null);
      Assert.IsFalse(result);
    }
  }
}
