using UnityEngine;
using TMPro;

public class UI_Wave : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text enemyText;


    private void OnEnable()
    {
        if (waveManager != null)
        {
            waveManager.OnwaveChanged += WaveUIUpdate;
        }
    }

    private void OnDisable()
    {
        if (waveManager != null)
        {
            waveManager.OnwaveChanged -= WaveUIUpdate;
        }
    }

    private void WaveUIUpdate(int wave, int enemy)
    {
        waveText.text = $"Wave : {wave+1}";
        enemyText.text = $"Enemy : {enemy}";
    }
}
