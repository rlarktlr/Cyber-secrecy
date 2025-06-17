using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour, IWindowUI
{
    [SerializeField] GameObject _root;
    public PlayerStat playerStatus;
    [SerializeField] TMP_Text coinText;

    [Header("Slot")]
    [SerializeField] GameObject inventorySlotPrefab;
    [SerializeField] List<SlotUI> inventorySlots;
    public SlotUI[] equipmentSlots; // Inspector에서 할당
    [SerializeField] GameObject content;
    public GameObject activeSlotItem;

    [Header("Item")]
    [SerializeField] Item[] itemTable;
    [SerializeField] GameObject itemPrefab;

    public WindowUIType WindowType => WindowUIType.Inventory;

    public bool IsActive => _root.activeSelf;

    void Awake()
    {
        for (int count = 0; count < 28; count++)
        {
            SlotUI slot = Instantiate(inventorySlotPrefab, content.transform).GetComponent<SlotUI>();
            slot.Initialize(this);
            inventorySlots.Add(slot);
        }

        // 나중에 삭제
        PickItem(itemTable[0]);
    }

    void Update()
    {
        if (!IsActive)
        {
            ActiveSlotReset();
        }
        if (activeSlotItem != null)
        {
            activeSlotItem.transform.SetParent(transform);
            activeSlotItem.transform.position = Input.mousePosition;
        }
        //coinText.SetText($"{playerStatus.coin}");
    }

    public void CreateItem(int itemID, int slotIndex)
    {
        Instantiate(itemPrefab, inventorySlots[slotIndex].transform).GetComponent<ItemUI>().Initialize(itemTable[itemID]);
    }

    public void CreateEquipmentItem(int itemID, int slotIndex)
    {
        Instantiate(itemPrefab, equipmentSlots[slotIndex].transform).GetComponent<ItemUI>().Initialize(itemTable[itemID]);
    }

    public void PickItem(Item item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount == 0)
            {
                Instantiate(itemPrefab, slot.transform).GetComponent<ItemUI>().Initialize(item);
                break;
            }
        }
    }

    public void AutoPlaceItem(Transform item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount == 0)
            {
                item.SetParent(slot.transform);
                item.localPosition = Vector3.zero;
                break;
            }
        }
    }

    //public void BuyItem(Item item, int price)
    //{
    //    if (playerStatus.coin >= price)
    //    {
    //        playerStatus.coin -= price;
    //        PickItem(item);
    //    }
    //}

    public void ActiveSlotReset()
    {
        if (activeSlotItem != null)
        {
            AutoPlaceItem(activeSlotItem.transform);
            activeSlotItem = null;
        }
    }

    public List<SlotUI> GetInventorySlots()
    {
        return inventorySlots;
    }

    public void ClearInventory()
    {
        foreach (var slot in inventorySlots)
        {
            for (int i = slot.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(slot.transform.GetChild(i).gameObject);
            }
        }
    }

    public void Open()
    {
        UIAnimationUtil.PlayScaleIn(this, _root, _root.transform, 0.15f);
    }

    public void Close()
    {
        UIAnimationUtil.PlayScaleOut(this, _root, _root.transform, 0.1f);
    }

    public void Confirm()
    { }

    public void OnClose()
    {
        ActiveSlotReset();
    }
}
