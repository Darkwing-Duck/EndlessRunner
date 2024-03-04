using System;
using System.Linq;

namespace Game.Engine
{
	
	public class Engine
	{
		private readonly IEngineOutputGateway _outputGateway;
		private readonly World _world;
		private readonly CommandCenter _commandCenter;

		public Engine(IEngineOutputGateway outputGateway = null)
		{
			_outputGateway = outputGateway;
			_world = new World();
			_commandCenter = new CommandCenter();

			InitializeCommandCenter();
		}

		private void InitializeCommandCenter()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var assembly in assemblies) {
				var types = assembly.GetTypes()
				                    .Where(t => !t.IsAbstract)
				                    .Where(t => !t.IsInterface)
				                    .Where(t => t.IsSubclassOf(typeof(CommandExecutor<ICommand>)));

				foreach (var type in types) {
					var commandType = type.GetGenericArguments()[0];
					var instance = (CommandExecutor<ICommand>)Activator.CreateInstance(type, new object[] { _world, _commandCenter });
					
					_commandCenter.RegisterExecutor(commandType, instance);
				}
			}
		}

		/// <summary>
		/// The only way to make changes in the world.
		/// Applies changes to the world corresponding to the command
		/// </summary>
		/// <param name="command"></param>
		public void Apply(ICommand command)
		{
			var result = _commandCenter.Execute(command);
			
			if (result.Status == CmdStatus.Failed)
				return;
			
			_outputGateway?.Flush(command);
		}

		/// <summary>
		/// Simulates one frame of the world
		/// </summary>
		public void Update() { }
	}

}