using UnityEngine;
using UnityEngine.UI;

public class SetAllVrUi : MonoBehaviour
{
    public Material vrMat;
    
    
    // Start is called before the first frame update
    void Start()
    {
        MaskableGraphic[] allMg = transform.GetComponentsInChildren<MaskableGraphic>(true);
        foreach (var item in allMg)
        {
            item.material = vrMat;
        }
    }

}
