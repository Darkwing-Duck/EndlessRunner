namespace Game.Engine
{

	public class WorldEntity
	{
		public readonly uint Uid;
		public WorldEntity(uint uid) => Uid = uid;

		internal virtual void Update() { }
	}

}