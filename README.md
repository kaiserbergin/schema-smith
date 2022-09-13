# SchemaSmith

SchemaSmith is a CLI tool that enables simply expressed database schemas in yaml format, 
lints those schemas, and generates and executes scripts based on those schemas. The first
focus of this project is to provide utilities for Neo4j, but will hopefully include other
databases in the future.

## Installation

Simply install via nuget in your favorite IDE (it's Rider), or use the command line.

```powershell
dotnet tool install -g schemasmith --prerelease
```

## Use

The tool is invoked using the `schemasmith` command. There are several commands provided to assist
with linting, script generation, and script execution which you can read about by invoking the help
command:

```powershell
schemasmith --help
```

## But Why?

Neo4j has some tools around data modeling and script generation, but many are limited in scope
or rather expensive / complex. This project aims to be a no-nonsense way to manage your schema
when using Neo4j as a transactional / operational database.

Using this tool, you can easily generate idempotent scripts to update your graphs as well as
ensure that appropriate labels / relationships / properties are registered ahead of time so
you can better manage your app access levels (ever gone to prod with an app with editor access
only to be told it can't create fresh labels?).

## SchemaSmith Yaml Schema

Yo dawg, I heard you liked schemas...

But seriously, this is under development, so if you check the tests folder under Schemas you
will see an example of a good schema. Since this is in flux and this library is in early stages
of development, I'll save updating this section until later.