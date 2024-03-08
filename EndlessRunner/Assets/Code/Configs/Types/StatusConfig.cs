using UnityEngine;

namespace Game.Configs
{

	[CreateAssetMenu(fileName = "SO_Status_{ID}_{NAME}", menuName = "Configs/Status")]
	public class StatusConfig : GameConfig
	{
		// duration is in ticks, not in seconds
		// -1 means the status is persistent
		[SerializeField]
		private int _duration = -1;

		[SerializeField] 
		private EffectConfig[] _effects; 
		
		public int Duration => _duration;
		public EffectConfig[] Effects => _effects;
	}

}