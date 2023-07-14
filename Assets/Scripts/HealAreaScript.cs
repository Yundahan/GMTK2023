using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAreaScript : MonoBehaviour
{
    public float INDICATOR_TIME = 1f;
    public float DURATION = 0.5f;
    public int HEAL_VALUE = 20;

    private SpriteRenderer spriteRenderer;

    private Simulation simulation;
    private float timer;
    private Phase phase = Phase.NONE;
    private GameState gameState;

    private struct GameState
    {
        public float timerDifference;
    }

    private enum Phase
    {
        NONE,
        INDICATOR,
        ACTIVE,
        SPENT
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!simulation.IsSimulationRunning())
        {
            return;
        }

        if(Time.time - timer > INDICATOR_TIME && phase == Phase.INDICATOR)
        {
            timer = Time.time;

            phase = Phase.ACTIVE;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
        else if(Time.time - timer > DURATION && phase == Phase.ACTIVE || phase == Phase.SPENT)
        {
            Destroy(gameObject);
        }
    }

    public void StartIndicator()
    {
        timer = Time.time;

        phase = Phase.INDICATOR;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.4f);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!simulation.IsSimulationRunning())
        {
            return;
        }

        if (phase == Phase.ACTIVE)
        {
            AttackController attackController = collider.gameObject.GetComponent<AttackController>();

            if (attackController != null && phase == Phase.ACTIVE)
            {
                attackController.ChangeHitpoints(HEAL_VALUE);
                phase = Phase.SPENT;
            }
        }
    }

    public void StartSimulationGO()
    {
        timer = Time.time - gameState.timerDifference;
    }

    public void PauseSimulationGO()
    {
        gameState.timerDifference = Time.time - timer;
    }
}
