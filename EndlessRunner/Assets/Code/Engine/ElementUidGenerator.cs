namespace Game.Engine
{

	public interface IElementUidGenerator
	{
		uint Next();
	}
	
	public class ElementUidGenerator : IElementUidGenerator
	{
		private uint _lastId;

		public uint Next() => ++_lastId;
	}

}