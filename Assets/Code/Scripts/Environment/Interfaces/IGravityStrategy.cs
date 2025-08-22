using UnityEngine;

public interface IGravityStrategy
{
    public Vector3 Position { get; }
    public float Gravity { get; }
}