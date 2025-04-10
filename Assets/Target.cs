using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
   [SerializeField] private Material goHitMaterial;

   private void OnCollisionEnter(Collision collision)
   {
      if (collision.gameObject.CompareTag("Bullet"))
      {
         GetComponent<MeshRenderer>().material = goHitMaterial;
      }
   }
   
}
