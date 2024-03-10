namespace Game.Engine
{

	/// <summary>
	/// World element Uid generator interface
	/// Can be used to generate uid for new created elements. 
	/// </summary>
	public interface IElementUidGenerator
	{
		uint Next();
	}
	
	/// <summary>
	/// Describes default world element uid generator
	/// </summary>
	public class ElementUidGenerator : IElementUidGenerator
	{
		private uint _lastId;

		public uint Next() => ++_lastId;
	}

}