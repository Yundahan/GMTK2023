using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float SPEED = 2f;
    public float MAX_DIRECTION_ROTATION = 20f;
    public float DIRECTION_CHANGE_CD = 2f;

    private SpriteRenderer spriteRenderer;
    private EnemyCombat enemyCombat;

    private MovementController player;
    private Simulation simulation;
    private Vector3 movementVector;
    private float directionTimer;
    private GameState gameState;

    private struct GameState
    {
        public float directionTimerDifference;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
        player = GameObject.FindObjectOfType<MovementController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCombat = GetComponent<EnemyCombat>();
        directionTimer = Time.time;
        ChangeMovement(player.transform.position - this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!simulation.IsSimulationRunning())
        {
            return;
        }

        Vector3 playerVector = player.transform.position - this.transform.position;

        if (playerVector.magnitude < enemyCombat.KILL_DISTANCE) {
            enemyCombat.Kill(true);
        }

        if(Time.time - directionTimer > DIRECTION_CHANGE_CD)
        {
            directionTimer = Time.time;

            ChangeMovement(playerVector);
        }

        this.transform.position += SPEED * movementVector.normalized * Time.deltaTime;
    }

    private void ChangeMovement(Vector3 newMovementVector)
    {
        movementVector = newMovementVector;

        if (movementVector.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void StartSimulationGO()
    {
        directionTimer = Time.time - gameState.directionTimerDifference;
    }

    public void PauseSimulationGO()
    {
        gameState.directionTimerDifference = Time.time - directionTimer;
    }
}
