# ReadmeLinkVerifier

A tool to verify that the links used in your readme file are active

ReadmeLinkVerifier can verify 3 types of links:
- Links to other files inside the repository.
- Links to a section in the readme file (e.g. `[Link](#how-to-use)`).
- Links to web-sights (e.g. `[Link](https://github.com/ZviRosenfeld)`).

# How to Use

The tool is written in dotnet core, so you'll need to verify that you have dotnet core (v-2.2) installed on you machine.

To run, use the following command. (We'll verify the readme file at the repository root named README.md):

```
dornet ReadmeLinkVerifierConsoleApp.dll <RepositoryPath>
```

If you want to specify a different file, you can use the command:

```
dornet ReadmeLinkVerifierConsoleApp.dll -r <ReadmeFileRelativePath> <RepositoryPath>
```

The tool will exit with a -1 if something went wrong (normally, the parameters are wrong, or the repository or readme doesn't exist).
The tool will exit with a -2 if at least one of the links in invalid.
