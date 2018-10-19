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
- [Commands](#commands)


### Commands

Before diving into `Command` configuration, we need to understand the order in which commands, and their child items like subcommands and options are processed.

![alt text](/Documentation/Images/command_flow_diagram.png)