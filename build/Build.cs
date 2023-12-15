using System.Collections.Generic;
using System.Linq;
using Hexagrams.Nuke.Components;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

// ReSharper disable RedundantExtendsListEntry
// ReSharper disable InconsistentNaming

[DotNetVerbosityMapping]
[ShutdownDotNetAfterServerBuild]
[GitHubActions(
    "continuous",
    GitHubActionsImage.UbuntuLatest,
    OnPullRequestBranches = ["main"],
    OnPushBranches = ["main"],
    PublishArtifacts = true,
    InvokedTargets = [nameof(ICompile.Compile)],
    CacheKeyFiles = ["global.json", "src/**/*.csproj"])]
class Build : NukeBuild,
    IHasGitRepository,
    IRestore,
    IFormat,
    IClean,
    ICompile
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => ((ICompile) x).Compile);

    [Required]
    [Solution]
    readonly Solution Solution;
    Solution IHasSolution.Solution => Solution;

    IEnumerable<AbsolutePath> IFormat.ExcludedFormatPaths => new[]
    {
        AbsolutePath.Create(RootDirectory.GetRelativePathTo(
            Solution.GetAllProjects("BlazorIdentity.Relational").Single().Directory / "Migrations"))
    };

    Target ICompile.Compile => _ => _
        .Inherit<ICompile>()
        .DependsOn<IFormat>(x => x.VerifyFormat);
}
