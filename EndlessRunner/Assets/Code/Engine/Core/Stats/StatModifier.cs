namespace Game.Engine
{

	public abstract class StatModifier : IStatModifier
	{
		protected readonly float ModifierValue;

		public object Group { get; private set; }

		/// <param name="group">Indicates that the modifier is related to the group and can be used to remove all modifiers by the group</param>
		protected StatModifier(float modifierValue, object group = default) {
			ModifierValue = modifierValue;
			Group = group;
		}

		public abstract float Modify(float statValue);

		public class Add : StatModifier
		{
			public Add(float modifierValue, object group = null) : base(modifierValue, group) { }
			public override float Modify(float statValue) => statValue + ModifierValue;
		}
		
		public class Sub : StatModifier
		{
			public Sub(float modifierValue, object group = null) : base(modifierValue, group) { }
			public override float Modify(float statValue) => statValue - ModifierValue;
		}
	}

}