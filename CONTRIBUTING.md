Thank you for considering contributing to the _RunInfoBuilder_ project.

Before contributing, please carefully read the guidelines below to get an idea of the expectations for this project.

- Feel free to try to tackle a reported issue. Unless it's the simplest of fixes, I'd prefer that we discuss the strategy/fix (on that issue's thread) first. The last thing I want is for someone to put in a lot of work, only to have to re-write a bunch of it.
- Pull requests for brand new features won't be accepted, unless it's something that's been planned and in the roadmap. You're always welcome to fork the project to add your own features that's not supported here.

### Submitting Issues/Bugs

Simply create a new issue if there's a problem found. Please spent a minute poking around to make sure the same issue doesn't already exist.

For the best chance of a timely response and resolution, please be as descriptive as possible. I recommend including the following key pieces of information:

1. Repro steps - detailed steps to reproduce the issue you're having.
2. Configurations - include copies of your command configurations so we can debug it locally on our machines.
3. Stack Trace
4. Environment information - operating system, processor architecture, etc.

### Suggesting New Features / Roadmap

I'm always open to new ideas or enhancements to the library. If you have any suggestions, feel free to open a new issue requesting it. Be as detailed as possible about what it is, and also explain your actual use cases that it would solve.

New features ideas that are accepted will eventually be placed onto the roadmap, which you can check out in the ROADMAP.md file. It'll only be placed there once all the details have been figured out so it can be documented there.

### Branch Strategy & Pull Requests

In general, there will usually only be 2 live branches: `Master` and `Development`.

Once a version is released, all work afterwards will be done on the `Development` branch, so be sure to branch from there. The `Development` branch will be merged into `Master` only when a new version of the library is ready for release.

All pull requests should target the `Development` branch.

### Testing

Ensure tests all pass before putting your pull request up for review.

Fix any broken tests, and if you added new features, be sure to add new tests that cover them. Functional tests are what I'm primarily looking for (in the `FunctionalTests` project) to ensure everything works from end-to-end. 

Be sure to checkout the tests that already exist to get a feel for how they should be written. Also, for tests relating to commands, be sure to write tests for both a single-level command, and multi-level commands (nested subcommands).