using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class TouchHandler : MonoBehaviour
    {
        public string handlerId;
        public ITouchParent parent;

        public void OnClick(BaseEventData data)
        {
            parent.OnClick(handlerId, this.gameObject);
        }
    }
}
