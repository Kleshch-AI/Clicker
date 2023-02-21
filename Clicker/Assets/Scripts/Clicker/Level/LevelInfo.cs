using System;
using System.Collections.Generic;
using Clicker.Level.Bonuses;

// using UnityEngine;

namespace Clicker.Level
{
    [Serializable]
    public struct LevelInfo
    {
        public int clicks;
        public int seconds;
        public List<BonusSpawnInfo> bonuses;
        // public RectTransform target;
    }
    
    [Serializable]
    public struct BonusSpawnInfo
    {
        public Bonus type;
        public float chance;
        public int seconds;
    }
}