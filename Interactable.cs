using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Outline _outline;
    public Quiz quiz;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Interactable";

        _outline = gameObject.GetComponent<Outline>();
        if (GameManager.instance.getGameState() == GameManager.GameState.Decide) //if DECIDE gameState
        {
            _outline.enabled = true;
        }
        else
        {
            _outline.enabled = false;
        }
        
    }

    public void onInteract()
    {
        if(GameManager.instance.getGameState() == GameManager.GameState.Observe)//if ORIENT gameState
        {
            _outline.enabled = true;
        }
        
    }
}
