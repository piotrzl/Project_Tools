using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //  public UnityEvent<InteractiveObject> NewInteractiveObjectEvent;
    //public Event<InteractiveObject> NewInteractiveObjectEvent1;
    [Header("Components")]
    [SerializeField] PlayerControler playerControler;
    [Header("Interaction Settings")]
    [SerializeField] LayerMask objectLayer;
    [SerializeField] float objectInteractRange = 1f;
    [SerializeField] InteractiveObject interactObject;
    [SerializeField] InteractiveHandle InteractiveHandle;

    public InteractiveObject InteractiveObject => interactObject;
    bool startinteract = false;
    float interactionCharging = 0;

    Transform nextTargetToCheck;

    public void FixedUpdate()
    {
        //RaycastHit InteractionTarget = FindInteractionTarget();

        FindObjectToInteract();

        if (startinteract)
            LoadingInteract();
    }

   
  
    public void FindObjectToInteract() 
    {
        Ray ray = new Ray(playerControler.Camera.transform.position, playerControler.Camera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, objectInteractRange, objectLayer))
        {

            if (!interactObject || hitInfo.transform != interactObject.transform || !InteractiveHandle || hitInfo.transform != InteractiveHandle.transform)
            {
                if (hitInfo.transform.TryGetComponent(out InteractiveObject IO))
                {
                    interactObject = IO;
                    InteractiveHandle = null;
                    playerControler.NewInteractoveObject(interactObject);
                }
                else if (hitInfo.transform.TryGetComponent(out InteractiveHandle IH) && IH.InteractiveObject)
                {
                    InteractiveHandle = IH;
                    interactObject = IH.InteractiveObject;
                    playerControler.NewInteractoveObject(interactObject);
                }
                
            }
        }
        else if(interactObject)
        {
            interactObject = null;
            InteractiveHandle = null;
            playerControler.NewInteractoveObject(null);
            EndInteractWithObject();
        }
    }

    public void SetInteraction(ItemObject item) 
    {
        if(playerControler.PlayerHands.currentItem)
        {
           int slot = playerControler.PlayerItemEQ.FindFreeSlot();

            playerControler.PlayerHands.PutItemToEQ(playerControler.PlayerItemEQ, slot);

            playerControler.PlayerHands.AddItem(item);
        }

        playerControler.PlayerHands.AddItem(item);
    }

    public void StartInteractWithObject()
    {
        if (interactObject) 
        {
            if (interactObject.GetInteractionTime() > 0)
            {
                interactionCharging = Time.deltaTime;
                startinteract = true;
            }
            else
            {
                interactObject.Interact(this);
                startinteract = false;
            }
        }
    }

    public void LoadingInteract() 
    {
        if (interactObject) 
        {
            interactionCharging += Time.deltaTime;

            if (interactionCharging / interactObject.GetInteractionTime() > 1f)
            {
                interactObject.Interact(this);
                EndInteractWithObject();
            }
            else
                playerControler.UpdateInteract(interactionCharging / interactObject.GetInteractionTime());
        }
        else 
        {
            EndInteractWithObject();
        }
    }


    public void EndInteractWithObject()
    {
        interactionCharging = -1;
        startinteract = false;
        playerControler.UpdateInteract(interactionCharging);
    }


    public bool TryLookAtObject(GameObject targetObject) 
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerControler.Camera);
        Vector3 point = targetObject.transform.position;

        foreach (Plane plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        return true;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out InteractiveArea area)) 
        {
             area.InteractOnEnter(this);

            if (area.UseOnStay)
                lastEnterArea = area;
        }
    }

    void OnTriggerExit(Collider other)
    {
     //   InteractiveArea area = other.gameObject.GetComponent<InteractiveArea>();
        if (other.gameObject.TryGetComponent(out InteractiveArea area))
        {
            area.InteractOnExit(this);
        }
    }

    InteractiveArea lastEnterArea;
    private void OnTriggerStay(Collider other)
    {
       if(lastEnterArea && lastEnterArea.gameObject == other.gameObject) 
       {
           lastEnterArea.InteractOnStay(this);
       }
       else if (other.gameObject.TryGetComponent(out InteractiveArea area))
       {
            area.InteractOnStay(this);
       }
    }
}
