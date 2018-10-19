# Command Line Parser for NetStandard

This library provides a clean and simple API for parsing program arguments into a `RunInfo` object.

### Core Features at a Glance

- Supports core command line abstractions such as `Commands`, `Subcommands`, `Arguments`, `Options`, etc.
- Comes with many different `Argument` types to handle common use cases, such as 1-to-1 property mappings, binding values to a sequence, mutually-exclusive sets, etc.
- Comes with a `Parser` that handles the most common system types out of the box. Easily configurable to handle any arbitrary types.
- Provides a cleanly formatted `help` menu by default, with options to configure further.
- Several `hooks` which provide custom extensibility points in various stages of the build process.

### Configuration over Convention

This library prefers _configuration_ over _convention_ (you'll see more further down in the docs). 

There are no attributes used to mark the resulting `RunInfo` class properties. Rather, you configure commands by providing a `Command` object as a representation of an object tree. C# provides a clean way to express nested objects through its object initializers so RunInfoBuilder makes use of that.

Using attributes to _tell_ a command line parser how to interpret things works for very simple binding schemes. But if you need to go beyond that, for example, by using custom callbacks for validations or as extensibility points, using pure code to define the commands works better.

### Getting Started

Install via __NuGet__ or __DotNet__:

```
Install-Package aaa.aaa.aaa
```

```
dotnet add package aaa.aaa.aaa
```

### A Simple Example

A program is desired that can read some message from the `args`, and then do one of many things as determined by a command.

For example, it may take the message and send it off to some HTTP endpoint. The required information for this has been collected into this `RunInfo` class:

```
public class SendRequestRunInfo
{
	public string RequestUrl { get; set; }
	public string Message { get; set; }
	public int DelayMinutes { get; set; }
}
```

The program should take three `args` and simply bind them to the properties. 
To do this, a `Command` called `sendhttp` is added to the _CommandStore_:

```
// initialize a builder instance
var builder = new RunInfoBuilder();

// add the 'sendhttp' command to the store
builder.Commands.Add(new Command<SendRequestRunInfo>
{
	Key = "sendhttp",
	Arguments =
	{
		new PropertyArgument<SendRequestRunInfo, string>
		{
			Property = ri => ri.RequestUrl
		},
		new PropertyArgument<SendRequestRunInfo, string>
		{
			Property = ri => ri.Message
		},
		new PropertyArgument<SendRequestRunInfo, int>
		{
			Property = ri => ri.DelayMinutes
		}
	}
});

// build the run info object by passing args (led by the command's key)
var args = new string[] { "sendhttp", "http://www.somewhere.com", "hello from program!", "3" };
var runInfo = builder.Build(args);
```

The resulting `runInfo` variable will be of type `SendRequestRunInfo` with the expected values:

```
{
	RequestUrl: 'http://www.somewhere.com',
	Message: 'hello from program!',
	DelayMinutes: 3
}
```

This is an overly simple and contrived example. It illustrates the most basic of binding requirements, simple 1-to-1 mappings of args to properties. 

There's a lot more that can be done and configured, but hopefully you can at least see how simple and expressive defining commands through an object is. You can take a quick look at any command configuration and immediately know how it handles and interacts with `args`.

If this has captured your interest, keep reading below for a deeper dive into all the features and areas of RunInfoBuilder.

## In-Depth Documentation

Topics covered below:
- [Command Processing Overview](#command-processing-overview)
- [Commands and the Default Command](#command)

### Command Processing Overview

Before diving into `Command` configuration, we need to understand the order in which commands, and their child items like subcommands and options are processed.

![alt text](/Documentation/Images/command_flow_diagram.png)

#### 1. Arguments

Arguments are processed first, and in the same order they're defined in the `Command` object added to the store.

They are also all required, so the builder will always try to take the next program argument and handle it using the configuration of the next `Argument` in the list.

#### 2. Options

Any `Options` are processed immediately after the command's `Arguments` are, and can appear in any order. They are also.. optional.

`Options` are bound to a property on the `RunInfo`, and its value is determined in one of two ways:

- By parsing the right side of the `=` character in an option program argument: For example, if the program argument is `"--option=value"`, then the string `"value"` will be parsed into the expected type and bound to the property.
- By parsing the next program argument: If an option was declared without the `=`, the builder will simply assume the next program argument is its intended value, and will parse and bind that to the property.

#### 3. SubCommands

A `Command` can contain nested `SubCommands` in a list, which are processed after `Options` (if any are found). 

The structure of a `SubCommand` is exactly the same as the `Command`, and you use the same type in code: `Command<TRunInfo>`.

This results in a `Command` definition being a recursive tree structure, which can be nested arbitrarily deep. However, you'd want to limit the levels of nesting or the program will probably end up with a confusing API.

__To recap: All `Arguments` and `Options` for a given `Command` are processed first, in that order. After which, the matching `SubCommand` will be processed in the same manner. And so on and so forth.__

I know I stated that this library prefers configuration over convention, but I decided that enforcing a specific ordering for processing had more pros than cons. Having these assumptions in place will also help when you're designing your program's API.

_There are still some limitations however_. I'll illustrate by continuing off of the example from earlier. 

Lets imagine the `Command` expects a single `Argument` mapped to the `string` property `Message`. You also define some `Options` as so:

```
Arguments =
{
	new PropertyArgument<SendRequestRunInfo, string>
	{
		Property = ri => ri.Message
	}
},
Options =
{
	// .. some options defined here (you'll learn about these further down) ..
}
```

What happens when a user forgets to include a value for the `Argument` and passes these args:

```
[ "sendhttp", "--some-option" ]
```

Well, the builder has no way to know whether a given program argument is valid for the expected string `Argument`. So it would take the `"--some-option"` value and bind it to the `RunInfo`s `Message` property.

What if the `Argument` was instead mapped to an `int` property? In this case, the build would throw an exception because the string `"--some-option"` is not parseable into an `int` type.

Just be aware of this when designing your program, and let me know if you have any suggestions or ideas on how to circumvent this. It's a common issue for many command line parsing libraries, and might only be solvable through thoughtful design of your program.

_Alright. Now that we understand the order in which items are processed, we'll take a look at the specifics of each core type available, starting with commands.

### Commands and the Default Command

##### Command<TRunInfo>

The command is really the core item, as everything else is nested within it. Its' properties are:

__Key__ (`string`): A unique keyword that represents the command. This only needs to be unique within a given `Command`. For example, both a `Command` and one of its nested `SubCommands` can have the same key.
__Description__ (`string`): Text that's displayed in the help menu.
__Arguments__ (`List<ArgumentBase<TRunInfo>>`): A list of `Arguments` required by the `Command`. Details of the different `Argument` types are discussed later.
__Options__ (`List<OptionBase<TRunInfo>>`): A list of `Options` associated to the `Command`.


There are actually two types of commands. The `Default Command` is pretty much the same thing, but it doesn't have a `key`. It allows you to configure building a default `RunInfo` object, where a user only includes `Arguments` and `Options`.

