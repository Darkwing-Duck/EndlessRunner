using Game.Common;

namespace Game.Infrastructure
{

	public abstract class PlayerInput : IUpdatable
	{
		protected IHeroActionsMapper ActionsMapper;
		public readonly uint PlayerId;

		public PlayerInput(uint playerId, IHeroActionsMapper heroActionsMapper)
		{
			PlayerId = playerId;
			ActionsMapper = heroActionsMapper;
		}

		public abstract void Update();
	}

}