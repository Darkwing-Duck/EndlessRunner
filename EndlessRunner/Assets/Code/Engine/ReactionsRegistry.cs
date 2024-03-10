using System.Collections.Generic;

namespace Game.Engine
{

	public interface IReactionsRegistry
	{
		public void Flush(CmdResult cmdResult);
	}

	/// <summary>
	/// Container of all registered reactions of certain type.
	/// </summary>
	public class ReactionsRegistry<T> : IReactionsRegistry where T : CmdResult
	{
		private List<IEngineReactionOn<T>> _reactions = new();

		public void Add(IEngineReactionOn<T> reaction)
		{
			_reactions.Add(reaction);
		}
		
		public void Remove(IEngineReactionOn<T> reaction)
		{
			_reactions.Remove(reaction);
		}
		
		void IReactionsRegistry.Flush(CmdResult cmdResult)
		{
			Flush((T)cmdResult);
		}

		private void Flush(T cmdResult)
		{
			foreach (var reaction in _reactions) {
				reaction.ReactOn(cmdResult);
			}
		}
	}

}