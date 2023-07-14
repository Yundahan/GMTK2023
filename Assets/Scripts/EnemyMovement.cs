using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private Simulation simulation;
    private SpriteRenderer spriteRenderer;
    private Vector3 movementVector;
    private float directionTimer;
    private float invulnerabilityTimer;
    private int healValue;
    private int damageValue;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        if (playerVector.magnitude < simulation.GetGeneralConfig().enemyHitDistance) {
            this.Kill(true);
        }

        if(Time.time - directionTimer > simulation.GetGeneralConfig().directionChangeCD)
        {
            directionTimer = Time.time;

            ChangeMovement(playerVector);
        }

        this.transform.position += simulation.GetLevelConfig().enemySpeed * movementVector.normalized * Time.deltaTime;
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
        return Time.time - invulnerabilityTimer > simulation.GetGeneralConfig().enemyInvulnerabilityTime;
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
