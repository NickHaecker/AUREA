using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private Transform camPosition = null;

    [SerializeField]
    private List<GameObject> spawnPlaces = new List<GameObject>();

    [SerializeField]
    private List<GameObject> prefabItem = new List<GameObject>();

    private List<GameObject> storedPrefab = new List<GameObject>();

    [SerializeField]
    private GameObject characterPosition = null;

    [SerializeField]
    private ItemHUDController itemHUDController = null;

    [SerializeField]
    private int maxItemCount = 5;


    private ItemData activeItem = null;

    private void Awake()
    {
        ResetItems();
        if (spawnPlaces.Count > 0)
        {
            int i = 0;
            foreach (GameObject spawn in spawnPlaces)
            {
                if (i < prefabItem.Count)
                {
                    SpawnItems(i, spawn.transform);
                }
                else
                {
                    SpawnItems(0, spawn.transform);
                }
                i++;
            }
        }
    }

    private void ResetItems()
    {
        if (storedPrefab.Count > 0)
        {
            int i = 0;
            foreach (GameObject prefab in storedPrefab)
            {
                Destroy(prefab);
                i++;
            }
        }
    }

    public Transform GetCamPosition()
    {
        return camPosition;
    }

    private void SpawnItems(int index, Transform transform)
    {
        GameObject prefab = Instantiate(prefabItem[index], transform);
        // ItemData itemData = prefab.GetComponent<ItemData>();
        // itemData.Init("item" + index.ToString(), "schmack schmack", index + index);
        storedPrefab.Add(prefab);
    }
    public void ActivateItemHUD(ItemData item, GameObject gobject)
    {
        activeItem = item;
        string title = item.GetTitle();
        string description = item.GetDescription();
        string price = item.GetPrice().ToString();
        Sprite image = item.GetImage();
        if (itemHUDController != null)
        {
            itemHUDController.Init(title, description, price, true, image, gobject.transform);
        }

    }

    public GameObject GetCharacterPosition()
    {
        return characterPosition;
    }
    public ItemHUDController GetItemHUDController()
    {
        return itemHUDController;
    }
    public void BuyItem()
    {
        if (activeItem != null)
        {
            if (Player.Instance.GetMoney() - activeItem.GetPrice() > 0 && Player.Instance.GetItems().Count <= maxItemCount)
            {
                PlayerItemData item = new PlayerItemData();
                item.amount = activeItem.GetPrice();
                item.name = activeItem.GetTitle();
                item.img = activeItem.GetImage();
                Player.Instance.BuyItem(activeItem.GetPrice(), item);
                itemHUDController.CloseHUD();

            }


        }
    }

}
