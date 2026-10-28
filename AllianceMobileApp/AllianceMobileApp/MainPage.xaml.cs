using ZXing.Net.Maui;

namespace AllianceMobileApp
{
    public partial class MainPage : ContentPage
    {
        string generatorValue = "test";
        BarcodeFormat generatorFormat = BarcodeFormat.Aztec;
        string lastScannedValue;
        public MainPage()
        {
            InitializeComponent();

            barcodeView.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormat.Aztec,
                AutoRotate = true,
                Multiple = false,
                DelayBetweenAnalyzingFrames = 300,
                DelayBetweenContinuousScans = 1000,
                CameraResolutionSelector = SelectCameraResolution
            };
        }

        static CameraResolution SelectCameraResolution (IReadOnlyList<CameraResolution> resolutions)
        {
            if (resolutions.Count == 0)
            {
                return new CameraResolution(640, 480);
            }
            return resolutions
                .OrderBy(resolution => Math.Abs((resolution.Width * resolution.Height) - (1280 * 720)))
                .ThenBy(resolution => Math.Abs(resolution.Width - 1280) + Math.Abs(resolution.Height - 720))
                .First();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            barcodeView.IsDetecting = true;
        }

        protected override void OnDisappearing()
        {
            barcodeView.IsDetecting = false;
            barcodeView.IsTorchOn = false;

            base.OnDisappearing();
        }

        private void BarcodeDetected(object sender, BarcodeDetectionEventArgs e)
        {
            foreach (var barcode in e.Results)
            {
                Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");

                var first = e.Results?.FirstOrDefault();
                if (first is not null)
                {
                    Dispatcher.Dispatch(() =>
                    {
                        ShowBarcodeResult(first);
                    });
                }
            }
        }

        void ShowBarcodeResult(BarcodeResult barcode)
        {
            generatorFormat = barcode.Format;
            generatorValue = barcode.Value;
            lastScannedValue = barcode.Value;
            ResultFormatLabel.Text = barcode.Format.ToString();
            ResultValueLabel.Text = barcode.Value;
            ResultHintLabel.IsVisible = true;
        }
        async void ResultPanel_Tapped(object sender, TappedEventArgs e)
        {
            if (string.IsNullOrEmpty(lastScannedValue))
            {
                return;
            }

            await DisplayAlertAsync(generatorFormat.ToString(), lastScannedValue, "Ok");
        }
    }
}
