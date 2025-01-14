﻿using System;
using System.Collections.Generic;
using System.Linq;
using ImageDiff.Analyzers;
using ImageDiff.BoundingBoxes;
using ImageDiff.Labelers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageDiff
{
  public class BitmapComparer : IImageComparer<Image<Rgba32>>
  {
    private LabelerTypes LabelerType { get; set; }
    private double JustNoticeableDifference { get; set; }
    private int DetectionPadding { get; set; }
    private int BoundingBoxPadding { get; set; }
    private Rgba32 BoundingBoxColor { get; set; }
    private int BoundingBoxThickness { get; set; }
    private BoundingBoxModes BoundingBoxMode { get; set; }
    private AnalyzerTypes AnalyzerType { get; set; }

    private IDifferenceLabeler Labeler { get; set; }
    private IBoundingBoxIdentifier BoundingBoxIdentifier { get; set; }
    private IBitmapAnalyzer BitmapAnalyzer { get; set; }

    public BitmapComparer(CompareOptions options = null)
    {
      if (options == null)
      {
        options = new CompareOptions();
      }
      Initialize(options);

      BitmapAnalyzer = BitmapAnalyzerFactory.Create(AnalyzerType, JustNoticeableDifference);
      Labeler = LabelerFactory.Create(LabelerType, DetectionPadding);
      BoundingBoxIdentifier = BoundingBoxIdentifierFactory.Create(BoundingBoxMode, BoundingBoxPadding);
    }

    private void Initialize(CompareOptions options)
    {
      if (options.BoundingBoxPadding < 0)
      {
        throw new ArgumentException("bounding box padding must be non-negative");
      }
      if (options.DetectionPadding < 0)
      {
        throw new ArgumentException("detection padding must be non-negative");
      }

      LabelerType = options.Labeler;
      JustNoticeableDifference = options.JustNoticeableDifference;
      BoundingBoxColor = options.BoundingBoxColor;
      DetectionPadding = options.DetectionPadding;
      BoundingBoxPadding = options.BoundingBoxPadding;
      BoundingBoxMode = options.BoundingBoxMode;
      AnalyzerType = options.AnalyzerType;
      BoundingBoxThickness = options.BoundingBoxThickness;
    }

    public Image<Rgba32> Generate(Image<Rgba32> firstImage, Image<Rgba32> secondImage)
    {
      Result result = Compare(firstImage, secondImage);
      return result.Image;
    }

    public Result Compare(Image<Rgba32> firstImage, Image<Rgba32> secondImage)
    {
      if (firstImage == null)
      {
        throw new ArgumentNullException("firstImage");
      }
      if (secondImage == null)
      {
        throw new ArgumentNullException("secondImage");
      }
      if (firstImage.Width != secondImage.Width || firstImage.Height != secondImage.Height)
      {
        throw new ArgumentException("Bitmaps must be the same size.");
      }

      var differenceMap = BitmapAnalyzer.Analyze(firstImage, secondImage);
      var differenceLabels = Labeler.Label(differenceMap);
      var boundingBoxes = BoundingBoxIdentifier.CreateBoundingBoxes(differenceLabels);
      var differenceBitmap = CreateImageWithBoundingBoxes(secondImage, boundingBoxes);
      return Result.Create(differenceBitmap, boundingBoxes);
    }

    public bool Equals(Image<Rgba32> firstImage, Image<Rgba32> secondImage)
    {
      if (firstImage == null && secondImage == null)
      {
        return true;
      }
      if (firstImage == null)
      {
        return false;
      }
      if (secondImage == null)
      {
        return false;
      }
      if (firstImage.Width != secondImage.Width || firstImage.Height != secondImage.Height)
      {
        return false;
      }

      var differenceMap = BitmapAnalyzer.Analyze(firstImage, secondImage);

      // differenceMap is a 2d array of boolean values, true represents a difference between the images
      // iterate over the dimensions of the array and look for a true value (difference) and return false
      for (var i = 0; i < differenceMap.GetLength(0); i++)
      {
        for (var j = 0; j < differenceMap.GetLength(1); j++)
        {
          if (differenceMap[i, j])
          {
            return false;
          }
        }
      }
      return true;
    }

    private Image<Rgba32> CreateImageWithBoundingBoxes(Image<Rgba32> secondImage, IEnumerable<Rectangle> boundingBoxes)
    {
      var differenceBitmap = secondImage.Clone() as Image<Rgba32>;
      if (differenceBitmap == null)
      {
        throw new Exception("Could not copy secondImage");
      }

      var boundingRectangles = boundingBoxes.ToArray();
      if (boundingRectangles.Length == 0)
      {
        return differenceBitmap;
      }

      var pen = Pens.Solid(BoundingBoxColor, BoundingBoxThickness);

      foreach (var boundingRectangle in boundingRectangles)
      {
        differenceBitmap.Mutate(x => x.Draw(pen , boundingRectangle));
      }
      return differenceBitmap;
    }
  }
}
