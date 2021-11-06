using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitPlane : MonoBehaviour
{
    /// <summary><c>minForce</c> deðiþkeni patlama etkisinin olasý minimum etkisini tutar</summary>
    public float minForce;
    /// <summary><c>maxForce</c> deðiþkeni patlama etkisinin olasý maksimum etkisini tutar</summary>
    public float maxForce;
    /// <summary><c>radius</c> deðiþkeni patlama alanýnýn yarýçapýný tutar </summary>
    public float radius;
    /// <summary><c>force</c>uçakla ayný doðrultuda olan uçaðýn arka tarafýnda bulunan child nesnesidir</summary>
    public GameObject force;
    /// <summary><c>fireEffect</c> ateþ çýkarma modelini tutar</summary>
    public GameObject fireEffect;
    /// <summary>
    /// Script çalýþtýðýnda oluþan aksiyonlar
    /// </summary>
    void Start()
    {
        Exp();
        Fire();
    }
    /// <summary>
    /// Script çalýþtýðý sürece oluþan aksiyonlar
    /// </summary>
    private void Update()
    {
        FirePiece();
    }
    /// <summary>
    /// Uçak parçalarýnýn alevlerini konumlandýrýr
    /// </summary>
    private void FirePiece()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            if (transform.GetChild(i).CompareTag("fire"))
            {
                transform.GetChild(i).GetChild(0).localPosition = Vector3.zero;
                transform.GetChild(i).GetChild(0).rotation = Quaternion.Euler(Vector3.up);
            }
        }
    }
    /// <summary>
    /// Uçaðýn daðýlan parçalarýndan rastgele bazýlarýna alev efekti verir
    /// </summary>
    private void Fire()
    {
        for (int i = 0; i < transform.childCount-1; i++)
        {          
            if (Random.Range(1, 3) == 1)
            {
               GameObject smokeGameobject = Instantiate(fireEffect, Vector3.zero, Quaternion.Euler(Vector3.up));
               smokeGameobject.transform.SetParent(transform.GetChild(i));
               transform.GetChild(i).tag = "fire";
            }                      
        }
    }
    /// <summary>
    /// Uçaðýn daðýlan parçalarýna patlama ve uçuþ yönüne doðru kuvvet verir
    /// </summary>
    private void Exp()
    {
        foreach (Transform t in transform)
        {
            var rb = t.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.AddForce(3 * force.transform.forward, ForceMode.Impulse);
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
            }

        }
    } 
}