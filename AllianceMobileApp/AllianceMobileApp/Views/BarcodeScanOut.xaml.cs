using AllianceMobileApp.ViewModel;

namespace AllianceMobileApp.Views;

public partial class BarcodeScanOut : ContentView
{
	public BarcodeScanOut(BarcodeScanOutViewModel barcodeScanOutViewModel)
	{
		InitializeComponent();
		BindingContext = barcodeScanOutViewModel;
	}
}