using UnityEngine;

namespace Game.System
{
    public class KeyboardInput : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Messenger.Broker.Publish(new InputReloadScene());
            }
        }
    }
}
