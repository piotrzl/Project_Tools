using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InteractiveHandle : MonoBehaviour
{
    [SerializeField] InteractiveObject interactiveObject;
    public InteractiveObject InteractiveObject => interactiveObject;
}
