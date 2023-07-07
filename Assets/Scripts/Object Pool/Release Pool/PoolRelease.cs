using UnityEngine;

namespace TowerDefense.Pooling
{
    public class PoolRelease : MonoBehaviour
    {
        public int InstanceID { get; private set; } = -1;

        public GameObject Initialize(int instanceID)
        {
            InstanceID = instanceID;

            return gameObject;
        }

        public void Release()
        {
            if (InstanceID != -1)
                ObjectPool.ReleasePool(InstanceID, gameObject);
        }
    }
}