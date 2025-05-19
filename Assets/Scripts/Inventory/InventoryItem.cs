using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string ItemName;
    public Type ItemType;
    public string ItemDescription;
    public int ItemBuyPrice;
    public int ItemSellPrice;
    public int ItemWeight;
    public Rarity ItemRarity;
    public int ItemQuantity;
    public Sprite ItemIcon;
}

public enum Rarity
{
    Very_Common,
    Common,
    Rare,
    Epic,
    Legendary,
}

public enum Type
{
    Materials,
    Weapons,
    Consumables,
    Treasure,
}
