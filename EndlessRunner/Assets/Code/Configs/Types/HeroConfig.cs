using UnityEngine;

namespace Game.Configs
{

	[CreateAssetMenu(fileName = "SO_Hero_{ID}_{NAME}", menuName = "Configs/Hero")]
	public class HeroConfig : ElementConfig
	{
		[SerializeField]
		private float _speed;
		
		public float Speed => _speed;
	}

}