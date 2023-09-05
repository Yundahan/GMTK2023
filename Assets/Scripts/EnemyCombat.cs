using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private float INVULNERABILITY_TIME = 0.5f;
    public float KILL_DISTANCE = 1f;

    private Simulation simulation;
    private AttackController player;

    private float invulnerabilityTimer;
    private int healValue = 10;
    private int damageValue = -5;

    private GameState gameState;

    private struct GameState
    {
        public float invulnerabilityTimerDifference;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
        player = GameObject.FindObjectOfType<AttackController>();
        invulnerabilityTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!simulation.IsSimulationRunning())
        {
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        AttackScript attackScript = collider.gameObject.GetComponent<AttackScript>();

        if (attackScript != null && CheckDamageable())
        {
            this.Kill(false);
        }
    }

    public void Kill(bool saved)
    {
        if (saved)
        {
            player.ChangeHitpoints(damageValue);
        }
        else
        {
            player.ChangeHitpoints(healValue);
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

    public void StartSimulationGO()
    {
        invulnerabilityTimer = Time.time - gameState.invulnerabilityTimerDifference;
    }

    public void PauseSimulationGO()
    {
        gameState.invulnerabilityTimerDifference = Time.time - invulnerabilityTimer;
    }

    private bool CheckDamageable()
    {
        return Time.time - invulnerabilityTimer > INVULNERABILITY_TIME;
    }
}
