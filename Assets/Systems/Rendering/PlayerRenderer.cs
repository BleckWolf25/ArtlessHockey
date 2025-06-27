using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
/// <summary>
/// Player renderer with modern practices
/// </summary>
public sealed class PlayerRenderer : ProceduralRenderer
{
    protected override void Awake()
    {
        spriteName = "CircleSprite";
        base.Awake();
    }
}
