using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int Damage = 5;

    public int GetDamage => Damage;
    public void SetDamage(int value) { Damage = value; }

    public void IncreaseDamage(int amount)
    {
        Damage += amount;
    }

    public void DecreaseDamage(int amount)
    {
        Damage -= amount;
    }
}
