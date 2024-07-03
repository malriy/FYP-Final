using UnityEngine;
using System.Collections.Generic;

public class DontDestroyOnLoadManager : MonoBehaviour
{
    public void DestroySpecificDontDestroyOnLoadObjects()
    {
        // List of specific objects to destroy
        List<string> objectsToDestroy = new List<string> { "UICanvas", "Player", "GameController", "Managers" };

        GameObject temp = null;
        try
        {
            temp = new GameObject();
            DontDestroyOnLoad(temp);

            List<GameObject> dontDestroyOnLoadObjects = new List<GameObject>();

            // Find all root objects in DontDestroyOnLoad
            foreach (GameObject go in temp.scene.GetRootGameObjects())
            {
                if (go != temp && objectsToDestroy.Contains(go.name))
                {
                    dontDestroyOnLoadObjects.Add(go);
                }
            }

            // Destroy all specific objects found
            foreach (GameObject go in dontDestroyOnLoadObjects)
            {
                Destroy(go);
            }
        }
        finally
        {
            if (temp != null)
            {
                Destroy(temp);
            }
        }
    }
}
