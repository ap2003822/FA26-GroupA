using AllianceMobileApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AllianceMobileApp.Utility
{
    public class InventoryService
    {
        private readonly HttpClient _client;

        public InventoryService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<Item>> GetItemsAsync(CancellationToken ct = default)
        {
            var stream = await _client.GetStreamAsync("Item/GetAllItems", ct);
            return (await JsonSerializer.DeserializeAsync<List<Item>>(stream, cancellationToken: ct))!;
        }
    }
}
