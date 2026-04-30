using System;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int currentHP;
    [SerializeField] private int maxHP = 100;

    private bool IsDead => currentHP <= 0;

    public event Action<int, int> OnHPChanged;
    public event Action OnDead;
    public event Action OnDamaged;

    //======================================================//

    #region Ref Function
    public void GetFullHP()
    {
        FullHP();
    }

    public void SetTakeDamage(int amount)
    {
        TakeDamage(amount);
    }

    public void SetHeal(int amount)
    {
        Heal(amount);
    }

    public int GetCurrentHP => currentHP;

    public int GetMaxHP => maxHP;

    public bool GetIsDead => IsDead;
    #endregion

    //======================================================//

    private void FullHP()
    {
        currentHP = maxHP;
        OnDamaged?.Invoke();
    }

    private void TakeDamage(int amount)
    {
        if (IsDead) return;

        currentHP = Mathf.Max(0, currentHP - amount);
        OnHPChanged?.Invoke(currentHP, maxHP);

        if (!IsDead)
        {
            OnDamaged?.Invoke();
        }

        if (IsDead )
        {
            OnDead?.Invoke();
        }
    }

    private void Heal(int amount)
    {
        if (IsDead) return;

        currentHP = Mathf.Min(maxHP, currentHP + amount);
        OnHPChanged?.Invoke(currentHP, maxHP);
    }


    //2차 빌드 임시코드
    public void IncreaseMaxHP(int amount)
    {
        maxHP += amount;
        currentHP += amount;
    }
}
