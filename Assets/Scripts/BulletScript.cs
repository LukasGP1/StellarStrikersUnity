using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BulletScript : MonoBehaviour
{
    private bool up;
    private float speed;
    private Color color;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySpriteRenderer;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        myRigidbody.linearVelocityY = speed * (up ? 1f : -1f);
        mySpriteRenderer.color = color;
    }

    void Update()
    {
        if(Math.Abs(transform.position.y) > 8f)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float speed, bool up)
    {
        this.speed = speed;
        this.up = up;
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public bool GoesUp()
    {
        return up;
    }

    public void Collision()
    {
        Destroy(gameObject);
    }
}
