using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class SceneChangingTestSuite
    {
        [UnityTest]
        public IEnumerator CorrectSceneChangeToEditorScene()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Menu"));
            SceneChanging sc = obj.GetComponent<SceneChanging>();
            sc.ChangeScene("Scene");

            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "Scene");
            GameObject.Destroy(obj);
            GameObject.Destroy(sc);
        }

        [UnityTest]
        public IEnumerator CorrectSceneChangeToMainMenu()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Menu"));
            SceneChanging sc = obj.GetComponent<SceneChanging>();
            sc.ChangeScene("MainMenu");

            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(SceneManager.GetActiveScene().name, "MainMenu");
            GameObject.Destroy(obj);
            GameObject.Destroy(sc);
        }

        [UnityTest]
        public IEnumerator CorrectApplicationClosing()
        {
            GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Menu"));
            SceneChanging sc = obj.GetComponent<SceneChanging>();
            sc.close();

            yield return new WaitForSeconds(0.1f);
            Assert.IsTrue(UnityEngine.Application.isEditor);
            GameObject.Destroy(obj);
            GameObject.Destroy(sc);
        }
    }
}
