using UnityEngine;
using UnityEngine.Serialization;

namespace com.bhambhoo.fairludo
{
    public class GameManager : MonoBehaviour
    {
        private enum GameSpeed { Normal, Fast, SuperFast, Ultra };
        public static GameManager Instance;

        [Header("Factors for Game Speed")]
        public int DiceRollShuffles = 8;
        public float AIDelayPlusMinus = 0.5f,
            AIDelayBeforeRollingDice = 0.5f, 
            AIDelayInChoosingToken = 0.5f, 
            DiceRollTime = 0.6f, 
            IdleTimeAfterDiceRoll = 0.5f, 
            DelayAfterTokenMoveComplete = 0.5f, 
            DelayBetweenTokenMoves = 0.15f;

        public Constants.MatchType SelectedMatchType = Constants.MatchType.PassNPlay;
        [Range(2, 4)]
        public byte SelectedNumPlayers = 2;
        
        [Header("Press F to search for tokens wrt player 1")]
        public byte WaypointIndexToSearch;
        
        private void OnEnable()
        {
            Instance = this;
        }

        private void Start()
        {
            SetGameSpeed(GameSpeed.Normal);

            MatchManager.Instance.StartMatch(SelectedNumPlayers, SelectedMatchType);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayersManager.ChangePlayerType(PlayersManager.Players[0], Constants.PlayerType.Bot);
            }
            if (Input.GetKeyDown(KeyCode.F))
                Debug.Log(PlayersManager.GetTokensAt(WaypointIndexToSearch, PlayersManager.Players[0]).Count + " tokens present there!");
            if (Input.GetKeyDown(KeyCode.Z))
            {
                SetGameSpeed(GameSpeed.Fast);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                SetGameSpeed(GameSpeed.SuperFast);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                SetGameSpeed(GameSpeed.Ultra);
            }
        }

        private void SetGameSpeed(GameSpeed gameSpeed)
        {
            switch (gameSpeed)
            {
                case GameSpeed.Normal:
                    LudoAI.DelayPlusMinus = AIDelayPlusMinus;
                    LudoAI.DelayBeforeRollingDice = AIDelayBeforeRollingDice;
                    LudoAI.DelayInChoosingToken = AIDelayInChoosingToken;

                    Constants.diceRollShuffles = DiceRollShuffles;
                    Constants.diceRollTime = DiceRollTime;
                    Constants.idleTimeAfterDiceRoll = IdleTimeAfterDiceRoll;
                    Constants.delayAfterTokenMoveComplete = DelayAfterTokenMoveComplete;
                    Constants.delayBetweenTokenMoves = DelayBetweenTokenMoves;
                    break;
                case GameSpeed.Fast:
                    LudoAI.DelayPlusMinus = 0;
                    LudoAI.DelayBeforeRollingDice = 0.1f;
                    LudoAI.DelayInChoosingToken = 0.1f;
                    Constants.diceRollShuffles = 5;
                    Constants.diceRollTime = 0.2f;
                    Constants.idleTimeAfterDiceRoll = 0.1f;
                    Constants.delayAfterTokenMoveComplete = 0.5f;
                    Constants.delayBetweenTokenMoves = 0.05f;
                    break;
                case GameSpeed.SuperFast:
                    LudoAI.DelayPlusMinus = 0;
                    LudoAI.DelayBeforeRollingDice = 0.05f;
                    LudoAI.DelayInChoosingToken = 0.05f;
                    Constants.diceRollShuffles = 5;
                    Constants.diceRollTime = 0.1f;
                    Constants.idleTimeAfterDiceRoll = 0.05f;
                    Constants.delayAfterTokenMoveComplete = 0.05f;
                    Constants.delayBetweenTokenMoves = 0.05f;
                    break;
                case GameSpeed.Ultra:
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
                    SetGameSpeed(GameSpeed.Normal);
                    break;
            }
            
            Debug.Log("Set GameSpeed to " + gameSpeed);
        }
    }
}