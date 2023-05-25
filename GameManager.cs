using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject quizPanel;
    public GameObject[] interactables;
    public GameObject transitionPanel;

    private GameState _gameState;
    private GameObject _mainCam;
    private GameObject _prepRoomCam;
    private Camera _activeCam;
    private bool _canInteract = true;
    private bool _isStageComplete;
    private List<GameObject> _interactedList = new List<GameObject>();
    private List<Quiz> _quizzesAnsweredList = new List<Quiz>();

    public static GameManager instance { get; private set; }

    public enum GameState
    {
        Pickup,
        Lecture,
        Orient,
        Observe,
        Decide,
        Act
    }

    void Awake()
    {
        instance = this;

        foreach (Camera c in Camera.allCameras)
        {
            if(c.gameObject.name == "Main Camera")
            {
                _mainCam = c.gameObject;
            }
            else
            {
                _prepRoomCam = c.gameObject;
            }
        }
        _mainCam.SetActive(false);
        _prepRoomCam.SetActive(false);
    }

    void Start()
    {
        _isStageComplete = false;
        setGameState(GameState.Pickup);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            RaycastHit hit;
            Ray ray = _activeCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Interactable")
                {
                    hit.collider.GetComponent<Interactable>().onInteract();
                    //play confirm sound
                    if (_gameState == GameState.Decide && _canInteract)
                    {
                        _activeCam.GetComponent<CameraMove>().QuizLookAt(hit.transform);              //move cam to object
                        UIManager.instance.LoadQuiz(hit.collider.GetComponent<Interactable>().quiz);  // show quiz
                    }
                    else if (_gameState == GameState.Observe)
                    {
                        onInteract(hit.transform.gameObject);
                    }
                }
                else if (hit.collider.tag == "PickupItem")
                {
                    if (_gameState == GameState.Pickup && _canInteract)
                    {
                        hit.collider.GetComponent<PickupableItem>().onInteract();
                    }
                }
                //play miss sound
            }
        }
    }

    public void SaveQuizResult(Quiz q)
    {
        if (!_quizzesAnsweredList.Contains(q))
        {
            _quizzesAnsweredList.Add(q);
        }
        if (_quizzesAnsweredList.Count == interactables.Length)
        {
            StageComplete();
        }
    }

    public void StageComplete()
    {
        _isStageComplete = true;
        UIManager.instance.SetConfirmButton(true);
        //audio
    }

    public void GoNextStage()
    {
        if (_isStageComplete)
        {
            UIManager.instance.SetConfirmButton(false);
            _isStageComplete = false;
            Debug.Log("swapping stage...");
            if (_gameState != GameState.Act && _gameState != GameState.Pickup)
            {
                StartCoroutine(TransitionWrapperCoroutine(_gameState+1));
            }
            else if (_gameState == GameState.Pickup)
            {
                setGameState(_gameState+1);
            }
            else
            {
                StartCoroutine(TransitionWrapperCoroutine(GameState.Decide)); //only here if failed decide
            }
        }
    }

    private void onInteract(GameObject o)
    {
        if (!_interactedList.Contains(o))
        {
            _interactedList.Add(o);
        }
        if (_interactedList.Count == interactables.Length)
        {
            StageComplete();
        }
    }

    private void clearQuizSelectedAnswers()
    {
        Debug.Log(interactables);
        foreach (GameObject o in interactables)
        {
            Debug.Log(o.GetComponent<Interactable>().quiz.SelectedAnswer);
            o.GetComponent<Interactable>().quiz.SelectedAnswer = -1;
        }
    }

    public string GetScore()
    {
        return getCorrectScoreCount() + "/" + interactables.Length;
    }

    public int getCorrectScoreCount()
    {
        int count = 0;
        foreach (Quiz q in _quizzesAnsweredList)
        {
            if (q.SelectedAnswer == q.CorrectAnswer)
            {
                count++;
            }
        }
        return count;
    }

    private void SwapCamera()
    {
        if (_activeCam == _mainCam.GetComponent<Camera>() || _activeCam == null)
        {
            _mainCam.SetActive(false);
            _prepRoomCam.SetActive(true);
            _activeCam = _prepRoomCam.GetComponent<Camera>();
        }
        else
        {
            _prepRoomCam.SetActive(false);
            _mainCam.SetActive(true);
            _activeCam = _mainCam.GetComponent<Camera>();
        }
    }

    public GameState getGameState()
    {
        return _gameState;
    }

    public void setGameState(GameState state)
    {
        if (_gameState != state)
        {
            _gameState = state;
        }
        if (_gameState == GameState.Pickup )
        {    
            SwapCamera();
        }
        else if (_gameState == GameState.Orient)
        {
            SwapCamera();
        }
        else if(_gameState == GameState.Decide)
        {
            clearQuizSelectedAnswers();
        }
        UIManager.instance.LoadStageUI(_gameState);
    }

    public bool getCanInteract()
    {
        return _canInteract;
    }

    public IEnumerator TransitionWrapperCoroutine(GameState state)
    {
        _canInteract = false;
        yield return StartCoroutine(gameObject.GetComponent<StateTransition>().Transition(state));
        _canInteract = true;
    }
}
