using UnityEngine;

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

public class PlayerStats : MonoBehaviour
{
    [Header("Settings")]
    public Difficulty currentDifficulty;

    [Header("Vida")]
    public int maxLives = 3;
    public int currentLives;

    [Header("Energia")]
    public int maxEnergy = 4;
    public int currentEnergy;

    [Header("Cura (apenas modo fácil)")]
    public GameObject healItemPrefab;   // item de cura no Prefab
    public Vector2 spawnAreaMin;        // posição mínima da tela
    public Vector2 spawnAreaMax;        // posição máxima da tela

    void Start()
    {
        SetupDifficulty();
    }

    void SetupDifficulty()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                currentLives = 3;
                break;

            case Difficulty.Normal:
                currentLives = 3;
                break;

            case Difficulty.Hard:
                currentLives = 1;
                break;
        }

        currentEnergy = 0;
    }

    // ----------------------------------------------------
    // TOMAR DANO
    // ----------------------------------------------------
    public void TakeDamage(int dmg)
    {
        currentLives -= dmg;

        if (currentDifficulty == Difficulty.Easy)
        {
            TrySpawnHealItem();
        }

        if (currentLives <= 0)
        {
            PlayerDie();
        }
    }

    // ----------------------------------------------------
    // CURA
    // ----------------------------------------------------
    void TrySpawnHealItem()
    {
        if (currentLives < maxLives)
        {
            Vector2 spawnPos = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            Instantiate(healItemPrefab, spawnPos, Quaternion.identity);
        }
    }

    public void Heal(int amount)
    {
        currentLives += amount;
        if (currentLives > maxLives)
            currentLives = maxLives;
    }

    // ----------------------------------------------------
    // ENERGIA
    // ----------------------------------------------------
    public void AddEnergy(int amount)
    {
        currentEnergy += amount;

        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;
    }

    public bool UseEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            return true;
        }

        return false;
    }

    // ----------------------------------------------------
    // MORTE DO PLAYER
    // ----------------------------------------------------
    void PlayerDie()
    {
        Debug.Log("PLAYER MORREU!");
        // pode chamar GameOver, reiniciar fase etc.
    }
}
