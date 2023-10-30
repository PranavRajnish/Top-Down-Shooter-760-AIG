using UnityEngine;

namespace Player
{
    public abstract class PlayerComponent : MonoBehaviour
    {
        public int CharacterID { get; private set; }

        public virtual void Init(int playerID)
        {
            CharacterID = playerID;
        }
    }
}