using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float SPEED = 0.015f;

    private float BOUNDS_VERTICAL = 4f;
    private float BOUNDS_HORIZONTAL = 8f;

    private Simulation simulation;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!simulation.IsSimulationRunning())
        {
            return;
        }

        float newX = this.transform.position.x + SPEED * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        float newY = this.transform.position.y + SPEED * Input.GetAxisRaw("Vertical") * Time.deltaTime;
        this.transform.position = new Vector3(Mathf.Clamp(newX, -BOUNDS_HORIZONTAL, BOUNDS_HORIZONTAL), Mathf.Clamp(newY, -BOUNDS_VERTICAL, BOUNDS_VERTICAL), 0);
    }
}
