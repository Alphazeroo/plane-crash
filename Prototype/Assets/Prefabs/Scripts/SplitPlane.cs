using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitPlane : MonoBehaviour
{
    /// <summary><c>minForce</c> de�i�keni patlama etkisinin olas� minimum etkisini tutar</summary>
    public float minForce;
    /// <summary><c>maxForce</c> de�i�keni patlama etkisinin olas� maksimum etkisini tutar</summary>
    public float maxForce;
    /// <summary><c>radius</c> de�i�keni patlama alan�n�n yar��ap�n� tutar </summary>
    public float radius;
    /// <summary><c>force</c>u�akla ayn� do�rultuda olan u�a��n arka taraf�nda bulunan child nesnesidir</summary>
    public GameObject force;
    /// <summary><c>fireEffect</c> ate� ��karma modelini tutar</summary>
    public GameObject fireEffect;
    /// <summary>
    /// Script �al��t���nda olu�an aksiyonlar
    /// </summary>
    void Start()
    {
        Exp();
        Fire();
    }
    /// <summary>
    /// Script �al��t��� s�rece olu�an aksiyonlar
    /// </summary>
    private void Update()
    {
        FirePiece();
    }
    /// <summary>
    /// U�ak par�alar�n�n alevlerini konumland�r�r
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
    /// U�a��n da��lan par�alar�ndan rastgele baz�lar�na alev efekti verir
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
    /// U�a��n da��lan par�alar�na patlama ve u�u� y�n�ne do�ru kuvvet verir
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