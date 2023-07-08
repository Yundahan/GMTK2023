using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float SPEED = 1f;
    public float KILL_DISTANCE = 2f;
    public float MAX_DIRECTION_ROTATION = 20;
    public float DIRECTION_CHANGE_CD = 2f;

    private GameObject player;
    private Simulation simulation;
    private Vector3 movementVector;
    private float directionTimer;

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
        }

        this.transform.position += SPEED * movementVector.normalized * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        BulletScript bullet = collider.gameObject.GetComponent<BulletScript>();

        if (bullet != null)
        {
            bullet.Kill();
            this.Kill(false);
        }
    }

    private void Kill(bool saved)
    {
        simulation.UpdateCounts(saved);
        Destroy(gameObject);
    }
}
