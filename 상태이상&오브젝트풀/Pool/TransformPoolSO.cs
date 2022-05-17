using UnityEngine;


namespace EZPool
{
    [CreateAssetMenu(fileName = "Transform Pool", menuName = "Pool/Transform Pool")]

    public class TransformPoolSO : ComponentPoolSO<Transform>
    {
        //[SerializeField]
        //private TransformFactorySO _factory;

        //public override IFactory<Transform> Factory
        //{
        //    get
        //    {
        //        return _factory;
        //    }
        //    set
        //    {
        //        _factory = value as TransformFactorySO;
        //    }
        //}
        
    }
}