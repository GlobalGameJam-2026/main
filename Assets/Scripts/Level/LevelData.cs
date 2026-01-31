using UnityEngine;
using System.Collections.Generic;

namespace ThawTheMask
{
    /// <summary>
    /// Defines a single platform in a level
    /// </summary>
    [System.Serializable]
    public class PlatformData
    {
        public Vector2 position;
        public Vector2 size = new Vector2(2f, 0.5f);
        public MaskVisibility visibleIn = MaskVisibility.Both;
        public PlatformType type = PlatformType.Normal;

        public PlatformData(float x, float y, MaskVisibility visibility = MaskVisibility.Both)
        {
            position = new Vector2(x, y);
            visibleIn = visibility;
        }

        public PlatformData(float x, float y, float width, float height, MaskVisibility visibility = MaskVisibility.Both)
        {
            position = new Vector2(x, y);
            size = new Vector2(width, height);
            visibleIn = visibility;
        }
    }

    /// <summary>
    /// Types of platforms
    /// </summary>
    public enum PlatformType
    {
        Normal,         // Basic platform
        Spike,          // Deadly obstacle
        Collapsing,     // Falls after stepping on it
        Moving          // Moves back and forth
    }

    /// <summary>
    /// ScriptableObject that holds level data
    /// </summary>
    [CreateAssetMenu(fileName = "LevelData", menuName = "Thaw The Mask/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Info")]
        public string levelName = "Stage 1";
        public int stageNumber = 1;

        [Header("Player Spawn")]
        public Vector2 playerSpawnPosition = new Vector2(-8f, 0f);

        [Header("Goal")]
        public Vector2 goalPosition = new Vector2(8f, 0f);

        [Header("Platforms")]
        public List<PlatformData> platforms = new List<PlatformData>();

        [Header("Visual Theme")]
        public Color backgroundColor = Color.white;
        public Sprite platformSprite;

        /// <summary>
        /// Helper method to add platforms in code
        /// </summary>
        public void AddPlatform(float x, float y, MaskVisibility visibility = MaskVisibility.Both)
        {
            platforms.Add(new PlatformData(x, y, visibility));
        }

        public void AddPlatform(float x, float y, float width, float height, MaskVisibility visibility = MaskVisibility.Both)
        {
            platforms.Add(new PlatformData(x, y, width, height, visibility));
        }
    }
}
