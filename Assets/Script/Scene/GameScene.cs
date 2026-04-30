using UnityEngine;

public class StartScene : MonoBehaviour
{
    public void GoToMainGame()
    {
        GameSceneManager.Instance.LoadSceneByName("MainGame");
    }
}
