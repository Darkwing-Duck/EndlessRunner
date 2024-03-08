using Game.Engine;
using Game.Presentation;

namespace Game.Infrastructure
{

	public class EngineReactionOn<T> : IEngineReactionOn<T> where T : CmdResult
	{
		private readonly IListenersProvider _listenersProvider;

		public EngineReactionOn(IListenersProvider listenersProvider) => _listenersProvider = listenersProvider;

		public void ReactOn(T cmdResult)
		{
			var listeners = _listenersProvider.FindAllFor<T>();
			
			foreach (var listener in listeners) {
				listener.On(cmdResult);
			}
		}
	}

}