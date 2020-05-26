using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class NewMapMenuTestSuite
    {
        GameObject[] goA;

        [UnityTest]
        public IEnumerator CameraIsLockedAndNewMapMenuIsActiveAfterButtonOpenPressed()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("New Map Menu").gameObject;
            NewMapMenu nmm = go.GetComponent<NewMapMenu>();

            nmm.Open();

            Assert.IsTrue(HexMapCamera.Locked);
            Assert.IsTrue(go.activeSelf);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(nmm);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CameraIsUnlockedAndNewMapMenuIsInactiveAfterButtonClosePressed()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("New Map Menu").gameObject;
            NewMapMenu nmm = go.GetComponent<NewMapMenu>();

            nmm.Close();

            Assert.IsFalse(HexMapCamera.Locked);
            Assert.IsFalse(go.activeSelf);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(nmm);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CorrectCreatingSmallMap()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("New Map Menu").gameObject;
            NewMapMenu nmm = go.GetComponent<NewMapMenu>();

            nmm.hexGrid = goA[1].gameObject.GetComponent<HexGrid>();
            nmm.CreateSmallMap();
            
            Assert.AreEqual(nmm.hexGrid.cellCountX, 20);
            Assert.AreEqual(nmm.hexGrid.cellCountZ, 15);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(nmm);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CorrectCreatingMediumMap()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("New Map Menu").gameObject;
            NewMapMenu nmm = go.GetComponent<NewMapMenu>();

            nmm.hexGrid = goA[1].gameObject.GetComponent<HexGrid>();
            nmm.CreateMediumMap();

            Assert.AreEqual(nmm.hexGrid.cellCountX, 40);
            Assert.AreEqual(nmm.hexGrid.cellCountZ, 30);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(nmm);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CorrectCreatingLargeMap()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject go = goA[3].transform.Find("New Map Menu").gameObject;
            NewMapMenu nmm = go.GetComponent<NewMapMenu>();

            nmm.hexGrid = goA[1].gameObject.GetComponent<HexGrid>();
            nmm.CreateLargeMap();

            Assert.AreEqual(nmm.hexGrid.cellCountX, 80);
            Assert.AreEqual(nmm.hexGrid.cellCountZ, 60);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(go);
            GameObject.Destroy(nmm);
            SceneManager.UnloadScene("Scene");
        }
    }
}
