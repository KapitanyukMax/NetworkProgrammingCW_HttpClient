namespace NetworkProgrammingCW_HttpClient
{
    internal class ImageViewModel
    {
        public string ImageSource { get; set; } = string.Empty;

        public ImageViewModel(string imageSource)
        {
            ImageSource = imageSource;
        }
    }
}