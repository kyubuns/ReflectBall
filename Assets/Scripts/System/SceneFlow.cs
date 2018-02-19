using System.Collections;
using UnityEngine;

namespace Game.System
{
    public class SceneFlow : MonoBehaviour
    {
        public IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Start");
        }
    }
}
