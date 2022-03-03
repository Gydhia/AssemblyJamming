using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public Camera charCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interract"))
        {
            var ray = charCamera.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hit;
            // Shot ray to find object to pick
            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                // Check if object is pickable
                Component[] allComponents = new Component[5];
                allComponents = hit.transform.GetComponents<Component>();
                
                foreach(Component comp in allComponents)
                {
                    if(comp.GetType() == typeof(PickableItem))
                        PickItem((PickableItem)comp);
                    
                    else if(comp.GetType() == typeof(ActivableItem))
                        ActivateItem((ActivableItem)comp);
                }
            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            var ray = charCamera.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                if(hit.transform.tag == "CodeLockButton")
                {
                    DigicodeManager.Instance.EnterredNumber(int.Parse(hit.transform.name));
                }
            }
        }
    }
    private void PickItem(PickableItem item)
    {
        SoundManager.Instance.PlaySound(SOUND_ID.PickUpItem);
        GameManager.Instance.AddItem(item.item);
        Destroy(item.transform.gameObject);
    }
    private void ActivateItem(ActivableItem item)
    {
        item.Execute();
    }
}
