using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
/// <summary>
/// Puck renderer
/// </summary>
public sealed class PuckRenderer : ProceduralRenderer
{
    protected override void Awake()
    {
        spriteName = "CircleSprite";
        color = Color.black;
        scale = 0.6f;
        base.Awake();
    }
}
