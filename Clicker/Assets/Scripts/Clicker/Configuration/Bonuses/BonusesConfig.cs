// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Clicker.Configuration.Bonuses
// {
//     public enum Bonus
//     {
//         X2,
//         Stop,
//         Size
//     }
//     
//     [Serializable]
//     public class BonusData
//     {
//         [SerializeField] private Bonus type;
//         [SerializeField] private float spawnChance;
//         
//         public Bonus Type => type;
//         public float SpawnChance => spawnChance;
//     }
//     
//     public class BonusesConfig : ScriptableObject
//     {
//         [SerializeField] private List<BonusData> bonuses;
//
//         public BonusData GetBonusData(Bonus type)
//         {
//             return bonuses.Find(b => b.type == type);
//         }
//     }
// }