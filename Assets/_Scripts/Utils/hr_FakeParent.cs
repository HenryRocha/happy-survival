using UnityEngine;

public class hr_FakeParent : MonoBehaviour
{
    [SerializeField] private Transform fakeParent;

    private Vector3 pos, fw, up;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        pos = fakeParent.transform.InverseTransformPoint(transform.position);
        fw = fakeParent.transform.InverseTransformDirection(transform.forward);
        up = fakeParent.transform.InverseTransformDirection(transform.up);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        var newpos = fakeParent.transform.TransformPoint(pos);
        var newfw = fakeParent.transform.TransformDirection(fw);
        var newup = fakeParent.transform.TransformDirection(up);
        var newrot = Quaternion.LookRotation(newfw, newup);
        transform.position = newpos;
        transform.rotation = newrot;
    }
}
