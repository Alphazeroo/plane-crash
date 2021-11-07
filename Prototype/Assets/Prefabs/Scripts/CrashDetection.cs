using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashDetection : MonoBehaviour
{
    ///////////////////////////////////////////////////// 
    /// <summary><c>explosionEffect</c> deðiþkeni patlama efekti modelini tutar</summary>
    [SerializeField]
    private GameObject explosionEffect;
    /// <summary><c>decalEffect</c> deðiþkeni kaza alanýnýn efekti modelini tutar</summary>
    [SerializeField]
    private GameObject decalEffect;
    /// <summary><c>fireBallEffect</c> deðiþkeni alev toplarý efekti modelini tutar</summary>
    [SerializeField]
    private GameObject fireBallEffect;
    /// <summary><c>frictionEffect</c> deðiþkeni sürtünme efekti modelini tutar</summary>
    [SerializeField]
    private GameObject frictionEffect;
    /// <summary><c>smokeEffect</c> deðiþkeni duman efekti modelini tutar</summary>
    [SerializeField]
    private GameObject smokeEffect;
    /// <summary><c>flyEngine</c> deðiþkeni uçuþ mekaniðinin bilgisini tutar</summary>
    [SerializeField]
    private GameObject flyEngine;
    /// <summary><c>wreck</c> deðiþkeni uçaðýn parçalanma modelini tutar</summary>
    [SerializeField]
    private GameObject wreck;
    /// <summary><c>flyCam</c> deðiþkeni mevcut kamera bilgisini tutar</summary>
    [SerializeField]
    private Camera flyCam;
    /// <summary><c>crashView</c> deðiþkeni kaza alanýnýn bilgisini tutar</summary>
    private Transform crashView;
    /// <summary><c>normal</c> deðiþkeni temas noktasýnýn yüzeye olan normal vektörü bilgisini tutar</summary>
    private Vector3 normal;
    /// <summary><c>contactPoint</c> deðiþkeni temas noktasýnýn bilgisini tutar</summary>
    private Vector3 contactPoint;
    /// <summary><c>timer</c> deðiþkeni kameranýn uzaklaþma süresini tutar</summary>
    public float timer = 1;
    /// <summary><c>crashTime</c> deðiþkeni kýrýlma aksiyonu için gereken sürtünme süresini tutar</summary>
    public float crashTime = 3;
    /// <summary><c>fly</c> deðiþkeni uçaðýn kalkýþ bilgisini tutar</summary>
    private bool fly = false;
    /// <summary><c>crash</c> deðiþkeni kaza olma bilgisini tutar</summary>
    private bool crash = false;
    /// <summary>
    /// Script çalýþtýðý sürece oluþan aksiyonlar
    /// </summary>
    private void Update()
    {
        CameraCrashPosition(flyCam, crashView, 1);
    }
    /// <summary>
    /// Uçak temas ettiðinde oluþan aksiyonlar 
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void OnCollisionEnter(Collision collision)
    {
        DetectLending(collision);
    }
    /// <summary>
    /// Uçak temas ettiði sürece oluþan aksiyonlar
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void OnCollisionStay(Collision collision)
    {
        DetectCrashPoint(collision);
        DetectPlaneCol(collision);
    }
    /// <summary>
    /// Uçak temasý kestiðinde oluþan aksiyonlar 
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void OnCollisionExit(Collision collision)
    {
        crashTime = 3;
        fly = true;
    }
    /// <summary>
    /// Uçaðýn iniþini tespit eder
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void DetectLending(Collision collision)
    {
        if (collision.collider.CompareTag("wheels"))
        {
            if (fly == false)
            {
                Debug.Log("Kalkýþa hazýr");
            }
            else
            {
                Debug.Log("Ýniþ Yapýldý");
            }
        }
    }
    /// <summary>
    /// Etkileþim olduðunda temas noktasýný ve normal doðrusunu atama aksiyonu
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
    /// Uçaðýn temas ettiði parçasýna göre oluþacak aksiyonlar
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
    /// Uçaðýn pozisyonu ile temas noktasý arasýndaki mesafeyi döndürür
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private float Distance(Collision collision)
    {
        return Vector3.Distance(collision.gameObject.transform.position, contactPoint);
    }
    /// <summary>
    /// Uçaðýn doðrultusu ile temas noktasýnýn normal doðrusu arasýndaki açýyý döndürür
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private float Angle(Collision collision)
    {
        return Vector3.Angle(collision.gameObject.transform.forward, normal);
    }
    /// <summary>
    /// Temas noktasýnýn uçaða olan uzaklýðýna göre oluþacak kýrýlma veya patlama aksiyonlarý
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="dir">Temas eden kanadýn yönü</param>
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
    /// Temas noktasý ile uçaðýn doðrultusu arasýndaki açýya göre oluþacak kanat kýrýlma aksiyonlarý
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="angle">Çarpa açýsý</param>
    /// <param name="dir">Çarpan kanadýn yönü</param>
    /// <param name="range">Çarpma mesafesinin kanat üzerindeki uzaklýðý</param>
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
    /// Sürtünme oluþtuðu sýrada çýkan kývýlcým efekti
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void FrictionEffect(Collision collision)
    {
        GameObject friction = Instantiate(frictionEffect, contactPoint, collision.gameObject.transform.rotation, collision.gameObject.transform);
        Destroy(friction, 0.5f);
    }
    /// <summary>
    /// Kanadýn kýrýlma boyutu ve tarafý ile ilgili tüm aksiyonlarý
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="dir">Çarpan kanadýn yönü</param>
    /// <param name="range">Çarpma mesafesinin kanat üzerindeki uzaklýðý</param>
    private void BrokeWing(Collision collision, string dir, string range)
    {
        DestroyWing(collision, dir, range);
        ReplaceWing(collision, dir + range + "BrokeWing", dir + range + "WingEdge");
        WingSmoke(collision, dir + range + "BrokeWing");
    }
    /// <summary>
    /// Gecikmeli kanat kýrýlma boyutu ve tarafý ile ilgili tüm aksiyonlar
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="dir">Çarpan kanadýn yönü</param>
    /// <param name="range">Çarpma mesafesinin kanat üzerindeki uzaklýðý</param>
    /// <param name="time">Gecikme süresi</param>
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
    /// Kanadý yok etme iþlemi
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="dir">Çarpan kanadýn yönü</param>
    /// <param name="range">Çarpma mesafesinin kanat üzerindeki uzaklýðý</param>
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
    /// Kanat kýrýldýktan sonra kýrýk kanadý ve kýrýlan parçayý oluþturma iþlemi
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="wing">Oluþturulacak kýrýk kanat</param>
    /// <param name="edge">Oluþturulacak kanat parçasý</param>
    private void ReplaceWing(Collision collision, string wing, string edge)
    {
        collision.gameObject.transform.GetChild(0).Find(wing).gameObject.SetActive(true);
        collision.gameObject.transform.GetChild(0).Find(edge).gameObject.SetActive(true);
        collision.gameObject.transform.GetChild(0).Find(edge).SetParent(null);
    }
    /// <summary>
    /// Kýrýlan kanatdan duman çýkartma aksiyonu
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="wing">Oluþturulan kanat</param>
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
    /// Duman çýkartma efekti
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="wing">Duman efekti oluþturulacak kanat</param>
    /// <param name="number">Duman çýkacak child nesnenin numarasý</param>
    private void SmokeEffect(Collision collision, string wing, int number)
    {
        Instantiate(smokeEffect, collision.gameObject.transform.GetChild(0).Find(wing).GetChild(number));
    }
    /// <summary>
    /// Temas noktasý ile uçaðýn doðrultusu arasýndaki açýya göre oluþacak patlama aksiyonlarý
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="angle">Çarpma açýsý</param>
    /// <param name="part">Çarpan uçak bölümü</param>
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
    /// <param name="normal">Çarpma noktasýnýn yüzeye olan normal doðrusu</param>
    /// <param name="contactPoint">Çarpma noktasý</param>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="cam">Kamera</param>
    /// <param name="flyEngine">Uçuþ mekaniði</param>
    private void Explode(Vector3 normal, Vector3 contactPoint, Collision collision, Camera cam, GameObject flyEngine)
    {
        StopFly(collision, cam, flyEngine);
        CrashEffects(normal, contactPoint, collision);
        CrashCamActive(collision);
    }
    /// <summary>
    /// Gecikmeli patlama aksiyonu
    /// </summary>
    /// <param name="normal">Çarpma noktasýnýn yüzeye olan normal doðrusu</param>
    /// <param name="contactPoint">Çarpma noktasý</param>
    /// <param name="collision">Temas bilgileri</param>
    /// <param name="cam">Kamera</param>
    /// <param name="flyEngine">Uçuþ mekaniði</param>
    /// <param name="time">Gecikme süresi</param>
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
    /// Uçuþu durdurma ve uçaðý yok etme aksiyonu
    /// </summary>
    /// <param name="collision">Temas bilgisi</param>
    /// <param name="cam">Kamera</param>
    /// <param name="engine">Uçuþ mekaniði</param>
    private void StopFly(Collision collision, Camera cam, GameObject engine)
    {
        cam.transform.SetParent(null);
        engine.SetActive(false);
        collision.gameObject.SetActive(false);
    }
    /// <summary>
    /// Kaza anýnda oluþan efektlerin tümü
    /// </summary>
    /// <param name="normal">Çarpma noktasýnýn yüzeye olan normal doðrusu</param>
    /// <param name="contactPoint">Çarpma noktasý</param>
    /// <param name="collision">Temas bilgileri</param>
    private void CrashEffects(Vector3 normal, Vector3 contactPoint, Collision collision)
    {
        Wreck(collision);
        ExplosionEffect(collision);
        FireBallEffect(normal, contactPoint, collision);
        DecalEffect(normal, contactPoint, collision);
    }
    /// <summary>
    /// Uçaðýn enkaz aksiyonu
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
    /// Patlama sonucu oluþan alev parçalarý efekti
    /// </summary>
    /// <param name="normal">Çarpma noktasýnýn yüzeye olan normal doðrusu</param>
    /// <param name="contactPoint">Çarpma noktasý</param>
    /// <param name="collision">Temas bilgileri</param>
    private void FireBallEffect(Vector3 normal, Vector3 contactPoint, Collision collision)
    {
        Instantiate(fireBallEffect, collision.transform.GetChild(0).position, Quaternion.LookRotation(contactPoint, normal));
    }
    /// <summary>
    /// Kaza izi efekti
    /// </summary>
    /// <param name="normal">Çarpma noktasýnýn yüzeye olan normal doðrusu</param>
    /// <param name="contactPoint">Çarpma noktasý</param>
    /// <param name="collision">Temas bilgileri</param>
    private void DecalEffect(Vector3 normal, Vector3 contactPoint, Collision collision)
    {
        GameObject decal = Instantiate(decalEffect, collision.transform.GetChild(0).position, Quaternion.LookRotation(contactPoint, normal));
        decal.gameObject.transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
        decal.gameObject.transform.Translate(-decal.gameObject.transform.forward * 15, Space.World);
    }
    /// <summary>
    /// Kaza kamera görüþünü aktifleþtirir.
    /// </summary>
    /// <param name="collision">Temas bilgileri</param>
    private void CrashCamActive(Collision collision)
    {
        crashView = collision.transform;
        crash = true;
    }
    /// <summary>
    /// Kamera uzaklaþýp bir alana bakarak etrafýnda dönüyor
    /// </summary>
    /// <param name="cam">Kamera</param>
    /// <param name="view">Kameranýn bakacaðý konum</param>
    /// <param name="speed">kamera uzaklaþma hýzý</param>
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