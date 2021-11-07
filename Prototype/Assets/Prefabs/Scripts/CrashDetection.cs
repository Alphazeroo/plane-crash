using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashDetection : MonoBehaviour
{
    ///////////////////////////////////////////////////// 
    /// <summary><c>explosionEffect</c> de�i�keni patlama efekti modelini tutar</summary>
    [SerializeField]
    private GameObject explosionEffect;
    /// <summary><c>decalEffect</c> de�i�keni kaza alan�n�n efekti modelini tutar</summary>
    [SerializeField]
    private GameObject decalEffect;
    /// <summary><c>fireBallEffect</c> de�i�keni alev toplar� efekti modelini tutar</summary>
    [SerializeField]
    private GameObject fireBallEffect;
    /// <summary><c>frictionEffect</c> de�i�keni s�rt�nme efekti modelini tutar</summary>
    [SerializeField]
    private GameObject frictionEffect;
    /// <summary><c>smokeEffect</c> de�i�keni duman efekti modelini tutar</summary>
    [SerializeField]
    private GameObject smokeEffect;
    /// <summary><c>flyEngine</c> de�i�keni u�u� mekani�inin bilgisini tutar</summary>
    [SerializeField]
    private GameObject flyEngine;
    /// <summary><c>wreck</c> de�i�keni u�a��n par�alanma modelini tutar</summary>
    [SerializeField]
    private GameObject wreck;
    /// <summary><c>flyCam</c> de�i�keni mevcut kamera bilgisini tutar</summary>
    [SerializeField]
    private Camera flyCam;
    /// <summary><c>crashView</c> de�i�keni kaza alan�n�n bilgisini tutar</summary>
    private Transform crashView;
    /// <summary><c>normal</c> de�i�keni temas noktas�n�n y�zeye olan normal vekt�r� bilgisini tutar</summary>
    private Vector3 normal;
    /// <summary><c>contactPoint</c> de�i�keni temas noktas�n�n bilgisini tutar</summary>
    private Vector3 contactPoint;
    /// <summary><c>timer</c> de�i�keni kameran�n uzakla�ma s�resini tutar</summary>
    public float timer = 1;
    /// <summary><c>crashTime</c> de�i�keni k�r�lma aksiyonu i�in gereken s�rt�nme s�resini tutar</summary>
    public float crashTime = 3;
    /// <summary><c>fly</c> de�i�keni u�a��n kalk�� bilgisini tutar</summary>
    private bool fly = false;
    /// <summary><c>crash</c> de�i�keni kaza olma bilgisini tutar</summary>
    private bool crash = false;
    /// <summary>
    /// Script �al��t��� s�rece olu�an aksiyonlar
    /// </summary>
    private void Update()
    {
        CameraCrashPosition(flyCam, crashView, 1);
    }
    /// <summary>
    /// U�ak temas etti�inde olu�an aksiyonlar 
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void OnCollisionEnter(Collision collision)
    {
        DetectLending(collision);
    }
    /// <summary>
    /// U�ak temas etti�i s�rece olu�an aksiyonlar
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void OnCollisionStay(Collision collision)
    {
        DetectCrashPoint(collision);
        DetectPlaneCol(collision);
    }
    /// <summary>
    /// U�ak temas� kesti�inde olu�an aksiyonlar 
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void OnCollisionExit(Collision collision)
    {
        crashTime = 3;
        fly = true;
    }
    /// <summary>
    /// U�a��n ini�ini tespit eder
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void DetectLending(Collision collision)
    {
        if (collision.collider.CompareTag("wheels"))
        {
            if (fly == false)
            {
                Debug.Log("Kalk��a haz�r");
            }
            else
            {
                Debug.Log("�ni� Yap�ld�");
            }
        }
    }
    /// <summary>
    /// Etkile�im oldu�unda temas noktas�n� ve normal do�rusunu atama aksiyonu
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void DetectCrashPoint(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            normal = -contact.normal;
            contactPoint = contact.point;
        }
    }
    /// <summary>
    /// U�a��n temas etti�i par�as�na g�re olu�acak aksiyonlar
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void DetectPlaneCol(Collision collision)
    {
        if (collision.collider.CompareTag("rightWing"))
        {
            DetectWingDist(collision, "right", " rightwing");
        }

        if (collision.collider.CompareTag("leftWing"))
        {
            DetectWingDist(collision, "left", " leftwing");
        }

        if (collision.collider.CompareTag("body"))
        {
            DetectExpAction(collision, 75, "body");
        }

        if (collision.collider.CompareTag("brokeWing"))
        {
            DetectExpAction(collision, 90, "brokeWing");
        }
    }
    /// <summary>
    /// U�a��n pozisyonu ile temas noktas� aras�ndaki mesafeyi d�nd�r�r
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private float Distance(Collision collision)
    {
        return Vector3.Distance(collision.gameObject.transform.position, contactPoint);
    }
    /// <summary>
    /// U�a��n do�rultusu ile temas noktas�n�n normal do�rusu aras�ndaki a��y� d�nd�r�r
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private float Angle(Collision collision)
    {
        return Vector3.Angle(collision.gameObject.transform.forward, normal);
    }
    /// <summary>
    /// Temas noktas�n�n u�a�a olan uzakl���na g�re olu�acak k�r�lma veya patlama aksiyonlar�
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="dir">Temas eden kanad�n y�n�</param>
    /// <param name="part">Temas eden kanat</param>
    private void DetectWingDist(Collision collision, string dir, string part)
    {
        if (Distance(collision) > 5.5f)
        {
            DetectBrokeAction(collision, 95, dir, "long");
        }
        else if (Distance(collision) > 2.5f && Distance(collision) < 5.5f)
        {
            DetectBrokeAction(collision, 95, dir, "short");
        }
        else if (Distance(collision) < 2.5f)
        {
            DetectExpAction(collision, 90, part);
        }
    }
    /// <summary>
    /// Temas noktas� ile u�a��n do�rultusu aras�ndaki a��ya g�re olu�acak kanat k�r�lma aksiyonlar�
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="angle">�arpa a��s�</param>
    /// <param name="dir">�arpan kanad�n y�n�</param>
    /// <param name="range">�arpma mesafesinin kanat �zerindeki uzakl���</param>
    private void DetectBrokeAction(Collision collision, float angle, string dir, string range)
    {
        if (Angle(collision) > angle)
        {
            Debug.Log("hit " + dir + "wing " + Angle(collision));
            BrokeWing(collision, dir, range);
        }
        else
        {
            Debug.Log("rub " + dir + "wing " + Angle(collision));
            BrokeWing(collision, dir, range, crashTime);
            FrictionEffect(collision);
        }
    }
    /// <summary>
    /// S�rt�nme olu�tu�u s�rada ��kan k�v�lc�m efekti
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void FrictionEffect(Collision collision)
    {
        GameObject friction = Instantiate(frictionEffect, contactPoint, collision.gameObject.transform.rotation, collision.gameObject.transform);
        Destroy(friction, 0.5f);
    }
    /// <summary>
    /// Kanad�n k�r�lma boyutu ve taraf� ile ilgili t�m aksiyonlar�
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="dir">�arpan kanad�n y�n�</param>
    /// <param name="range">�arpma mesafesinin kanat �zerindeki uzakl���</param>
    private void BrokeWing(Collision collision, string dir, string range)
    {
        DestroyWing(collision, dir, range);
        ReplaceWing(collision, dir + range + "BrokeWing", dir + range + "WingEdge");
        WingSmoke(collision, dir + range + "BrokeWing");
    }
    /// <summary>
    /// Gecikmeli kanat k�r�lma boyutu ve taraf� ile ilgili t�m aksiyonlar
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="dir">�arpan kanad�n y�n�</param>
    /// <param name="range">�arpma mesafesinin kanat �zerindeki uzakl���</param>
    /// <param name="time">Gecikme s�resi</param>
    private void BrokeWing(Collision collision, string dir, string range, float time)
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            DestroyWing(collision, dir, range);
            ReplaceWing(collision, dir + range + "BrokeWing", dir + range + "WingEdge");
            WingSmoke(collision, dir + range + "BrokeWing");
        }
    }
    /// <summary>
    /// Kanad� yok etme i�lemi
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="dir">�arpan kanad�n y�n�</param>
    /// <param name="range">�arpma mesafesinin kanat �zerindeki uzakl���</param>
    private void DestroyWing(Collision collision, string dir, string range)
    {
        if (range == "long")
            Destroy(collision.gameObject.transform.GetChild(0).Find(dir + "Wing").gameObject);
        else if (range == "short")
        {
            Destroy(collision.gameObject.transform.GetChild(0).Find(dir + "Wing").gameObject);
            Destroy(collision.gameObject.transform.GetChild(0).Find("wing_support_" + dir).gameObject);
        }
    }
    /// <summary>
    /// Kanat k�r�ld�ktan sonra k�r�k kanad� ve k�r�lan par�ay� olu�turma i�lemi
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="wing">Olu�turulacak k�r�k kanat</param>
    /// <param name="edge">Olu�turulacak kanat par�as�</param>
    private void ReplaceWing(Collision collision, string wing, string edge)
    {
        collision.gameObject.transform.GetChild(0).Find(wing).gameObject.SetActive(true);
        collision.gameObject.transform.GetChild(0).Find(edge).gameObject.SetActive(true);
        collision.gameObject.transform.GetChild(0).Find(edge).SetParent(null);
    }
    /// <summary>
    /// K�r�lan kanatdan duman ��kartma aksiyonu
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="wing">Olu�turulan kanat</param>
    private void WingSmoke(Collision collision, string wing)
    {
        if (wing == "leftlongBrokeWing")
            SmokeEffect(collision, wing, 5);
        else if (wing == "leftshortBrokeWing")
            SmokeEffect(collision, wing, 1);
        else if (wing == "rightlongBrokeWing")
            SmokeEffect(collision, wing, 5);
        else if (wing == "rightshortBrokeWing")
            SmokeEffect(collision, wing, 1);
    }
    /// <summary>
    /// Duman ��kartma efekti
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="wing">Duman efekti olu�turulacak kanat</param>
    /// <param name="number">Duman ��kacak child nesnenin numaras�</param>
    private void SmokeEffect(Collision collision, string wing, int number)
    {
        Instantiate(smokeEffect, collision.gameObject.transform.GetChild(0).Find(wing).GetChild(number));
    }
    /// <summary>
    /// Temas noktas� ile u�a��n do�rultusu aras�ndaki a��ya g�re olu�acak patlama aksiyonlar�
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="angle">�arpma a��s�</param>
    /// <param name="part">�arpan u�ak b�l�m�</param>
    private void DetectExpAction(Collision collision, float angle, string part)
    {
        if (Angle(collision) > angle)
        {
            Debug.Log("hit " + part + Angle(collision));
            Explode(normal, contactPoint, collision, flyCam, flyEngine);
        }
        else
        {
            Debug.Log("rub " + part + Angle(collision));
            Explode(normal, contactPoint, collision, flyCam, flyEngine, crashTime);
            FrictionEffect(collision);
        }
    }
    /// <summary>
    /// Patlama aksiyonu
    /// </summary>
    /// <param name="normal">�arpma noktas�n�n y�zeye olan normal do�rusu</param>
    /// <param name="contactPoint">�arpma noktas�</param>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="cam">Kamera</param>
    /// <param name="flyEngine">U�u� mekani�i</param>
    private void Explode(Vector3 normal, Vector3 contactPoint, Collision collision, Camera cam, GameObject flyEngine)
    {
        StopFly(collision, cam, flyEngine);
        CrashEffects(normal, contactPoint, collision);
        CrashCamActive(collision);
    }
    /// <summary>
    /// Gecikmeli patlama aksiyonu
    /// </summary>
    /// <param name="normal">�arpma noktas�n�n y�zeye olan normal do�rusu</param>
    /// <param name="contactPoint">�arpma noktas�</param>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="cam">Kamera</param>
    /// <param name="flyEngine">U�u� mekani�i</param>
    /// <param name="time">Gecikme s�resi</param>
    private void Explode(Vector3 normal, Vector3 contactPoint, Collision collision, Camera cam, GameObject flyEngine, float time)
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            StopFly(collision, cam, flyEngine);
            CrashEffects(normal, contactPoint, collision);
            CrashCamActive(collision);
        }        
    }
    /// <summary>
    /// U�u�u durdurma ve u�a�� yok etme aksiyonu
    /// </summary>
    /// <param name="collision">Temas bilgisi</param>
    /// <param name="cam">Kamera</param>
    /// <param name="engine">U�u� mekani�i</param>
    private void StopFly(Collision collision, Camera cam, GameObject engine)
    {
        cam.transform.SetParent(null);
        engine.SetActive(false);
        collision.gameObject.SetActive(false);
    }
    /// <summary>
    /// Kaza an�nda olu�an efektlerin t�m�
    /// </summary>
    /// <param name="normal">�arpma noktas�n�n y�zeye olan normal do�rusu</param>
    /// <param name="contactPoint">�arpma noktas�</param>
    /// <param name="collision">Temas bilgileri</param>
    private void CrashEffects(Vector3 normal, Vector3 contactPoint, Collision collision)
    {
        Wreck(collision);
        ExplosionEffect(collision);
        FireBallEffect(normal, contactPoint, collision);
        DecalEffect(normal, contactPoint, collision);
    }
    /// <summary>
    /// U�a��n enkaz aksiyonu
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void Wreck(Collision collision)
    {
        Instantiate(wreck, collision.transform.position, collision.transform.rotation);
    }
    /// <summary>
    /// Patlama efekti
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void ExplosionEffect(Collision collision)
    {
        Instantiate(explosionEffect, collision.transform.GetChild(0).position, collision.transform.GetChild(0).rotation);
    }
    /// <summary>
    /// Patlama sonucu olu�an alev par�alar� efekti
    /// </summary>
    /// <param name="normal">�arpma noktas�n�n y�zeye olan normal do�rusu</param>
    /// <param name="contactPoint">�arpma noktas�</param>
    /// <param name="collision">Temas bilgileri</param>
    private void FireBallEffect(Vector3 normal, Vector3 contactPoint, Collision collision)
    {
        Instantiate(fireBallEffect, collision.transform.GetChild(0).position, Quaternion.LookRotation(contactPoint, normal));
    }
    /// <summary>
    /// Kaza izi efekti
    /// </summary>
    /// <param name="normal">�arpma noktas�n�n y�zeye olan normal do�rusu</param>
    /// <param name="contactPoint">�arpma noktas�</param>
    /// <param name="collision">Temas bilgileri</param>
    private void DecalEffect(Vector3 normal, Vector3 contactPoint, Collision collision)
    {
        GameObject decal = Instantiate(decalEffect, collision.transform.GetChild(0).position, Quaternion.LookRotation(contactPoint, normal));
        decal.gameObject.transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
        decal.gameObject.transform.Translate(-decal.gameObject.transform.forward * 15, Space.World);
    }
    /// <summary>
    /// Kaza kamera g�r���n� aktifle�tirir.
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void CrashCamActive(Collision collision)
    {
        crashView = collision.transform;
        crash = true;
    }
    /// <summary>
    /// Kamera uzakla��p bir alana bakarak etraf�nda d�n�yor
    /// </summary>
    /// <param name="cam">Kamera</param>
    /// <param name="view">Kameran�n bakaca�� konum</param>
    /// <param name="speed">kamera uzakla�ma h�z�</param>
    private void CameraCrashPosition(Camera cam, Transform view, float speed)
    {
        if (crash == true)
        {
            if (timer > 0)
            {
                cam.transform.LookAt(view);
                cam.transform.position = Vector3.Lerp(cam.transform.position,
                                                      cam.transform.position + new Vector3(30, 30, -30),
                                                      Time.deltaTime * speed);
                timer -= Time.deltaTime * speed;
            }
            else
            {
                cam.transform.RotateAround(view.position, Vector3.up, -20 * Time.deltaTime);
                cam.transform.LookAt(view);
            }
        }
    }
}