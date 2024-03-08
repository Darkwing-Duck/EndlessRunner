using Game.Common;

namespace Game.Engine.Core
{

	public class WorldTimer : IUpdatable
	{
		public int Duration { get; private set; }
		public int Elapsed { get; private set; }
		public int Left => Duration - Elapsed;
		public bool IsFinished => Elapsed >= Duration;

		public WorldTimer(int duration) => Duration = duration;

		public void Update()
		{
			if (IsFinished) {
				return;
			}
			
			Elapsed++;
		}

		public void ChangeDuration(int to)
		{
			Duration = to;
		}

		public void Reset()
		{
			Elapsed = 0;
		}
	}

}