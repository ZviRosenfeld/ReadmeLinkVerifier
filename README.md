# ReadmeLinkVerifier

A tool to verify that the links used in your readme file are active

# How to Use

The tool is written in dotnet core, so you'll need to verify that you have dotnet core (v-2.2) installed on you machine.

To run, use the following command. (We'll verify the readme file at the repository root named README.md):

```
dornet  ReadmeLinkVerifierConsoleApp.dll <RepositoryPath>
```

If you want to specify a different file, you can use the command:

```
dornet  ReadmeLinkVerifierConsoleApp.dll -r <ReadmeFileRelativePath> <RepositoryPath>
```