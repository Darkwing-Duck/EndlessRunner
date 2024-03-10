using System.Collections.Generic;
using Game.Common;
using Game.Configs;
using Game.Engine;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Presentation
{

	/// <summary>
	/// Displays hero.
	/// </summary>
	public class HeroPresenter : ElementPresenter<HeroConfig, Hero, HeroView>
		, IUpdatable
		, IListener<AddStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>
		, IListener<RemoveStatModifierCommand<GameStat.Speed>.Result<GameStat.Speed>>
		, IListener<SetHeroStateCommand.Result>
		, IListener<RemoveStatusCommand.Result>
		, IListener<HeroJumpCommand.Result>
		, IListener<HeroLandCommand.Result>
	{
		private Dictionary<HeroStateType, HeroState> _statesMap;
		
		// current hero visual state
		private HeroState _heroState;
		
		protected override string InitializeViewGroup() => "Hero";

		public HeroPresenter(Hero model) : base(model)
		{
			// initialize states map
			_statesMap = new Dictionary<HeroStateType, HeroState> {
				{ HeroStateType.Run, new RunningState(this) },
				{ HeroStateType.SuperFly, new SuperFlyState(this) },
				{ HeroStateType.Jump, new JumpState(this) }
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
			
			// set speed depend on speed stat
			var speedStat = Model.Stats.Find<GameStat.Speed>();
			View.SetSpeed(speedStat.GetValue());

			// add event handlers to listen when hero collides with collectibles
			// and when is landed to ground
			View.OnCollideWith += OnCollideWith;
			View.OnLanded += OnLanded;
		}

		/// <summary>
		/// Calls when hero collides with any of element.
		/// Pushes HeroCollectItemCommand to engine
		/// </summary>
		private void OnCollideWith(ElementUidRef elementUid)
		{
			EngineInput.Push(new HeroCollectItemCommand(Model.Uid, elementUid.Value));
		}
		
		// On hero view says that hero is landed
		// we need to push land command to the engine to notify it about the landing
		private void OnLanded()
		{
			EngineInput.Push(new HeroLandCommand(Model.Uid));
		}

		protected override void OnDeactivate()
		{
			View.OnCollideWith -= OnCollideWith;
			View.OnLanded -= OnLanded;
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

		/// <summary>
		/// Changes hero visual state
		/// </summary>
		private void ChangeStateTo(HeroStateType value)
		{
			if (_heroState == _statesMap[value]) 
				return;
			
			_heroState.OnDeactivate(() => {
				_heroState = _statesMap[value];
				_heroState.OnActivate();
			});
		}
		
		// Checks if the status config has setState(SuperFly) effect
		private bool IsSuperFlyStatus(StatusConfig config)
		{
			foreach (var effect in config.Effects) {
				if (effect.Action == EffectAction.SetState && effect.StateValue == HeroStateType.SuperFly) {
					return true;
				}
			}

			return false;
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

		/// <summary>
		/// Reacts on remove status from the hero
		/// </summary>
		/// <param name="cmdResult"></param>
		public void On(RemoveStatusCommand.Result cmdResult)
		{
			if (cmdResult.Status == CmdStatus.Failed)
				return;
			
			if (cmdResult.TargetUid != Model.Uid)
				return;

			var statusConfig = Configs.Statuses.Get(cmdResult.RemovedStatusConfigId);

			// change state back to run only if SuperFly status is finished
			if (IsSuperFlyStatus(statusConfig)) {
				ChangeStateTo(HeroStateType.Run);
			}
		}

		/// <summary>
		/// Reacts on jump.
		/// </summary>
		public void On(HeroJumpCommand.Result cmdResult)
		{
			if (cmdResult.Status == CmdStatus.Failed)
				return;
			
			if (cmdResult.HeroUid != Model.Uid)
				return;

			ChangeStateTo(HeroStateType.Jump);
		}

		/// <summary>
		/// Reacts on land to the ground.
		/// </summary>
		public void On(HeroLandCommand.Result cmdResult)
		{
			if (cmdResult.Status == CmdStatus.Failed)
				return;
			
			if (cmdResult.HeroUid != Model.Uid)
				return;
			
			ChangeStateTo(HeroStateType.Run);
		}
	}

}