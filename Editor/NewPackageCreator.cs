
using System.IO;


namespace FreakshowStudio.NewPackage.Editor
{
    public static class NewPackageCreator
    {
        private static string PackageContents(
            string identifier,
            string name) => $@"{{
    ""name"": ""{identifier}"",
    ""displayName"": ""{name}"",
    ""version"": ""1.0.0"",
    ""unity"": ""2018.3"",
    ""description"": ""A new Unity package"",
    ""keywords"": [],
    ""category"": ""Scripting"",
    ""dependencies"": {{ }}
}}";

        private static string ChangelogContents() => $@"
# Changelog

## 1.0.0

  * Initial release
";

        private static string LicenseContent() => $@"# MIT LICENSE

Copyright 2019 <Author>

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

        private static string ReadmeContent(string name) => $@"
# {name}

A new Unity package.";

        private static string RuntimeAsmDefContent(
            string identifier) => $@"{{
    ""name"": ""{identifier}.Runtime"",
    ""references"": [],
    ""optionalUnityReferences"": [],
    ""includePlatforms"": [],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": []
}}";

        private static string EditorAsmDefContent(
            string identifier) => $@"{{
    ""name"": ""{identifier}.Editor"",
    ""references"": [
        ""{identifier}.Runtime""
    ],
    ""optionalUnityReferences"": [],
    ""includePlatforms"": [
        ""Editor""
    ],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": []
}}";

        private static string RuntimeTestsAsmDefContent(
            string identifier) => $@"{{
    ""name"": ""{identifier}.Tests.Runtime"",
    ""references"": [
        ""{identifier}.Runtime""
    ],
    ""optionalUnityReferences"": [
        ""TestAssemblies""
    ],
    ""includePlatforms"": [],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": []
}}";

        private static string EditorTestsAsmDefContent(
            string identifier) => $@"{{
    ""name"": ""{identifier}.Tests.Editor"",
    ""references"": [
        ""{identifier}.Runtime"",
        ""{identifier}.Editor""
    ],
    ""optionalUnityReferences"": [
        ""TestAssemblies""
    ],
    ""includePlatforms"": [
        ""Editor""
    ],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": []
}}";

        public static void Create(string path, string identifier, string name)
        {
            var editorPath = Path.Combine(path, "Editor");
            var runtimePath = Path.Combine(path, "Runtime");
            var samplesPath = Path.Combine(path, @"Samples~");
            var testsPath = Path.Combine(path, "Tests");
            var editorTestsPath = Path.Combine(testsPath, "Editor");
            var runtimeTestsPath = Path.Combine(testsPath, "Runtime");

            Directory.CreateDirectory(path);
            Directory.CreateDirectory(editorPath);
            Directory.CreateDirectory(runtimePath);
            Directory.CreateDirectory(samplesPath);
            Directory.CreateDirectory(testsPath);
            Directory.CreateDirectory(editorTestsPath);
            Directory.CreateDirectory(runtimeTestsPath);

            var packagePath = Path.Combine(path, "package.json");
            var changelogPath = Path.Combine(path, "CHANGELOG.md");
            var licensePath = Path.Combine(path, "LICENSE.md");
            var readmePath = Path.Combine(path, "README.md");

            var runtimeAsmDefPath = Path.Combine(
                runtimePath, $"{identifier}.Runtime.asmdef");

            var editorAsmDefPath = Path.Combine(
                editorPath, $"{identifier}.Editor.asmdef");

            var runtimeTestsAsmDefPath = Path.Combine(
                runtimeTestsPath, $"{identifier}.Tests.Runtime.asmdef");

            var editorTestsAsmDefPath = Path.Combine(
                editorTestsPath, $"{identifier}.Tests.Editor.asmdef");

            File.WriteAllText(
                packagePath,
                PackageContents(identifier, name));

            File.WriteAllText(
                changelogPath,
                ChangelogContents());

            File.WriteAllText(
                licensePath,
                LicenseContent());

            File.WriteAllText(
                readmePath,
                ReadmeContent(name));

            File.WriteAllText(
                runtimeAsmDefPath,
                RuntimeAsmDefContent(identifier));

            File.WriteAllText(
                editorAsmDefPath,
                EditorAsmDefContent(identifier));

            File.WriteAllText(
                runtimeTestsAsmDefPath,
                RuntimeTestsAsmDefContent(identifier));

            File.WriteAllText(
                editorTestsAsmDefPath,
                EditorTestsAsmDefContent(identifier));
        }
    }
}
