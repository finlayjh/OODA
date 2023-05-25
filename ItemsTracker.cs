using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsTracker : MonoBehaviour
{
    public GameObject glasses;
    public GameObject vest;
    public GameObject gloves;

    private List<GameObject> _itemsUIList = new List<GameObject>();
    private List<string> _itemsFoundList = new List<string>();


    void Start()
    {
        foreach (Transform t in gameObject.transform)
        {
            _itemsUIList.Add(t.gameObject);
        }
    }

    public void UnlockItem(string item)
    {
        for (int i = 0; i<_itemsUIList.Count; i++)
        {
            if (_itemsUIList[i].name == item && !_itemsFoundList.Contains(item))
            {
                _itemsFoundList.Add(item);
                _itemsUIList[i].transform.GetChild(0).gameObject.SetActive(false);
                break;
            }
        }
        if (_itemsUIList.Count == _itemsFoundList.Count)
        {
            GameManager.instance.StageComplete();
        }
    }
}
