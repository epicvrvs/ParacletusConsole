﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace ParacletusConsole
{
	public partial class ConsoleHandler
	{
		void PrintPrompt()
		{
			UpdateWorkingDirectory();
			FormattedPrinting(ProgramConfiguration.Prompt);
		}

		const int WM_VSCROLL = 0x115;
		const int SB_BOTTOM = 7;

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr window, int message, int wparam, int lparam);

		public void Print(string text)
		{
			MainForm.ConsoleBox.Invoke
			(
				(MethodInvoker)delegate
				{
					MainForm.ConsoleBox.AppendText(text);
					SendMessage(MainForm.ConsoleBox.Handle, WM_VSCROLL, SB_BOTTOM, 0);
				}
			);
		}

		public void PrintLine(string line)
		{
			Print(line + "\n");
		}

		public void PrintWithColour(string line, SerialisableColour colour)
		{
			SetOutputColour(colour.ToColour());
			Print(line);
			SetOutputColour(ProgramConfiguration.DefaultOutputColour.ToColour());
		}

		public void PrintLineWithColour(string line, SerialisableColour colour)
		{
			SetOutputColour(colour.ToColour());
			PrintLine(line);
			SetOutputColour(ProgramConfiguration.DefaultOutputColour.ToColour());
		}

		public void PrintWarning(string warningLine)
		{
			PrintLineWithColour(warningLine, ProgramConfiguration.WarningColour);
		}

		public void PrintError(string errorLine)
		{
			PrintLineWithColour(errorLine, ProgramConfiguration.ErrorColour);
		}

		public void FormattedPrinting(string input, bool useVariables = true, bool useColours = true)
		{
			string currentOutput = "";

			for (int i = 0; i < input.Length; )
			{
				char currentChar = input[i];
				if (useColours && currentChar == ColourIdentifier)
				{
					int offset = i + 1;
					if (offset + HexCodeTotalSize >= input.Length)
					{
						//actually this is an error since the user specified an incomplete colour code, but just keep on printing
						currentOutput += currentChar;
					}
					else
					{
						Print(currentOutput);
						currentOutput = "";
						string hexString = input.Substring(offset, HexCodeTotalSize);
						i = offset + HexCodeTotalSize;
						try
						{
							Color colour = ConvertHexStringToColour(hexString);
							SetOutputColour(colour);
						}
						catch (FormatException)
						{
							//error, the user has specified an invalid hex string, let's just print some garbage instead
							currentOutput += ColourIdentifier + "InvalidColour";
						}
						continue;
					}
				}
				else if (useVariables && currentChar == VariableSeparator)
				{
					int start = i + 1;
					int offset = input.IndexOf(VariableSeparator, start);
					if (offset == -1)
					{
						//error: the user failed to specify a terminating separator, just print it anyways
						currentOutput += currentChar;
					}
					else
					{
						string variable = input.Substring(start, offset - start);
						i = offset + 1;
						if (variable.Length == 0)
						{
							//it's a double separator - print the separator instead
							currentOutput += VariableSeparator;
							continue;
						}
						if (VariableDictionary.ContainsKey(variable))
							currentOutput += VariableDictionary[variable];
						else
							//error: the user has specified an invalid variable name, just append a warning string instead
							currentOutput += "Unknown";
						continue;
					}
				}
				else
					currentOutput += currentChar;
				i++;
			}
			Print(currentOutput);
		}

		public void PrintCommandString(string line)
		{
			int spaceOffset = line.IndexOf(' ');
			if (spaceOffset == -1)
				spaceOffset = line.Length;
			string command = line.Substring(0, spaceOffset);
			string argumentString = line.Substring(spaceOffset);
			SetOutputColour(ProgramConfiguration.CommandTextOutputHighlightColour.ToColour());
			Print(command);
			SetOutputColour(ProgramConfiguration.CommandTextOutputColour.ToColour());
			PrintLine(argumentString);
			SetOutputColour(ProgramConfiguration.DefaultOutputColour.ToColour());
		}

		void PrintBuffer(byte[] buffer, int bytesRead)
		{
			string data = System.Text.UTF8Encoding.UTF8.GetString(buffer, 0, bytesRead);
			Print(data);
		}

		public void PrintTable(ColouredString[][] table)
		{
			const int padding = 4;
			List<int> columnWidths = new List<int>();
			int columnCount = table.First().Length;
			for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
			{
				int currentWidth = 0;
				for (int rowIndex = 0; rowIndex < table.Length; rowIndex++)
				{
					ColouredString[] row = table[rowIndex];
					int width = row[columnIndex].Content.Length;
					currentWidth = Math.Max(currentWidth, width);
				}
				if (columnIndex != columnCount - 1)
					currentWidth += padding;
				columnWidths.Add(currentWidth);
			}

			for (int rowIndex = 0; rowIndex < table.Length; rowIndex++)
			{
				for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
				{
					int width = columnWidths[columnIndex];
					ColouredString field = table[rowIndex][columnIndex];
					string fieldContent = field.Content;
					fieldContent += new string(' ', width - fieldContent.Length);
					if (field.Colour == null)
						Print(fieldContent);
					else
						PrintWithColour(fieldContent, field.Colour);
				}
				PrintLine("");
			}
		}
	}
}
