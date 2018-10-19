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


## In-Depth Documentation

Topics covered below:
- [Commands](#commands)

asdfasdf
asdfasdfasdf
asdfasdfsfd
asdfasdf
asdfasdfasdf
asdfasdfsfd
asdfasdf
asdfasdfasdf
asdfasdfsfd
asdfasdf
asdfasdfasdf
asdfasdfsfd
asdfasdf
asdfasdfasdf
asdfasdfsfd
asdfasdf
asdfasdfasdf
asdfasdfsfd
asdfasdf
asdfasdfasdf
asdfasdfsfd
asdfasdf
asdfasdfasdf
asdfasdfsfd


### Commands