namespace Game.Engine
{

	public class WorldEntityWithConfig : WorldEntity
	{
		public readonly uint ConfigId;
		public WorldEntityWithConfig(uint uid, uint configId) : base(uid) => ConfigId = configId;
	}

}