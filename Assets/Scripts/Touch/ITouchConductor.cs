using UnityEngine;

namespace Touch
{
    public interface ITouchConductor
    {
        public void OnClick(string id, GameObject gameObject);
    }
}
