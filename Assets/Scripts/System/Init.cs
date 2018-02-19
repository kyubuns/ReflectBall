using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem
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
