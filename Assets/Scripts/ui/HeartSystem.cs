using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartSystem : MonoBehaviour
{
    [Header("Referência do Player")]
    public PlayerStats playerStats;

    [Header("Corações")]
    public List<Image> hearts;   // Arraste os 3 corações aqui
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Energia")]
    public List<Image> energyBars; // Arraste as 4 barras
    public Sprite energyOn;
    public Sprite energyOff;

    void Update()
    {
        UpdateHearts();
        UpdateEnergy();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < playerStats.currentLives)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    void UpdateEnergy()
    {
        for (int i = 0; i < energyBars.Count; i++)
        {
            if (i < playerStats.currentEnergy)
                energyBars[i].sprite = energyOn;
            else
                energyBars[i].sprite = energyOff;
        }
    }
}
