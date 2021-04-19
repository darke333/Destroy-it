using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] Transform BulletPlace;
    [SerializeField] GameObject Puff;
    public int BulletsLeft = 4;
    public float MaxForce = 30;

    Animator animator;
    GameObject LoadedBullet;
    GrabbedBullet grabbedBullet;
    Collider trigger;
    [HideInInspector]
    public bool Reloading = true;
    
    // Start is called before the first frame update

    class GrabbedBullet
    {
        public Vector3 StartPos;
        public GameObject gameObject;
        public Transform transform;
        public float zDistance;
        
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        trigger = GetComponent<Collider>();
    }
    
    public void StartShooting()
    {
        animator.Play("Armature_animShoot");
    }

    public void Shoot()
    {
        float Force = GameManager.gameManager.ui.ShootForce;
        LoadedBullet.SetActive(true);

        LoadedBullet.GetComponent<Rigidbody>().AddForce(BulletPlace.transform.forward * Force, ForceMode.Impulse);
        Destroy(Instantiate(Puff), 1);
        
        Destroy(LoadedBullet, 10);
    }

    void Reload()
    {
        if(Input.touchCount > 0)
        {

            switch (Input.touches[0].phase)
            {
                //Grabbing
                case TouchPhase.Began:
                    Ray ray = GameManager.gameManager.cameraManager.camera.ScreenPointToRay(Input.touches[0].position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "bullet")
                        {                            
                            grabbedBullet = new GrabbedBullet();
                            grabbedBullet.transform = hit.collider.transform;
                            grabbedBullet.zDistance = hit.distance;
                            grabbedBullet.gameObject = hit.collider.gameObject;
                            grabbedBullet.StartPos = grabbedBullet.transform.position;
                        }
                    }
                    break;
                //Moving
                case TouchPhase.Moved:
                    if (grabbedBullet != null)
                    {
                        
                        Vector3 TouchPos = GameManager.gameManager.cameraManager.camera.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, grabbedBullet.zDistance));
                        grabbedBullet.transform.position = TouchPos;
                    }
                    break;
                //Loading
                case TouchPhase.Ended:
                    if (grabbedBullet != null)
                    {
                        if (trigger.bounds.Contains(grabbedBullet.transform.position))
                        {
                            
                            LoadedBullet = grabbedBullet.gameObject;                            
                            LoadedBullet.SetActive(false);
                            LoadedBullet.GetComponent<Rigidbody>().isKinematic = false;
                            LoadedBullet.transform.position = BulletPlace.position;
                            BulletsLeft--;
                            //ReadyToShoot
                            GameManager.gameManager.SetCondition(GameManager.Condition.shooting);                            
                        }
                        else
                        {
                            grabbedBullet.transform.position = grabbedBullet.StartPos;
                        }
                        grabbedBullet = null;
                    }
                    break;
            }
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if(Reloading)
            Reload();
    }
}
