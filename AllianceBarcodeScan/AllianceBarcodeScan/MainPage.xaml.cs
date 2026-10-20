using ZXing.Net.Maui;

namespace AllianceBarcodeScan
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
                Formats = BarcodeFormat.Aztec, //Maybe swap barcode format? we should see which ones meet requirements & run them past the clients for a preference, aztec may be good just for printing space tbh!
                AutoRotate = true,
                Multiple = false, //only scanning one at a time
                //Next three are default, we can alter them as we go.
                DelayBetweenAnalyzingFrames = 150,
                InitialDelayBeforeAnalyzingFrames = 300,
                DelayBetweenContinuousScans = 1000,
                CameraResolutionSelector = SelectCameraResolution
            };
        }

        static CameraResolution SelectCameraResolution (IReadOnlyList<CameraResolution> availableResolutions)
        {
            if (availableResolutions.Count == 0)
                return new CameraResolution(640, 480);

            return availableResolutions
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
            //Testing Barcode scan functionality
            foreach (var barcode in e.Results)
                Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");

            var first = e.Results?.FirstOrDefault();
            if (first is not null)
            {
                Dispatcher.Dispatch(() =>
                {
                    ShowBarcodeResult(first);
                });
            }
            //where we handle barcode info
            /*Theorizing here
            Would be something like "
            itemInfo = await [APICALLNAME](barcode.Value)
            
            Probably best to use the serial num for the barcodes, since barcoding a single digit for the id's can be odd, and we can just request from the items table based on serial num as the endpoint.

            Then

            navigation.pushasync(Itemin/outPage(itemInfo))

            Travel to the check in/out page, could have more detail on the backend barcode page to fork in the road direct them one way or the other, or just a "barcodeIN" "barcodeOUT" for the simplest directest path.

            Shows the item info's with the check in/out button -> check in/out panel we discussed in class with the room selection ect.
            "
            */
        }
        void ShowBarcodeResult(BarcodeResult barcode)
        { //Temp Barcode Results display)
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
                return;

            await DisplayAlertAsync(generatorFormat.ToString(), lastScannedValue, "OK");
        }
    }
}
