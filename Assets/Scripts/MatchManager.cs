using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.bhambhoo.fairludo
{
    public class MatchManager : MonoBehaviour
    {
        public Constants.MatchType currentMatchType = Constants.MatchType.PassNPlay;
        public static bool InputAllowed = true;
        public static bool MatchRunning = false;
        public byte numPlayers = 2;
        public AudioSource audioSource;

        public static MatchManager Instance;

        //public List<PlayerToken> AllTokens;

        public void StartMatch(byte numPlayers, Constants.MatchType matchType)
        {
            MatchRunning = true;
            //AllTokens.Clear();
            this.numPlayers = numPlayers;
            this.currentMatchType = matchType;

            // De-highlight all turn highlighters
            foreach (GameObject oneTurnHighlighter in Constants.Instance.PlayerTurnHighlighters)
            {
                oneTurnHighlighter.SetActive(false);
            }

            PlayersManager.Instance.Initialize(numPlayers, matchType);

            NextTurn();
            // After this we will await for the dice to be rolled
        }

        private void OnEnable()
        {
            Instance = this;
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void InitiatePlayerTurn(Player player)
        {
            if (player == null)
                Debug.LogError("Player is Null");

            player.turnHighlighter.SetActive(true);

            switch (currentMatchType)
            {
                case Constants.MatchType.VsComputer:
                    if (player.IsLocal)
                    {
                        Dice.Highlight(true);
                        SanUtils.PlaySound(Constants.Instance.sfxLocalPlayerTurn);
                        Dice.RollAllowed = true;
                    }
                    else
                    {
                        // Means it's a bot
                        SanUtils.PlaySound(Constants.Instance.sfxOtherPlayerTurn);
                        LudoAI.Instance.PlayTurn();
                    }
                    break;
                case Constants.MatchType.Online:
                    if (player.IsLocal)
                    {
                        Dice.Highlight(true);
                        SanUtils.PlaySound(Constants.Instance.sfxLocalPlayerTurn);
                        Dice.RollAllowed = true;
                    }
                    else
                    {
                        // TODO
                    }
                    break;
                case Constants.MatchType.PassNPlay:
                    Dice.Highlight(true);
                    SanUtils.PlaySound(Constants.Instance.sfxLocalPlayerTurn);
                    Dice.RollAllowed = true;
                    break;
                default:
                    break;
            }

        }

        List<PlayerToken> tokensWeCanMove = new List<PlayerToken>(4);
        public static int DiceResult = 1;

        public Player whoseTurn;
        // if player kills another token, this is incremented
        // this is decremented before every dice roll
        // if player gets a six (and can move a token), this is incremented
        // on new player's turn this is set to 1
        public int diceRollsRemaining = 0;
        public int numSixes = 0;

        // This function gives turn to local player or next player based on if dice rolls are remaining, or not.
        public void NextTurn()
        {
            // If it's the start of the match
            if (whoseTurn == null)
            {
                // Give turn to Player1 (fix this for networked game)
                diceRollsRemaining = 1;
                whoseTurn = PlayersManager.Players[0];

                InitiatePlayerTurn(whoseTurn);
                return;
            }

            diceRollsRemaining--;
            if (diceRollsRemaining < 1)
            {
                // It's time for next player's turn
                numSixes = 0;
                diceRollsRemaining = 1;
                whoseTurn = NextPlayer();
                
                InitiatePlayerTurn(whoseTurn);
            }
            else
            {
                InitiatePlayerTurn(whoseTurn);
            }

        }

        /// <summary>
        /// This can be dice rolled by either local player, or it's a PassNPlay type match.
        /// </summary>
        /// <param name="diceResult"></param>
        public void OnDiceRolledLocally(int diceResult)
        {
            // Remove player highlights
            Constants.Instance.P1TurnHighlight.SetActive(false);
            Constants.Instance.P2TurnHighlight.SetActive(false);
            Constants.Instance.P3TurnHighlight.SetActive(false);
            Constants.Instance.P4TurnHighlight.SetActive(false);

            // Clear cache
            tokensWeCanMove.Clear();

            // Store result
            DiceResult = diceResult;

            if (DiceResult == 6)
            {
                numSixes++;

                if (numSixes > 2)
                {
                    numSixes = 0;
                    NextTurn();
                }
            }

            // Calculate which tokens can be moved from this dice result
            foreach (PlayerToken oneToken in whoseTurn.playerTokens)
            {
                if (oneToken.CanMove(diceResult))
                    tokensWeCanMove.Add(oneToken);
            }

            if (tokensWeCanMove.Count == 0)
            {
                NextTurn();
            }
            else
            {
                // If this diceroll wasn't empty, and number of sixes isn't 3 (checked above), give this player another chance.
                if (DiceResult == 6)
                    diceRollsRemaining++;

                if (tokensWeCanMove.Count == 1)
                {

                    // move that damn token and go to next turn
                    tokensWeCanMove[0].Move(diceResult);
                }
                else
                {
                    // if we're localplayer, give user choice to select desired token to move
                    // else perform bot action
                    if (whoseTurn.IsLocal)
                        foreach (PlayerToken item in tokensWeCanMove)
                        {
                            item.Highlight(true);
                        }
                    else if (whoseTurn.type == Constants.PlayerType.Bot)
                        LudoAI.Instance.ChooseToken(tokensWeCanMove, diceResult);
                }
            }
        }

        public void OnTokenTouchUserInput(PlayerToken token)
        {
            // Check if this token was among the list of tokens we were waiting for
            if (tokensWeCanMove.Contains(token))
            {
                // De-highlight every token
                foreach (PlayerToken item in tokensWeCanMove)
                {
                    item.Highlight(false);
                }
                token.Move(DiceResult);

                tokensWeCanMove.Clear();
            }

        }

        public Player NextPlayer()
        {
            return PlayersManager.GetPlayer(NextPlayerIndex(whoseTurn.playerIndex));
        }

        public byte NextPlayerIndex(byte currentIndex)
        {
            switch (currentIndex)
            {
                case 1:
                    if (numPlayers > 2)
                        return 2;
                    else return 3;
                case 2:
                    return 3;
                case 3:
                    if (numPlayers > 3)
                        return 4;
                    else return 1;
                case 4:
                    return 1;
                default:
                    if (numPlayers > 2)
                        return 2;
                    else return 3;
            }
        }
    }
}