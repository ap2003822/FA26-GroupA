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

        private readonly IInventoryService _inventoryAPI = RestService.ForGenerated<IInventoryService>("https://localhost:7170/", GetAllItemsResponseCONTEXT.Default);

        private GetAllItemsResponseModel items = new GetAllItemsResponseModel();
        
        public MainPageViewModel ()
        {
            //GetItemsAsync(cancellationToken: default);

        }

    
        [RelayCommand]
        private async Task GetItemsAsync(CancellationToken cancellationToken = default)
        {
            //try
            //{

            //    items = await _inventoryAPI.GetItemsAsync(cancellationToken);
            //}
            //catch (ApiException ex)
            //{
            //    await Shell.Current.DisplayAlertAsync("Error", $"Failed to get Items List: {ex.StatusCode}", "OK");
            //    Console.WriteLine(ex.Message);
            //}
            var response = await _inventoryAPI.GetItemsAsync();
        }

    }
}
