using System;
using System.Collections.Generic;

namespace Game.Engine
{

	public class CommandCenter
	{
		private Dictionary<Type, CommandExecutor<ICommand>> _executors = new();

		public void RegisterExecutor(Type commandType, CommandExecutor<ICommand> executor)
		{
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