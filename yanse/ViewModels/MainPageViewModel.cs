using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using yanse.Models;

namespace yanse.ViewModels
{
    /// <summary>
    /// Yanse View model
    /// </summary>
    /// <param name="cameraProvider"><seealso cref="ICameraProvider"/> implementation</param>
    public partial class MainPageViewModel(ICameraProvider cameraProvider):BaseViewModel
    {
        public CancellationToken Token => CancellationToken.None;

        [ObservableProperty]
        public partial Color DetectedColor { get; set; } = Color.FromRgb(223, 55, 6);

        [ObservableProperty]
        public partial Yanse Yanse { get; set; }

        [ObservableProperty]
        public partial Color UIColor {  get;  private set; }

        readonly ICameraProvider cameraProvider = cameraProvider;
        public IReadOnlyList<CameraInfo> Cameras => cameraProvider.AvailableCameras ?? [];
        public ICollection<CameraFlashMode> FlashModes { get; } = Enum.GetValues<CameraFlashMode>();

        [ObservableProperty]
        public partial CameraFlashMode FlashMode { get; set; }

        [ObservableProperty]
        public partial CameraInfo? SelectedCamera { get; set; }

        [ObservableProperty]
        public partial Size SelectedResolution { get; set; }

        public Size LowestResolution => SelectedCamera?.SupportedResolutions.OrderBy(x => x.Width * x.Height).FirstOrDefault() ?? default;

        [ObservableProperty]
        public partial float CurrentZoom { get; set; }

        [ObservableProperty]
        public partial string CameraNameText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string ZoomRangeText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string CurrentZoomText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string FlashModeText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string ResolutionText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string StatusMessageText { get; set; } = string.Empty;

        [RelayCommand]
        async Task RefreshCameras(CancellationToken token) => await cameraProvider.RefreshAvailableCameras(token);

        partial void OnFlashModeChanged(CameraFlashMode value)
        {
            UpdateFlashModeText();
        }

        partial void OnCurrentZoomChanged(float value)
        {
            UpdateCurrentZoomText();
        }

        partial void OnSelectedResolutionChanged(Size value)
        {
            UpdateResolutionText();
        }

        partial void OnSelectedCameraChanged(CameraInfo? value)
        {
            SelectedResolution = value?.SupportedResolutions.OrderBy(x => Math.Abs((x.Width * x.Height)- 76800/*320×240*/)).FirstOrDefault() ?? default;
            CurrentZoom = value?.MaximumZoomFactor ?? 1f;
        }

        partial void OnYanseChanged(Yanse value)
        {
           UIColor = new Color(value.Color.Red, value.Color.Green, value.Color.Blue);
        }

        void UpdateFlashModeText()
        {
            if (SelectedCamera is null)
            {
                return;
            }
            FlashModeText = $"{(SelectedCamera.IsFlashSupported ? $"Flash mode: {FlashMode}" : "Flash not supported")}";
        }

        void UpdateCurrentZoomText()
        {
            CurrentZoomText = $"Current Zoom: {CurrentZoom}";
        }

        void UpdateResolutionText()
        {
            ResolutionText = $"Selected Resolution: {SelectedResolution.Width} x {SelectedResolution.Height}";
        }
    }
}
