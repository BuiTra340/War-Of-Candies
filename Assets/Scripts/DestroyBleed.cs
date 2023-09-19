using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBleed : MonoBehaviour
{
    
    public void DesBleedEffect()
    {
        if(this.gameObject.activeInHierarchy)
        {
            Destroy(this.gameObject);
        }
    }
}
