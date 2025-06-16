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
        if (eventData.button == PointerEventData.InputButton.Left) // ���� Ŭ��
            SlotClick();
        else if (eventData.button == PointerEventData.InputButton.Right) // ������ Ŭ��
            UseItem();
    }
    void UseItem()
    {
        if (transform.childCount > 0) // ���Կ� �������� �ִ� ���
            transform.GetChild(0).GetComponent<ItemUI>().item.Use(this);
    }
    void SlotClick()
    {
        if (transform.childCount > 0) // �����µ� ���Կ� �������� �ִ� ���
        {
            if (inventory.activeSlotItem != null) // ��� �ִ� �������� ������ ���� ĭ�� ����
            {
                inventory.activeSlotItem.transform.SetParent(transform);
                inventory.activeSlotItem.transform.localPosition = Vector3.zero;
            }
            // �� ���Կ� �ִ� ������ ���
            inventory.activeSlotItem = transform.GetChild(0).gameObject;
        }

        else //  �����µ� �������� ���� ���
        {
            if (inventory.activeSlotItem != null) // �������� ��� �ִ� ���
            {
                inventory.activeSlotItem.TryGetComponent<ItemUI>(out ItemUI carriedItemComponent);
                if ((myTag == carriedItemComponent.item.itemTag) || (myTag == SlotTag.None)) // ���� �ױ� �˻�
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

        if (myTag == SlotTag.None) // �⺻ ���Կ��� ��Ŭ��
        {
            if (equipmentSlot.transform.childCount != 0) // ��� ���Կ� �ٸ� ��� ����
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
