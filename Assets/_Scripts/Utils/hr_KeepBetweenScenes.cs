using UnityEngine;

public class hr_KeepBetweenScenes : MonoBehaviour
{
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(this.gameObject.tag);
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}
