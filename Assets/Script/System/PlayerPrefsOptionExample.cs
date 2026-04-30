using UnityEngine;

public class PlayerPrefsOptionExample : MonoBehaviour
{
    private const string PLAYER_NAME_KEY = "PlayerName";
    private const string BGM_VOLUME_KEY = "BgmVolume";

    public string playerName = "Hero";
    public float bgmVolume = 0.8f;

    public void SaveOption()
    {
        // 문자열 저장
        PlayerPrefs.SetString(PLAYER_NAME_KEY, playerName);

        // 실수 저장
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, bgmVolume);

        PlayerPrefs.Save();

        Debug.Log("이름과 볼륨 저장 완료");
    }

    public void LoadOption()
    {
        string loadedName = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Guest");
        float loadedVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1.0f);

        Debug.Log("불러온 이름: " + loadedName);
        Debug.Log("불러온 볼륨: " + loadedVolume);
    }

    public void DeleteOption()
    {
        PlayerPrefs.DeleteKey(PLAYER_NAME_KEY);
        PlayerPrefs.DeleteKey(BGM_VOLUME_KEY);
        PlayerPrefs.Save();

        Debug.Log("옵션 데이터 삭제 완료");
    }
}

