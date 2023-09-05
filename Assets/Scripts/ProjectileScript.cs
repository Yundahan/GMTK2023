using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : AttackScript
{
    public float DESPAWN_TIME = 3f;
    public float SPEED = 10f;

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
        
    }

    private void FixedUpdate()
    {
        if (Time.time - spawnTime > DESPAWN_TIME)
        {
            this.Kill();
        }

        this.transform.position += direction * Time.deltaTime * SPEED;
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
