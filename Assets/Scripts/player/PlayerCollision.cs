using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerStats playerStats;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            playerStats.TakeDamage(1);
            Debug.Log("Player levou dano!");
        }
    }
}
