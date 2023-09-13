---
order: -1
title: Troubleshooting
category: Introduction
permalink: /troubleshooting
---

Here are some common issues you might run into when using Ignis.

## There is no registered service of type 'Ignis.Components.IHostContext'

```
InvalidOperationException: Cannot provide a value for property 'HostContext' on type 'XXX'. There is no registered service of type 'Ignis.Components.IHostContext'.
```

**Solution:** Make sure to install either the `Ignis.Components.Server` or `Ignis.Components.WebAssembly` package in
your project and call either `AddIgnisServer` or `AddIgnisWebAssembly` in your `Program.cs`.

[Read our guide on how to get started for more information.](/docs/)
