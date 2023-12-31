using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float SPEED = 0.015f;

    private float BOUNDS_VERTICAL = 4.7f;
    private float BOUNDS_HORIZONTAL = 9f;

    private Simulation simulation;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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

        float deltaX = SPEED * horizontalAxis * Time.deltaTime;
        float newX = this.transform.position.x + deltaX;
        float newY = this.transform.position.y + SPEED * verticalAxis * Time.deltaTime;
        this.transform.position = new Vector3(Mathf.Clamp(newX, -BOUNDS_HORIZONTAL, BOUNDS_HORIZONTAL), Mathf.Clamp(newY, -BOUNDS_VERTICAL, BOUNDS_VERTICAL), 0);

        UpdateVisuals((int)Mathf.Round(horizontalAxis), (int)Mathf.Round(verticalAxis));
    }

    private void UpdateVisuals(int horizontalAxis, int verticalAxis)
    {
        if (horizontalAxis == 1)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalAxis == -1)
        {
            spriteRenderer.flipX = false;
        }

        if (horizontalAxis == 0 && verticalAxis == 0)
        {
            animator.enabled = false;
        }
        else
        {
            animator.enabled = true;
        }
    }
}
