using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupableItem : MonoBehaviour
{
    public string itemName;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "PickupItem";
        gameObject.SetActive(true);
    }

    public void onInteract()
    {
        if (GameManager.instance.getGameState() == GameManager.GameState.Pickup) //if PICKUP gameState
        {
            gameObject.SetActive(false);
            UIManager.instance.ItemsTracker.GetComponent<ItemsTracker>().UnlockItem(itemName);
        }

    }
}
