 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject QuizPanel;
    public GameObject BlackoutPanel;
    public GameObject ActReport;
    public GameObject ConfirmButton;
    public GameObject ContinueToMenuButton;
    public GameObject ExitButton;
    public GameObject DialogPanel;
    public GameObject ItemsTracker;

    private GameObject[] quizButtons = new GameObject[3];
    private Quiz activeQuiz;

    public static UIManager instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        int i = 0;
        foreach (Transform t in QuizPanel.transform)
        {
            if (t.name.Contains("Button"))
            {
                quizButtons[i] = t.gameObject;
                i++;
            }
        }
        QuizPanel.SetActive(false);
        BlackoutPanel.SetActive(false);
        ActReport.SetActive(false);
        ConfirmButton.SetActive(false);
        ContinueToMenuButton.SetActive(false);
        DialogPanel.SetActive(false);
        ItemsTracker.SetActive(false);
    }

    public void LoadStageUI(GameManager.GameState state)
    {
        if(state == GameManager.GameState.Pickup)
        {
            ItemsTracker.SetActive(true);
        }
        else if (state == GameManager.GameState.Lecture)
        {
            ItemsTracker.SetActive(false);
            DialogPanel.SetActive(true);

        }
        else if (state == GameManager.GameState.Orient)
        {
            DialogPanel.SetActive(false);
        }
        else if (state == GameManager.GameState.Observe)
        {
            //
        }
        else if (state == GameManager.GameState.Decide)
        {
            ActReport.SetActive(false);
            ContinueToMenuButton.SetActive(false);
        }
        else if (state == GameManager.GameState.Act)
        {
            ActReport.SetActive(true);
            ActReport.GetComponent<TextMeshProUGUI>().text = GameManager.instance.GetScore();
            if (GameManager.instance.getCorrectScoreCount() == GameManager.instance.interactables.Length)
            {
                ContinueToMenuButton.SetActive(true);
                SetConfirmButton(false);
            }
            else
            {
                SetConfirmButton(true);
            }
        }
        if (state != GameManager.GameState.Act)
        {
            SetConfirmButton(false);
        }

    }

    public void LoadQuiz(Quiz q)
    {       
        activeQuiz = q;
        highlightSelectedAnswer();
        for (int i = 0; i<quizButtons.Length; i++)
        {
            quizButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.options[i];
        }
        QuizPanel.SetActive(true);
    }

    public void OnAnswerSelect(int answer)
    {
        activeQuiz.SelectedAnswer = answer;
        highlightSelectedAnswer();
        QuizPanel.SetActive(false);
        GameManager.instance.SaveQuizResult(activeQuiz);
    }

    private void highlightSelectedAnswer()
    {
        for (int i = 0; i<quizButtons.Length; i++)
        {
            if (activeQuiz.SelectedAnswer == i)
            {
                quizButtons[i].GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                quizButtons[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void ToggleResultsCard(bool show, bool button = false)
    {
        if (show)
        {
            ActReport.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = GameManager.instance.GetScore();
            if (button)
            {
                ContinueToMenuButton.SetActive(true);
            }
        }
        else
        {
            ContinueToMenuButton.SetActive(false);
        }
        ActReport.SetActive(show);
    }

    public void SetConfirmButton(bool b)
    {
        ConfirmButton.SetActive(b);
    }
}