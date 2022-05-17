
using UnityEngine;


namespace EZPool
{
    [CreateAssetMenu(fileName = "GameObject Pool", menuName = "Pool/GameObject Pool")]

    public class GameObjectPoolSO : PoolSO<GameObject>
    {
        [SerializeField]
        private GameObjectFactorySO _factory;

        //public override IFactory<GameObject> Factory
        //{
        //    get
        //    {
        //        return _factory;
        //    }
        //    set
        //    {
        //        _factory = value as GameObjectFactorySO;
        //    }
        //}

		private Transform _poolRoot;
		private Transform PoolRoot
		{
			get
			{
				if (_poolRoot == null)
				{
					_poolRoot = new GameObject(this.name).transform;
					_poolRoot.SetParent(_parent);
				}
				return _poolRoot;
			}
		}

		private Transform _parent;

		/// <summary>
		/// Parents the pool root transform to <paramref name="t"/>.
		/// </summary>
		/// <param name="t">The Transform to which this pool should become a child.</param>
		/// <remarks>NOTE: Setting the parent to an object marked DontDestroyOnLoad will effectively make this pool DontDestroyOnLoad.<br/>
		/// This can only be circumvented by manually destroying the object or its parent or by setting the parent to an object not marked DontDestroyOnLoad.</remarks>
		public void SetParent(Transform t)
		{
			_parent = t;
			PoolRoot.SetParent(_parent);
		}

		public override GameObject Pop()
		{
			GameObject member = null;
			do
			{
				member = base.Pop();
			} while (member == null);
			member.gameObject.SetActive(true);
			return member;
		}

		public override void Push(GameObject member)
		{
			member.transform.SetParent(PoolRoot.transform);
			member.gameObject.SetActive(false);
			base.Push(member);
		}

		protected override GameObject Create()
		{
			GameObject newMember = base.Create();
			newMember.transform.SetParent(PoolRoot.transform);
			newMember.gameObject.SetActive(false);
			return newMember;
		}

		public override void OnDisable()
		{
			base.OnDisable();
			if (_poolRoot != null)
			{
#if UNITY_EDITOR
				DestroyImmediate(_poolRoot.gameObject);
#else
				Destroy(_poolRoot.gameObject);
#endif
			}
		}

	}
}