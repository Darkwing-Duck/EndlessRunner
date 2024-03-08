using System;
using System.Linq;
using Game.Common;
using Game.Configs;
using Utils;

namespace Game.Engine
{
	
	public class GameEngine : IEngineInput, IExecutorRegistry, IUpdatable
	{
		public readonly World World;
		
		private readonly CommandCenter _commandCenter;
		private readonly ConfigsRegistry _configsRegistry;

		public GameEngine(ConfigsRegistry configsRegistry)
		{
			World = new World(new ElementUidGenerator());
			_commandCenter = new CommandCenter();
			_configsRegistry = configsRegistry;

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
			_commandCenter.RegisterExecutor(new AddStatModifierCommand<GameStat.Speed>.Executor(World, _commandCenter, _configsRegistry));
			_commandCenter.RegisterExecutor(new RemoveStatModifierCommand<GameStat.Speed>.Executor(World, _commandCenter, _configsRegistry));
		}

		/// <summary>
		/// Collects and registers all the simple executors automatically through the reflection 
		/// </summary>
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
					
					var instance = (ICommandExecutor<ICommand>)Activator.CreateInstance(type, new object[] { World, _commandCenter, _configsRegistry });
					_commandCenter.RegisterExecutor(commandType, instance);
				}
			}
		}

		/// <summary>
		/// The only way to make changes in the world.
		/// Pushes command to command queue to be processed on Update
		/// </summary>
		/// <param name="command"></param>
		public void Push(ICommand command)
		{
			_commandCenter.Enqueue(command);
		}

		/// <summary>
		/// Simulates one frame of the world
		/// </summary>
		public void Update()
		{
			UpdateStatuses();
			_commandCenter.Update();
		}

		public void RegisterReaction<T>(IEngineReactionOn<T> reaction) where T : CmdResult =>
			_commandCenter.RegisterReaction(reaction);

		public void UnregisterReaction<T>(IEngineReactionOn<T> reaction) where T : CmdResult =>
			_commandCenter.UnregisterReaction(reaction);

		private void UpdateStatuses()
		{
			World.Elements.ForEach(element => {
				foreach (var status in element.Statuses.GetAll()) {
					if (status.IsPersistent)
						continue;
					
					status.Timer.Update();

					if (status.Timer.IsFinished) {
						_commandCenter.Enqueue(new RemoveStatusCommand(element.Uid, status.Uid));
					}
				}
			});
		}
	}

}