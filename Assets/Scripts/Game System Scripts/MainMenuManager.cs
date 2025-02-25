using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnPlay() 
    {
        SceneManager.LoadScene(StringManager.tutorialScene);
    }
}

