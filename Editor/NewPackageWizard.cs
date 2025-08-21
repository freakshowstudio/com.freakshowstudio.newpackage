
using System.IO;
using UnityEditor;
using UnityEngine;
using Directory = UnityEngine.Windows.Directory;


namespace FreakshowStudio.NewPackage.Editor
{
    public class NewPackageWizard : ScriptableWizard
    {
        [SerializeField]
        private string _packageName = "New Package";

        [SerializeField]
        private string _packageIdentifier = "com.company.package";

        [MenuItem("Assets/Create Package", false, 14560)]
        private static void ShowWizard()
        {
            DisplayWizard<NewPackageWizard>(
                "Create Package",
                "Create",
                "Cancel");
        }

        private void OnWizardCreate()
        {
            string path = Path.GetFullPath($"Packages/{_packageIdentifier}");
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

            NewPackageCreator.Create(path, _packageIdentifier, _packageName);

            AssetDatabase.Refresh();
        }

        private void OnWizardOtherButton()
        {
            Close();
        }
    }
}
