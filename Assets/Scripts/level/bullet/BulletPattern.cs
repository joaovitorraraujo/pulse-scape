using UnityEngine;

public enum PatternType
{
    Circle,
    Spiral,
    Wave,
    TargetPlayer,
    Rain
}

[System.Serializable]
public class BulletPattern
{
    public float beat = 0f;
    public PatternType type;

    public GameObject bulletPrefab;

    public int count = 10;
    public float speed = 5f;

    public float radius = 2f;
    public float angleOffset = 0f;
    public float duration = 1f;
}
