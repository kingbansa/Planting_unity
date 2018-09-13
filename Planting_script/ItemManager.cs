using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{

    string st;
    Renderer mt;
    string state = "enable";
    bool boxOpen;
    private Animator boxAnimator;
    public GameObject BalloonPosition;
    public GameObject WBalloon;
    public GameObject FBalloon;
    public GameObject NBalloon;
    public GameObject SBalloon;
    public GameObject SunflowerSeedBalloon;
    public GameObject CactusSeedBalloon;
    public GameObject TomatoSeedBalloon;
    public Transform treasureBoxTr;
    public Transform playerTr;
    private float dist;
    public SphereCollider treasureBoxColl;
    private const int maxVal = 1000;
    private float code;
    public string userName, pass, wItem, fItem, sItem, nItem, sfsItem, csItem, tsItem;
    // 각각 사용자 아이디, 패스워드, 물, 비료, 태양석, 영양제, 해바라기씨, 선인장씨, 토마토씨 
    public AudioClip boxOpenSound;
    AudioSource boxSound;


    // Use this for initialization
    void Start()
    {
        st = "아이템 습득 성공";
        mt = GetComponent<Renderer>();
        mt.material.color = Color.blue;
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        treasureBoxColl = this.gameObject.GetComponent<SphereCollider>();
        treasureBoxTr = this.gameObject.GetComponent<Transform>();
        dist = Vector3.Distance(treasureBoxTr.position, playerTr.position);
        boxSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound()
    {
        boxSound.PlayOneShot(boxOpenSound);
    }
    /*private void OnTriggerStay(Collider other)
    {
        //Debug.Log("부딪힌애 = " + other.tag);
        if (other.tag == "Player")
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("부딪힌애 = " + other.tag);
                Debug.Log("클릭됨");
                boxAnimator = GetComponent<Animator>();
                boxAnimator.SetBool("boxOpen", true);
                StartCoroutine(ItemEnable());
            }
        }
    } */

    //문제점 ontriggerstay로 실행하면 한번 클릭만으로 플레이어랑 부딪히고 있는 상자 다 열림
    //또 위 방식을 사용하려면 플레이어에 rigidBody랑 Collider 추가하고 is trigger랑 is kinematic 체크 해줘야 한다
    //OnMouseDown으로 실행하면 클릭한 위치가 트레져박스의 콜리전 안쪽에있는 트레저박스만 열림. 즉 클릭한것만 열림  다만 debug이용해서 확인한 결과 
    //플레이어가 트러저박스 콜라이더 내부에 있음에도 둘 사이의 거리가 콜라이더의 radius 값보다 크게 나온다


    public void OnMouseDown()
    {
        dist = Vector3.Distance(treasureBoxTr.position, playerTr.position);
        Debug.Log("눌림, 플레이어와의 거리 = " + dist.ToString() + "  radius = " + treasureBoxColl.radius.ToString());
        if (dist < treasureBoxColl.radius * 7.0f)
        {
            boxAnimator = GetComponent<Animator>();
            boxAnimator.SetBool("boxOpen", true);
            StartCoroutine(ItemEnable());
        }
    }

    IEnumerator ItemEnable()
    {
        if (state == "enable")
        {
            Debug.Log(st);
            //mt.material.color = Color.red;
            state = "disable";
            code = (float)Random.Range(1, 19000) / maxVal;
            PlaySound();
            if (0 <= code && code < 4)
            {
                loginScript.Instance.GetItem("wItem");
                loginScript.Instance.ItemCountCheck(userName, wItem);
                yield return new WaitForSeconds(1.0f);
                GameObject WBalloonP = Instantiate(WBalloon, BalloonPosition.transform.position, transform.rotation) as GameObject;
                WBalloonP.transform.parent = BalloonPosition.transform;
                WBalloonP.transform.localPosition = transform.localPosition;
                
            }

            else if (4 <= code && code < 8)
            {
                loginScript.Instance.GetItem("fItem");
                loginScript.Instance.ItemCountCheck(userName, fItem);
                yield return new WaitForSeconds(1.0f);
                GameObject FBalloonP = Instantiate(FBalloon, BalloonPosition.transform.position, transform.rotation) as GameObject;
                FBalloonP.transform.parent = BalloonPosition.transform;
                FBalloonP.transform.localPosition = transform.localPosition;
            }
            else if (8 <= code && code < 12)
            {
                loginScript.Instance.GetItem("sItem");
                loginScript.Instance.ItemCountCheck(userName, sItem);
                yield return new WaitForSeconds(1.0f);
                GameObject SBalloonP = Instantiate(SBalloon, BalloonPosition.transform.position, transform.rotation) as GameObject;
                SBalloonP.transform.parent = BalloonPosition.transform;
                SBalloonP.transform.localPosition = transform.localPosition;
            }
            else if (12 <= code && code < 13)
            {
                loginScript.Instance.GetItem("nItem");
                loginScript.Instance.ItemCountCheck(userName, nItem);
                yield return new WaitForSeconds(1.0f);
                GameObject NBalloonP = Instantiate(NBalloon, BalloonPosition.transform.position, transform.rotation) as GameObject;
                NBalloonP.transform.parent = BalloonPosition.transform;
                NBalloonP.transform.localPosition = transform.localPosition;
            }
            else if (13 <= code && code < 16)
            {
                loginScript.Instance.GetItem("sfsItem");
                loginScript.Instance.ItemCountCheck(userName, sfsItem);
                yield return new WaitForSeconds(1.0f);
                GameObject SunflowerSeedBalloonP = Instantiate(SunflowerSeedBalloon, BalloonPosition.transform.position, transform.rotation) as GameObject;
                SunflowerSeedBalloonP.transform.parent = BalloonPosition.transform;
                SunflowerSeedBalloonP.transform.localPosition = transform.localPosition;
            }
            else if (16 <= code && code < 18)
            {
                loginScript.Instance.GetItem("csItem");
                loginScript.Instance.ItemCountCheck(userName, csItem);
                yield return new WaitForSeconds(1.0f);
                GameObject CactusSeedBalloonP = Instantiate(CactusSeedBalloon, BalloonPosition.transform.position, transform.rotation) as GameObject;
                CactusSeedBalloonP.transform.parent = BalloonPosition.transform;
                CactusSeedBalloonP.transform.localPosition = transform.localPosition;
            }

            else if (18 <= code && code < 19)
            {
                loginScript.Instance.GetItem("tsItem");
                loginScript.Instance.ItemCountCheck(userName, tsItem);
                yield return new WaitForSeconds(1.0f);
                GameObject TomatoSeedBalloonP = Instantiate(TomatoSeedBalloon, BalloonPosition.transform.position, transform.rotation) as GameObject;
                TomatoSeedBalloonP.transform.parent = BalloonPosition.transform;
                TomatoSeedBalloonP.transform.localPosition = transform.localPosition;

            }

            yield return new WaitForSeconds(3.0f);
            boxAnimator.SetBool("boxOpen", false);
            state = "enable";
            //mt.material.color = Color.blue;
        }
    }
}
