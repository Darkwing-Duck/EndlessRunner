using Game.Engine;

namespace Game.Presentation
{

	public interface IListenersProvider
	{
		IListener<TCmdResult>[] FindAllFor<TCmdResult>() where TCmdResult : CmdResult;
	}

}