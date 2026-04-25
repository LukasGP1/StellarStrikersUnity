using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyScript : MonoBehaviour
{
    public float movementSpeed;
    public float movementTime;
    public float bulletShootCooldown;
    public GameObject bullet;
    public float bulletSpeed;
    public Color bulletColor;
    private Rigidbody2D myRigidbody;
    private float movementTimer;
    private float bulletShootTimer;
    private int health = 3;

    void Start()
    {
        bulletShootTimer = 0f;
        movementTimer = movementTime / 2f;
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.linearVelocityX = movementSpeed;
    }

    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }

        movementTimer += Time.deltaTime;

        if(movementTimer >= movementTime)
        {
            movementTimer = 0f;
            myRigidbody.linearVelocityX *= -1f;
        }

        bulletShootTimer += Time.deltaTime;

        if(bulletShootTimer >= bulletShootCooldown)
        {
            bulletShootTimer = 0f;
            BulletScript instantiatedBullet = Instantiate(bullet, transform.position, transform.rotation).GetComponent<BulletScript>();
            instantiatedBullet.SetColor(bulletColor);
            instantiatedBullet.SetSpeed(bulletSpeed, false);
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

    void SetHealth(int health)
    {
        this.health = health;
    }
}
