using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameSystem
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
