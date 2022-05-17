
using Sirenix.OdinInspector;

using UnityEngine;

namespace EZPool
{

    [CreateAssetMenu(fileName = "Projectile Pool", menuName = "Pool/Projectile Pool")]
    public class ProjectilePoolSO : ComponentPoolSO<Projectile>
    {
        //[AssetsOnly] [SerializeField] private GameObject _prefab;

       

        //    public override IFactory<Projectile> Factory
        //    {
        //        get
        //        {
        //            return _factory;
        //        }
        //        set
        //        {
        //            _factory = value as ProjectileFactorySO;
        //        }
        //    }

        //}

    }
}

