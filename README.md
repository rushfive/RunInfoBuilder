# Command Line Parser for NetStandard

[![Build status](https://img.shields.io/appveyor/ci/rushfive/runinfobuilder.svg)](https://ci.appveyor.com/project/rushfive/runinfobuilder)
[![Nuget](https://img.shields.io/nuget/v/r5.runinfobuilder.svg)](https://www.nuget.org/packages/R5.RunInfoBuilder/)

This library provides a clean and simple API for parsing program arguments into a `RunInfo` object.

_Targets NET Standard 2.0_

### Core Features at a Glance

- Supports core command line abstractions such as `Commands`, `Subcommands`, `Arguments`, `Options`, etc.
- Comes with many different `Argument` types to handle common use cases, such as 1-to-1 property mappings, binding values to a sequence, mutually-exclusive sets, etc.
- Comes with a `Parser` that handles the most common system types out of the box. Easily configurable to handle any arbitrary types.
- Provides a cleanly formatted `help` menu by default, with options to configure further.
- Several `hooks` which provide custom extensibility points in various stages of the build process.

### Code Configuration over Attributes

There are no attributes used to mark the resulting `RunInfo` class properties. Rather, you configure commands by providing a `Command` object as a representation of an object tree. C# provides a clean way to express nested objects through its object initializers so RunInfoBuilder makes use of that.

Using attributes to _tell_ a command line parser how to interpret things works for very simple binding schemes. But if you need to go beyond that, for example, by using custom callbacks for validations or as extensibility points, using pure code to define the commands works better.

---

### Getting Started

Install via __NuGet__ or __DotNet__:

```
Install-Package R5.RunInfoBuilder
```

```
dotnet add package R5.RunInfoBuilder
```

---

### A Simple Example

A program is desired that can read some message from the program arguments, and then do one of many things as determined by a command.

For example, it may take the message and send it off to some HTTP endpoint. Also, the user can _optionally_ specify that the request should be retried on fail.

The required information for this has been collected into this `RunInfo` class:

```
public class SendRequestRunInfo
{
    public string RequestUrl { get; set; }
    public string Message { get; set; }
    public int DelayMinutes { get; set; }
    public bool RetryOnFail { set; set; }
}
```

The program should take three program arguments and simply bind them to the properties. 
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
    },
    Options =
    {
        new Option<SendRequestRunInfo, bool>
        {
            Key = "retry | r",
            Property = ri => ri.RetryOnFail
        }
    }
});

// build the run info object by passing program arguments (led by the command's key)
var args = new string[] { "sendhttp", "http://www.somewhere.com", "hello from program!", "3", "--retry" };
var runInfo = builder.Build(args);
```

The resulting `runInfo` variable will be of type `SendRequestRunInfo` with the expected values:

```
{
    RequestUrl: 'http://www.somewhere.com',
    Message: 'hello from program!',
    DelayMinutes: 3,
    RetryOnFail: true
}
```

The values were parsed from the program arguments and bound to the RunInfo properties as configured. Also, the `RetryOnFail` property was set to `true` because the option was specified (`--retry`). The option could also have been specified by `-r` instead because a short key was configured for the option.

This is a very simple example, illustrating the most basic of binding requirements: simple 1-to-1 mappings of program arguments to properties. 

There's a lot more that can be done and configured, but hopefully you can at least see how simple and expressive defining commands through an object is. You can take a quick look at any command configuration and immediately know how it parses the program arguments.

If this has captured your interest, keep reading below for a deeper dive into all the features and areas of RunInfoBuilder.

---

## In-Depth Documentation

Topics covered below:
- [Command Processing Overview](#command-processing-overview)
- [Custom Callbacks](#custom-callbacks)
- [Commands and the Default Command](#commands-and-the-default-command)
  - [Command Store](#command-store)
  - [Command](#command)
  - [Default Command](#default-command)
- [Arguments](#arguments)
  - [Property Argument](#property-argument)
  - [Set Argument](#set-argument)
  - [Custom Argument](#custom-argument)
  - [Sequence Argument](#sequence-argument)
- [Options](#options)
- [Parser](#parser)
- [Hooks](#hooks)
- [Help Menu](#help-menu)
- [Version](#version)


---

### Command Processing Overview

Before diving into `Command` configuration, we need to understand the order in which commands, and their child items like subcommands and options are processed.

![alt text](/Documentation/Images/command_flow_diagram.png)

#### 1. Arguments

Arguments are processed first, and in the same order they're defined in the `Command` object added to the store.

They are also all required, so an exception will be thrown if no more program arguments are found.

#### 2. Options

Any `Options` are processed immediately after the command's `Arguments` are, and can appear in any order. They are optional and any number of them can be specified.

#### 3. SubCommands

A `Command` can contain nested `SubCommands` in a list, which are processed after `Options`. 

A SubCommand is essentially the same type as a Command (just without the `GlobalOptions` property, more on that later). This results in a `Command` definition being a recursive tree structure, which can be nested arbitrarily deep. However, you'd want to limit the levels of nesting or the program will probably end up with a confusing API.

_To recap: All `Arguments` and `Options` for a given `Command` are processed first, in that order. After which, the matching `SubCommand` will be processed in the same manner. And so on and so forth._

#### A Limitation of the Processing Flow

There's a gotcha here due to the order of processing. I'll illustrate by continuing off of the example from earlier. 

Lets imagine the `Command` expects a single `Argument` mapped to the `string` property `Message`. You also define some `Options`:

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
    // .. some options defined here ..
}
```

What happens when a user forgets to include a value for the `Argument` and passes these program arguments:

```
[ "sendhttp", "--some-option" ]
```

Well, the builder has no way to know whether a given program argument is valid for the expected string `Argument`. So it would take the `"--some-option"` value and bind it to the `RunInfo`s `Message` property.

What if the `Argument` was instead mapped to an `int` property? In this case, the build would throw an exception because the string `"--some-option"` is not parseable into an `int` type.

Just be aware of this when designing your program, and let me know if you have any suggestions or ideas on how to circumvent this. It's a common issue for many command line parsing libraries, and might only be solvable through thoughtful design of your program.

_Alright. Now that we understand the order in which items are processed, we'll take a look at the specifics of each core type available, starting with commands.

---

### Custom Callbacks

When configuring `Commands`, there are several places where you can provide custom callbacks. Most of these are `Func`s that must return a `ProcessStageResult`. The type that is returned will determine whether the builder continues processing or stops early.

A static helper class exists with some members that makes returning the correct type easier.

To continue: `return ProcessResult.Continue`.

To end early: `return ProcessResult.End`.

---

### Commands and the Default Command

#### Command Store

All `Commands` are configured on the builder's `CommandStore` object. The store provides two methods with the following interface:

```
CommandStore Add(Command<TRunInfo> command, Action<TRunInfo> postBuildCallback = null);
CommandStore AddDefault(DefaultCommand<TRunInfo> defaultCommand), Action<TRunInfo> postBuildCallback = null;
```

If the optional `postBuildCallback` action is set, it will be called after the program arguments are done processing, receiving the resolved `RunInfo` object as its single argument:

```
builder.Commands.Add(command, runInfo =>
{
	// do something with the resolved runInfo
});
```


#### Command

Type: `Command<TRunInfo>`
- `TRunInfo`parameter is the `RunInfo` class the command is associated to.

The `Command` is really the core entity of this library, as everything else is nested within it.

Properties:
- Key - `string` - A unique keyword that represents the `Command`. This only needs to be unique within a given `Command`. For example, both a `Command` and one of its nested `SubCommands` can have the same key.
- Description - `string` - Text that's displayed in the help menu.
- Arguments - `List<ArgumentBase<TRunInfo>>` - A list of `Arguments` required by the `Command`. Details of the different `Argument` types are discussed later.
- Options - `List<OptionBase<TRunInfo>>` - A list of `Options` associated to the `Command`.
- SubCommands - `List<SubCommand<TRunInfo>>` - A list of `SubCommands`. The `SubCommand<TRunInfo>` type has the same properties as `Command<TRunInfo>`, with the exception of `GlobalOptions`.
- GlobalOptions - `List<OptionBase<TRunInfo>>` - A list of `Options` that are available to the `Command` and any of its nested `SubCommands`.

[![Build Status](https://semaphoreapp.com/api/v1/projects/d4cca506-99be-44d2-b19e-176f36ec8cf1/128505/badge.svg)](https://semaphoreapp.com/boennemann/badges)
[![Build Status](https://semaphoreapp.com/api/v1/projects/d4cca506-99be-44d2-b19e-176f36ec8cf1/128505/badge.svg)](https://semaphoreapp.com/boennemann/badges)
[![start with why](https://img.shields.io/badge/start%20with-why%3F-brightgreen.svg?style=flat)](http://www.ted.com/talks/simon_sinek_how_great_leaders_inspire_action)
[![start with why 222](https://img.shields.io/badge/start%20with-why22%3F-brightgreen.svg?style=flat)](http://www.ted.com/talks/simon_sinek_how_great_leaders_inspire_action)

`Commands` are really nothing more than a container for its child items, which does all the real processing and binding.

An arbitrary number of `Commands` can be added to the store:

```
builder.Commands.Add(new Command<TRunInfo>
{
    Key = "command_key",
    Description = "command description",
    Arguments =
    {
        // ... arguments ...
    },
    Options =
    {
        // ... options ...
    },
    SubCommands =
    {
        // ... subcommands ...
    }
});
```

#### Default Command

Type: `DefaultCommand<TRunInfo>`
- `TRunInfo` is the `RunInfo` class the default command is associated to.

You can optionally include a single `DefaultCommand`. This behaves exactly like a normal `Command`, except that it doesn't include a `Key` or `SubCommands`. It's a simple single-level command that processes only `Arguments` and `Options`.

The idea is to offer default behavior that's simple and lightweight. If your program requires a scenario that doesn't necessarily fit into the group of `SubCommands`, providing this default behavior could be useful.

Only a single `DefaultCommand` can be configured:

```
builder.Commands.AddDefault(new DefaultCommand<TRunInfo>
{
    Description = "default command description",
    Arguments =
    {
        // ... arguments ...
    },
    Options =
    {
        // ... options ...
    }
});
```

---

### Arguments

All `Arguments` are required (matching program arguments must be found). The order in which they're configured is significant: program arguments must also appear in the same order to correctly match an `Argument`.

#### Property Argument

Type: `PropertyArgument<TRunInfo, TProperty>`
- `TRunInfo` is the `RunInfo` class the property is associated to.
- `TProperty` represents the type of the mapped `RunInfo` property.

Property argument's take the next single program argument, then attempts to parse and bind it to the configured `RunInfo` property

_An exception is thrown if the program argument cannot be parsed into a `TProperty` type._

Properties:
- HelpToken - `string` - The text that appears in the help menu representing this `PropertyArgument`. It should be short and succinct. For example, a `HelpToken` could be `"<string>"`, indicating to the user that this `PropertyArgument` binds to a string property.
- Property - `Expression<Func<TRunInfo, TProperty>>` - An expression representing the `RunInfo` property the parsed value will be bound to.
- OnParsed - `Func<TProperty, ProcessStageResult>` - An optional custom callback that is invoked after a valid value has been parsed. The callback will be invoked with that value as its single argument, and return a `ProcessStageResult`. If the callback returns `ProcessResult.End`, processing will stop __before__ the parsed value is bound to the property.
- OnParseErrorUseMessage - `Func<string, string>` - An optional function used to generate the error message on parsing error. The single argument is the program argument that failed to parse.

_Example Configuration:_

```
Arguments =
{
    new PropertyArgument<SendRequestRunInfo, string>
    {
        HelpToken = "<msg>",
        Property = ri => ri.Message,
        OnParsed = value =>
        {
            if (value == "dont send")
            {
                throw new Exception("Shouldn't send!");
            }
            return ProcessResult.Continue;
        },
		OnParseErrorUseMessage = arg => $"Failed to parse program argument '{arg}' because ..."
    }
}
```

#### Set Argument

Type: `SetArgument<TRunInfo, TProperty>`
- `TRunInfo` is the `RunInfo` class the property is associated to.
- `TProperty` represents the type of the mapped `RunInfo` property.

Set arguments provide a list of tuples in the form `(key, boundValue)`. If the program argument matches one of the keys, its paired value will be bound to the `RunInfo` property.

_An exception is thrown if the program argument doesn't match a key._

Properties:
- HelpToken - `string` - The text that appears in the help menu representing this `SetArgument`. It should be short and succinct. For example, a `HelpToken` could be `"(a|b|c)"`, indicating that the acceptable values are "a", "b", and "c".
- Property - `Expression<Func<TRunInfo, TProperty>>` - An expression representing the `RunInfo` property the paired value will be bound to.
- Values - `List<(string, TProperty)>` - List of tuples representing the key and value pairings.

_Example Configuration:_

```
Arguments =
{
    new SetArgument<SendRequestRunInfo, int>
    {
        HelpToken = "(now|one|five)",
        Property = ri => ri.DelayMinutes,
        Values =
        {
            ("now", 0), ("one", 1), ("five", 5)
        }
    }
}
```

In the example above, a value of 0, 1, or 5 will be bound to the `DelayMinutes` property, depending on which key the program argument matched.

#### Custom Argument

Type: `CustomArgument<TRunInfo>`
- `TRunInfo` is the `RunInfo` class the property is associated to.

Custom arguments handle a configurable number of consecutive program arguments through a callback that you provide.

Properties:
- HelpToken - `string` - The token that appears in the help menu representing this custom argument. Example: `"<first> <second> <third>"` 
- Count - `int` - The number of program arguments the callback will handle.
- Handler - `Func<CustomHandlerContext<TRunInfo>, ProcessStageResult>` - The custom callback that will handle the program arguments.

The callback provides a `CustomHandlerContext` with the following properties:
- RunInfo - `TRunInfo` - The RunInfo instance so you can modify it yourself.
- ProgramArguments - `List<string>` - A list containing the program arguments to be handled (as set by the `Count` property).
- Parser - `ArgumentParser` - The same Parser that's configured on the builder.

_Example Configuration:_

```
Arguments =
{
    new CustomArgument<SendRequestRunInfo>
    {
        HelpToken = "<greeting> <name>",
        Count = 2,
        Handler = context =>
        {
            string greeting = context.ProgramArguments[0];
            string name = context.ProgramArguments[1];
            context.RunInfo.Message = $"{greeting} {name}!";
            
            return ProcessResult.Continue;
        }
    }
}
```

In this example, the custom argument will handle two program arguments: the first representing a greeting (eg "hello"), and the second representing the name of the recipient (eg "bob"). The callback simply concatenates the two values to use as the message (`"hello bob!"`).

#### Sequence Argument

Type: `SequenceArgument<TRunInfo, TListProperty>`
- `TRunInfo` is the `RunInfo` class the property is associated to.
- `TListProperty` represents the type of the `List<T>` the parsed values will be added to.

Sequence arguments take consecutive program arguments, parsing and adding them to the configured list.

_Note: the builder will continue to consider program arguments as long as they aren't an `Option` or `SubCommand`. Don't configure other `Arguments` after a `SequenceArgument` - sequences should either be the last or only `Argument` in a command._

_An exception is thrown if any of the considered program arguments fail to parse into a `TListProperty`._

Properties:
- `HelpToken` (`string`) - The token that appears in the help menu representing this `SequenceArgument`. Example: `"<...int>"` .
- `ListProperty` (`Expression<Func<TRunInfo, List<TListProperty>>>`) - An expression representing the `RunInfo` list property the values will be added to.
- `OnParsed` (`Func<TListProperty, ProcessStageResult>`) - An optional custom callback that is invoked for every value after they are parsed. The callback will be invoked with that value as its single argument, and return a `ProcessStageResult`. If the callback returns `ProcessResult.End`, processing will stop __before__ the parsed value is added to the property. 
- `OnParseErrorUseMessage` (`Func<string, string>`) - An optional function used to generate the error message on parsing error. The single argument is the program argument that failed to parse.

_Example Configuration:_

```
Arguments =
{
    new SequenceArgument<RunInfo, int>
    {
        HelpToken = "<...int>",
        ListProperty = ri => ri.ListOfNumbers,
        OnParsed = value =>
        {
            if (value < 10) 
            {
                return ProcessResult.End;
            }
            return ProcessResult.Continue;
        },
		OnParseErrorUseMessage = arg => $"Failed to parse program argument '{arg}' because ..."
    }
}
```

In the example above, the builder will continue to parse and add program arguments as ints. However, if the parsed int value is less than 10, it will stop further processing.

---

### Options

Options allow you to setup optional 1-to-1 bindings to a property on the `RunInfo`. The user specifies an option using the standard `--option` (full) or `-o` (short) syntax. 

Multiple `bool` options can be _stacked_ using the short syntax by combining their single character short keys. Re-emphasis on the `bool` constraint: stacking short options are not allowed on any other types.

_`Options` can appear in any order in the `Command` configuration, unlike `Arguments` where order matters (because they're all required)._

`Options` are bound to a property on the `RunInfo`, and its value is determined in one of two ways:

- By parsing the right side of the `=` character in an option program argument: For example, if the program argument is `"--option=value"`, then the string `"value"` will be parsed into the expected type and bound to the property.
- By parsing the next program argument: If an option was declared without the `=`, the builder will simply assume the next program argument is its intended value, and will parse and bind that to the property.

Type: `Option<TRunInfo, TProperty>`
- `TRunInfo` is the `RunInfo` class the option is associated to.
- `TProperty` represents the type of the property the parsed option value will be bound to.

Properties:
- `Key` (`string`) - A string representing the option key. For example, if it's set as `"hide"`, it would be called as `"--hide"` in a program argument. You can optionally set a short key by delimiting the string with a `|` character. If the key is set to `"hide | h"`, then you could use this option with either `"--hide"` or `"-h"`.
- `Property` (`Expression<Func<TRunInfo, TProperty>>`) - An expression representing the `RunInfo` property the parsed value will be bound to.
- `HelpToken` (`string`) - The token that appears in the help menu representing this option. Example: `"[--hide|-h]"`.
- `OnParseErrorUseMessage` (`Func<string, string>`) - An optional function used to generate the error message on parsing error. The single argument is the program argument (representing option's value) that failed to parse.

```
Options =
{
    new Option<RunInfo, int>
    {
        Key = "minutes | m"
        HelpToken = "[--minutes|-m]",
        Property = ri => ri.DelayMinutes,
        OnParsed = value =>
        {
            if (value < 10) 
            {
                return ProcessResult.End;
            }
            return ProcessResult.Continue;
        },
		OnParseErrorUseMessage = arg => $"Failed to parse program argument '{arg}' because ..."
    }
}
```

In the example above, the user can set an `int` value for the `DelayMinutes` property on the `RunInfo` object through these program argument(s):
- `"--minutes=5"`
- `"--minutes", "5"`
- `"-m=5"`
- `"-m", "5"`

Both _full_ and _short_ keys must be unique within a given `Command`. This means that a command and its subcommand can have options that share the same option keys.

---

### Parser

The parsing of program arguments into the configured `Types` is handled by the `ArgumentParser`, available as a property on the `RunInfoBuilder` object and in some callback contexts.

The following types are automatically handled out-of-the-box, using the standard `TYPE.TryParse`  methods:
- string
- bool
- byte
- char
- DateTime
- decimal
- double
- int

Configuring the parser is easy, the following methods are available:

__`ArgumentParser EnumParsingIgnoresCase()`__

The parser automatically handles enum types. By default, it does a _case-sensitive_ comparison of the program argument to the enum values. Calling this method will set all future comparisons to be _case-insensitive_.

__`ArgumentParser SetPredicateForType<T>(Func<string, (bool isValid, T parsed)> predicateFunc)`__

This method allows you to extend the parser to handle additional types (or re-configure how an already-handled `Type` should be parsed).

You provide a `Func` that takes in the program argument as its single argument, and it returns a `ValueTuple` where the first item represents whether the parsing was successful, and the second item being the parsed object. The custom predicates set using this method are used internally, and the second item (value) in the tuple is _always_ ignored if the parsing failed.

__`bool TryParseAs(Type type, string value, out object parsed)`__

Attempts to parse the value as the specified type. The method returns a `bool` indicating a successful parse, with the parsed object being returned as an `out` parameter.

__`bool TryParseAs<T>(string value, out T parsed)`__

The same as above, but the `Type` is specified generically.

__`bool HandlesType(Type type)`__

Returns a `bool`, indicating whether the parser handles the given `Type`.

__`HandlesType<T>()`__

The same as above, but the `Type` is specified generically.

---

### Hooks

The builder provides hooks (currently only one) to invoke custom functionality at different phases of the build process. 

Setting these hooks is done on the `BuildHooks` object, found as the property `Hooks` on the `RunInfoBuilder` class.

The following methods are available:

__`BuildHooks SetOnStartBuild(Action<string[]> onStartCallback)`__

Set a callback that receives the program arguments as it's single argument. This is invoked as the very first thing, immediately after the builder's `Build(args)` method is called.

_Hooks are kind of an experimental thing for now, I can't gauge how useful it really is. Mainly because I'd like this library to focus primarily on parsing program arguments (drawing a boundary against having this library run a lot of app specific code)._

---

### Help Menu

Providing a help menu is essential to any program with a CLI, so this library provides some nice default behavior in that area.

The help menu is configured on the `HelpManager` object, found as the property `Help` on the `RunInfoBuilder` class. 

Here's an example of a help menu that's displayed by default (taken from the _GitCloneSample_ project):

```
add - Add file contents to the index.
  Usage: git add [--dry-run|-n] [--verbose|-v] [--ignore-errors] [--refresh]

branch - List, create, or delete branches.
  Usage: git branch <branchname> [--delete|-d] [--force|-f] [--ignore-case|-i]

checkout - Switch branches or restore working tree files.
  Usage: git checkout <branch> [--quiet|-q] [--force|-f] [--track|-t]

commit - Stores the current contents of the index in a new commit along with a log message from the user describing the changes.
  Usage: git commit [--all|-a] [--patch|-p] [--branch]

diff - Switch branches or restore working tree files.
  Usage: git diff [first-branch]...[second-branch] [--no-patch|-s] [--raw] [--minimal]

init - Create an empty Git repository or reinitialize an existing one.
  Usage: git init [--quiet|-q] [--bare]
```

The following methods are available:

__`HelpManager SetProgramName(string name)`__

Sets the name of your program. This will most likely be the name of your program's executable file. For example, if the file is `git.exe`, you'd want to set your program name as `git`. If set, the name is displayed in the help menu.

__`HelpManager SetTriggers(params string[] triggers)`__

Sets the triggers that will display the help menu. 

The help menu displays if the very first argument matches one of the triggers. By default, the following triggers are set:

`"--help", "-h", "/help"`

You can replace the default triggers by passing in a comma-separated list of strings:

```
builder.Help.SetTriggers("--?", "/h");
```

__`HelpManager DisplayOnBuildFail()`__

By default, the help menu is only displayed if explicitly called by the user's program argument. This method will configure the builder to automatically display the help on _any_ exceptions thrown.

_Note: this also suppresses any exceptions from bubbling up to the client._

__`HelpManager OnTrigger(Action customCallback)`__

If you need something other than the default help menu, you can set your own callback that is invoked instead. The callback is simple an `Action`, so you'll need to handle everything including the displaying of help text.

```
builder.Help.OnTrigger(() => {
    Console.WriteLine("my custom help menu.");
});
```

---

### Version

Users should be able to quickly note the version of the program. The setup is similar to that of the help menu, and default behavior is provided as well.

The version is configured on the `VersionManager` object, found as the property `Version` on the `RunInfoBuilder` class. 

The following methods are available:

__`VersionManager Set(string version)`__

Set the version string that is displayed. The default is `"1.0.0"`.

__`VersionManager SetTriggers(params string[] triggers)`__

Sets the triggers that will display the version. 

The version displays if the very first argument matches one of the triggers. By default, the following triggers are set:

`"--version", "-v", "/version"`

You can replace the default triggers by passing in a comma-separated list of strings:

```
builder.Version.SetTriggers("--v", "/v");
```