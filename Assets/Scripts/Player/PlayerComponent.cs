using UnityEngine;

namespace Player
{
    public abstract class PlayerComponent : MonoBehaviour
    {
        public int PlayerID { get; private set; }

        public virtual void Init(int playerID)
        {
            PlayerID = playerID;
        }
    }
}