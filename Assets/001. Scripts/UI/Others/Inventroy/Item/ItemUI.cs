using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    public Item item;

    public void Initialize(Item item)
    {
        this.item = item;
        itemIcon.sprite = item.sprite;
        gameObject.name = item.name;
    }
}
