using UnityEngine;

public class UpDownMove : MonoBehaviour
{
    [Header("Up Down Settings")]
    public float moveHeight = 1.0f; // è„â∫ÇÃïù
    public float moveSpeed = 2.0f;  // ìÆÇ≠ë¨Ç≥

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * moveSpeed) * moveHeight;
        transform.position = startPos + new Vector3(0, y, 0);
    }
}