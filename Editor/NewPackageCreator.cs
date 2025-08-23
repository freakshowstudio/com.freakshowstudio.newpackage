
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace FreakshowStudio.NewPackage.Editor
{
    public static class NewPackageCreator
    {
        public static void Create(
            string path,
            string identifier,
            string name,
            string author,
            List<string> compilerFlags,
            bool useReleaseCi)
        {
            var assembly = new string(
                name
                    .Where(c => !char.IsWhiteSpace(c))
                    .ToArray());

            Directory.CreateDirectory(path);
            Directory.CreateDirectory(Path.Combine(path, "Runtime"));
            Directory.CreateDirectory(Path.Combine(path, "Editor"));
            Directory.CreateDirectory(Path.Combine(path, "Tests", "Runtime"));
            Directory.CreateDirectory(Path.Combine(path, "Tests", "Editor"));
            Directory.CreateDirectory(Path.Combine(path, "Samples~"));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "package.json"),
                PackageJson(identifier, name));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "CHANGELOG.md"),
                Changelog());

            File.WriteAllText(
                Path.Combine(
                    path,
                    "LICENSE.md"),
                License(author));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "README.md"),
                Readme(name));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Runtime",
                    $"{assembly}.Runtime.asmdef"),
                AsmDefRuntime(assembly));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Runtime",
                    "AssemblyInfo.cs"),
                AssemblyInfoRuntime(name, author, assembly));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Runtime",
                    "csc.rsp"),
                CompilerFlags(compilerFlags));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Editor",
                    $"{assembly}.Editor.asmdef"),
                AsmDefEditor(assembly));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Editor",
                    "AssemblyInfo.cs"),
                AssemblyInfoEditor(name, author));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Editor",
                    "csc.rsp"),
                CompilerFlags(compilerFlags));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Tests",
                    "Runtime",
                    $"{assembly}.Tests.Runtime.asmdef"),
                AsmDefTestsRuntime(assembly));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Tests",
                    "Runtime",
                    "AssemblyInfo.cs"),
                AssemblyInfoTestsRuntime(name, author));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Tests",
                    "Runtime",
                    "csc.rsp"),
                CompilerFlags(compilerFlags));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Tests",
                    "Editor",
                    $"{assembly}.Tests.Editor.asmdef"),
                AsmDefTestsEditor(assembly));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Tests",
                    "Editor",
                    "AssemblyInfo.cs"),
                AssemblyInfoTestsEditor(name, author));

            File.WriteAllText(
                Path.Combine(
                    path,
                    "Tests",
                    "Editor",
                    "csc.rsp"),
                CompilerFlags(compilerFlags));

            if (useReleaseCi)
            {
                Directory.CreateDirectory(
                    Path.Combine(
                        path,
                        ".github",
                        "workflows"));

                File.WriteAllText(
                    Path.Combine(
                        path,
                        ".github",
                        "workflows",
                        "release.yaml"),
                    ReleaseYaml());

                File.WriteAllText(
                    Path.Combine(
                        path,
                        ".releaserc.json"),
                    ReleaseRc());
            }
        }

        private static string PackageJson(
            string identifier,
            string packageName) =>
$@"{{
    ""name"": ""{identifier}"",
    ""displayName"": ""{packageName}"",
    ""version"": ""1.0.0"",
    ""unity"": ""{string.Join(".", Application.unityVersion.Split('.')[..2])}"",
    ""description"": ""A new Unity package"",
    ""keywords"": [],
    ""category"": ""Scripting"",
    ""dependencies"": {{ }}
}}";

        private static string Changelog(
            ) => string.Empty;

        private static string License(
            string packageAuthor) =>
$@"# MIT LICENSE

Copyright (c) {DateTime.Now.Year} {packageAuthor}

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is furnished
to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.";

        private static string Readme(
            string packageName) =>
$@"# {packageName}

A new Unity package.";

        private static string AsmDefRuntime(
            string assemblyName) =>
$@"{{
    ""name"": ""{assemblyName}.Runtime""
}}";

        private static string AsmDefEditor(
            string assemblyName) =>
            $@"{{
    ""name"": ""{assemblyName}.Editor"",
    ""references"": [
        ""{assemblyName}.Runtime""
    ],
    ""includePlatforms"": [
        ""Editor""
    ]
}}";

        private static string AsmDefTestsRuntime(
            string assemblyName) =>
$@"{{
    ""name"": ""{assemblyName}.Tests.Runtime"",
    ""references"": [
        ""{assemblyName}.Runtime""
    ],
    ""optionalUnityReferences"": [
        ""TestAssemblies""
    ]
}}";

        private static string AsmDefTestsEditor(
            string assemblyName) =>
$@"{{
    ""name"": ""{assemblyName}.Tests.Editor"",
    ""references"": [
        ""{assemblyName}.Runtime"",
        ""{assemblyName}.Editor""
    ],
    ""optionalUnityReferences"": [
        ""TestAssemblies""
    ],
    ""includePlatforms"": [
        ""Editor""
    ]
}}";

        private static string AssemblyInfoRuntime(
            string packageName,
            string packageAuthor,
            string assemblyName) =>
$@"
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly:AssemblyTitle(""{packageName} Runtime"")]
[assembly:AssemblyProduct(""{packageName}"")]
[assembly:AssemblyCompany(""{packageAuthor}"")]
[assembly:AssemblyCopyright(""(c) {DateTime.Now.Year} {packageAuthor}"")]

[assembly:InternalsVisibleTo(""{assemblyName}.Editor"")]
";

        private static string AssemblyInfoEditor(
            string packageName,
            string packageAuthor) =>
$@"
using System.Reflection;


[assembly:AssemblyTitle(""{packageName} Editor"")]
[assembly:AssemblyProduct(""{packageName}"")]
[assembly:AssemblyCompany(""{packageAuthor}"")]
[assembly:AssemblyCopyright(""(c) {DateTime.Now.Year} {packageAuthor}"")]
";

        private static string AssemblyInfoTestsRuntime(
            string packageName,
            string packageAuthor) =>
$@"
using System.Reflection;

[assembly:AssemblyTitle(""{packageName} Runtime Tests"")]
[assembly:AssemblyProduct(""{packageName}"")]
[assembly:AssemblyCompany(""{packageAuthor}"")]
[assembly:AssemblyCopyright(""(c) {DateTime.Now.Year} {packageAuthor}"")]
";

        private static string AssemblyInfoTestsEditor(
            string packageName,
            string packageAuthor) =>
$@"
using System.Reflection;

[assembly:AssemblyTitle(""{packageName} Editor Tests"")]
[assembly:AssemblyProduct(""{packageName}"")]
[assembly:AssemblyCompany(""{packageAuthor}"")]
[assembly:AssemblyCopyright(""(c) {DateTime.Now.Year} {packageAuthor}"")]
";

        private static string CompilerFlags(
            List<string> flags) =>
            string.Join("\n", flags);

        private static string ReleaseYaml() =>
@"name: Release
on:
  push:
    branches:
      - master
jobs:
  release:
    name: release
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v5
        with:
          fetch-depth: 0
      - name: Release
        uses: cycjimmy/semantic-release-action@v4
        with:
          extra_plugins: |
            @semantic-release/changelog
            @semantic-release/git
          branch: master
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
";

        private static string ReleaseRc() =>
@"{
  ""tagFormat"": ""v${version}"",
  ""plugins"": [
    [""@semantic-release/commit-analyzer"", {
      ""preset"": ""angular""
    }],
    ""@semantic-release/release-notes-generator"",
    [""@semantic-release/changelog"", {
      ""preset"": ""angular"",
      ""changelogFile"": ""CHANGELOG.md""
    }],
    [""@semantic-release/npm"", {
      ""npmPublish"": false
    }],
    [""@semantic-release/git"", {
      ""assets"": [
        ""package.json"",
        ""CHANGELOG.md""
      ],
      ""message"": ""chore(release): ${nextRelease.version} [skip ci]\n\n${nextRelease.notes}""
    }],
    ""@semantic-release/github""
  ]
}
";
    }
}
