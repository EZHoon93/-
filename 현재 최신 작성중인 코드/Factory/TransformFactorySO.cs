using UnityEngine;

namespace EZPool
{
    [CreateAssetMenu(fileName = "Transform Factory", menuName = "Pool/Factory/Transform Factory")]
    public class TransformFactorySO : FactorySO<Transform>
    {
        [SerializeField] private Transform _prefab;

        public void SetupPrefab(Transform newPrefab)
        {
            _prefab = newPrefab;
        }
        public override Transform Create()
        {
            return Instantiate(_prefab);

        }

    }
}