using Game.Engine;

namespace Game.Presentation
{

	/// <summary>
	/// Interface listeners provider.
	/// </summary>
	public interface IListenersProvider
	{
		IListener<TCmdResult>[] FindAllFor<TCmdResult>() where TCmdResult : CmdResult;
	}

}