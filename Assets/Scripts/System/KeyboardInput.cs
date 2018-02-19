using Game;
using UnityEngine;

namespace GameSystem
{
    public class KeyboardInput : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Messenger.Broker.Publish(new InputReloadScene());
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Messenger.Broker.Publish(new OnShowTopBar());
                Messenger.Broker.Publish(new FinishTutorial());
            }
        }
    }
}
