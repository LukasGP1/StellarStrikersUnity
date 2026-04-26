using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    private float moveSpeed;
    private GameObject bulletPrefab;
    private float bulletSpeed;
    private Color bulletColor;
    private int health;
    private Rigidbody2D myRigidbody;
    private InputAction moveAction;
    private InputAction shootAction;
    private GameControllerScript gameController;
    private readonly List<GameObject> bullets = new();

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
        bulletPrefab = settings.bullet;
        bulletSpeed = settings.bulletSpeed;
        bulletColor = settings.bulletColor;
    }

    public void SetGameController(GameControllerScript gameController)
    {
        this.gameController = gameController;
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

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
            gameController.ReturnToMainMenu();
        }
    }

    public void Hit()
    {
        health--;
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
