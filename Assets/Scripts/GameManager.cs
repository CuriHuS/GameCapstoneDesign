using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static string coinPrefsName = "Coins_Player";

    public TMP_Text sunDisp;
    public int startingSunAmnt;
    public int SunAmount = 0;

    public static int health = 100; // Player's health

    public GameObject decorativeZombies;

    public Transform cardSlotsHolder;

    public ZombieManager zombieManager;

    public Animator cameraPan;

    public static int currentAmount;
    public int preCurrentAmount = -1;

    public TMP_Text coinDisplay;

    public HealthManager HealthBar;

    private void Start()
    {
        currentAmount = PlayerPrefs.GetInt(coinPrefsName);

        coinDisplay.SetText(currentAmount + "");

        CardManager.isGameStart = false;
        AddSun(startingSunAmnt);
    }

	public void Update()
	{
		if (preCurrentAmount != currentAmount)
		{
            preCurrentAmount = currentAmount;
            coinDisplay.SetText(currentAmount + "");
        }

        if (health <= 0)
        {
            GameOver();
        }
	}

	public void StartMatch()
	{
        cameraPan.SetTrigger("PanToPlants");
        CardManager.isGameStart = true;
        RefreshAllPlantCards();
        zombieManager.SpawnZombies();
	}

    public void AddSun(int amnt)
    {
        SunAmount += amnt;
        sunDisp.text = "" + SunAmount;
    }
    
    public void DeductSun(int amnt)
    {
        SunAmount -= amnt;
        sunDisp.text = "" + SunAmount;
    }

    public void RefreshAllPlantCards()
	{
        foreach (Transform card in cardSlotsHolder)
        {
            try
            {
                card.GetComponent<CardManager>().StartRefresh();
            }
            catch (System.Exception)
            {
                Debug.LogError("Card does not contain CardManager script!");
            }
        }
    }

    public static void IncrementCoins(int value)
	{
        currentAmount += value;
    }

    public static void DecreaseHealth(int value)
    {
        health -= value;
    }

    void GameOver()
    {

    }

	public void OnApplicationQuit()
	{
        PlayerPrefs.SetInt(coinPrefsName, currentAmount);
	}
}
