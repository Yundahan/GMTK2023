using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float DESPAWN_TIME = 3f;
    public float BULLET_SPEED = 0.01f;

    private float BOUNDS_VERTICAL = 4f;
    private float BOUNDS_HORIZONTAL = 8f;

    private float spawnTime;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - spawnTime > DESPAWN_TIME || CheckOutOfBounds())
        {
            this.Kill();
        }

        this.transform.position += direction * Time.deltaTime * BULLET_SPEED;
    }

    private bool CheckOutOfBounds()
    {
        if (this.transform.position.x < -BOUNDS_HORIZONTAL || this.transform.position.x > BOUNDS_HORIZONTAL)
        {
            return true;
        }
        if (this.transform.position.y < -BOUNDS_VERTICAL || this.transform.position.y > BOUNDS_VERTICAL)
        {
            return true;
        }

        return false;
    }

    public Vector3 GetDirection()
    {
        return this.direction;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
