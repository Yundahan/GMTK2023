using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float SPEED = 2f;
    public float KILL_DISTANCE = 1f;
    public float MAX_DIRECTION_ROTATION = 20f;
    public float DIRECTION_CHANGE_CD = 2f;
    public SpriteRenderer spriteRenderer;

    private float BOUNDS_VERTICAL = 4f;
    private float BOUNDS_HORIZONTAL = 8f;

    private GameObject player;
    private Simulation simulation;
    private Vector3 movementVector;
    private float directionTimer;
    private int healValue = 10;
    private int damageValue = -5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
        player = GameObject.FindObjectOfType<MovementController>().gameObject;
        directionTimer = Time.time;
        movementVector = player.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerVector = player.transform.position - this.transform.position;

        if (playerVector.magnitude < KILL_DISTANCE) {
            this.Kill(true);
        }

        if(Time.time - directionTimer > DIRECTION_CHANGE_CD)
        {
            directionTimer = Time.time;

            movementVector = playerVector;

            if(movementVector.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }

        this.transform.position += SPEED * movementVector.normalized * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        LaserScript laser = collider.gameObject.GetComponent<LaserScript>();

        if (laser != null && !CheckOutOfBounds())
        {
            this.Kill(false);
        }
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

    private void Kill(bool saved)
    {
        if (saved)
        {
            player.GetComponent<AttackController>().ChangeHitpoints(damageValue);
        }
        else
        {
            player.GetComponent<AttackController>().ChangeHitpoints(healValue);
        }

        simulation.UpdateCounts(saved);
        Destroy(gameObject);
    }

    public int GetHealValue(int value)
    {
        return healValue;
    }

    public void SetHealValue(int value)
    {
        this.healValue = value;
    }

    public int GetDamageValue(int value)
    {
        return damageValue;
    }

    public void SetDamageValue(int value)
    {
        this.damageValue = value;
    }
}
