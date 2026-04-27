using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerScript : MonoBehaviour
{
    private float moveSpeed;
    private GameObject bulletPrefab;
    private float bulletSpeed;
    private Color bulletColor;
    private int health;
    private int maxHealth;
    private Rigidbody2D myRigidbody;
    private InputAction moveAction;
    private InputAction shootAction;
    private GameControllerScript gameController;
    private readonly List<GameObject> bullets = new();
    private SpriteRenderer mySpriteRenderer;
    private Sprite baseSprite;
    private Sprite damage0;
    private Sprite damage1;
    private Sprite damage2;

    public void OnDestroy()
    {
        foreach(GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }

    public void SetSettings(GameControllerScript.PlayerSettings settings)
    {
        moveSpeed = settings.moveSpeed;
        health = settings.health;
        maxHealth = settings.health;
        bulletPrefab = settings.bullet;
        bulletSpeed = settings.bulletSpeed;
        bulletColor = settings.bulletColor;
        baseSprite = settings.baseSprite;
        damage0 = settings.damage0;
        damage1 = settings.damage1;
        damage2 = settings.damage2;
    }

    public void SetGameController(GameControllerScript gameController)
    {
        this.gameController = gameController;
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        UpdateHealth();

        moveAction = InputSystem.actions.FindAction("Move");
        shootAction = InputSystem.actions.FindAction("Shoot");
    }

    void Update()
    {
        float moveValue = moveAction.ReadValue<float>();
        myRigidbody.linearVelocityX = moveSpeed * moveValue;

        if(shootAction.WasPressedThisFrame())
        {
            BulletScript bullet = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<BulletScript>();
            bullet.SetColor(bulletColor);
            bullet.SetSpeed(bulletSpeed, true);
            bullets.Add(bullet.gameObject);
        }

        if(health <= 0)
        {
            gameController.ReturnToMainMenu(false);
        }
    }

    private void UpdateHealth()
    {
        float healthProportion = ((float) health) / maxHealth;
        if(healthProportion < 0.33f)
        {
            mySpriteRenderer.sprite = damage2;
        } 
        else if(healthProportion < 0.66f)
        {
            mySpriteRenderer.sprite = damage1;
        }
        else if(healthProportion < 1f)
        {
            mySpriteRenderer.sprite = damage0;
        } 
        else
        {
            mySpriteRenderer.sprite = baseSprite;
        }
        gameController.UpdateHealthText(health);
    }

    public void Hit()
    {
        health--;
        UpdateHealth();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            BulletScript bullet = collision.gameObject.GetComponent<BulletScript>();
            if(!bullet.GoesUp())
            {
                Hit();
                bullet.Collision();
            }
        }
    }
}
