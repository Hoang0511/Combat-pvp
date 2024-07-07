using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public GameObject Item
    {
        get
        {
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (!Item)
        {
            DragDrop.itemBeginDragged.transform.SetParent(transform);
            DragDrop.itemBeginDragged.transform.localPosition = new Vector2(0,0);

            if (transform.CompareTag("QuickSlot") == false)
            {
                DragDrop.itemBeginDragged.GetComponent<InventoryItem>().isNowEquipped = false;
                InventorySystem.Instance.ReCalculateList();
            }
            if (transform.CompareTag("QuickSlot"))
            {
                DragDrop.itemBeginDragged.GetComponent<InventoryItem>().isNowEquipped = true;
                InventorySystem.Instance.ReCalculateList();
            }
        }
    }
}