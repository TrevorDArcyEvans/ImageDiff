namespace ImageDiff
{
  public interface IImageComparer<T>
  {
    T Generate(T firstImage, T secondImage);
    Result Compare(T firstImage, T secondImage);
    bool Equals(T firstImage, T secondImage);
  }
}
