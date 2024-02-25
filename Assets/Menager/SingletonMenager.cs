using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SingletonMenager<T> : MonoBehaviour where T : Component
{
    static T instance;

    public static T Instance 
    {
        get 
        {
            if (instance == null) 
            {
                instance = FindObjectOfType<T>();


                if (instance == null) 
                {
                    GameObject gameObject = new GameObject("New Meanger");
                    instance = gameObject.AddComponent<T>();
                }
            }

            return instance;
        }

        
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
        }
        else 
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }
}
