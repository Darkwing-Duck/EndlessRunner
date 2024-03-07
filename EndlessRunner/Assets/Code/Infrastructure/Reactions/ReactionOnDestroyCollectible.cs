using Game.Engine;
using Game.Presentation;

namespace Game.Infrastructure
{

	public class ReactionOnDestroyCollectible : EngineReactionOn<DestroyCollectibleCommand.Result>
	{
		public ReactionOnDestroyCollectible(IListenersProvider listenersProvider) : base(listenersProvider) { }
	}

}