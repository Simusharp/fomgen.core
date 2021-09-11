/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Diagnostics;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.ChangeLog.ChangelogTasks;
using Nuke.Common.CI.GitHubActions;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[GitHubActions("continuous",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(Test) })]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion(Framework = "net5.0")] readonly GitVersion GitVersion;

    [Parameter] string NugetApiUrl = "https://api.nuget.org/v3/index.json";
    [Parameter] string NugetApiKey;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath OutputDirectory => RootDirectory / "output";
    AbsolutePath ArtifactsDirectory => OutputDirectory / "nuget";
    AbsolutePath ChangeLogFile => RootDirectory / "CHANGELOG.md";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .SetLogger("trx")
                .SetResultsDirectory(OutputDirectory)
                .CombineWith(
                    Solution.GetProjects("*.*Tests"), (cs, v) => cs
                        .SetProjectFile(v)));
        });

    //TODO set properties in Directory.Build.props
    Target Pack => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            var changeLog = GetNuGetReleaseNotes(ChangeLogFile, GitRepository);

            DotNetPack(s => s
                .SetProject(Solution.GetProject("Simusharp.FomGen.Core"))
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetPackageReleaseNotes(changeLog)
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetDescription("A lightweight library to manage HLA 1516e FOMs")
                .SetPackageTags("Simulation Distributed FOM HLA 1516e")
                .SetOutputDirectory(ArtifactsDirectory));
        });

    Target Push => _ => _
        .DependsOn(Pack)
        .Requires(() => NugetApiUrl)
        .Requires(() => NugetApiKey)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {
            GlobFiles(ArtifactsDirectory, "*.nupkg")
                .NotEmpty()
                .Where(x => !x.EndsWith("symbols.nupkg"))
                .ForEach(x =>
                {
                    DotNetNuGetPush(s => s
                        .SetTargetPath(x)
                        .SetSource(NugetApiUrl)
                        .SetApiKey(NugetApiKey)
                    );
                });
        });
}
