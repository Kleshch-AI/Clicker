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
        [SerializeField] private string title;
        [SerializeField] private int clicks;
        [SerializeField] private int seconds;
        [SerializeField] [TableList] private List<BonusSpawnInfo> bonuses;
        [SerializeField] private Sprite bg;

        public string Title => title;
        public int Clicks => clicks;
        public int Seconds => seconds;
        public IReadOnlyList<BonusSpawnInfo> Bonuses => bonuses;
        public Sprite Bg => bg;
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
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField] private List<LevelInfo> levels;

        public int MaxLevelId => levels.Count - 1;

        public LevelInfo GetById(int id)
        {
            if (id >= 0 && levels.Count > id)
                return levels[id];

            return null;
        }
    }
}