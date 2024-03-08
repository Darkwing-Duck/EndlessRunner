using System.Collections.Generic;
using Game.Common;
using Game.Configs;
using Game.Engine;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Presentation
{

	public class HeroPresenter : ElementPresenter<HeroConfig, Hero, HeroView>
		, IUpdatable
		, IListener<AddStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>
		, IListener<RemoveStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>
		, IListener<SetHeroStateCommand.Result>
		, IListener<RemoveStatusCommand.Result>
		, IListener<HeroJumpCommand.Result>
	{
		private Dictionary<HeroStateType, HeroState> _statesMap;
		private HeroState _heroState;
		
		protected override string InitializeViewGroup() => "Hero";

		public HeroPresenter(Hero model) : base(model)
		{
			// initialize states map
			_statesMap = new Dictionary<HeroStateType, HeroState> {
				{ HeroStateType.Run, new RunningState(this) },
				{ HeroStateType.SuperFly, new SuperFlyState(this) }
			};
		}

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

			// set initial state
			_heroState = _statesMap[Model.State];
			
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

		public void Update()
		{
			_heroState.Update();
		}

		private void ChangeStateTo(HeroStateType value)
		{
			if (_heroState == _statesMap[value]) 
				return;
			
			_heroState.OnDeactivate(() => {
				_heroState = _statesMap[value];
				_heroState.OnActivate();
			});
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

		/// <summary>
		/// Reacts on hero state changed.
		/// </summary>
		public void On(SetHeroStateCommand.Result cmdResult)
		{
			if (cmdResult.Status == CmdStatus.Failed)
				return;
			
			if (cmdResult.TargetUid != Model.Uid)
				return;
			
			if (!_statesMap.ContainsKey(cmdResult.State))
				return;

			ChangeStateTo(cmdResult.State);
		}

		public void On(RemoveStatusCommand.Result cmdResult)
		{
			if (cmdResult.Status == CmdStatus.Failed)
				return;

			var statusConfig = Configs.Statuses.Get(cmdResult.RemovedStatusConfigId);

			// change state back to run only if SuperFly status is finished
			if (IsSuperFlyStatus(statusConfig)) {
				ChangeStateTo(HeroStateType.Run);
			}
		}

		private bool IsSuperFlyStatus(StatusConfig config)
		{
			foreach (var effect in config.Effects) {
				if (effect.Action == EffectAction.SetState && effect.StateValue == HeroStateType.SuperFly) {
					return true;
				}
			}

			return false;
		}

		public void On(HeroJumpCommand.Result cmdResult)
		{
			if (cmdResult.Status == CmdStatus.Failed)
				return;
			
			if (cmdResult.HeroUid != Model.Uid)
				return;

			View.Jump();
		}
	}

}