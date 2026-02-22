using UnityEngine;
using System;

public static class NoiseSystem
{
    public static event Action<Vector3, float, float> GenNoise;

    public static void PlayerNoise(Vector3 position, float hearRadius, float searchRadius)
    {
        GenNoise?.Invoke(position, hearRadius, searchRadius);
    }
}
