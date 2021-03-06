namespace ParacletusConsole
{
	public class CommandScript: IConsoleScript
	{
		public void Execute(ConsoleHandler consoleHandler)
		{
			consoleHandler.AddCommand("cd", "<directory>", "change the working directory", consoleHandler.ChangeDirectory, 1);
			consoleHandler.AddCommand("help", null, "prints the help menu", consoleHandler.PrintHelp);
			consoleHandler.AddCommand("clear", null, "clears the console", consoleHandler.ClearConsole);
			consoleHandler.AddCommand("print", "<path to file>", "prints a file to the console", consoleHandler.PrintFile, 1);
			consoleHandler.AddCommand("cp", "<source> <destination>", "copy a file or a directory", consoleHandler.CopyFile, 2);
			consoleHandler.AddCommand("echo", "<strings with colour codes and variables>", "print a formatted string", consoleHandler.Echo, -1);
			consoleHandler.AddCommand("ps", null, "print a list of running processes", consoleHandler.ProcessList);
			consoleHandler.AddCommand("kill", "<PID or name of executable>", "terminate a process", consoleHandler.KillProcess, 1);
			consoleHandler.AddCommand("ls", "<path>", "list the contents of a directory", consoleHandler.ListDirectoryContents, -1);
			consoleHandler.AddCommand("mkdir", "<path>", "create a directory", consoleHandler.CreateDirectory, 1);
			consoleHandler.AddCommand("mv", "<source> <destination>", "move a file/directory", consoleHandler.MoveFile, 2);
			consoleHandler.AddCommand("pwd", null, "print the working directory", consoleHandler.PrintWorkingDirectory);
			consoleHandler.AddCommand("rm", "<files/directories to be removed>", "remove files or directories recursively", consoleHandler.RemoveFile, -1);
			consoleHandler.AddCommand("osinfo", null, "retrieve information about the operating system", consoleHandler.OperatingSystemInformation);
			consoleHandler.AddCommand("alias", "<aliased command and arguments>", "defines an alias for a command", consoleHandler.DefineAlias, -1);
			consoleHandler.AddCommand("unalias", "<alias>", "removes an existing alias", consoleHandler.RemoveAlias, 1);
			consoleHandler.AddCommand("aliases", null, "print a list of the aliases available", consoleHandler.ListAliases);
		}
	}
}
