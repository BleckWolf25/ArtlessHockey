using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerRenderer : MonoBehaviour
{
    public Color color = Color.white;
    public float scale = 1f;

    private void Awake()
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("CircleSprite");
        sr.color = color;
        transform.localScale = Vector3.one * scale;
    }
}