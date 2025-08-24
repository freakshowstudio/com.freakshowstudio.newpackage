
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace FreakshowStudio.NewPackage.Editor
{
    public class NewPackageWizard : ScriptableWizard
    {
        [Header("Package Info")]
        [SerializeField]
        [Tooltip("The author of the package")]
        private string _packageAuthor = "Company";

        [SerializeField]
        [Tooltip("Name of the new package to create")]
        private string _packageName = "Package";

        [SerializeField]
        [Tooltip("Identifier for the new package")]
        private string _packageIdentifier = "com.company.package";

        [Header("Compiler Flags")]
        [SerializeField]
        [Tooltip("Extra compiler flags for the package assemblies")]
        private List<string> _compilerFlags = new()
        {
            "-nullable",
            "-warnaserror",
        };

        [Header("CI")]
        [SerializeField]
        [Tooltip("Add files for automating releases on GitHub")]
        private bool _useReleaseCi;

        [MenuItem("Assets/Create Package", false, 14560)]
        private static void ShowWizard()
        {
            DisplayWizard<NewPackageWizard>(
                "Create Package",
                "Create",
                "Cancel");
        }

        private void OnEnable()
        {
            helpString =
                "Fill in the name and identifier for the new package and " +
                "click create to create the new package in your " +
                "Packages folder. Select the Use Release CI checkbox if " +
                "you want to include a GitHub workflow for automating " +
                "releases when pushing to the main branch, for example " +
                "for managing OpenUPM packages.";
        }

        private void OnWizardCreate()
        {
            var path = Path.GetFullPath($"Packages/{_packageIdentifier}");
            var exists = Directory.Exists(path);

            if (exists)
            {
                var overwrite = EditorUtility.DisplayDialog(
                    "Folder Exists",
                    $"Path {path} already exists. Overwrite?",
                    "Overwrite",
                    "Cancel");

                if (!overwrite)
                {
                    return;
                }
            }

            NewPackageCreator.Create(
                path,
                _packageIdentifier,
                _packageName,
                _packageAuthor,
                _compilerFlags,
                _useReleaseCi);

            AssetDatabase.Refresh();
            UnityEditor.PackageManager.Client.Resolve();
        }

        private void OnWizardOtherButton()
        {
            Close();
        }
    }
}
