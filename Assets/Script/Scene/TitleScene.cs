using UnityEngine;

public class TitleScene : MonoBehaviour
{
    public void GoToTitle()
    {
        GameSceneManager.Instance.LoadSceneByName("Title");
    }
}
