using System;
using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Utils;

namespace Game.Engine
{
	
	public class GameEngine : IUpdatable
	{
		public readonly World World;
		
		private readonly List<IReactionsRegistry> _reactionRegistries;
		private readonly CommandCenter _commandCenter;
		private readonly ElementUidGenerator _uidGenerator;

		public GameEngine()
		{
			World = new World(new ElementUidGenerator());
			_reactionRegistries = new();
			_uidGenerator = new ElementUidGenerator();
			_commandCenter = new CommandCenter(_uidGenerator);

			InitializeCommandCenter();
		}

		private void InitializeCommandCenter()
		{
			RegisterSimpleExecutors();
			RegisterComplicatedExecutors();
		}

		/// <summary>
		/// Because we have generic commands like AddStatModifierCommand<T> we have to create executors for the command for each Stat.
		/// For example:
		/// AddStatModifierCommand<GameStat.Speed>
		/// AddStatModifierCommand<GameStat.Health>
		/// AddStatModifierCommand<GameStat.Shield>
		///
		/// Of course we can do it also automatically through the reflection, but to simplify the project I don't do it here.
		/// </summary>
		private void RegisterComplicatedExecutors()
		{
			// register stat modifiers executors
			_commandCenter.RegisterExecutor(new AddStatModifierCommand<GameStat.Speed>.Executor(World, _commandCenter));
			_commandCenter.RegisterExecutor(new RemoveStatModifierCommand<GameStat.Speed>.Executor(World, _commandCenter));
		}

		private void RegisterSimpleExecutors()
		{
			var assemblies = AppDomain.CurrentDomain.GetUserDefinedAssemblies();

			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes()
				                    .Where(t => !t.IsAbstract)
				                    .Where(t => !t.IsInterface)
				                    .Where(t => t.BaseType != null)
				                    .Where(t => t.BaseType.IsGenericType)
				                    .Where(t => t.BaseType.GetGenericTypeDefinition() == typeof(CommandExecutor<>));

				foreach (var type in types) {
					var commandType = type.BaseType!.GetGenericArguments()[0];
					
					if (commandType.IsGenericType) {
						// Here we can also automatically register executors for generic commands like
						// AddStatModifierCommand<GameStat.Speed>
						// RemoveStatModifierCommand<GameStat.Speed>
						// But to simplify the code we register them manually
						continue;
					}
					
					var instance = (ICommandExecutor<ICommand>)Activator.CreateInstance(type, new object[] { World, _commandCenter });
					_commandCenter.RegisterExecutor(commandType, instance);
				}
			}
		}

		/// <summary>
		/// The only way to make changes in the world.
		/// Applies changes to the world corresponding to the command
		/// </summary>
		/// <param name="command"></param>
		public void Push(ICommand command)
		{
			var result = _commandCenter.Execute(command);
			
			if (TryFindReactionRegistry(result.GetType(), out var registry)) {
				registry.Flush(result);
			}
		}

		public void RegisterReaction<T>(IEngineReactionOn<T> reaction) where T : CmdResult
		{
			if (!TryFindReactionRegistry<T>(out var registry)) {
				registry = new ReactionsRegistry<T>();
				_reactionRegistries.Add(registry);
			}
			
			registry.Add(reaction);
		}
		
		public void UnregisterReaction<T>(IEngineReactionOn<T> reaction) where T : CmdResult
		{
			if (!TryFindReactionRegistry<T>(out var registry)) {
				throw new NullReferenceException($"Can't find reaction for command result '{typeof(T).Name}'");
			}
			
			registry.Remove(reaction);
		}

		private bool TryFindReactionRegistry<T>(out ReactionsRegistry<T> result) where T : CmdResult
		{
			foreach (var registry in _reactionRegistries) {
				if (registry is not ReactionsRegistry<T> casted)
					continue;

				result = casted;
				return true;
			}

			result = null;
			return false;
		}
		
		private bool TryFindReactionRegistry(Type cmdResultType, out IReactionsRegistry result)
		{
			foreach (var registry in _reactionRegistries) {
				var type = registry.GetType();
				var genericType = type.GetGenericArguments()[0];
				
				if (genericType != cmdResultType)
					continue;

				result = registry;
				return true;
			}

			result = null;
			return false;
		}

		/// <summary>
		/// Simulates one frame of the world
		/// </summary>
		public void Update() { }
	}

}