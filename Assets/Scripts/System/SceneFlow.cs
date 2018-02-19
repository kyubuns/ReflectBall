using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.System
{
    public class SceneFlow : MonoBehaviour
    {
        public IEnumerator Start()
        {
            Messenger.Broker.Receive<InputReloadScene>().Subscribe(_ => SceneManager.LoadScene("Main")).AddTo(this);

            while (true)
            {
                yield return Messenger.Broker.Receive<InputGameStart>().First().ToYieldInstruction();

                Debug.Log("Game Start");
                Messenger.Broker.Publish(new OnGameStart());

                yield return Messenger.Broker.Receive<OnGameOver>().First().ToYieldInstruction();

                Debug.Log("Game Finish");
                Messenger.Broker.Publish(new OnGameFinish());
            }
        }
    }

    public class InputReloadScene
    {
    }

    public class InputGameStart
    {
    }

    public class OnGameStart
    {
    }

    public class OnGameFinish
    {
    }
}
