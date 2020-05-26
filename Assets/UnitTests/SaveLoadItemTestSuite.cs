using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SaveLoadItemTestSuite
    {
        [UnityTest]
        public IEnumerator MapNameIsSetCorrectly()
        {
            GameObject obj1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Save Load Item"));
            SaveLoadItem sli = obj1.GetComponent<SaveLoadItem>();
            sli.MapName = "Map1";

            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(sli.MapName, "Map1");

            GameObject.Destroy(obj1);
            GameObject.Destroy(sli);
        }

        [UnityTest]
        public IEnumerator MapNameIsCorrectlyPassedToMenu()
        {
            GameObject obj1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Save Load Item"));
            SaveLoadItem sli = obj1.GetComponent<SaveLoadItem>();
            sli.MapName = "Map1";
            sli.Select();

            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(sli.menu.nameInput.text, "Map1");

            GameObject.Destroy(obj1);
            GameObject.Destroy(sli);
        }
    }
}
