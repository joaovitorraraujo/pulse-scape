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
                maxLives = 3;
                currentLives = 3;
                currentEnergy = 4;
                break;

            case Difficulty.Normal:
                maxLives = 3;
                currentLives = 3;
                currentEnergy = 4;
                break;

            case Difficulty.Hard:
                maxLives = 1;
                currentLives = 1;
                currentEnergy = 0;
                break;
        }
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
        Debug.Log("SPAWN HEAL CHAMADO");

        if (currentLives < maxLives)
        {
            Vector2 spawnPos = Camera.main.ScreenToWorldPoint(
                new Vector2(Random.Range(100, Screen.width - 100),
                            Random.Range(100, Screen.height - 100))
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
