using System;
using ImageDiff;
using ImageDiff.Analyzers;
using NUnit.Framework;

namespace ImageDiffTests
{
  [TestFixture]
  public class AnalyerFactoryTests
  {
    [Test]
    public void FactoryThrowsWithInvalidType()
    {
      Assert.Throws<ArgumentException>(() => BitmapAnalyzerFactory.Create((AnalyzerTypes)100, 2.3));
    }

    [Test]
    public void FactoryCreatesExactMatchAnalyzer()
    {
      var target = BitmapAnalyzerFactory.Create(AnalyzerTypes.ExactMatch, 2.3);
      Assert.IsInstanceOf(typeof(ExactMatchAnalyzer), target);
    }

    [Test]
    public void FactoryCreatesCIE76Analyzer()
    {
      var target = BitmapAnalyzerFactory.Create(AnalyzerTypes.CIE76, 2.3);
      Assert.IsInstanceOf(typeof(CIE76Analyzer), target);
    }
  }
}
