using UnityEngine;

public class EnergyPickup : MonoBehaviour
{
    public int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().Energy(healAmount);
            Destroy(gameObject);
        }
    }
}
