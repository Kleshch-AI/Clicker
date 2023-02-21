using UnityEngine;

namespace Clicker.Level
{
    public static class SpawnHelper
    {
        public static Vector2 GetRandomNormalizedPoint()
        {
            return new Vector2(Random.value, Random.value);
        }

        public static Vector2 CalculateSpawnCoordinates(Vector2 gameFieldSize, Vector2 factor, Vector2 targetSize)
        {
            var targetHalfSize = targetSize * 0.5f;
            return new Vector2
            (
                Mathf.Clamp(gameFieldSize.x * factor.x, targetHalfSize.x, gameFieldSize.x - targetHalfSize.x),
                Mathf.Clamp(gameFieldSize.y * factor.y, targetHalfSize.y, gameFieldSize.y - targetHalfSize.y)
            );
        }
    }
}