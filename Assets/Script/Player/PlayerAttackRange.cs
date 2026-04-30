using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    private List<EnemyController> enemies = new List<EnemyController>();

    public EnemyController GetNearestEnemy(Vector2 playerPos)
    {
        enemies.RemoveAll(e => e == null);

        float minDist = Mathf.Infinity;
        EnemyController target = null;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(playerPos, enemy.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                target = enemy;
            }
        }

        return target;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();

            if (enemy != null && !enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemies.Remove(enemy);
            }
        }
    }
}
