namespace Game.Engine.Core
{

	/// <summary>
	/// Describes a status applied to element.
	/// And timer update logic
	/// </summary>
	public class Status : WorldEntityWithConfig
	{
		internal readonly WorldTimer Timer;
		public bool IsPersistent => Timer.Duration < 0;

		public Status(uint uid, uint configId, int duration) : base(uid, configId)
		{
			Timer = new WorldTimer(duration);
		}

		internal override void Update()
		{
			base.Update();
			Timer.Update();
		}
	}

}