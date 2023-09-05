using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearScript : AttackScript
{
    private float MAX_SPEAR_RANGE = 5f;

    private float timer;
    private GameState gameState;

    private struct GameState
    {
        public float timerDifference;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!simulation.IsSimulationRunning())
        {
            return;
        }

        if (Time.time - timer > attackController.GetSpearDuration())
        {
            gameObject.SetActive(false);
            return;
        }

        AdjustSpearScale();
    }

    public override void SetTimer()
    {
        timer = Time.time;
    }

    public override void StartSimulationGO()
    {
        timer = Time.time - gameState.timerDifference;
    }

    public override void PauseSimulationGO()
    {
        gameState.timerDifference = Time.time - timer;
    }

    private void AdjustSpearScale()
    {
        float halfDuration = attackController.GetSpearDuration() / 2f;
        float timeFromPeak = Mathf.Abs(halfDuration - (Time.time - timer));
        float spearExtension = MAX_SPEAR_RANGE * (halfDuration - timeFromPeak) / halfDuration;
        this.transform.localPosition = new Vector3(spearExtension / 2f, 0f, 0f);
        this.transform.localScale = new Vector2(spearExtension, this.transform.localScale.y);
    }
}
