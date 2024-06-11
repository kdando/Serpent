using UnityEngine;

// Added comments to explain the purpose of the class
/// <summary>
/// This script controls the behavior of the food in the game.
/// </summary>
public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start()
    {
        RandomisePosition();
    }

    // Renamed the method to make it clearer
    public void RandomisePosition()
    {
        Bounds bounds = this.gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RandomisePosition();
        }
    }
}