using Game.Engine;
using Game.Presentation;

namespace Game.Infrastructure
{

	public class ReactionOnCreateHero : EngineReactionOn<CreateHeroCommand.Result>
	{
		public ReactionOnCreateHero(IListenersProvider listenersProvider) : base(listenersProvider) { }
	}

}