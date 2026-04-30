/*using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // ======================
    // 저장
    // ======================
    public void SaveGame(PlayerController player)
    {

        SaveData data = new SaveData();

        // Wave
        data.wave = WaveManager.Instance.GetCurrentWave;

        // HP
        HPManager hp = player.GetComponent<HPManager>();
        data.currentHP = hp.GetCurrentHP;
        data.maxHP = hp.GetMaxHP;

        // 공격력
        DamageManager dmg = player.GetComponent<DamageManager>();
        data.damage = dmg.GetDamage;

        // 공격속도
        AttackManager atk = player.GetComponent<AttackManager>();
        data.attackCooldown = atk.GetAttackCooldown;

        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();

        Debug.Log("게임 저장 완료");
    }

    // ======================
    // 불러오기
    // ======================
    public void LoadGame(PlayerController player)
    {
        if (!PlayerPrefs.HasKey("SaveData"))
        {
            Debug.Log("저장 데이터 없음");
            return;
        }

        string json = PlayerPrefs.GetString("SaveData");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // Wave
        int wave = data.wave;

        int maxWave = WaveManager.Instance.GetWaveCount();

        if (wave >= maxWave || wave < 0)
        {
            Debug.Log("클리어 상태 → 초기화");
            wave = 0;
        }

        WaveManager.Instance.SetWave(wave);
        WaveManager.Instance.ResetWaveState();

        // HP
        HPManager hp = player.GetComponent<HPManager>();
        hp.SetHeal(data.maxHP);
        hp.SetTakeDamage(data.maxHP - data.currentHP);

        // 공격력
        DamageManager dmg = player.GetComponent<DamageManager>();
        dmg.SetDamage(data.damage);

        // 공격속도
        AttackManager atk = player.GetComponent<AttackManager>();
        atk.SetAttackCooldown(data.attackCooldown);

        Debug.Log("게임 불러오기 완료");

    }

    // ======================
    // 초기화
    // ======================
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("SaveData");
    }
}*/
