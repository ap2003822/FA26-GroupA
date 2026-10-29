using AllianceMobileApp.ViewModel;

namespace AllianceMobileApp.Views;

public partial class BarcodeScanIn : ContentView
{
	public BarcodeScanIn(BarcodeScanInViewModel barcodeScanInViewModel)
	{
		InitializeComponent();
		BindingContext = barcodeScanInViewModel;
	}
}