using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    protected AttackController attackController;
    protected Simulation simulation;

    private void Awake()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
        attackController = GameObject.FindObjectOfType<AttackController>();
    }

    public virtual void SetTimer()
    {

    }

    public virtual void StartSimulationGO()
    {

    }

    public virtual void PauseSimulationGO()
    {

    }
}
