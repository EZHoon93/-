using UnityEngine;


namespace EZPool
{
	[CreateAssetMenu(fileName = "ProjectileFactory", menuName = "Pool/Factory/Projectile Factory")]
	public class ProjectileFactorySO : FactorySO<Projectile>
	{
		[SerializeField] Projectile _projectilePrefab;
		public override Projectile Create()
		{
			return Instantiate(_projectilePrefab);
		}
	}
}