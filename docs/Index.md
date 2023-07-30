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

### Installation

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

