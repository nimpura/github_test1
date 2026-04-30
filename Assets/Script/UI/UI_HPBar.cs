using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : MonoBehaviour
{
    [SerializeField] private HPManager targetHP;
    [SerializeField] private Image fillImage;

    private void OnDisable()
    {
        if (targetHP != null)
        {
            targetHP.OnHPChanged -= UpdateHP;
        }
    }

    private void UpdateHP(int current, int max)
    {
        Debug.Log($"HP ¾÷µ¥ÀÌÆ® È£ÃâµÊ: {current}/{max}");

        if (fillImage == null)
        {
            Debug.LogError("fillImage ¾øÀ½!");
            return;
        }

        fillImage.fillAmount = (float)current / max;
    }

    public void SetTarget(HPManager hp)
    {
        targetHP = hp;

        Debug.Log("HPBar Å¸°Ù ¿¬°áµÊ");

        targetHP.OnHPChanged += UpdateHP;

        UpdateHP(targetHP.GetCurrentHP, targetHP.GetMaxHP);
    }
}
