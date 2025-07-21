using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int gameSceneIndex;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(gameSceneIndex);
    }
}
