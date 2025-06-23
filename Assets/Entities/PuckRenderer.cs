using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PuckRenderer : MonoBehaviour
{
    public Color color = Color.black;
    public float scale = 0.6f;

    private void Awake()
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("CircleSprite");
        sr.color = color;
        transform.localScale = Vector3.one * scale;
    }
}