using System.Collections;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public CollectableItem item;

    MeshRenderer meshRenderer;
    BoxCollider boxCollider;
    CapsuleCollider capsuleCollider;
    public bool isDestroy = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void Destroy()
    {
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        if (boxCollider != null)
            boxCollider.enabled = false;
        if (capsuleCollider != null)
            capsuleCollider.enabled = false;

        StartCoroutine(ActiveTime());
    }

    IEnumerator ActiveTime()
    {
        yield return new WaitForSeconds(1.8f);
        if(meshRenderer!= null )
        {
            meshRenderer.enabled = true;
        }
        if (boxCollider != null)
            boxCollider.enabled = true;
        if (capsuleCollider != null)
            capsuleCollider.enabled = true;
    }
}
