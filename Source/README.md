# Command Line Parser for NetStandard

This library provides a clean and simple API for parsing program arguments into a run information (RunInfo) object.

## Core Features at a Glance:

- Supports core command line abstractions such as `Commands`, `Subcommands`, `Arguments`, `Options`, etc.
- Comes with many different Argument types to handle common use cases, such as 1-to-1 property mappings, binding values to a sequence, choosing a value from a set, etc.
- Comes with a __Parser__ that handles the most common system Types out of the box. Easily configurable to handle any arbitrary Types.
- Provides a cleanly formatted `help` menu by default, with options to configure further.
- Several `hooks` to provide custom extensibility in various stages of the building process.