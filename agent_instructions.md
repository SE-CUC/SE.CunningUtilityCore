# MDK Project Build & Verification Guide for Linux (Mono)

This document outlines the specific steps required to build, test, and verify this C# 6.0 MDK project in a Linux-based environment. The standard build process fails due to limitations in the MDK build tools on non-Windows platforms. The following workaround is necessary to validate changes.

## 1. Environment Prerequisites

- **Mono:** The project targets .NET Framework 4.7.2 and requires the `mono` runtime and Mono's `msbuild` to compile and execute.
- **Base `msbuild` command:** All build commands should be prefixed with `/usr/bin/mono /usr/lib/mono/msbuild/15.0/bin/MSBuild.dll`.

## 2. Initial Setup & Expected Failure

1.  **Restore NuGet Packages:** Before any build attempt, restore the solution's NuGet packages:
    ```bash
    /usr/bin/mono /usr/lib/mono/msbuild/15.0/bin/MSBuild.dll -t:restore SE.CunningUtilityCore.sln
    ```
2.  **Standard Build (Will Fail):** A direct build of the solution will fail.
    ```bash
    # This command is expected to fail
    /usr/bin/mono /usr/lib/mono/msbuild/15.0/bin/MSBuild.dll -t:build SE.CunningUtilityCore.sln
    ```
    - **Reason for Failure:** The build will be blocked by the `SpaceEngineersFinder` task, a part of the MDK toolchain that cannot automatically locate game binaries on Linux. Attempts to configure it via `.mdk.local.ini` are also ineffective in this environment.

## 3. Verification Workaround: Manual Compilation

To validate code changes, the `NonGameDebugLaunch` project must be run. This is achieved by manually compiling the core library and referencing it directly.

### Step A: Manually Compile the Core Library

1.  Gather all C# source files from `SE.CunningUtilityCore.Mixin` and `SE.CunningUtilityCore`.
2.  Compile them into a single DLL using the `csc` compiler.

    ```bash
    /usr/lib/mono/msbuild/Current/bin/Roslyn/csc.exe \
        /target:library \
        /out:NonGameDebugLaunch/Dlls/SE.CunningUtilityCore.dll \
        /reference:/usr/lib/mono/4.7.2-api/mscorlib.dll \
        /reference:/app/NonGameDebugLaunch/Dlls/Sandbox.Common.dll \
        /reference:/app/NonGameDebugLaunch/Dlls/Sandbox.Game.dll \
        /reference:/app/NonGameDebugLaunch/Dlls/VRage.dll \
        /reference:/app/NonGameDebugLaunch/Dlls/VRage.Game.dll \
        /reference:/app/NonGameDebugLaunch/Dlls/VRage.Math.dll \
        $(find SE.CunningUtilityCore.Mixin -name "*.cs")
    ```
    *Note: Additional standard library references may be needed depending on the code.*

### Step B: Modify `NonGameDebugLaunch.csproj`

1.  **Add DLL Reference:** Add a reference to the newly created `SE.CunningUtilityCore.dll`.
    ```xml
    <Reference Include="SE.CunningUtilityCore">
      <HintPath>Dlls\SE.CunningUtilityCore.dll</HintPath>
    </Reference>
    ```
2.  **Remove Mixin Import:** Find and **delete or comment out** the line that imports the mixin project. This is critical to prevent dozens of `CS0436` "type conflict" warnings.
    ```xml
    <!-- <Import Project="..\SE.CunningUtilityCore.Mixin\SE.CunningUtilityCore.Mixin.projitems" Label="Shared" /> -->
    ```

### Step C: Build the Debug Project

Build **only** the `NonGameDebugLaunch.csproj` project directly.

```bash
/usr/bin/mono /usr/lib/mono/msbuild/15.0/bin/MSBuild.dll -t:build -p:Configuration=Debug NonGameDebugLaunch/NonGameDebugLaunch.csproj
```

### Step D: Run and Verify

Execute the compiled debug application using `mono`.

```bash
mono NonGameDebugLaunch/bin/Debug/NonGameDebugLaunch.exe
```
Check the console output for logs confirming that the application initialized and ran as expected.

## 4. Finalization: Pre-Submission Cleanup

**CRITICAL:** Before submitting the final code, all temporary modifications made to the project files (`.csproj`, `.projitems`) during the verification process **must be reverted**.

Use the `restore_file` tool on all modified project files to return them to their original state. This ensures the repository remains clean and buildable for other users and environments.
