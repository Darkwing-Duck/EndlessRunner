using UnityEngine;

namespace Game.Configs
{

	public class ElementConfig : GameConfig
	{
		[SerializeField]
		private string _viewKey;
		
		public string ViewKey => _viewKey;
	}

}