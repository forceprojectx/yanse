using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Camera;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using SkiaSharp;
using yanse.Models;
using yanse.ViewModels;


namespace yanse
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel viewModel;

        public MainPage(MainPageViewModel viewModel)
        {
            if (null == viewModel)
            {
                throw new ArgumentNullException("viewModel", "viewModel cannot be null");
            }
            InitializeComponent();
            this.viewModel = viewModel;
            viewModel.FlashMode = CameraFlashMode.Off;
            viewModel.Yanse = Yanse.Colors[new Random().Next(Yanse.Colors.Count)];
            BindingContext = this.viewModel;

            Camera.MediaCaptured += OnMediaCaptured;
        }

        private void OnMediaCaptured(object? sender, MediaCapturedEventArgs e)
        {
            using var bitmap = SKBitmap.Decode(e.Media);
            var quantizedColors = QuantizeBitmap2(bitmap);

            //var color = quantizedColors.MaxBy(kvp => kvp.Value).Key;
            viewModel.DetectedColor = Color.FromHsv(quantizedColors.Hue / 360f, quantizedColors.Saturation / 100f, quantizedColors.Value / 100f);

            viewModel.Yanse = FindBestMatch(quantizedColors);

            viewModel.StatusMessageText = $"Image Captured! -- color={viewModel.DetectedColor}";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            await viewModel.RefreshCamerasCommand.ExecuteAsync(tokenSource.Token);
        }
        void Cleanup()
        {
            Camera.MediaCaptured -= OnMediaCaptured;
            Camera.Handler?.DisconnectHandler();
        }

        void OnUnloaded(object? sender, EventArgs e)
        {
            Cleanup();
        }

        void ZoomIn(object? sender, EventArgs e)
        {
            viewModel.CurrentZoom += 1.0f;
        }

        void ZoomOut(object? sender, EventArgs e)
        {
            viewModel.CurrentZoom -= 1.0f;
        }

        private Dictionary<Color, int> QuantizeBitmap(SKBitmap bitmap, int quantizeStep=16)
        {
            var quantizedValues = new Dictionary<Color, int>();

            // it probably doesnt matter since we can't access the bitmap directly,
            // but processing by row should be more performant since the data there should be contiguous
            var pixels = bitmap.Pixels;
            var pixelCount = bitmap.Height * bitmap.Width;

            for (int i = 0; i < pixelCount; i+=4)
            {
                //byte r = QuantizeValue(pixels[i].Red, quantizeStep);
                //byte g = QuantizeValue(pixels[i].Green, quantizeStep);
                //byte b = QuantizeValue(pixels[i].Blue, quantizeStep);

                byte r = (byte)(pixels[i].Red / quantizeStep * quantizeStep);
                byte g = (byte)(pixels[i].Green / quantizeStep * quantizeStep);
                byte b = (byte)(pixels[i].Blue / quantizeStep * quantizeStep);

                var color = Color.FromRgb(r, g, b);
                quantizedValues.TryGetValue(color, out var count);
                quantizedValues[color] = count+1;
            }

            return quantizedValues;
        }

        private HSV QuantizeBitmap2(SKBitmap bitmap, int quantizeStep = 16)
        {
            var resized = new SKBitmap(10, 10);
            bitmap.ScalePixels(resized,new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None));

            float h, s, v;
            bitmap.Pixels[50].ToHsv(out h, out s, out v);

            var retval = new HSV
            {
                Hue = (float)Math.Round(h, 3),
                Saturation = (float)Math.Round(s, 3),
                Value = (float)Math.Round(v, 3)
            };

            return retval;
        }

        private byte QuantizeValue(byte value, int step)
        {
            int quantized = (value / step) * step;
            return (byte)Math.Clamp(quantized, 0, 255);
        }

        private Yanse FindBestMatch(Color color)
        {
            return Yanse.Colors.MinBy(x =>
            Math.Pow(color.Red - x.Color.Red, 2) +
            Math.Pow(color.Green - x.Color.Green, 2) +
            Math.Pow(color.Blue - x.Color.Blue, 2))?? Yanse.Colors[0];
        }

        private Yanse FindBestMatch(HSV color)
        {
            return Yanse.Colors.MinBy(x =>
            HsvDistance(color, x.Hsv)) ?? Yanse.Colors[0];
        }

        public static double HsvDistance(HSV a, HSV b)
        {
            float dh = Math.Min(Math.Abs(a.Hue - b.Hue), 360 - Math.Abs(a.Hue - b.Hue)); // circular hue distance
            float ds = a.Saturation - b.Saturation;
            float dv = a.Value - b.Value;

            //return Math.Sqrt(dh * dh + ds * ds * 100 + dv * dv * 100); // weighted distance
            return dh  ; // weighted distance
        }
    }
}
