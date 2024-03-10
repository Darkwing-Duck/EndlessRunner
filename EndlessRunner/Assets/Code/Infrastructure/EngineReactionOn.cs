using Game.Engine;
using Game.Presentation;

namespace Game.Infrastructure
{

	/// <summary>
	/// Provides mechanism to react on specific type of command result 'T'.
	/// Calls method 'On(T)' on each IListener provided by IListenersProvider
	/// </summary>
	/// <typeparam name="T">Engine command result</typeparam>
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