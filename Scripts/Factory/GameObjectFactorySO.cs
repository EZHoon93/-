
using UnityEngine;

namespace EZPool
{
    [CreateAssetMenu(fileName = "GameObject Factory", menuName = "Pool/Factory/GameObject Factory")]
    public class GameObjectFactorySO : FactorySO<GameObject>
    {
        [SerializeField] private GameObject _prefab;

        public void SetupPrefab(GameObject newPrefab)
        {
            _prefab = newPrefab;
        }
        public override GameObject Create()
        {
            return Instantiate(_prefab);

        }

    }
}