using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_info : MonoBehaviour
{
    public int Hp;
    public string name;

    public GameObject My_Obj;
    public GameObject[] Enemy_Objs;
    public GameObject Enemy_obj;
    public GameObject Target_obj;

    public Transform My_Trans;
    public Transform Enemy_Trans;
    public Transform Target_Trans;

    public float distance_from_enemy;
    public float distance_from_castle;

    enum state { Stop, Walk, S_Attack, L_Attack };
    state enemy_state;

    UnityEngine.AI.NavMeshAgent agent;

    public float attackrate = 1.0f;
    private float nextattack = 0.0f;

    private static Enemy1_info instance;
    public static Enemy1_info Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        My_Obj = this.gameObject;
        My_Trans = this.GetComponent<Transform>();
        Target_obj = GameObject.Find("EnemyCastle");
        Target_Trans = Target_obj.GetComponent<Transform>();
    }

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(Target_Trans.position);
        agent.isStopped = false;
        Debug.Log("" + Target_Trans.position);
    }

    public void DestroyMyObj()
    {
        DestroyObject(this.gameObject);
    }

    public void attack(GameObject obj)
    {
        obj.GetComponent<Enemy2_info>().Hp -= 10;
    }

    public void attack1(GameObject obj)
    {
        obj.GetComponent<EnemyCastle>().Hp -= 50;
    }

    public void Object_state(string enemy_state, GameObject obj)
    {
        switch (enemy_state)
        {
            case "S_Attack":
                if (Time.time > nextattack)
                {
                    nextattack = Time.time + attackrate;
                    attack(obj);
                    Debug.Log("Enemy2_Hp = " + obj.GetComponent<Enemy2_info>().Hp);
                }
                break;

            case "SS_Attack":
                if (Time.time > nextattack)
                {
                    nextattack = Time.time + attackrate;
                    attack1(obj);
                    Debug.Log("EnemyCastle Hp " + obj.GetComponent<EnemyCastle>().Hp);
                }
                break;
            case "Nothing":
                break;
            default:
                break;
        }
    }

    void Find_Castle()
    {
        Target_obj = GameObject.FindWithTag("EnemyCastle");
        distance_from_castle = Vector3.Distance(Target_obj.transform.position, this.transform.position);
        if(distance_from_castle <= 4.0f)
        {
            agent.isStopped = true;
            Object_state("SS_Attack", Target_obj);
            if (Target_obj.GetComponent<EnemyCastle>().Hp == 0)
            {
                loginScript.Instance.SendDestroyCastle();
                EnemyCastle.Instance.Enemy_Castle.SetActive(false);
                agent.isStopped = false;
            }
        }
    }

    void Find_Near_Target()
    {
        float shortestDistance;
        Enemy_Objs = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject obj in Enemy_Objs)
        {
            distance_from_enemy = Vector3.Distance(obj.transform.position, this.transform.position);
            if (distance_from_enemy <= 6.0f)
            {
                Enemy_obj = obj;
                agent.isStopped = true;
                shortestDistance = distance_from_enemy;
                Object_state("S_Attack", Enemy_obj);
                if (obj.GetComponent<Enemy2_info>().Hp == 0)
                {
                    loginScript.Instance.SendDestroyOtherObject();
                    //DestroyObject(obj);
                    Enemy2_info.Instance.My_Obj.SetActive(false);
                    agent.isStopped = false;
                }
            }
            else
            {
                agent.isStopped = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Find_Near_Target();
        Find_Castle();
        if (Hp == 0) //나의 오브젝트가 HP가 0이 되었을때.
        {
            loginScript.Instance.SendDestroyOtherObject(); //내 오브젝트 깨졌다고 서버로 메세지를 보내 서버에서는 이제 나의 화면의 내 오브젝트와 상대방화면의 AI오브젝트를 없애야한다.
            Debug.Log("Die");
        }
        else { }
    }
}

//다 따로 적용되는게 아닌것같아;
