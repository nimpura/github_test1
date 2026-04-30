//2차 빌드 임시코드
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private TMP_Text[] optionTexts;

    private PlayerController player;
    private Action onSelected;

    private List<UpgradeData> currentOptions = new List<UpgradeData>();

    public void Open(PlayerController player, Action callback)
    {
        this.player = player;
        onSelected = callback;

        gameObject.SetActive(true);

        GenerateOptions();
    }

    private void GenerateOptions()
    {
        currentOptions.Clear();

        int count = Mathf.Min(3, optionButtons.Length, optionTexts.Length);

        for (int i = 0; i < count; i++)
        {
            UpgradeData data = GetRandomUpgrade();
            currentOptions.Add(data);

            optionTexts[i].text = GetDescription(data);

            int index = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => SelectUpgrade(index));
        }
    }

    private UpgradeData GetRandomUpgrade()
    {
        UpgradeType type = (UpgradeType)UnityEngine.Random.Range(0, 3);

        switch (type)
        {
            case UpgradeType.Damage:
                return new UpgradeData { type = type, value = 5 };

            case UpgradeType.AttackSpeed:
                return new UpgradeData { type = type, value = 0.2f };

            case UpgradeType.MaxHP:
                return new UpgradeData { type = type, value = 20 };
        }

        return null;
    }

    private string GetDescription(UpgradeData data)
    {
        switch (data.type)
        {
            case UpgradeType.Damage:
                return $"Damage +{data.value}";

            case UpgradeType.AttackSpeed:
                return $"AttackCoolDown +{data.value * 100}%";

            case UpgradeType.MaxHP:
                return $"Max HP +{data.value}";
        }

        return "";
    }

    private void SelectUpgrade(int index)
    {
        player.ApplyUpgrade(currentOptions[index]);

        gameObject.SetActive(false);

        onSelected?.Invoke();
    }


}
