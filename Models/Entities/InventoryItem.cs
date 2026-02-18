namespace TheSignal.Models.Entities;

public class InventoryItem
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public Character? Character { get; set; }
    
    public int ItemId { get; set; }
    public Item? Item { get; set; }
    
    public int Quantity { get; set; } = 1;
    public bool IsEquipped { get; set; }
}
