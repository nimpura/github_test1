using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private GameObject pauseUI;

    private bool isPaused = false;

    private void OnEnable()
    {
        inputReader.OnPauseAction += TogglePause;
    }

    private void OnDisable()
    {
        inputReader.OnPauseAction -= TogglePause;
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
        inputReader.IsInputBlocked = true;
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        inputReader.IsInputBlocked = false;
        isPaused = false;
    }
}
