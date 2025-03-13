using System.Collections;
using UnityEngine;

namespace com.bhambhoo.fairludo
{
    public class Dice : MonoBehaviour
    {
        private Sprite[] diceSides;
        private SpriteRenderer rend;

        private bool coroutineAllowed = true;

        public PlayerToken TestToken;

        public bool BiasedDice;
        public int BiasedOutcome = 1;
        public static bool RollAllowed;
        public static Dice Instance;
        
        private void Start()
        {
            rend = GetComponent<SpriteRenderer>();
            diceSides = Resources.LoadAll<Sprite>("DiceSides/");
            rend.sprite = diceSides[5];
        }

        private void OnEnable()
        {                                 
            Instance = this;
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
                SanUtils.PlaySound(Constants.Instance.sfxDiceRoll, MatchManager.Instance.AudioSource);
                yield return new WaitForSeconds(Constants.diceRollTime / Constants.diceRollShuffles);
            }

            // TODO delete before release
            if (BiasedDice)
            {
                SanUtils.PlaySound(Constants.Instance.sfxDiceRoll, MatchManager.Instance.AudioSource);
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