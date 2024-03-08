namespace Game.Engine
{

	public class World
	{
		public readonly ElementsRegistry Elements;

		public World(IElementUidGenerator uidGenerator)
		{
			Elements = new (uidGenerator);
		}
	}

}