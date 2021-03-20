using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class TouchHandler : MonoBehaviour
   {
        public void OnClick(BaseEventData data)
        {
            Debug.Log("yes");
            Destroy(gameObject);
        }
    }
}
