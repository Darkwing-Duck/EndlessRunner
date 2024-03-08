using UnityEngine;

namespace Game.Configs
{

	[CreateAssetMenu(fileName = "SO_Collectible_{ID}_{NAME}", menuName = "Configs/Collectible")]
	public class CollectibleConfig : GameplayElementConfig
	{
		[SerializeField]
		private EffectConfig[] _effects;
		public EffectConfig[] Effects => _effects;
	}

}