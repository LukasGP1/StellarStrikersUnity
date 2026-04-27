using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(Animator))]
public class EnemyFighterScript : MonoBehaviour
{
    private float movementSpeed;
    private float movementTime;
    private float destructionTimer;
    private float bulletShootCooldown;
    private GameObject bulletPrefab;
    private float bulletSpeed;
    private Color bulletColor;
    private int health;
    private Rigidbody2D myRigidbody;
    private float movementTimer;
    private float bulletShootTimer;
    private readonly List<GameObject> bullets = new();
    private Animator myAnimator;
    private bool isDestroying = false;

    void OnDestroy()
    {
        foreach(GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }

    public void StartDestruction()
    {
        isDestroying = true;
        myRigidbody.linearVelocityX = 0f;
    }

    public void SetSettings(GameControllerScript.EnemyFighterSettings settings)
    {
        movementSpeed = settings.movementSpeed;
        movementTime = settings.movementTime;
        bulletShootCooldown = settings.bulletShootCooldown;
        bulletPrefab = settings.bullet;
        bulletSpeed = settings.bulletSpeed;
        bulletColor = settings.bulletColor;
        health = settings.health;
    }

    void Start()
    {
        bulletShootTimer = 0f;
        destructionTimer = 0f;
        movementTimer = movementTime / 2f;

        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.linearVelocityX = movementSpeed;
    }

    void Update()
    {
        myAnimator.SetBool("IsDestroyign", isDestroying);

        if(health <= 0)
        {
            StartDestruction();
        }

        movementTimer += Time.deltaTime;

        if(movementTimer >= movementTime)
        {
            movementTimer = 0f;
            myRigidbody.linearVelocityX *= -1f;
        }

        bulletShootTimer += Time.deltaTime;

        if(isDestroying) destructionTimer += Time.deltaTime;

        if(destructionTimer >= 2.3f) Destroy(gameObject);

        if(bulletShootTimer >= bulletShootCooldown && !isDestroying)
        {
            bulletShootTimer = 0f;
            BulletScript bullet = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<BulletScript>();
            bullet.SetColor(bulletColor);
            bullet.SetSpeed(bulletSpeed, false);
            bullets.Add(bullet.gameObject);
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
            if(bullet.GoesUp())
            {
                Hit();
                bullet.Collision();
            }
        }
    }
}
