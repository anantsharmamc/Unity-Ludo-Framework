using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

namespace com.bhambhoo.fairludo
{
    public class PlayerToken : MonoBehaviour
    {
        private static readonly int[] SafeWaypoints = { 0, 8, 13, 21, 26, 34, 39, 47 };
        
        // From 1 to 4
        //public int playerIndex = 1;
        // Also from 1 to 4. This is used to move particular token on other clients.
        public int TokenIndex = 1;
        
        // Waypoint index according to this player's path. -1 means player is at base.
        public int LocalWaypointIndex = -1;

        public GameObject InputHighlight;

        // This token will always return to this base when it dies.
        public Transform Base;
        
        private Player player;

        /// <summary>
        /// This method is used to initialize this token.
        /// </summary>
        /// <param name="PlayerIndex">Any from 1 to 4.</param>
        /// <param name="Base">The player base this should stand upon, in the start.</param>
        public PlayerToken Initialize(Player player, Transform Base, Color tokenColor)
        {
            this.Base = Base;
            transform.position = Base.position;
            LocalWaypointIndex = -1;

            this.player = player;

            GetComponent<SpriteRenderer>().color = tokenColor;

            return this;
        }
        
        private void OnMouseDown()
        {
            MatchManager.Instance.OnTokenTouchUserInput(this);
            // Match manager will take care if this token touch was being awaited or not.
        }

        public void Highlight(bool on)
        {
            InputHighlight.SetActive(on);
        }
        
        public bool CanMove(int stepsToTake)
        {
            if ((Constants.LastWaypointIndex - LocalWaypointIndex) >= stepsToTake)
            {
                // if player is at base
                if (LocalWaypointIndex == -1)
                {
                    // if player is at base, they can move only if diceResult is 6
                    if (stepsToTake == 6)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This function will start moving the token strictly, without any checks.
        /// Checks like if the token IsLocal and if the token CanMove should be made before calling this function.
        /// </summary>
        /// <param name="diceResult"></param>
        public void Move(int diceResult)
        {
            StartCoroutine(MoveCoroutine(diceResult, LocalWaypointIndex));
        }
        
        public void MoveNonLocalPlayer(int diceResult, int currentWaypointIndex)
        {
            StartCoroutine(MoveCoroutine(diceResult, currentWaypointIndex));
        }

        private IEnumerator MoveCoroutine(int diceResult, int currentWaypointIndex)
        {
            // The remaining Steps is provided to place token at certain position
            // before starting the move, to sync in case of network lag.
            if (!player.IsLocal && LocalWaypointIndex != currentWaypointIndex)
            {
                // teleport to the correct waypoint
                transform.position = Constants.Instance.GetWaypoint(player.PlayerIndex, currentWaypointIndex).position;
                LocalWaypointIndex = currentWaypointIndex;
            }

            if (LocalWaypointIndex == -1 && diceResult == 6)
                diceResult = 1;

            for (int i = 0; i < diceResult; i++)
            {
                yield return new WaitForSeconds(Constants.delayBetweenTokenMoves);
                LocalWaypointIndex++;
                SanUtils.PlaySound(Constants.Instance.sfxTokenHop);
                transform.position = Constants.Instance.GetWaypoint(player.PlayerIndex, LocalWaypointIndex).position;
            }

            yield return new WaitForSeconds(Constants.delayAfterTokenMoveComplete);

            // Check if we've reached endpoint, or we've killed another token
            if (LocalWaypointIndex == Constants.LastWaypointIndex)
            {
                Debug.LogError("Player " + player.PlayerIndex + " reached endpoint, should get an extra turn!");
                //UnityEditor.EditorApplication.isPaused = true;
                //GameManager.Instance.SetGameSpeed(GameManager.GameSpeed.Normal);
                MatchManager.Instance.DiceRollsRemaining++;
            }
            // else if we're not at a safe waypoint now, we can check for tokens to kill
            else if (!SafeWaypoints.Contains(LocalWaypointIndex))
            {
                foreach (PlayerToken oneToken in PlayersManager.GetTokensAt(LocalWaypointIndex, player))
                {
                    // If this token isn't our player's token
                    if (oneToken.player != player)
                    {
                        Debug.LogError("Player " + player.PlayerIndex + " killed " + oneToken.player.PlayerIndex + "'s token, should get an extra turn too!");
                        //UnityEditor.EditorApplication.isPaused = true;
                        //GameManager.Instance.SetGameSpeed(GameManager.GameSpeed.Normal);
                        // Creating killed token's die animation here to create systematic delays in game-actions
                        // TODO update this for networked game
                        yield return new WaitForSeconds(Constants.delayBetweenTokenMoves);
                        oneToken.LocalWaypointIndex = -1;
                        SanUtils.PlaySound(Constants.Instance.sfxTokenKill);
                        oneToken.transform.position = oneToken.Base.position;
                        yield return new WaitForSeconds(Constants.delayAfterTokenMoveComplete);

                        MatchManager.Instance.DiceRollsRemaining++;
                    }
                }
            }

            MatchManager.Instance.NextTurn();
            yield return null;
        }
        
       
    }
}