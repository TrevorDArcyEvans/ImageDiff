namespace ImageDiff.UI.Web.Pages;

using System.Drawing;
using Microsoft.AspNetCore.Components.Forms;

public sealed partial class Index
{
  private Bitmap _img1;
  private Bitmap _img2;
  private string _img1FileName { get; set; } = "Upload image 1";
  private string _img2FileName { get; set; } = "Upload image 2";
  private string _img1Url { get; set; } = GetDefaultImageString();
  private string _img2Url { get; set; } = GetDefaultImageString();
  private string _diffUrl { get; set; } = GetDefaultImageString();
  private string _text { get; set; }

  private bool _canDiff
  {
    get => _img1 is not null && _img2 is not null;
  }

  private async Task LoadFile1(InputFileChangeEventArgs e)
  {
    _img1 = await GetImage(e.File);
    _img1FileName = e.File.Name;
    _text = _img1 is null ? $"<b>{_img1FileName}</b> --> unknown format" : string.Empty;
    _img1Url = await GetImageString(e.File);
  }

  private async Task LoadFile2(InputFileChangeEventArgs e)
  {
    _img2 = await GetImage(e.File);
    _img2FileName = e.File.Name;
    _text = _img2 is null ? $"<b>{_img2FileName}</b> --> unknown format" : string.Empty;
    _img2Url = await GetImageString(e.File);
  }

  private static async Task<Bitmap> GetImage(IBrowserFile file)
  {
    var data = file.OpenReadStream();
    var ms = new MemoryStream();
    await data.CopyToAsync(ms);
    ms.Seek(0, SeekOrigin.Begin);
    return new Bitmap(ms);
  }

  private static async Task<string> GetImageString(IBrowserFile file)
  {
    var buffers = new byte[file.Size];
    await file.OpenReadStream().ReadAsync(buffers);
    return $"data:{file.ContentType};base64,{Convert.ToBase64String(buffers)}";
  }

  private static string GetImageString(Bitmap img)
  {
    using var ms = new MemoryStream();
    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
    var bytes = ms.ToArray();
    return $"data:img/png;base64,{Convert.ToBase64String(bytes)}";
  }

  private static string GetDefaultImageString(int width = 64, int height = 64)
  {
    var img = new Bitmap(width, height);
    using var ms = new MemoryStream();
    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
    var bytes = ms.ToArray();
    return $"data:img/png;base64,{Convert.ToBase64String(bytes)}";
  }

  private async Task OnDiff()
  {
    _text = $"Diffing images ...";
    StateHasChanged();
    await Task.Run(() =>
    {
      var comparer = new BitmapComparer();
      var diff = comparer.Compare(_img1, _img2);
      _diffUrl = GetImageString(diff.Image);
      _text = $"Finished diffing!";
    });
  }
}
