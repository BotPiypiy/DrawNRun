using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject restartButton;

    private void Start()
    {
        restartButton.SetActive(false);
        GameManager.GameOver += GameOverHandler;
    }

    private void GameOverHandler(Transform none)
    {
        restartButton.SetActive(true);
    }

    public void OnRestartButtonCLick(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnDestroy()
    {
        GameManager.GameOver -= GameOverHandler;
    }
}
