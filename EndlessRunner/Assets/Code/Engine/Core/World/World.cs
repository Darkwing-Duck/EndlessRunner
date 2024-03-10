using Game.Common;

namespace Game.Engine
{

	public class World : IUpdatable
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