using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    public InventoryUI inventory;
    [SerializeField] SlotTag myTag;

    public void Initialize(InventoryUI inventory)
    {
        this.inventory = inventory;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // 왼쪽 클릭
            SlotClick();
        else if (eventData.button == PointerEventData.InputButton.Right) // 오른쪽 클릭
            UseItem();
    }
    void UseItem()
    {
        if (transform.childCount > 0) // 슬롯에 아이템이 있는 경우
            transform.GetChild(0).GetComponent<ItemUI>().item.Use(this);
    }
    void SlotClick()
    {
        if (transform.childCount > 0) // 눌렀는데 슬롯에 아이템이 있는 경우
        {
            if (inventory.activeSlotItem != null) // 들고 있는 아이템이 있으면 누른 칸에 놓고
            {
                inventory.activeSlotItem.transform.SetParent(transform);
                inventory.activeSlotItem.transform.localPosition = Vector3.zero;
            }
            // 이 슬롯에 있는 아이템 들기
            inventory.activeSlotItem = transform.GetChild(0).gameObject;
        }

        else //  눌렀는데 아이템이 없는 경우
        {
            if (inventory.activeSlotItem != null) // 아이템을 들고 있는 경우
            {
                inventory.activeSlotItem.TryGetComponent<ItemUI>(out ItemUI carriedItemComponent);
                if ((myTag == carriedItemComponent.item.itemTag) || (myTag == SlotTag.None)) // 슬롯 테그 검사
                {
                    inventory.activeSlotItem.transform.SetParent(transform);
                    inventory.activeSlotItem.transform.localPosition = Vector3.zero;
                    inventory.activeSlotItem = null;
                }
            }
        }
    }
    #region Use
    public void EquipArmor()
    {
        Transform thisItem = transform.GetChild(0).gameObject.transform;
        var equipmentSlot = inventory.equipmentSlots[(int)thisItem.GetComponent<ItemUI>().item.itemTag];

        if (myTag == SlotTag.None) // 기본 슬롯에서 우클릭
        {
            if (equipmentSlot.transform.childCount != 0) // 장비 슬롯에 다른 장비가 있음
            {
                GameObject currentEquipment = equipmentSlot.transform.GetChild(0).gameObject;
                currentEquipment.transform.SetParent(transform);
                currentEquipment.transform.localPosition = Vector3.zero;
            }
            thisItem.SetParent(equipmentSlot.transform);
            thisItem.localPosition = Vector3.zero;
        }
        else
            inventory.AutoPlaceItem(thisItem);
    }
    #endregion
}
