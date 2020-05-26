using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;
using System;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class SaveLoadMenuTestSuite
    {
        GameObject[] goA;

        [UnityTest]
        public IEnumerator OpeningSaveLoadMenuWindow()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("Save Load Menu").gameObject;
            SaveLoadMenu slm = go.GetComponent<SaveLoadMenu>();

            slm.Open(true);

            Assert.IsTrue(HexMapCamera.Locked);
            Assert.IsTrue(go.activeSelf);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(slm);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator ClosingSaveLoadMenuWindow()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("Save Load Menu").gameObject;
            SaveLoadMenu slm = go.GetComponent<SaveLoadMenu>();

            slm.Close();

            Assert.IsFalse(HexMapCamera.Locked);
            Assert.IsFalse(go.activeSelf);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(slm);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator FillingMenuListWithMapNames()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("Save Load Menu").gameObject;
            SaveLoadMenu slm = go.GetComponent<SaveLoadMenu>();
            string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.map");

            slm.Open(true);

            Assert.AreEqual(paths.Length, slm.listContent.childCount);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(slm);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator DeleteMapFromMenuList()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("Save Load Menu").gameObject;
            SaveLoadMenu slm = go.GetComponent<SaveLoadMenu>();
            string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.map");

            slm.SelectItem("test3");
            slm.hexGrid = goA[1].GetComponent<HexGrid>();
            slm.Open(true);
            slm.Action();
            int MapCount = slm.listContent.childCount;
            slm.Delete();

            for (int i = 0; i < slm.listContent.childCount; i++)
            {
                for (int j = 0; j < paths.Length; j++)
                {
                    if (slm.listContent.GetChild(i).gameObject.GetComponent<SaveLoadItem>().MapName != "test3")
                    {
                        Assert.IsTrue(true);
                        yield return null;
                    }
                }
            }
            Assert.IsFalse(false);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(slm);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator ReturnNullIfPathIsNullDuringRemove()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("Save Load Menu").gameObject;
            SaveLoadMenu slm = go.GetComponent<SaveLoadMenu>();
            string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.map");
            slm.SelectItem("");
            int MapCount = slm.listContent.childCount;
            slm.Delete();

            Assert.IsEmpty(slm.path);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(slm);
            SceneManager.UnloadScene("Scene");
        }


        [UnityTest]
        public IEnumerator ChangingTheInterfaceWhenSaving()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("Save Load Menu").gameObject;
            SaveLoadMenu slm = go.GetComponent<SaveLoadMenu>();

            slm.Open(true);

            Assert.AreEqual(slm.menuLabel.text, "Save Map");
            Assert.AreEqual(slm.actionButtonLabel.text, "Save");

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(slm);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator ChangingTheInterfaceWhenLoading()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("Save Load Menu").gameObject;
            SaveLoadMenu slm = go.GetComponent<SaveLoadMenu>();

            slm.Open(false);

            Assert.AreEqual(slm.menuLabel.text, "Load Map");
            Assert.AreEqual(slm.actionButtonLabel.text, "Load");

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(slm);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator ReturnNullIfFileDontExistsDuringTheLoading()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("Save Load Menu").gameObject;
            SaveLoadMenu slm = go.GetComponent<SaveLoadMenu>();

            slm.SelectItem("test6");
            slm.Open(false);
            slm.Action();

            
            Assert.IsNull(slm.path);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(slm);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CorrectMapSaving()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("Save Load Menu").gameObject;
            SaveLoadMenu slm = go.GetComponent<SaveLoadMenu>();
            GameObject go2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Save Load Item"));
            SaveLoadItem sli = go2.GetComponent<SaveLoadItem>();

            sli.MapName = "test4";
            sli.Select();
            slm.hexGrid = goA[1].GetComponent<HexGrid>();
            slm.Open(true);
            slm.Action();
            string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.map");
            bool b = false;
            for (int i = 0; i < paths.Length; i++)
            {
                if (Path.GetFileNameWithoutExtension(paths[i]) == sli.MapName)
                {
                    b = true;
                    break;
                }
            }

            Assert.IsTrue(b);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(slm);
            GameObject.Destroy(go2);
            GameObject.Destroy(sli);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CorrectMapLoading()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("Save Load Menu").gameObject;
            SaveLoadMenu slm = go.GetComponent<SaveLoadMenu>();
            GameObject go2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Save Load Item"));
            SaveLoadItem sli = go2.GetComponent<SaveLoadItem>();

            sli.MapName = "test16";
            sli.menu = slm;
            slm.itemPrefab = sli;
            sli.Select();
            slm.hexGrid = goA[1].GetComponent<HexGrid>();
            slm.Open(true);
            slm.Action();
            slm.Open(false);
            slm.Action();
            string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.map");
            bool b = false;
            for (int i = 0; i < paths.Length; i++)
            {
                if (Path.GetFileNameWithoutExtension(paths[i]) == sli.MapName)
                {
                    b = true;
                    break;
                }
            }

            Assert.IsTrue(b);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(slm);
            GameObject.Destroy(go2);
            GameObject.Destroy(sli);
            SceneManager.UnloadScene("Scene");
        }

    }
}
