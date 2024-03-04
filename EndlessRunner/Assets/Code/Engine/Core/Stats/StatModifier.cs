namespace Game.Engine
{

	public abstract class StatModifier : IStatModifier
	{
		protected readonly float ModifierValue;

		protected StatModifier(float modifierValue) => ModifierValue = modifierValue;

		public abstract float Modify(float statValue);

		public class Add : StatModifier
		{
			public Add(float modifierValue) : base(modifierValue) { }
			public override float Modify(float statValue) => statValue + ModifierValue;
		}
		
		public class Sub : StatModifier
		{
			public Sub(float modifierValue) : base(modifierValue) { }
			public override float Modify(float statValue) => statValue - ModifierValue;
		}
	}

}