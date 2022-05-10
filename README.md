# ImageDiff

A .NET library for comparing images and highlighting the differences.

![first image](docs/images/firstImage.png) **vs**
![second image](docs/images/secondImage.png) **=**
![image diff](docs/images/diffImage.png)

## Background
* based on [ImageDiff](https://github.com/richclement/ImageDiff)
* ported to .NET Core 6
* runs in browser with webassembly
* supports Linux
* commandline client

## Prerequisites
* .NET Core 6

## Getting started
```bash
$ git clone https://github.com/TrevorDArcyEvans/ImageDiff
$ cd ImageDiff
$ dotnet restore
$ dotnet build
```

### Web UI
```bash
$ cd ImageDiff.UI.Web/
$ dotnet restore
$ dotnet build
$ dotnet run
```
open [ImageDiff](http://localhost:5069)

## How it works
_ImageDiff_ has three stages to its processing.
 
1. Analyze the provided images.
2. Detect and label the differences between the images.
3. Build the bounding boxes for the identified blobs.

After processing is finished, it will generate a new image which is derived
from the second image provided to the `Compare` method. This `diff` image
will contain highlighted bounding boxes around the differences between the
two images.

![first image](docs/images/firstImage.png) compared to
![second image](docs/images/secondImage.png) produces 
![image diff](docs/images/diffImage.png)

### Restrictions
_ImageDiff_ does a mildly intelligent pixel to pixel comparison.
As such, each image must have the same dimensions.

Further, colour changes eg brightness, contrast or grayscale, will fatally
defeat the algorithm.

However, the biggest restriction is that the underlying algorithm works
best with an original and an edited version of the the original.  It will
not work very well with two similar images which do not have a common
ancestor eg two photos taken from the same point.

## API Usage

<details>

Default usage:

```csharp
    var firstImage = new Bitmap("path/to/first/image");
    var secondImage = new Bitmap("path/to/second/image);

    var comparer = new BitmapComparer();

    //Returns a result with the differences + the Bitmap
    var diff = comparer.Compare(firstImage, secondImage);

    // Generates the bitmap image and returns a Bitmap
    var generate = comparer.Generate(firstImage, secondImage);
```

When initialized without options, the following values are used:

- AnalyzerType: ExactMatch
- Labeler: Basic
- JustNoticeableDifference: 2.3
- DetectionPadding: 2
- BoundingBoxPadding: 2
- BoundingBoxColor: Red
- BoundingBoxMode: Single
- BoundingBoxThickness: 1

The compare object can be configured to use different settings for the
different stages of processing.

```csharp
    var options = new CompareOptions 
    {
	    AnalyzerType = AnalyzerTypes.CIE76,
        JustNoticableDifference = 2.3,
        DetectionPadding = 2,
        Labeler = LabelerTypes.ConnectedComponentLabeling,
        BoundingBoxColor = Color.Red,
        BoundingBoxPadding = 2,
        BoundingBoxMode = BoundingBoxModes.Multiple,
        BoundingBoxThickness = 1 
    };
    var comparer = new BitmapComparer(options);
```

</details>

## Analyzer Type
<details>

Two forms of image analysis are currently supported:

- ExactMatch - requires that the RGB values of each pixel in the image
 be equal.
- CIE76 - follows the [color difference formula](http://en.wikipedia.org/wiki/Color_difference "color difference formula")
to generate a Euclidean distance between the colors in the pixels and
flags a difference when the Just Noticeable Difference (JND) is greater
than a value of 2.3.

### Just Noticeable Difference
Specify this to control how distant two pixels can be in the color space
before they are marked as different.

### Detection Padding
How many pixels away from the current pixel to look, for neighbors that
should be grouped together for labeling purposes.

### Labeler
Two forms of blob labeling are currently supported:

- Basic - basic labeling will group all differences together into a single group.
This labeling format does not support `BoundingBoxMode.Multiple`.
- [Connected Component Labeling](http://en.wikipedia.org/wiki/Connected-component_labeling "Connected Component Labeling")
Uses a two-pass algorithm to label the differences in an image and then aggregate
the labels. The Detection Padding option is used to determine how far to travel
when checking neighbor pixels.

### Bounding Box Color
The color of the bounding box to be drawn when highlighting detected differences.

### Bounding Box Padding
The number of pixels of padding to include around the detected difference when
drawing a bounding box.

### Bounding Box Thickness
The thickness of the rectangle showing the difference in pixel. Default = 1

### Bounding Box Mode
Specifies how to build the bounding boxes when highlighting the detected
differences.

- Single - Only generate one bounding box that encompasses all of the detected
differences in the image.
- Multiple - Generate a bounding box around each separate group of detected
differences. This bounding box mode is not supported by `LabelerTypes.Basic`.

</details>
