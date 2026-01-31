using UnityEngine;
using System.Collections.Generic;

namespace ThawTheMask
{
    /// <summary>
    /// Generates a level from LevelData at runtime
    /// </summary>
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Level Data")]
        [SerializeField] private LevelData levelData;

        [Header("Prefabs")]
        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private GameObject goalPrefab;

        [Header("Settings")]
        [SerializeField] private bool generateOnStart = true;
        [SerializeField] private Transform platformParent;

        private List<GameObject> spawnedObjects = new List<GameObject>();

        private void Start()
        {
            if (generateOnStart && levelData != null)
            {
                GenerateLevel();
            }
        }

        /// <summary>
        /// Generate the level from data
        /// </summary>
        public void GenerateLevel()
        {
            if (levelData == null)
            {
                Debug.LogError("LevelData is not assigned!");
                return;
            }

            // Clear existing level
            ClearLevel();

            // Create parent if not assigned
            if (platformParent == null)
            {
                GameObject parent = new GameObject("Platforms");
                platformParent = parent.transform;
            }

            // Spawn platforms
            foreach (var platformData in levelData.platforms)
            {
                SpawnPlatform(platformData);
            }

            // Spawn goal (separate from platforms, slightly above)
            if (goalPrefab != null)
            {
                Vector3 goalPos = levelData.goalPosition;
                goalPos.y += 0.75f; // Place goal above the platform
                GameObject goal = Instantiate(goalPrefab, goalPos, Quaternion.identity);
                goal.name = "Goal";
                spawnedObjects.Add(goal);
            }

            // Move player to spawn position
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = levelData.playerSpawnPosition;
            }

            Debug.Log($"Level '{levelData.levelName}' generated with {levelData.platforms.Count} platforms");
        }

        /// <summary>
        /// Spawn a single platform
        /// </summary>
        private void SpawnPlatform(PlatformData data)
        {
            if (platformPrefab == null)
            {
                Debug.LogWarning("Platform prefab is not assigned!");
                return;
            }

            // Create platform
            GameObject platform = Instantiate(platformPrefab, data.position, Quaternion.identity, platformParent);
            platform.name = $"Platform_{data.visibleIn}";

            // Set size
            platform.transform.localScale = new Vector3(data.size.x, data.size.y, 1f);

            // Add MaskObject component if not present
            MaskObject maskObj = platform.GetComponent<MaskObject>();
            if (maskObj == null)
            {
                maskObj = platform.AddComponent<MaskObject>();
            }

            // Configure MaskObject based on data
            var maskObjType = maskObj.GetType();
            var visibleInField = maskObjType.GetField("visibleIn", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (visibleInField != null)
            {
                visibleInField.SetValue(maskObj, data.visibleIn);
            }

            spawnedObjects.Add(platform);
        }

        /// <summary>
        /// Clear all spawned objects
        /// </summary>
        public void ClearLevel()
        {
            foreach (var obj in spawnedObjects)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
            spawnedObjects.Clear();
        }

        /// <summary>
        /// Reload the current level
        /// </summary>
        public void ReloadLevel()
        {
            GenerateLevel();
        }

        private void OnValidate()
        {
            // Auto-generate in editor when values change (optional)
            if (Application.isPlaying && generateOnStart)
            {
                // Don't auto-regenerate in play mode to avoid issues
            }
        }
    }
}
