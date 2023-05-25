using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateTransition : MonoBehaviour
{
    public GameObject panel;
    private TextMeshProUGUI _text;
    private bool _fadingToBlack;
    private float _fadeAmount;
    private Color _objectColor;
    private Color _textColor;

    void Start()
    {
        _objectColor = panel.GetComponent<Image>().color;
        _text = panel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator Transition(GameManager.GameState newState)
    {
        panel.SetActive(true);
        yield return StartCoroutine(FadeToBlack());
        _text.text = newState.ToString();
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
        GameManager.instance.setGameState(newState); 
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(FadeToBlack(false));
        panel.SetActive(false);
    }

    public IEnumerator FadeToBlack(bool fadeToBlack = true,  float speed = .5f)
    {
        Debug.Log("fade to black");
        if (fadeToBlack)
        {
            while (panel.GetComponent<Image>().color.a < 1)
            {
                Debug.Log("fading...");
                _fadeAmount = _objectColor.a + (speed * Time.deltaTime);

                _objectColor = new Color(_objectColor.r, _objectColor.g, _objectColor.b, _fadeAmount);
                panel.GetComponent<Image>().color = _objectColor;
                yield return null;
            }
        }
        else if (!fadeToBlack)
        {
            while (panel.GetComponent<Image>().color.a > 0)
            {
                Debug.Log("fading in...");
                _fadeAmount = _objectColor.a - (speed * Time.deltaTime);

                _objectColor = new Color(_objectColor.r, _objectColor.g, _objectColor.b, _fadeAmount);
                panel.GetComponent<Image>().color = _objectColor;

                _textColor = new Color(_text.color.r, _text.color.g, _text.color.b, _fadeAmount);
                _text.color = _textColor;
                yield return null;
            }
        }  
    }
}
