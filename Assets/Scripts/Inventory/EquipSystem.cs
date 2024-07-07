using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    public GameObject quickSlotPanel;

    public List<GameObject> quickSlotList = new List<GameObject>();

    public List<string> itemList = new List<string>();


    public GameObject numbersHolder;

    public int selectNumber = -1;
    public GameObject selectedItem;

    public GameObject toolHolder;
    public GameObject selectedItemModel;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        PopulatSlotList();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuickSlot(7);
        }
    }

    public void SelectQuickSlot(int number)
    {
        if(CheckSlotIsFull(number) == true)
        {
            if (selectNumber != number)
            {
                selectNumber = number;

                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }
                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;


                SetEquipModel(selectedItem);

                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }
                Text toBeChanged = numbersHolder.transform.Find("Number" + number).transform.Find("Text").GetComponent<Text>();
                toBeChanged.color = Color.white;
            }
            else
            {
                selectNumber = -1;
                if(selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if (selectedItemModel != null)
                {
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent <Text>().color = Color.gray;
                }
            }

            
        }
        
    }


    private void SetEquipModel(GameObject selectedItem)
    {

        if (selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }


        string selectedItemName = selectedItem.name.Replace("(Clone)","");
        selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_model"),
            new Vector3(0,0,0), Quaternion.Euler(70f, -90f, -180f));
        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }

    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotList[slotNumber - 1].transform.GetChild(0).gameObject;  
    }

    bool CheckSlotIsFull(int slotNumber)
    {
        if (quickSlotList[slotNumber -1].transform.childCount> 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PopulatSlotList()
    {
       foreach (Transform child in quickSlotPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        GameObject availableSlot = FindNextEmptySlot();

        itemToEquip.transform.SetParent(availableSlot.transform, false);
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        itemList.Add(cleanName);

        InventorySystem.Instance.ReCalculateList();
        
    }

    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in quickSlotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }


    public bool CheckIfFull()
    {
        int counter = 0;

        foreach (GameObject slot in quickSlotList)
        {
            if(slot.transform.childCount > 0)
            {
                counter += 1;

            }
        }
        if(counter == 7)
        {
            return true;
        }
        return false;

    }
}
