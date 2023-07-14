using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Simulation simulation;
    private SpriteRenderer spriteRenderer;

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
        if (!simulation.IsSimulationRunning())
        {
            return;
        }

        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");

        float deltaX = simulation.GetLevelConfig().playerSpeed * horizontalAxis * Time.deltaTime;
        float newX = this.transform.position.x + deltaX;
        float newY = this.transform.position.y + simulation.GetLevelConfig().playerSpeed * verticalAxis * Time.deltaTime;
        float boundsHorizontal = simulation.GetGeneralConfig().playerBoundsHorizontal;
        float boundsVertical = simulation.GetGeneralConfig().playerBoundsVertical;
        this.transform.position = new Vector3(Mathf.Clamp(newX, -boundsHorizontal, boundsHorizontal), Mathf.Clamp(newY, -boundsVertical, boundsVertical), 0);

        UpdateVisuals(horizontalAxis, verticalAxis);
    }

    private void UpdateVisuals(float horizontalAxis, float verticalAxis)
    {
        if(horizontalAxis >0.5f)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalAxis < -0.5f)
        {
            spriteRenderer.flipX = false;
        }

        if (Mathf.Abs(horizontalAxis) < 0.5f && Mathf.Abs(verticalAxis) < 0.5f)
        {
            GetComponent<Animator>().enabled = false;
        }
        else
        {
            GetComponent<Animator>().enabled = true;
        }
    }
}
