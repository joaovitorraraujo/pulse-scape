using UnityEngine;
using UnityEngine.SceneManagement;

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
    private DamageVFX damageVFX;

    [Header("Energia")]
    public int maxEnergy = 4;
    public int currentEnergy;

    [Header("Cura (apenas modo fácil)")]
    public GameObject healItemPrefab;   // item de cura no Prefab
    public GameObject energyItemPrefab;   // item de cura no Prefab
    public Vector2 spawnAreaMin;        // posição mínima da tela
    public Vector2 spawnAreaMax;        // posição máxima da tela

    void Start()
    {
        SetupDifficulty();
        damageVFX = GetComponent<DamageVFX>();
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

        // efeito de hit
        if (damageVFX != null)
            damageVFX.PlayHitFlash();

        CameraShake.Instance.Shake(0.2f, 0.15f);

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
    void TrySpawnEnergyItem()
    {
        Debug.Log("SPAWN ENERGY CHAMADO");

        if (currentEnergy < maxEnergy)
        {
            Vector2 spawnPos = Camera.main.ScreenToWorldPoint(
                new Vector2(Random.Range(100, Screen.width - 100),
                            Random.Range(100, Screen.height - 100))
            );

            Instantiate(energyItemPrefab, spawnPos, Quaternion.identity);
        }
    }

    public void Heal(int amount)
    {
        currentLives += amount;
        if (currentLives > maxLives)
            currentLives = maxLives;
    }
    public void Energy(int amount)
    {
        currentEnergy += amount;
        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;
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
            TrySpawnEnergyItem();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // pode chamar GameOver, reiniciar fase etc.
    }
}   
