using UnityEngine;

namespace ThawTheMask
{
    /// <summary>
    /// Editor helper to create sample level data
    /// </summary>
    public class LevelDataCreator : MonoBehaviour
    {
        [ContextMenu("Create Stage 1 Data")]
        public void CreateStage1()
        {
            LevelData level = ScriptableObject.CreateInstance<LevelData>();
            level.levelName = "Stage 1";
            level.stageNumber = 1;
            level.playerSpawnPosition = new Vector2(-10f, 0f);
            level.goalPosition = new Vector2(17f, 3.5f);

            // Redesigned with MANDATORY mask switching - gaps force it!
            
            // Starting platform (always visible)
            level.AddPlatform(-10f, -1f, 3f, 0.5f, MaskVisibility.Both);
            
            // GAP! Must use WHITE mask to see bridge (3 unit gap)
            level.AddPlatform(-5f, -1f, 2f, 0.5f, MaskVisibility.WhiteOnly);
            
            // GAP! Must switch to BLACK mask (3 unit gap)
            level.AddPlatform(-1f, 0f, 2f, 0.5f, MaskVisibility.BlackOnly);
            
            // GAP! Vertical climb - WHITE mask (2.5 unit gap)
            level.AddPlatform(2.5f, 1.5f, 2f, 0.5f, MaskVisibility.WhiteOnly);
            
            // GAP! BLACK mask (2.5 unit gap)
            level.AddPlatform(6f, 2.5f, 2f, 0.5f, MaskVisibility.BlackOnly);
            
            // GAP! WHITE mask (3 unit gap)
            level.AddPlatform(10f, 3f, 2f, 0.5f, MaskVisibility.WhiteOnly);
            
            // GAP! BLACK mask (3 unit gap)
            level.AddPlatform(14f, 3f, 2f, 0.5f, MaskVisibility.BlackOnly);
            
            // Goal platform (always visible) - ABOVE the last platform
            level.AddPlatform(17f, 3f, 2f, 0.5f, MaskVisibility.Both);

            Debug.Log("Stage 1 data created! (MANDATORY mask switching with gaps)");
            
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset(level, "Assets/Resources/Levels/Stage1.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log("Stage 1 saved to Assets/Resources/Levels/Stage1.asset");
#endif
        }

        [ContextMenu("Create Stage 2 Data")]
        public void CreateStage2()
        {
            LevelData level = ScriptableObject.CreateInstance<LevelData>();
            level.levelName = "Stage 2";
            level.stageNumber = 2;
            level.playerSpawnPosition = new Vector2(-12f, 0f);
            level.goalPosition = new Vector2(19f, 4.5f);

            // Harder with BIGGER gaps - impossible without mask switching
            
            // Starting area
            level.AddPlatform(-12f, -1f, 3f, 0.5f, MaskVisibility.Both);
            
            // LARGE GAP! BLACK mask only (4 unit gap)
            level.AddPlatform(-7f, 0f, 1.5f, 0.5f, MaskVisibility.BlackOnly);
            
            // LARGE GAP! WHITE mask (4 unit gap)
            level.AddPlatform(-3f, 1f, 1.5f, 0.5f, MaskVisibility.WhiteOnly);
            
            // LARGE GAP! BLACK mask (3.5 unit gap)
            level.AddPlatform(1f, 2f, 1.5f, 0.5f, MaskVisibility.BlackOnly);
            
            // Tiny platform! WHITE mask (3.5 unit gap)
            level.AddPlatform(5f, 3f, 1f, 0.5f, MaskVisibility.WhiteOnly);
            
            // Tiny platform! BLACK mask (3 unit gap)
            level.AddPlatform(8.5f, 3.5f, 1f, 0.5f, MaskVisibility.BlackOnly);
            
            // Tiny platform! WHITE mask (3 unit gap)
            level.AddPlatform(12f, 4f, 1f, 0.5f, MaskVisibility.WhiteOnly);
            
            // Final jump! BLACK mask (3.5 unit gap)
            level.AddPlatform(16f, 4f, 1.5f, 0.5f, MaskVisibility.BlackOnly);
            
            // Goal platform (always visible)
            level.AddPlatform(19f, 4f, 2f, 0.5f, MaskVisibility.Both);

            Debug.Log("Stage 2 data created! (VERY challenging with large gaps)");
            
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset(level, "Assets/Resources/Levels/Stage2.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log("Stage 2 saved to Assets/Resources/Levels/Stage2.asset");
#endif
        }

        [ContextMenu("Create Stage 3 Data")]
        public void CreateStage3()
        {
            LevelData level = ScriptableObject.CreateInstance<LevelData>();
            level.levelName = "Stage 3";
            level.stageNumber = 3;
            level.playerSpawnPosition = new Vector2(-15f, 0f);
            level.goalPosition = new Vector2(22f, 5.5f);

            // EXPERT level - longest and hardest stage
            
            // Starting area
            level.AddPlatform(-15f, -1f, 3f, 0.5f, MaskVisibility.Both);
            
            // HUGE GAP! WHITE mask (5 unit gap)
            level.AddPlatform(-10f, 0f, 1.5f, 0.5f, MaskVisibility.WhiteOnly);
            
            // HUGE GAP! BLACK mask (4.5 unit gap)
            level.AddPlatform(-6f, 1f, 1f, 0.5f, MaskVisibility.BlackOnly);
            
            // Tiny! WHITE mask (4 unit gap)
            level.AddPlatform(-2.5f, 2f, 0.8f, 0.5f, MaskVisibility.WhiteOnly);
            
            // Tiny! BLACK mask (4 unit gap)
            level.AddPlatform(1f, 2.5f, 0.8f, 0.5f, MaskVisibility.BlackOnly);
            
            // Medium platform - WHITE mask (3.5 unit gap)
            level.AddPlatform(4.5f, 3f, 1.5f, 0.5f, MaskVisibility.WhiteOnly);
            
            // Tiny! BLACK mask (4 unit gap)
            level.AddPlatform(8f, 3.5f, 0.8f, 0.5f, MaskVisibility.BlackOnly);
            
            // Tiny! WHITE mask (3.5 unit gap)
            level.AddPlatform(11f, 4f, 0.8f, 0.5f, MaskVisibility.WhiteOnly);
            
            // Medium - BLACK mask (4 unit gap)
            level.AddPlatform(14.5f, 4.5f, 1.5f, 0.5f, MaskVisibility.BlackOnly);
            
            // Final jump! WHITE mask (4 unit gap)
            level.AddPlatform(18f, 5f, 1.5f, 0.5f, MaskVisibility.WhiteOnly);
            
            // Goal platform (always visible)
            level.AddPlatform(22f, 5f, 2f, 0.5f, MaskVisibility.Both);

            Debug.Log("Stage 3 data created! (EXPERT level - longest and hardest)");
            
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset(level, "Assets/Resources/Levels/Stage3.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log("Stage 3 saved to Assets/Resources/Levels/Stage3.asset");
#endif
        }
    }
}
