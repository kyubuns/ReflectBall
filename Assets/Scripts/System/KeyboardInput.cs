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
        }
    }
}
