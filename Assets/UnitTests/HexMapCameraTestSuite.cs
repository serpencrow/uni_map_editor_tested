using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

namespace Tests
{
    class HexMapCameraTestSuite
    {
        GameObject[] goA;

        [UnityTest]
        public IEnumerator CheckValidateCameraPosition()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject Camera = goA[2];

            HexMapCamera.ValidatePosition();
            Assert.AreEqual(Camera.gameObject.transform.localPosition, new Vector3(0f, 0f, 0f));

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(Camera);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CheckCameraZoom()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject Camera = goA[2];
            Quaternion rot = Camera.transform.GetChild(0).transform.localRotation;
            Vector3 pos = Camera.transform.GetChild(0).transform.GetChild(0).transform.localPosition;
            Camera.GetComponent<HexMapCamera>().zoomDeltaP = 0.1f;
            Camera.GetComponent<HexMapCamera>().Zooming();
            yield return new WaitForSeconds(1.0f);
            Assert.AreNotEqual(Camera.transform.GetChild(0).transform.localRotation, rot);
            Assert.AreEqual(Camera.transform.GetChild(0).transform.GetChild(0).transform.localPosition, pos);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(Camera);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CheckCameraRightRotation()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject Camera = goA[2];
            Quaternion rot = Camera.transform.localRotation;
            Camera.GetComponent<HexMapCamera>().rotationDeltaP = 0.1f;
            Camera.GetComponent<HexMapCamera>().rotationSpeed = 20.0f;
            Camera.GetComponent<HexMapCamera>().Rotation();
            yield return new WaitForSeconds(1.0f);
            Debug.Log(Camera.transform.localRotation);

            Assert.AreNotEqual(Camera.transform.localRotation, rot);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(Camera);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CheckCameraLeftRotation()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject Camera = goA[2];
            Quaternion rot = Camera.transform.localRotation;
            Camera.GetComponent<HexMapCamera>().rotationDeltaP = -0.1f;
            Camera.GetComponent<HexMapCamera>().rotationSpeed = 20.0f;
            Camera.GetComponent<HexMapCamera>().Rotation();
            yield return new WaitForSeconds(1.0f);
            Debug.Log(Camera.transform.localRotation);

            Assert.AreNotEqual(Camera.transform.localRotation, rot);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(Camera);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CheckCameraOverAngleRotation()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject Camera = goA[2];
            Quaternion rot = Camera.transform.localRotation;
            Camera.GetComponent<HexMapCamera>().rotationDeltaP = 1.0f;
            Camera.GetComponent<HexMapCamera>().rotationSpeed = 500.0f;
            Camera.GetComponent<HexMapCamera>().Rotation();
            yield return new WaitForSeconds(1.0f);
            Debug.Log(Camera.transform.localRotation);

            Assert.AreNotEqual(Camera.transform.localRotation, rot);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(Camera);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CheckCameraMoving()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject Camera = goA[2];
            Quaternion rot = Camera.transform.localRotation;
            Vector3 pos = Camera.transform.localPosition;
            Camera.GetComponent<HexMapCamera>().xDeltaP = 0.1f;
            Camera.GetComponent<HexMapCamera>().zDeltaP = 0.1f;
            Camera.GetComponent<HexMapCamera>().Moving();
            yield return new WaitForSeconds(1.0f);

            Assert.AreNotEqual(Camera.transform.localPosition, pos);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(Camera);
            SceneManager.UnloadScene("Scene");
        }

        [UnityTest]
        public IEnumerator CameraLockedSuccessfully()
        {
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
            yield return new WaitForSeconds(1.0f);
            goA = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject Camera = goA[2];
            HexMapCamera.Locked = true;

            Assert.IsTrue(HexMapCamera.Locked);

            foreach (GameObject g in goA)
            {
                GameObject.Destroy(g);
            }
            GameObject.Destroy(Camera);
            SceneManager.UnloadScene("Scene");
        }
    }
}
