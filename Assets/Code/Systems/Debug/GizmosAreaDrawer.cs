using System;
using UnityEngine;

[Serializable] public struct GizmosAreaDrawer
{
    [SerializeField] private Color color;
    [SerializeField, Range(0, 1)] private float alpha;

    private void SetColor() { color.a = alpha; Gizmos.color = color; }
    private void SetMatrix(Transform transform) => Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
    private void DrawArea(BoxCollider collider)
    {
        Gizmos.DrawCube(collider.center, collider.size);
        Gizmos.DrawWireCube(collider.center, collider.size);
    }
    private void DrawArea(SphereCollider collider)
    {
        Gizmos.DrawSphere(collider.center, collider.radius);
        Gizmos.DrawWireSphere(collider.center, collider.radius);
    }

    public void ConstrainHeight(BoxCollider collider)
    {
        if (!collider) return;

        var c = collider.center;
        collider.center = new(c.x, collider.size.y * 0.5f, c.z);
    }
    public void DrawArea(Transform transform, Collider collider)
    {
        if (!collider) return;

        SetColor();
        SetMatrix(transform);

        if (collider is BoxCollider box) DrawArea(box);
        if (collider is SphereCollider sphere) DrawArea(sphere);
    }
}