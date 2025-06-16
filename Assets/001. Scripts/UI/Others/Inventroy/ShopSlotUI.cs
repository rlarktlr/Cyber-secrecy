using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{

    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemPrice;
    Item item;
    int price;
    InventoryUI inventory;

    public void Initialize(Item item, int price, InventoryUI inventory)
    {
        this.inventory = inventory;
        this.item = item;
        this.price = price;

        itemImage.sprite = item.sprite;
        itemName.text = item.itemName;
        itemPrice.text = $"{price}";
    }

    public void OnBuyButton()
    {
        //inventory.BuyItem(item, price);
    }
}
