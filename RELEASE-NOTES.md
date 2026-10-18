# Mocklis release notes

Documentation: https://mocklis.readthedocs.io · Packages: https://www.nuget.org/profiles/mocklis · Source: https://github.com/mocklis/mocklis

## 1.4.0

The last stable release of the core Mocklis packages was 1.2.0 (1.3.0 only shipped `Mocklis.Serilog3`), so these notes cover everything since 1.2.0.

### Highlights

- **Source generator.** Mocklis classes are now written by a Roslyn incremental source generator. Declare an empty partial class with the `MocklisClass` attribute and the implementation is generated behind the scenes, and kept up to date whenever the interfaces change:

  ```csharp
  [MocklisClass]
  public partial class MockService : IMyService, IDisposable;
  ```

  There is no generated code to check in or regenerate by hand any more. To see the generated half of the class, use *Go To Implementation* on the class name, or look under *Dependencies → Analyzers → Mocklis.MockGenerator* in Solution Explorer.

- **New `Switch` step** for methods. Pick behaviour based on the parameters of a call, with branches checked in order and an `Otherwise` fallback:

  ```csharp
  var sw = mock.GetPrice.Switch();
  sw.When(p => p.sku == "APPLE").Return(0.5m);
  sw.Otherwise.Return(1.0m);
  ```

  Pass `consumeOnUse: true` to remove each branch once it has been used, and call `Clear()` to remove all branches.

- **New `ReturnTask` steps** for methods returning `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>`, e.g. `mock.DoStuffAsync.ReturnTask().Return(42);`.

- **Serilog 3 and 4 support** through the new `Mocklis.Serilog3` and `Mocklis.Serilog4` packages, alongside the existing `Mocklis.Serilog2`.

### Other changes

- `PropertyStepWithNext`, `IndexerStepWithNext`, `EventStepWithNext` and `MethodStepWithNext` have a new `Clear()` method that removes the next step, returning the mock to its unprogrammed behaviour.
- `Stored` and `StoredWithChangeNotification` now come as separate overloads with and without an `initialValue`, instead of a single method with an optional parameter. Existing source code compiles unchanged.
- Verification messages from `CurrentValue` and `CurrentValues` checks now show `<null>` for null values, so they can be told apart from empty strings.
- The `Raise` extension methods for events now have the correct nullability on the `sender` parameter.
- Fixed an exception in the code generator when comparing symbols without a containing namespace.
- The library assemblies remain unoptimised debug builds with portable PDBs and Source Link included in the packages, so you can step straight into the Mocklis source code from your tests.
- All packages now link to the GitHub repository in their NuGet metadata.
- `Mocklis.BaseApi` now depends on `System.Threading.Tasks.Extensions` 4.6.3 (and through it `System.Runtime.CompilerServices.Unsafe` 6.1.2) on .NET Framework and .NET Standard 2.0, up from 4.5.4. This avoids assembly load errors for `ValueTask` steps when running .NET Framework tests with recent test platforms.

### Breaking changes and requirements

- **Target frameworks.** The library packages now target `net462` and `netstandard2.0` (`Mocklis.BaseApi` also targets `netstandard2.1`). Support for .NET Framework 4.5–4.6.1 and .NET Standard 1.3 has been dropped.
- **Compiler version.** The source generator is built against Roslyn 4.3, so you need Visual Studio 2022 version 17.3 or later, or a .NET SDK of version 6.0.400 or later (or a recent JetBrains Rider).
- **Partial classes.** Only classes declared `partial` are picked up by the source generator. The empty-body syntax `public partial class X : I;` needs C# 12; with older language versions write `public partial class X : I { }` instead. The generated code uses nullable annotations when nullable reference types are enabled.
- **Migrating from 1.2.** Classes that are not declared `partial` still work the old way: the *Update Mocklis Class* code fix writes the implementation into the class itself. To move to the source generator, make the class `partial` and delete its body. If you use `MocklisClass` at several levels of an inheritance hierarchy within one project, note that the generator only sees members you wrote yourself in the base class, not ones generated for it, so a generated member in a derived class can hide one in its base.

### Packages

| Package | Version |
| --- | --- |
| Mocklis (meta-package: BaseApi + MockGenerator) | 1.4.0 |
| Mocklis.Core | 1.4.0 |
| Mocklis.BaseApi | 1.4.0 |
| Mocklis.MockGenerator | 1.4.0 |
| Mocklis.Serilog2 | 1.4.0 |
| Mocklis.Serilog3 | 1.4.0 |
| Mocklis.Serilog4 | 1.4.0 (new) |
| Mocklis.Experimental | 1.2.0-alpha (unchanged) |

## 1.3.0

- Added the `Mocklis.Serilog3` package with a logging context for Serilog 3.x. No other packages were released.
