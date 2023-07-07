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

    private GameObject player;
    private Simulation simulation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
        player = GameObject.FindObjectOfType<MovementController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerVector = player.transform.position - this.transform.position;

        if (playerVector.magnitude < KILL_DISTANCE) {
            this.Kill(true);
        }

        this.transform.position += SPEED * playerVector.normalized * Time.deltaTime;
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
