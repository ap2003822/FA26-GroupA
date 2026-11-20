using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AllianceMobileApp.Model
{
    [JsonSourceGenerationOptions(System.Text.Json.JsonSerializerDefaults.Web)]
    [JsonSerializable(typeof(GetAllItemsResponseModel))]
    [JsonSerializable(typeof(bool))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(List<Item>))]
    public partial class GetAllItemsResponseCONTEXT : JsonSerializerContext;
}
