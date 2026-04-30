using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject mainBuilding;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindMainBuilding();
    }

    private void RebindMainBuilding()
    {
        if (mainBuilding == null)
        {
            GameObject found = GameObject.FindWithTag("MainBuilding");

            if (found != null)
            {
                mainBuilding = found;
                Debug.Log("MainBuilding 재연결 완료");
            }
            else
            {
                Debug.LogError("MainBuilding 못 찾음!");
            }
        }
    }
}
