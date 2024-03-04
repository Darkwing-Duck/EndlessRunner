namespace Game.Engine
{

	public class Hero : Element
	{
		public Hero(uint id, float speed) : base(id)
		{
			Stats.Register<GameStat.Speed>(speed);
		}
	}

	

}