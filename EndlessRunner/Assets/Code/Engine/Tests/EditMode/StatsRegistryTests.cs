using System.Reflection;
using NUnit.Framework;
using Is = UnityEngine.TestTools.Constraints.Is;

namespace Game.Engine.Tests
{

	public class TestStat : Stat
	{
		public TestStat(float baseValue) : base(baseValue) { }
	}

	[TestFixture]
	public class StatsRegistryTests
	{
		[Test]
		public void When_RegisterStat_TheStatShouldExistOnTheElement()
		{
			var baseValue = 100;
			var stats = new StatsRegistry();
			stats.Register<TestStat>(baseValue);

			var exist = stats.TryFind<TestStat>(out var stat);

			Assert.IsTrue(exist);
			Assert.That(stat.BaseValue, Is.EqualTo(baseValue));
		}

		[Test]
		public void When_ApplyModifierAdd_ValueOfStatShouldBecomeGreater()
		{
			var baseValue = 100;
			var modifyByValue = 10;
			var stats = new StatsRegistry();
			var modifier = new StatModifier.Add(modifyByValue);
			var stat = stats.Register<TestStat>(baseValue);

			Assert.That(stat.GetValue(), Is.EqualTo(baseValue));

			// Call internal method 'AddModifier' through the Reflection
			InternalAddModifier(stat, modifier);

			var modifiedValue = baseValue + modifyByValue;
			Assert.That(stat.GetValue(), Is.EqualTo(modifiedValue));
		}
		
		[Test]
		public void When_ApplyModifierSub_ValueOfStatShouldBecomeLess()
		{
			var baseValue = 100;
			var modifyByValue = 10;
			var stats = new StatsRegistry();
			var modifier = new StatModifier.Sub(modifyByValue);
			var stat = stats.Register<TestStat>(baseValue);

			Assert.That(stat.GetValue(), Is.EqualTo(baseValue));

			// Call internal method 'AddModifier' through the Reflection
			InternalAddModifier(stat, modifier);

			var modifiedValue = baseValue - modifyByValue;
			Assert.That(stat.GetValue(), Is.EqualTo(modifiedValue));
		}

		private void InternalAddModifier(Stat stat, IStatModifier modifier)
		{
			var type = typeof(TestStat);
			var methodInfo = type.GetMethod("AddModifier", BindingFlags.Instance | BindingFlags.NonPublic);
			methodInfo!.Invoke(stat, new []{ modifier });
		}
	}

}