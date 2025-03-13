using UnityEngine;
using Utility;

namespace com.bhambhoo.fairludo
{
    public class GameManager : SingletonScene<GameManager>
    {
        public enum GameSpeed { Normal, Fast, SuperFast, Ultra };

        [SerializeField]
        private LudoConfiguration LudoConfiguration;
        
        public Constants.MatchType SelectedMatchType = Constants.MatchType.PassNPlay;
        [Range(2, 4)]
        public byte SelectedNumPlayers = 2;

        private SpeedController speedController;
        public AudioSource AudioSource;

        public override void Awake()
        {
            base.Awake();
            speedController = new SpeedController(LudoConfiguration); 
            AudioSource = GetComponent<AudioSource>();

            gameObject.AddComponent<Debugger>();
        }

        //TODO: Use this for start of the ludo game
        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            speedController.SetGameSpeed(GameSpeed.Normal);
            MatchManager.Instance.StartMatch(SelectedNumPlayers, SelectedMatchType);
        }
    }
}