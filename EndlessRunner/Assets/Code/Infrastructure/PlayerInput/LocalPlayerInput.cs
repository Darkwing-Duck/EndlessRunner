using UnityEngine;

namespace Game.Infrastructure
{

	public class LocalPlayerInput : PlayerInput
	{
		public LocalPlayerInput(uint playerId, IHeroActionsMapper heroActionsMapper) : base(playerId, heroActionsMapper) { }

		public override void Update()
		{
			if (Input.GetMouseButtonDown(0)) {
				ActionsMapper.Jump(PlayerId);
			}
		}
	}

}