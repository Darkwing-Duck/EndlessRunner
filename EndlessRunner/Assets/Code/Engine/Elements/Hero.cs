namespace Game.Engine
{

	public class Hero : Element
	{
		public Hero(uint uid, uint configId, float speed) : base(uid, configId)
		{
			Stats.Register<GameStat.Speed>(speed);
		}
	}

	

}