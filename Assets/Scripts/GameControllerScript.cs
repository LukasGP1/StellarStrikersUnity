using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    [Serializable]
    public struct PlayerSettings
    {
        public GameObject player;
        public float moveSpeed;
        public int health;
        public GameObject bullet;
        public float bulletSpeed;
        public Color bulletColor;
        public Sprite baseSprite;
        public Sprite damage0;
        public Sprite damage1;
        public Sprite damage2;
    }

    [Serializable]
    public struct EnemyFighterSettings
    {
        public GameObject enemyFighter;
        public float movementSpeed;
        public float movementTime;
        public float bulletShootCooldown;
        public GameObject bullet;
        public float bulletSpeed;
        public Color bulletColor;
        public int health;
    }

    [Serializable]
    public struct GUISettings
    {
        public GameObject mainMenuGUI;
        public GameObject inGameGUI;
        public TMP_Text levelText;
        public TMP_Text healthText;
    }

    public GUISettings guiSettings;
    public PlayerSettings playerSettings;
    public EnemyFighterSettings enemyFighterSettings;

    private bool inGame = false;
    private PlayerScript instantiatedPlayer;
    private readonly List<GameObject> instantiatedEnemies = new();
    private int level = 1;

    void Update()
    {
        guiSettings.inGameGUI.SetActive(inGame);
        guiSettings.mainMenuGUI.SetActive(!inGame);

        if(!inGame) return;

        bool allEnemiesDestroyed = true;
        foreach(GameObject enemyFighter in instantiatedEnemies)
        {
            if(enemyFighter != null) allEnemiesDestroyed = false;
        }
        if(allEnemiesDestroyed)
        {
            ReturnToMainMenu(true);
        }
    }

    public void StartGame()
    {
        inGame = true;

        instantiatedPlayer = Instantiate(playerSettings.player, new Vector3(0f, -3f), new Quaternion()).GetComponent<PlayerScript>();
        instantiatedPlayer.SetSettings(playerSettings);
        instantiatedPlayer.SetGameController(this);

        EnemyFighterScript enemyFighter = Instantiate(enemyFighterSettings.enemyFighter, new Vector3(0f, 1.5f), new Quaternion()).GetComponent<EnemyFighterScript>();
        enemyFighter.SetSettings(enemyFighterSettings);
        instantiatedEnemies.Add(enemyFighter.gameObject);
    }

    public void ReturnToMainMenu(bool completedLevel)
    {
        Destroy(instantiatedPlayer.gameObject);
        foreach(GameObject enemyFighter in instantiatedEnemies)
        {
            Destroy(enemyFighter);
        }

        if(completedLevel)
        {
            level++;
            UpdateLevelText();
        }

        inGame = false;
    }

    private void UpdateLevelText()
    {
        guiSettings.levelText.text = "Level: " + level;
    }

    public void UpdateHealthText(int health)
    {
        guiSettings.healthText.text = "Health: " + health;
    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
