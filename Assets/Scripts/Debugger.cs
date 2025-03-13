// 

using UnityEngine;

namespace com.bhambhoo.fairludo
{
    public class Debugger : MonoBehaviour
    {
        private readonly LudoConfiguration ludoConfiguration;
        private readonly SpeedController speedController;
        
        public Debugger(LudoConfiguration ludoConfiguration, SpeedController speedController)
        {
            this.ludoConfiguration = ludoConfiguration;
            this.speedController = speedController;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayersManager.ChangePlayerType(PlayersManager.Players[0], Constants.PlayerType.Bot);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                speedController.SetGameSpeed(GameManager.GameSpeed.Fast);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                speedController.SetGameSpeed(GameManager.GameSpeed.SuperFast);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                speedController.SetGameSpeed(GameManager.GameSpeed.Ultra);
            }
        }
    }
}