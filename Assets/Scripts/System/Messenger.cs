using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.System
{
    public class Messenger : MonoBehaviour
    {
        public static MessageBroker Broker { get; private set; }

        public void Awake()
        {
            Assert.IsNull(Broker);
            Broker = new MessageBroker();
        }

        public void OnDestroy()
        {
            Broker = null;
        }
    }
}
