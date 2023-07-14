using System.Collections;
using System.Collections.Generic;
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

    private float INVULNERABILITY_TIME = 0.5f;

    private GameObject player;
    private Simulation simulation;
    private Vector3 movementVector;
    private float directionTimer;
    private float invulnerabilityTimer;
    private int healValue = 10;
    private int damageValue = -5;
    private GameState gameState;

    private struct GameState
    {
        public float directionTimerDifference;
        public float invulnerabilityTimerDifference;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
        player = GameObject.FindObjectOfType<MovementController>().gameObject;
        directionTimer = Time.time; invulnerabilityTimer = Time.time;
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

        if (playerVector.magnitude < KILL_DISTANCE) {
            this.Kill(true);
        }

        if(Time.time - directionTimer > DIRECTION_CHANGE_CD)
        {
            directionTimer = Time.time;

            ChangeMovement(playerVector);
        }

        this.transform.position += SPEED * movementVector.normalized * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        LaserScript laser = collider.gameObject.GetComponent<LaserScript>();

        if (laser != null && CheckDamageable())
        {
            this.Kill(false);
        }
    }

    private bool CheckDamageable()
    {
        return Time.time - invulnerabilityTimer > INVULNERABILITY_TIME;
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

    public void StartSimulationGO()
    {
        directionTimer = Time.time - gameState.directionTimerDifference;
        invulnerabilityTimer = Time.time - gameState.invulnerabilityTimerDifference;
    }

    public void PauseSimulationGO()
    {
        gameState.directionTimerDifference = Time.time - directionTimer;
        gameState.invulnerabilityTimerDifference = Time.time - invulnerabilityTimer;
    }
}
