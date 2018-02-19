using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.System
{
    public class Init : MonoBehaviour
    {
        public IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            SceneManager.LoadScene("Main");
        }
    }
}
