using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadOutside()
    {
        SceneManager.LoadScene("Outside");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadTrailer()
    {
        SceneManager.LoadScene("Trailer");
    }
}
