using System;
using System.Collections.Generic;
using Clicker.Level.Bonuses;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configuration
{
    [Serializable]
    public class LevelInfo
    {
        [SerializeField] private int clicks;
        [SerializeField] private int seconds;
        [SerializeField] [TableList] private List<BonusSpawnInfo> bonuses;
        
        public int Clicks => clicks;
        public int Seconds => seconds;
        public IReadOnlyList<BonusSpawnInfo> Bonuses => bonuses;
    }

    [Serializable]
    public class BonusSpawnInfo
    {
        [SerializeField] private Bonus type;
        [SerializeField] private float chance;
        [SerializeField] private int seconds;
        
        public Bonus Type => type;
        public float Chance => chance;
        public int Seconds => seconds;
    }

    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Clicker/Configs/Levels")]
    public class LevelsConfig : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<int, LevelInfo> levels;

        public LevelInfo GetById(int id)
        {
            if (levels.ContainsKey(id))
                return levels[id];

            return null;
        }
    }
}