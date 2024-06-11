using System.Collections.Generic;
using UnityEngine;

public class Serpent : MonoBehaviour
{
    // Direction of the serpent's movement
    private Vector2 _direction = Vector2.right;

    // List of serpent's segments
    private List<Transform> _segments = new List<Transform>();

    // Prefab for serpent's segment
    public Transform segmentPrefab;

    // Initial size of the serpent
    public int initialSize = 4;

    // Movement speed
    public float moveSpeed = 0.5f;

    // Jump distance
    public float jumpDistance = 2.0f;

    private void Start()
    {
        // Reset the serpent's state at the start
        ResetState();
    }

    private void Update()
    {
        // Update the direction based on user input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _direction = Vector2.up;
            UpdateSegments();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _direction = Vector2.down;
            UpdateSegments();
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _direction = Vector2.left;
            UpdateSegments();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _direction = Vector2.right;
            UpdateSegments();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Make the serpent jump by changing its position
            this.transform.position = new Vector3(
                Mathf.Round(this.transform.position.x) + _direction.x * jumpDistance,
                Mathf.Round(this.transform.position.y) + _direction.y * jumpDistance,
                1.0f // Lift the serpent on the z-axis
            );

            // Update the serpent's segments after the jump
            UpdateSegments();
        }
    }

    private void FixedUpdate()
    {
        UpdateSegments();
    }

    private void UpdateSegments()
    {
        // Move the serpent's segments
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        };

        // Move the serpent's head
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x * moveSpeed,
            Mathf.Round(this.transform.position.y) + _direction.y * moveSpeed,
            this.transform.position.z // Keep the z-position
        );
    }

    // Method to grow the serpent
    private void Grow()
    {
        // Instantiate a new segment
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    // Method to reset the serpent's state
    private void ResetState()
    {
        // Destroy all segments except the head
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        _segments.Add(this.transform);

        // Instantiate new segments
        for (int i = 1; i < initialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }

        // Reset the serpent's position
        this.transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the serpent collides with food
        if (other.tag == "Food")
        {
            Grow();
        }
        // Check if the serpent collides with an obstacle
        else if (other.tag == "Obstacle")
        {
            ResetState();
        }
    }
}