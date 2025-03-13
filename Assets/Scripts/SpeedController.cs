// 

using UnityEngine;

namespace com.bhambhoo.fairludo
{
    public class SpeedController
    {
        private readonly LudoConfiguration ludoConfiguration;
        public SpeedController(LudoConfiguration ludoConfiguration)
        {
            this.ludoConfiguration = ludoConfiguration;
        }

        public void SetGameSpeed(GameManager.GameSpeed gameSpeed)
        {
            switch (gameSpeed)
            {
                case GameManager.GameSpeed.Normal:
                    LudoAI.DelayPlusMinus = ludoConfiguration.AIDelayPlusMinus;
                    LudoAI.DelayBeforeRollingDice = ludoConfiguration.AIDelayBeforeRollingDice;
                    LudoAI.DelayInChoosingToken = ludoConfiguration.AIDelayInChoosingToken;

                    Constants.diceRollShuffles = ludoConfiguration.DiceRollShuffles;
                    Constants.diceRollTime = ludoConfiguration.DiceRollTime;
                    Constants.idleTimeAfterDiceRoll = ludoConfiguration.IdleTimeAfterDiceRoll;
                    Constants.delayAfterTokenMoveComplete = ludoConfiguration.DelayAfterTokenMoveComplete;
                    Constants.delayBetweenTokenMoves = ludoConfiguration.DelayBetweenTokenMoves;
                    break;
                case GameManager.GameSpeed.Fast:
                    LudoAI.DelayPlusMinus = 0;
                    LudoAI.DelayBeforeRollingDice = 0.1f;
                    LudoAI.DelayInChoosingToken = 0.1f;
                    Constants.diceRollShuffles = 5;
                    Constants.diceRollTime = 0.2f;
                    Constants.idleTimeAfterDiceRoll = 0.1f;
                    Constants.delayAfterTokenMoveComplete = 0.5f;
                    Constants.delayBetweenTokenMoves = 0.05f;
                    break;
                case GameManager.GameSpeed.SuperFast:
                    LudoAI.DelayPlusMinus = 0;
                    LudoAI.DelayBeforeRollingDice = 0.05f;
                    LudoAI.DelayInChoosingToken = 0.05f;
                    Constants.diceRollShuffles = 5;
                    Constants.diceRollTime = 0.1f;
                    Constants.idleTimeAfterDiceRoll = 0.05f;
                    Constants.delayAfterTokenMoveComplete = 0.05f;
                    Constants.delayBetweenTokenMoves = 0.05f;
                    break;
                case GameManager.GameSpeed.Ultra:
                    LudoAI.DelayPlusMinus = 0;
                    LudoAI.DelayBeforeRollingDice = 0;
                    LudoAI.DelayInChoosingToken = 0;
                    Constants.diceRollShuffles = 1;
                    Constants.diceRollTime = 0;
                    Constants.idleTimeAfterDiceRoll = 0;
                    Constants.delayAfterTokenMoveComplete = 0;
                    Constants.delayBetweenTokenMoves = 0;
                    break;
                default:
                    SetGameSpeed(GameManager.GameSpeed.Normal);
                    break;
            }
            
            Debug.Log("Set GameSpeed to " + gameSpeed);
        }
    }
}