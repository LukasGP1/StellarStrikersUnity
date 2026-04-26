using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public GameObject mainMenu;
    public PlayerSettings playerSettings;
    public EnemyFighterSettings enemyFighterSettings;

    private bool inGame = false;
    private PlayerScript instantiatedPlayer;
    private readonly List<GameObject> instantiatedEnemies = new();

    void Update()
    {
        mainMenu.SetActive(!inGame);

        if(!inGame) return;

        bool allEnemiesDestroyed = true;
        foreach(GameObject enemyFighter in instantiatedEnemies)
        {
            if(enemyFighter != null) allEnemiesDestroyed = false;
        }
        if(allEnemiesDestroyed)
        {
            ReturnToMainMenu();
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

    public void ReturnToMainMenu()
    {
        Destroy(instantiatedPlayer.gameObject);
        foreach(GameObject enemyFighter in instantiatedEnemies)
        {
            Destroy(enemyFighter);
        }

        inGame = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
