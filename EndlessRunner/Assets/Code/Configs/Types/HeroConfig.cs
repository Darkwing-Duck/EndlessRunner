using UnityEngine;

namespace Game.Configs
{

	[CreateAssetMenu(fileName = "SO_Hero_{ID}_{NAME}", menuName = "Configs/Hero")]
	public class HeroConfig : GameplayElementConfig
	{
		[SerializeField]
		private float _speed;
		
		public float Speed => _speed;
	}

}