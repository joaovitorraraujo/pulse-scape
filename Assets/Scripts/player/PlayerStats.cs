using System.Collections;
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
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip damageSound;


    [Header("Settings")]
    public Difficulty currentDifficulty;

    [Header("Vida")]
    public int maxLives = 3;
    public int currentLives;
    private DamageVFX damageVFX;

    [Header("Invencibilidade")]
    public float invincibleDuration = 1f;
    private bool isInvincible = false;

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
        if (isInvincible) return; // ignora dano se estiver invencível
        currentLives -= dmg;

        // efeito de hit
        if (damageVFX != null)
            damageVFX.PlayHitFlash();

         // som de dano
        if (audioSource != null && damageSound != null)
            audioSource.PlayOneShot(damageSound);

        CameraShake.Instance.Shake(0.2f, 0.15f);
        StartCoroutine(InvincibilityRoutine()); // ativa invencibilidade

        if (currentDifficulty == Difficulty.Easy)
        {
            TrySpawnHealItem();
        }

        if (currentLives <= 0)
        {
            PlayerDie();
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        // troca layer para ignorar colisão com inimigos
        gameObject.layer = LayerMask.NameToLayer("InvinciblePlayer");

        yield return new WaitForSeconds(invincibleDuration);

        // volta ao layer normal
        gameObject.layer = LayerMask.NameToLayer("Default");

        isInvincible = false;
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
