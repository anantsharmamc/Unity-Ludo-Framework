// 

using UnityEngine;

namespace com.bhambhoo.fairludo
{
    [CreateAssetMenu(fileName = "LudoConfig", menuName = "Ludo/SO/Config", order = 0)]
    public class LudoConfiguration : ScriptableObject
    {
        public int DiceRollShuffles = 8;
        public float AIDelayPlusMinus = 0.5f,
            AIDelayBeforeRollingDice = 0.5f, 
            AIDelayInChoosingToken = 0.5f, 
            DiceRollTime = 0.6f, 
            IdleTimeAfterDiceRoll = 0.5f, 
            DelayAfterTokenMoveComplete = 0.5f, 
            DelayBetweenTokenMoves = 0.15f;
    }
}