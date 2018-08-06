using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
	// Examples of git commands that can be conveyed through the command models
	// https://services.github.com/on-demand/downloads/github-git-cheat-sheet.pdf
	// http://docopt.org/

	// run "git" from cmd to see example help and usage

	/*
		Model the following commands with annotations and notes:
			git init
			git clone "[url]"
			git pull
		
			git rm "[file]"
			git rm --cached "[file]"
			git mv [file-original] [file-renamed]

			git diff [first-branch]...[second-branch]
			  parsing or recognizing the "..." is not natively supported, so this scenario
			  would require a callback to search for it first.

			git config --global user.name "[name]"
			git config --global user.email "[email address]"
				"config" is the root command
				--global is an option for the config command
				    it must appear before any other subcommands
					formalized: any options of a command must appear before any subcommands
				"user.name" and "user.email" are subcommands, with a require argument afterwards

			
	 */


	class TempCommandExamples
    {
    }
}
