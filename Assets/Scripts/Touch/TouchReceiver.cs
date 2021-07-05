using UnityEngine;
using UnityEngine.EventSystems;

namespace Touch
{
    public class TouchReceiver : MonoBehaviour
    {
        public string handlerId;
        public ITouchConductor conductor;

        public void OnClick(BaseEventData data)
        {
            conductor.OnClick(handlerId, this.gameObject);
        }
    }
}
