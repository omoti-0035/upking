using UnityEngine;

public class MoveUpDownUI : MonoBehaviour
{
    public float moveRange = 20f;   // 上下に動く距離
    public float speed = 2f;        // 動く速さ

    private Vector2 startPos;

    void Start()
    {
        startPos = GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * moveRange;
        GetComponent<RectTransform>().anchoredPosition =
            startPos + new Vector2(0, y);
    }
}