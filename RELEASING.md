# Releasing Mocklis

How to cut a new release of the Mocklis NuGet packages, documentation and website.

## Things to know first

- **Debug builds on purpose.** The projects only define a `Debug` configuration. Mocklis is a library for tests, so the packages ship unoptimised assemblies with full portable PDBs and Source Link, letting users step straight into the Mocklis source. Don't add a Release configuration or pass `-c Release`.
- **Packages are created by `dotnet build`.** `src/NuGet.msbuild` sets `GeneratePackageOnBuild`, so there is no separate `dotnet pack` step. Each package ends up in its project's `bin/Debug/` folder.
- **The `Mocklis` meta-package** is defined by `src/Mocklis/Mocklis.nuspec`, not by MSBuild properties, so its version and dependency versions must be updated by hand.
- **`Mocklis.Experimental`** sets its own version in its project file and stays a perpetual pre-release. Only publish it when that version has been bumped.
- **SDK.** The SDK version is pinned in `src/global.json` (currently 10.0.x).

## 1. Update version numbers

| File | What to change |
| --- | --- |
| `src/NuGet.msbuild` | `Version` (e.g. `1.4.0`), `AssemblyVersion` and `FileVersion` (four-part, increase the last digit for every published build, e.g. `1.4.0.3`), and the year in `Copyright` |
| `src/Mocklis/Mocklis.nuspec` | `version`, the `version` of both dependencies, and the year in `copyright` |
| `RELEASE-NOTES.md` | Add a section for the new version |

## 2. Commit, tag and push

Source Link points at the commit the packages are built from, so that commit must be on GitHub before anyone debugs into the packages.

```powershell
git commit -am "Release 1.4.0"
git tag v1.4.0
git push origin main --tags
```

## 3. Build, test and collect the packages

Preferably build on Linux or in CI from a clean clone of the tag (see *Line endings* below):

```bash
git clone --branch v1.4.0 https://github.com/mocklis/mocklis.git
cd mocklis/src
dotnet build Mocklis.sln
dotnet test Mocklis.sln --no-build
```

On Windows the same commands work from `src` in PowerShell.

Check that the expected packages were created:

```powershell
Get-ChildItem -Recurse -Filter *.1.4.0.nupkg | Where-Object FullName -match '[\\/]bin[\\/]Debug[\\/]' | Select-Object Name
```

You should see `Mocklis`, `Mocklis.Core`, `Mocklis.BaseApi`, `Mocklis.MockGenerator`, `Mocklis.Serilog2`, `Mocklis.Serilog3` and `Mocklis.Serilog4`.

### Line endings and Source Link

Each PDB records a checksum of every source file. GitHub serves the files with LF line endings, but a Windows checkout normally has CRLF, so packages built on Windows have checksums that don't match what Source Link downloads, and debuggers then warn that the source doesn't match. Build from a Linux/CI checkout to avoid this, or add `*.cs text eol=lf` to `.gitattributes` and run `git add --renormalize .` once.

## 4. Push to nuget.org

The symbols are inside each `.nupkg`, so there are no `.snupkg` files to push.

```powershell
Get-ChildItem -Recurse -Filter *.1.4.0.nupkg | Where-Object FullName -match '[\\/]bin[\\/]Debug[\\/]' |
    ForEach-Object { dotnet nuget push $_.FullName --api-key <your key> --source https://api.nuget.org/v3/index.json }
```

## 5. Documentation and website

- **Documentation** (`mocklis.docs` repository, published on Read the Docs): set `version` and `release` and the copyright year in `source/conf.py`, merge the release branch into `main` and push. Read the Docs rebuilds automatically.
- **Website** (`Mocklis.github.io` repository, served as https://mocklis.net): update the samples if the generated code has changed, then push to `main`.

## 6. GitHub release

Create a release for the tag on GitHub, using the new section of `RELEASE-NOTES.md` as the description.
