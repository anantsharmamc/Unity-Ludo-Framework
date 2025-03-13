using System.Collections;
using UnityEngine;
using Utility;

namespace com.bhambhoo.fairludo
{
    public class Dice : SingletonScene<Dice>
    {
        private Sprite[] diceSides;
        private SpriteRenderer rend;

        private bool coroutineAllowed = true;

        public PlayerToken TestToken;

        public bool BiasedDice;
        public int BiasedOutcome = 1;
        public static bool RollAllowed;
        
        private void Start()
        {
            rend = GetComponent<SpriteRenderer>();
            diceSides = Resources.LoadAll<Sprite>("DiceSides/");
            rend.sprite = diceSides[5];
        }
        
        private void OnMouseDown()                                 
        {                                                          
            if (RollAllowed)                                       
            {                                                      
                if (MatchManager.InputAllowed && coroutineAllowed) 
                    StartCoroutine(RollTheDice());                 
            }                                                      
        }                                                          

        public static void Highlight(bool on)
        {
            Constants.Instance.DiceTurnHighlight.SetActive(on);
        }

        public void BotDiceRoll()
        {
            if (coroutineAllowed)
                StartCoroutine(RollTheDice());
        }

        private IEnumerator RollTheDice()
        {
            RollAllowed = false;
            coroutineAllowed = false;
            Constants.Instance.DiceTurnHighlight.SetActive(false);
            int randomDiceSide = 0;
            for (int i = 0; i <= Constants.diceRollShuffles; i++)
            {
                randomDiceSide = Random.Range(0, 6);
                rend.sprite = diceSides[randomDiceSide];
                SanUtils.PlaySound(Constants.Instance.sfxDiceRoll);
                yield return new WaitForSeconds(Constants.diceRollTime / Constants.diceRollShuffles);
            }

            // TODO delete before release
            if (BiasedDice)
            {
                SanUtils.PlaySound(Constants.Instance.sfxDiceRoll);
                randomDiceSide = BiasedOutcome - 1;
                rend.sprite = diceSides[randomDiceSide];
            }

            yield return new WaitForSeconds(Constants.idleTimeAfterDiceRoll);

            MatchManager.DiceResult = randomDiceSide + 1;

            // Logic to allow user to select a token, and move that token
            // TODO
            if (TestToken)
                TestToken.Move(MatchManager.DiceResult);

            if (MatchManager.DiceResult == 6)
                SanUtils.PlaySound(Constants.Instance.sfxLocalPlayer6);

            MatchManager.Instance.OnDiceRolledLocally(randomDiceSide + 1);
            coroutineAllowed = true;
        }
    }
}