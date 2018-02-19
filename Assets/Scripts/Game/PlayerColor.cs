using UnityEngine;

namespace Game
{
    public class PlayerColor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer top;
        [SerializeField] private SpriteRenderer bottom;

        public void Start()
        {
            top.color = Colors.Top;
            bottom.color = Colors.Bottom;
        }
    }
}
