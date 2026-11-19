using AllianceMobileApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AllianceMobileApp.Utility
{
    public class InventoryService
    {
        private readonly HttpClient _client;

        public InventoryService(HttpClient client)
        {
            _client = client;
        }

        public async Task<GetAllItemsResponseModel> GetItemsAsync(CancellationToken ct = default)
        {
            var stream = await _client.GetStreamAsync("/Item/GetAllItems", ct);
            return (await JsonSerializer.DeserializeAsync<GetAllItemsResponseModel>(stream, cancellationToken: ct))!;
        }

    }
    public static class AppJsonOptions
    {
        public static readonly JsonSerializerOptions Default = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };
    }
    
}
