using AllianceMobileApp.Model;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllianceMobileApp.Utility
{
    public interface IInventoryService
    {
        [Get("Item/GetAllItems")]
        Task<List<Item>> GetItemsAsync(CancellationToken cancellationToken = default);
    }
}
