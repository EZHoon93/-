
using UnityEngine;
namespace EZPool
{
    [CreateAssetMenu(fileName = "ParticleSystem Pool", menuName = "Pool/ParticleSystem Pool")]
    public class ParticlePoolSO : ComponentPoolSO<ParticleSystem>
    {
        [SerializeField]
        private FactorySO<ParticleSystem> _factorySO;

        public float lastPopTime;  //��� �����ϸ����Ÿ�����

        public override IFactory<ParticleSystem> Factory
        {
            get => _factorySO;
            set => _factorySO = value as ParticleFactorySO;

        }


    }
}