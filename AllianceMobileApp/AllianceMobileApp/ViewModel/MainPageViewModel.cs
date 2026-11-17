using AllianceMobileApp.Model;
using AllianceMobileApp.Utility;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace AllianceMobileApp.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {

        private readonly IInventoryService _inventoryAPI;

        [ObservableProperty]
        private List<Item> items = new List<Item>();
        
        public MainPageViewModel (IInventoryService inventoryAPI)
        {
            _inventoryAPI = inventoryAPI;
            GetProductsAsync(cancellationToken: default);

        }

        [RelayCommand]
        private async Task GetProductsAsync(CancellationToken cancellationToken)
        {
            try
            {
                items = await _inventoryAPI.GetItemsAsync(cancellationToken);
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to get Items List: {ex.StatusCode}", "OK");
                Console.WriteLine(ex.Message);
            }
        }

    }
}
