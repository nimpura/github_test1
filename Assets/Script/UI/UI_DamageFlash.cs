using UnityEngine;

public class UI_DamageFlash : MonoBehaviour
{
    [SerializeField] private HPManager targetHP;
    [SerializeField] private GameObject flashUI;

    public void SetTarget(HPManager hp)
    {
        targetHP = hp;

        targetHP.OnDamaged += ShowFlash;
    }

    private void OnDisable()
    {
        if (targetHP != null)
            targetHP.OnDamaged -= ShowFlash;
    }

    private void ShowFlash()
    {
        flashUI.SetActive(true);
        Invoke(nameof(HideFlash), 0.1f);
    }

    private void HideFlash()
    {
        flashUI.SetActive(false);
    }
}
