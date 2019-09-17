# ReadmeLinkVerifier

A tool to verify that the links used in your readme file are active

# How to Use

The tool is written in dotnet core, so you'll need to verify that you have dotnet core (v-2.2) installed on you machine.

To run, use the following command (if you want to verify the readme at the repository root named README.md):

```
dornet  ReadmeLinkVerifierConsoleApp.dll <RepositoryPath>
```

Or, if you want to scan a readme file at any other location:

```
dornet  ReadmeLinkVerifierConsoleApp.dll <RepositoryPath> <ReadmeFileRelativePath>
```