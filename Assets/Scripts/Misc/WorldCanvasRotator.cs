using UnityEngine;

namespace Misc
{
    [RequireComponent(typeof(Canvas))]
    public class WorldCanvasRotator : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}