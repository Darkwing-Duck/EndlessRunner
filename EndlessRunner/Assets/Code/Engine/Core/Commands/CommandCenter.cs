using System;
using System.Collections.Generic;

namespace Game.Engine
{

	public class CommandCenter
	{
		private Dictionary<Type, ICommandExecutor<ICommand>> _executors = new();
		private ElementUidGenerator _uidGenerator;

		public CommandCenter(ElementUidGenerator uidGenerator) => _uidGenerator = uidGenerator;

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

		public CmdResult Execute(ICommand command)
		{
			var type = command.GetType();
			var executor = _executors[type];

			return executor.Execute(command);
		}
	}

}