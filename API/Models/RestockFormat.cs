using AllianceInventoryApi.API.Models;

namespace AllianceInventoryApi.Models
{
	public class RestockFormat
	{ //very simple, just so we can take and put the Restock Email together easy
		public string Beggining = $"Hello,\nThis is your daily Restock Email, the Following Items have gone below the Restock Threshold:\n";
		public List<Item> Restockables = new List<Item>();
		public string End = $"\nHave a good day!";
		public string? EmailText;
	}
}