using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [Header("Controller")]
    private InventoryController inventoryController;

    [Header("Initial Data")]
    [SerializeField] private int size;
    [SerializeField] private int maxWeight;
    [SerializeField] private GameObject iconTransforms;
    [SerializeField] private InventoryItem[] AllItems;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI weightText;

    //[Header("Testing")]
    //[SerializeField] private InventoryItem apple;
    

    void Start() => inventoryController = new InventoryController(this, size, maxWeight);
    public void SetBalanceText(int balance) => balanceText.text = $"Gold: {balance}";
    public void SetWeightText(int weight) => weightText.text = $"Weight: {weight}";
    public void SetIcon(InventoryItem item, int i)
    {
        GameObject icon = iconTransforms.transform.GetChild(i).gameObject;

        icon.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = item.ItemIcon;
        icon.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = item.ItemSellPrice.ToString();
    }
    public void GatherResources() => inventoryController.AddResourcesInInventory(AllItems);
}
