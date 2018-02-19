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
            Messenger.Broker.Receive<ReloadScene>().Subscribe(_ => SceneManager.LoadScene("Main")).AddTo(this);

            yield return new WaitForSeconds(0.5f);
            Debug.Log("Start");
        }
    }

    public class ReloadScene
    {
    }
}
