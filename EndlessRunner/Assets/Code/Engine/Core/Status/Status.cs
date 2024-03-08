namespace Game.Engine.Core
{

	public class Status : WorldEntityWithConfig
	{
		public readonly WorldTimer Timer;
		public bool IsPersistent => Timer.Duration < 0;

		public Status(uint uid, uint configId, int duration) : base(uid, configId)
		{
			Timer = new WorldTimer(duration);
		}
	}

}