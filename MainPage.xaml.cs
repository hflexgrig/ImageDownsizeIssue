using System.Reflection;
using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace ImageDownsizeIssue;

public partial class MainPage : ContentPage
{
	private ImageSource _originalSource;
	private string _originalSize;
	private ImageSource _downsizedSource;
	private string _downsizedImageSize;

	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
		// Button downSizeButton = new Button
		// {
		// 	Text = "DownSize",
		// 	HorizontalOptions = LayoutOptions.Fill
		// };
		// downSizeButton.Clicked += OnDownSize;
		//
		// VerticalStackLayout stackLayout = new VerticalStackLayout
		// {
		// 	Padding = new Thickness(20),
		// 	Children = { downSizeButton }
		// };
		//
		// Content = new ScrollView { Content = stackLayout };
		InitAsync();
	}

	public ImageSource OriginalSource
	{
		get => _originalSource;
		set
		{
			if (Equals(value, _originalSource)) return;
			_originalSource = value;
			OnPropertyChanged();
		}
	}

	public ImageSource DownsizedSource
	{
		get => _downsizedSource;
		set
		{
			if (Equals(value, _downsizedSource)) return;
			_downsizedSource = value;
			OnPropertyChanged();
		}
	}

	public string OriginalSize
	{
		get => _originalSize;
		set
		{
			if (value == _originalSize) return;
			_originalSize = value;
			OnPropertyChanged();
		}
	}

	public string DownsizedImageSize
	{
		get => _downsizedImageSize;
		set
		{
			if (value == _downsizedImageSize) return;
			_downsizedImageSize = value;
			OnPropertyChanged();
		}
	}

	private async void InitAsync()
	{
		var origImage = await LoadImageAsync();
		OriginalSize = origImage.Width + "x" + origImage.Height;
		
		OriginalSource = ImageSource.FromResource("ImageDownsizeIssue.Resources.Images.royals.png");
		
		var downSized = origImage.Downsize(100);
		
		DownsizedImageSize = downSized.Width + "x" + downSized.Height;
		
		MemoryStream memStream = new MemoryStream();
		downSized.Save(memStream);
		
		File.WriteAllBytes("downsized.png", memStream.ToArray());
		
		DownsizedSource =  ImageSource.FromFile("downsized.png");
	}

	Task<IImage> LoadImageAsync()
	{
		var assembly = GetType().GetTypeInfo().Assembly;

		using (var stream = assembly.GetManifestResourceStream("ImageDownsizeIssue.Resources.Images.royals.png"))
		{
			return Task.FromResult(PlatformImage.FromStream(stream));
		}
	}

	private async void OnDownSize(object sender, EventArgs e)
	{
		var image = await LoadImageAsync();

		// Show original dimensions
		await DisplayAlert("Original Size", $"Width: {image.Width}, Height: {image.Height}", "OK");

		// Downsize the image
		var downsizedImage = image.Downsize(10, false);

		// Show downsized dimensions
		await DisplayAlert("Downsized", $"Width: {downsizedImage.Width}, Height: {downsizedImage.Height}", "OK");
	}
}