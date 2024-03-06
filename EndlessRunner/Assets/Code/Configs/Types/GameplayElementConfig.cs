using UnityEngine;

namespace Game.Configs
{

	public class GameplayElementConfig : GameConfig
	{
		[SerializeField]
		private string _viewKey;
		
		public string ViewKey => _viewKey;
	}

}