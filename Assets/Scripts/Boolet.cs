using UnityEngine;

public class Boolet : MonoBehaviour
{
    [SerializeField] private float enemyDamage = 50f;
    [SerializeField] private float playerDamage = 20f;

    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Player"))
        {
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(playerDamage);
        }
        if (hitTransform.CompareTag("Enemy"))
        {
            hitTransform.GetComponent<EnemyScript>().TakeDamage(enemyDamage);
        }
    }
}
