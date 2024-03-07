using System;
using System.Collections.Generic;
using Game.Common;

namespace Game.Engine
{

	public class CommandCenter : IExecutorRegistry, IUpdatable
	{
		private Dictionary<Type, ICommandExecutor<ICommand>> _executors = new();
		private ElementUidGenerator _uidGenerator;
		private readonly Queue<ICommand> _commandQueue;
		private readonly List<IReactionsRegistry> _reactionRegistries;

		public CommandCenter(ElementUidGenerator uidGenerator) 
		{
			_uidGenerator = uidGenerator;
			_commandQueue = new ();
			_reactionRegistries = new();
		}

		public void Enqueue(ICommand command)
		{
			_commandQueue.Enqueue(command);
		}

		public void Update()
		{
			// executes all commands accumulated during last frame
			while (_commandQueue.Count > 0) {
				ExecuteCommand(_commandQueue.Dequeue());
			}
		}
		
		public void RegisterExecutor(Type commandType, ICommandExecutor<ICommand> executor)
		{
			_executors.Add(commandType, executor);
		}
		
		public void RegisterExecutor(ICommandExecutor<ICommand> executor)
		{
			var type = executor.GetType();
		
			if (type.BaseType == null) {
				throw new NullReferenceException($"Command executor should have base class.");
			}
		
			if (type.BaseType.GetGenericTypeDefinition() != typeof(CommandExecutor<>)) {
				throw new ArrayTypeMismatchException("Command executor should be derived from 'CommandExecutor<T>' class.");
			}
			
			var commandType = type.BaseType.GetGenericArguments()[0];
			_executors.Add(commandType, executor);
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

		private void ExecuteCommand(ICommand command)
		{
			var type = command.GetType();
			var executor = _executors[type];
			var result = executor.Execute(command);
			
			// flush command result to be processed by reaction system and go outside the engine
			if (TryFindReactionRegistry(result.GetType(), out var registry)) {
				registry.Flush(result);
			}
		}
	}

}