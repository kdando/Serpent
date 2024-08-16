using System.Collections.Generic;
using UnityEngine;

public class Serpent : MonoBehaviour
{
    // Direction of the serpent's movement (initialized to right by default)
    private Vector2 direction = Vector2.right;
    // List of serpent's segments
    private List<Transform> segments = new List<Transform>();

    // Prefab for the serpent's segment
    public Transform segmentPrefab;

    // Initial size of the serpent
    public int initialSize = 4;

    // Movement speed of the serpent
    public float moveSpeed = 0.5f;

    // Jump distance of the serpent
    public float jumpDistance = 2.0f;

    private void Start()
    {
        ResetState();
        // Set an initial default direction for the snake to start moving.
        direction = Vector2.right;
    }
    private void Update()
    {
        // Update direction based on user input (if any)
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2.right;
        }

        // Move the head of the serpent in the intended direction continuously.
        transform.position = new Vector3(Mathf.Round(transform.position.x + direction.x * moveSpeed), Mathf.Round(transform.position.y + direction.y * moveSpeed), transform.position.z);
    }
    private void FixedUpdate()
    {
        // Move the head of the serpent in the intended direction
        transform.position = new Vector3(
             Mathf.Round(transform.position.x + direction.x * moveSpeed),
             Mathf.Round(transform.position.y + direction.y * moveSpeed),
            transform.position.z); // Keep the z-position unchanged

        UpdateSegments();
    }
    private void UpdateSegments()
    {
        // Move the segments to follow the head smoothly 
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }
    }
    private void Grow()
    {
        // Instantiate a new segment at the tail position (the position of the last segment before growth)
        Transform newSegment = Instantiate(segmentPrefab);
        newSegment.position = segments[segments.Count - 1].position;
        // Add the new segment to the list of segments
        segments.Add(newSegment);
    }
    private void ResetState()
    {
        foreach (Transform segment in segments)
        {
            Destroy(segment.gameObject);
        }
        segments.Clear();
        segments.Add(transform);
        for (int i = 1; i < initialSize; i++)
        {
            Transform segment = Instantiate(segmentPrefab);
            segments.Add(segment);
        }
    }
    private void Jump()
    {
        transform.position += new Vector3(direction.x * jumpDistance, direction.y * jumpDistance, 0f);
        UpdateSegments();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
        }
        else if (other.CompareTag("Obstacle"))
        {
            ResetState();
        }
    }
}