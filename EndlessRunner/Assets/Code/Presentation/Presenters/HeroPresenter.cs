using Game.Configs;
using Game.Engine;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Presentation
{

	public class HeroPresenter : ElementPresenter<HeroConfig, Hero, HeroView>
		, IListener<AddStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>
		, IListener<RemoveStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>
	{
		protected override string InitializeViewGroup() => "Hero";
		
		public HeroPresenter(Hero model) : base(model) { }

		/// <summary>
		/// Sets LevelPresenter's view as a parent for hero
		/// </summary>
		protected override Transform InitializeViewParent() => 
			Root.FindSinglePresenter<LevelPresenter>().View.ElementsContainer;

		/// <summary>
		/// Cache config of the hero
		/// </summary>
		protected override HeroConfig InitializeConfig() => 
			Configs.Heroes.Get(Model.ConfigId);

		protected override void OnActivate()
		{
			View.name = $"Hero<{Model.Uid}>";
			
			var speedStat = Model.Stats.Find<GameStat.Speed>();
			View.SetSpeed(speedStat.GetValue());

			View.OnCollideWith += OnCollideWith;
		}

		/// <summary>
		/// Calls when hero collides with any of element.
		/// Pushes HeroCollectItemCommand to engine
		/// </summary>
		private void OnCollideWith(ElementUidRef elementUid)
		{
			EngineInput.Push(new HeroCollectItemCommand(Model.Uid, elementUid.Value));
		}

		protected override void OnDeactivate()
		{
			View.OnCollideWith -= OnCollideWith;
		}
		
		// Syncs hero speed from model to view
		private void UpdateSpeed()
		{
			var stat = Model.Stats.Find<GameStat.Speed>();
			View.SetSpeed(stat.GetValue());
		}

		/// <summary>
		/// React on add speed stat modifier
		/// </summary>
		public void On(AddStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed> cmdResult)
		{
			// check if the modification related to this hero
			if (cmdResult.TargetUid != Model.Uid)
				return;
			
			UpdateSpeed();
		}

		/// <summary>
		/// React on remove speed stat modifier
		/// </summary>
		public void On(RemoveStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed> cmdResult)
		{
			if (cmdResult.TargetUid != Model.Uid)
				return;
			
			UpdateSpeed();
		}
	}

}