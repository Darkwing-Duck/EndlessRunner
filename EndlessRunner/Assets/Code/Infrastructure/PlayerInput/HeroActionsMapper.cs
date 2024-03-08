using Game.Engine;

namespace Game.Infrastructure
{

	public class HeroActionsMapper : IHeroActionsMapper
	{
		private readonly IEngineInput _engineInput;

		public HeroActionsMapper(IEngineInput engineInput)
		{
			_engineInput = engineInput;
		}

		public void Jump(uint playerId)
		{
			_engineInput.Push(new HeroJumpCommand(playerId));
		}
	}

}