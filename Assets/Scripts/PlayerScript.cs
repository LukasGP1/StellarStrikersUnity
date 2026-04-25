using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float moveSpeed;
    public GameObject bullet;
    public float bulletSpeed;
    public Color bulletColor;
    private Rigidbody2D myRigidbody;
    private InputAction moveAction;
    private InputAction shootAction;
    private int health = 3;

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
            BulletScript instantiatedBullet = Instantiate(bullet, transform.position, transform.rotation).GetComponent<BulletScript>();
            instantiatedBullet.SetColor(bulletColor);
            instantiatedBullet.SetSpeed(bulletSpeed, true);
        }

        if(health <= 0)
        {
            print("Game Over!");
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
