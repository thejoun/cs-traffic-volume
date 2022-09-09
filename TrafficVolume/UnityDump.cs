using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TrafficVolume
{
    public static class UnityDump
    {
        private static string DumpDirectory = "/dump";
        private static string DumpFile = "/hierarchy-dump.txt";

        private static readonly List<string> m_excluded = new List<string>()
        {
            "GameObject", "Transform"
        };

        public static void ShowSceneNames()
        {
            var sceneCount = SceneManager.sceneCount;

            var scenes = new List<Scene>();
            for (int i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                scenes.Add(scene);
            }

            var sceneNames = scenes.Select(scene => scene.name);
            var sceneNamesText = string.Join(",\n", sceneNames.ToArray());

            Manager.Log.WriteLog("Loaded scenes:\n" + sceneNamesText);
        }

        public static void DumpHierarchy()
        {
            var directoryPath = Application.persistentDataPath + DumpDirectory;
            var filePath = directoryPath + DumpFile;

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var sceneCount = SceneManager.sceneCount;
            var rootObjects = new List<GameObject>();

            using (var writer = new StreamWriter(filePath, false))
            {
                for (int i = 0; i < sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                    var sceneName = scene.name;
                    scene.GetRootGameObjects(rootObjects);

                    writer.WriteLine("SCENE " + sceneName);
                    rootObjects.ForEach(go => DumpTransform(go.transform, " ", writer));
                    writer.WriteLine("\n");
                }

                var ddolRoots = GetDontDestroyOnLoadObjects();

                writer.WriteLine("Don't Destroy On Load");
                ddolRoots.ForEach(go => DumpTransform(go.transform, " ", writer));
                writer.WriteLine("\n");
            }

            Manager.Log.WriteLog("Hierarchy dumped: " + filePath);
        }

        public static void DumpTransform(Transform tr, string linePrefix, StreamWriter writer)
        {
            var components = tr.gameObject.GetComponents<Component>();
            var componentNames = components.Select(c => c.GetType().Name);
            componentNames = componentNames.Except(m_excluded);
            var componentsText = string.Join(", ", componentNames.ToArray());

            writer.WriteLine(linePrefix + tr.name + " : " + componentsText);

            var newPrefix = " " + linePrefix;

            foreach (Transform child in tr)
            {
                DumpTransform(child, newPrefix, writer);
            }
        }

        public static List<GameObject> GetDontDestroyOnLoadObjects()
        {
            GameObject temp = null;
            try
            {
                temp = new GameObject();
                Object.DontDestroyOnLoad(temp);
                Scene ddolScene = temp.scene;
                Object.DestroyImmediate(temp);
                temp = null;

                return ddolScene.GetRootGameObjects().ToList();
            }
            finally
            {
                if (temp != null)
                    Object.DestroyImmediate(temp);
            }
        }
    }
}