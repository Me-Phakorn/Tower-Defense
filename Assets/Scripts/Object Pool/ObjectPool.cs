using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Threading.Tasks;

namespace TowerDefense.Pooling
{
    public class ObjectPool
    {
        private static Dictionary<int, ObjectPool<GameObject>> poolList = new Dictionary<int, ObjectPool<GameObject>>();

        public static bool AddObjectToPool(GameObject _object)
        {
            if (poolList.ContainsKey(_object.GetInstanceID()))
                return false;

            var _pooling = new ObjectPool<GameObject>(() =>
            {
                GameObject _objectPool = GameObject.Instantiate(_object);

                _objectPool.GetComponent<PoolRelease>().Initialize(_object.GetInstanceID());

                return _objectPool;
            }, OnGetPooling, OnReleasePooling, OnDestroyPooling, false, 10, 50);

            return poolList.TryAdd(_object.GetInstanceID(), _pooling);
        }

        private static void OnGetPooling(GameObject _object)
        {
            
        }

        private static void OnReleasePooling(GameObject _object)
        {
            _object.SetActive(false);
        }

        private static void OnDestroyPooling(GameObject _object)
        {
            GameObject.Destroy(_object);
        }

        public static GameObject GetPool(GameObject sourceObject, Vector3 position, Quaternion quaternion)
        {
            return SetTransform(GetPool(sourceObject), position, quaternion);
        }

        public static GameObject GetPool(int hashCode, Vector3 position, Quaternion quaternion)
        {
            var _pool = GetPool(hashCode);

            if (!_pool)
                return null;

            return SetTransform(_pool, position, quaternion);
        }

        public static GameObject GetPool(GameObject sourceObject)
        {
            AddObjectToPool(sourceObject);

            return poolList[sourceObject.GetInstanceID()].Get();
        }

        public static GameObject GetPool(int hashCode)
        {
            if (poolList.ContainsKey(hashCode))
                return null;

            return poolList[hashCode].Get();
        }

        public static void ReleasePool(int hashCode, GameObject _object)
        {
            ObjectPool<GameObject> _value;
            if (poolList.TryGetValue(hashCode, out _value))
                _value.Release(_object);
        }

        public static async void ReleasePool(int hashCode, GameObject _object, float delay)
        {
            await Task.Delay((int)(delay * 1000));

            ReleasePool(hashCode, _object);
        }

        public static void Dispose(int hashCode)
        {
            ObjectPool<GameObject> _value;
            if (poolList.TryGetValue(hashCode, out _value))
                _value.Dispose();

            poolList.Remove(hashCode);
        }

        public static void ClearAll()
        {
            poolList.Clear();
        }

        public static bool IsPool(int hashCode)
        {
            ObjectPool<GameObject> _value;
            return poolList.TryGetValue(hashCode, out _value);
        }

        private static GameObject SetTransform(GameObject _object, Vector3 position, Quaternion quaternion)
        {
            _object.SetActive(false);
            _object.transform.position = position;
            _object.transform.rotation = quaternion;
            _object.SetActive(true);

            return _object;
        }
    }
}