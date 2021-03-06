﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParacletusConsole
{
	public class CommandHandler: IComparable<CommandHandler>
	{
		public string Command;
		public string Description;
		public string ArgumentDescription;
		public ConsoleHandler.CommandHandlerFunction Function;
		public int ArgumentCount;

		public CommandHandler(string command, string argumentDescription,  string description, ConsoleHandler.CommandHandlerFunction function, int argumentCount)
		{
			this.Command = command;
			this.ArgumentDescription = argumentDescription;
			this.Description = description;
			this.Function = function;
			this.ArgumentCount = argumentCount;
		}

		public string Usage()
		{
			string argumentDescriptionString = "";
			if (ArgumentDescription != null)
				argumentDescriptionString = " " + ArgumentDescription;
			return Command + argumentDescriptionString + " - " + Description;
		}

		public int CompareTo(CommandHandler other)
		{
			return Command.CompareTo(other.Command);
		}
	}
}
