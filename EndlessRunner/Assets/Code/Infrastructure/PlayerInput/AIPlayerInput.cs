using UnityEngine;

namespace Game.Infrastructure
{

	public class AIPlayerInput : PlayerInput
	{
		private float _nextJumpIn = 4f; // sec
		private float _elapsedTime;
		
		public AIPlayerInput(uint playerId, IHeroActionsMapper heroActionsMapper) : base(playerId, heroActionsMapper) { }

		public override void Update()
		{
			_elapsedTime += Time.deltaTime;

			if (_elapsedTime >= _nextJumpIn) {
				_elapsedTime = 0f;
				_nextJumpIn = Random.Range(2f, 6f);
				ActionsMapper.Jump(PlayerId);
			}
		}
	}

}