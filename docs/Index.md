---
order: -2
title: Getting Started
category: Introduction
permalink: /
inject:
  type: Ignis.Website.Components.QuickLinks
  description: Learn how to get started with Ignis in your project.
---

## Quickstart

This guide will help you get started with Ignis in your project.

### Prerequisites

- [.NET 6.0 SDK](https://dot.net) or later
- A code editor or IDE of your choice (e.g. [Visual Studio](https://visualstudio.microsoft.com),
  [Visual Studio Code](https://code.visualstudio.com), [JetBrains Rider](https://www.jetbrains.com/rider), etc.)

### Using our Project Templates

The easiest way to get started with Ignis is to use one of our project templates. These templates will set up a new
project for you with all the required dependencies and configuration.

Open a terminal and run the following command to install the Ignis project templates:

```shell
dotnet new install Ignis.Templates
```

Once the templates are installed, create and navigate to a folder for your new project and run the following command to
create a new project based on the Ignis Blazor WebAssembly template:

```shell
dotnet new ignis-wasm
```

You can also create a new project based on the Ignis Blazor Server template:

```shell
dotnet new ignis-server
```

Or if you want to create a component library, you can use the Ignis Component Library template:

```shell
dotnet new ignis-components
```

### Manual Installation

There are multiple packages available for Ignis, depending on your needs. You can have a look at
the [Packages](/docs/components/packages) page for more information.

The bare minimum you need to get started is the `Ignis.Components` package. This package contains the core components
and utilities that are required for Ignis components to work.

You can easily install this package using the following command:

```shell
dotnet add package Ignis.Components
```

Visit [nuget.org](https://www.nuget.org/packages/Ignis.Components) for more information.

#### Blazor Server or WebAssembly?

Ignis components can be used in both Blazor Server and Blazor WebAssembly applications. The only difference is that
Blazor Server applications require the `Ignis.Components.Server` package, while Blazor WebAssembly applications require
the `Ignis.Components.WebAssembly` package.

Once you have installed the required packages, you need to add the following line to your `Program.cs` file, depending
on the type of application you are using:

**Blazor Server**

```csharp
builder.Services.AddIgnisServer();
```

**Blazor WebAssembly**

```csharp
builder.Services.AddIgnisWebAssembly();
```

## Build your own components

Ignis is built on top of the [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) framework, which means
that you can easily build your own components using the same techniques that you would use to build a Blazor component.

To fully understand and leverage the power of Ignis, you should have a basic understanding of the
Ignis [component lifecycle](/docs/components/lifecycle).

Furthermore, you can have a look at advanced topics like [dynamic components](/docs/components/dynamic) and the
[reactivity system](/docs/components/reactivity).
