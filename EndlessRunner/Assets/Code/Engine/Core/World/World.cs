using Game.Common;

namespace Game.Engine
{

	/// <summary>
	/// Root of the game state.
	/// </summary>
	public class World
	{
		public readonly ElementsRegistry Elements;

		public World(IElementUidGenerator uidGenerator)
		{
			Elements = new (uidGenerator);
		}

		public void Update()
		{
			Elements.Update();
		}
	}

}