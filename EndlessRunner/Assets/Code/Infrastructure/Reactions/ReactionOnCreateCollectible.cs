using Game.Engine;
using Game.Presentation;

namespace Game.Infrastructure
{

	public class ReactionOnCreateCollectible : EngineReactionOn<CreateCollectibleCommand.Result>
	{
		public ReactionOnCreateCollectible(IListenersProvider listenersProvider) : base(listenersProvider) { }
	}

}