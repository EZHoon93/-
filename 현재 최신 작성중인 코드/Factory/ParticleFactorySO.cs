

using UnityEngine;

namespace EZPool
{
    [CreateAssetMenu(fileName = "ParticleSystem Factory", menuName = "Pool/Factory/ParticleSystem Factory")]
    public class ParticleFactorySO : FactorySO<ParticleSystem>
    {
        [SerializeField] private ParticleSystem _prefab;

        public void SetupPrefab(ParticleSystem newPrefab)
        {
            _prefab = newPrefab;
        }
        public override ParticleSystem Create()
        {
            return Instantiate(_prefab);

        }
    }
}