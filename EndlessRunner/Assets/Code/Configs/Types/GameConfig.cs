using UnityEngine;

namespace Game.Configs
{

	public abstract class GameConfig : ScriptableObject, IGameConfig
	{
		[SerializeField]
		private uint _id;

		public uint Id => _id;
		
	}

}