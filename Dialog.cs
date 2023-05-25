using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public string[] lines;
    public float textSpeed;

    private int _index;
    private TextMeshProUGUI _text;
    private float _width;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Screen Width : " + Screen.width);
        //Debug.Log("exit Width : " + UIManager.instance.ExitButton.GetComponent<RectTransform>().sizeDelta.x);
        //_width = Screen.width - UIManager.instance.ExitButton.GetComponent<RectTransform>().sizeDelta.x;
        //gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_width, 200);

        _text = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _text.text = string.Empty;
        startDialog();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_text.text == lines[_index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                _text.text = lines[_index];
            }
        }  
    }

    void startDialog()
    {
        _index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[_index].ToCharArray())
        {
            _text.text += c;
            yield return new WaitForSeconds(textSpeed);

        }
    }

    void NextLine()
    {
        if ( _index < lines.Length - 1)
        {
            _index++;
            _text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
        if(_index == lines.Length - 1)
        {
            GameManager.instance.StageComplete();
        }
    }
}
