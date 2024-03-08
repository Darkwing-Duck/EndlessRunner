using UnityEngine;

namespace Game.Infrastructure
{

	public class AIPlayerInput : PlayerInput
	{
		private const float _aiJumpRate = 4f;
		private float _elapsedTime;
		
		public AIPlayerInput(uint playerId, IHeroActionsMapper heroActionsMapper) : base(playerId, heroActionsMapper) { }

		public override void Update()
		{
			_elapsedTime += Time.deltaTime;

			if (_elapsedTime >= _aiJumpRate) {
				_elapsedTime = 0f;
				ActionsMapper.Jump(PlayerId);
			}
		}
	}

}