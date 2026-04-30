using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float attackCooldown = 1.0f;

    public float GetAttackCooldown => attackCooldown;
    public void SetAttackCooldown(float value) {  attackCooldown = value; }


    //2차 빌드 임시코드
    public void ReduceCooldown(float percent)
    {
        attackCooldown *= (1f - percent);
    }

    public void Incease(float percent)
    {
        attackCooldown *= (1f + percent);
    }
}
