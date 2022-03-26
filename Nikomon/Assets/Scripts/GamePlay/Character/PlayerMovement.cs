
using GamePlay.Character;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerMovement : Player
{
    public NicomonInputSystem nicoInput;
    public float InteractDepth = 5f;
    public Transform HeadTrans;

    [Header("Gaze")] public Rig GazeRig;
    public Transform GazeTransform;
    public float GazeDistance;
    [SerializeField] private Animator animator;
    
    private GameObject VirtualController;

    private Rigidbody rigid;

    void Start()
    {
        animator = GetComponent<Animator>();
        VirtualController = GameObject.Find("VirtualController");
        if (nicoInput == null)
            nicoInput = FindObjectOfType<NicomonInputSystem>();
        if (nicoInput == null)
        {
            nicoInput = gameObject.AddComponent<NicomonInputSystem>();
        }

        nicoInput.NicomonInput.UI.Debug.started += (o) => { UIManager.Instance.Show<DebugPanel>(); };

        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGaze();
        CheckGrounded();
        if (GlobalManager.isBattling)
        {
            if (GlobalManager.Instance.Config.UseVirtualControl) VirtualController?.SetActive(false);
            isWalking = false;
            animator?.SetBool("IsWalking", false);
            move = Vector2.zero;
            return;
        }


        // if (GlobalManager.Instance.Config.UseVirtualControl) VirtualController?.SetActive(true);
        this.move = nicoInput.move;
        Movement();
        if (nicoInput.menu)
        {
            UIManager.Instance.Show<MainMenuUI>();
            // GlobalManager.Instance.SaveSaveData();
        }

        if (nicoInput.accept)
        {
            // CheckInteractable();
        }
    }

    private void CheckGaze()
    {
        if (GazeTransform == null) return;
        var colliders = Physics.OverlapSphere(transform.position, GazeDistance);
        if (colliders != null)
        {
            foreach (var collider1 in colliders)
            {
                if (collider1.GetComponent<PokemonIdentity>() != null)
                {
                    GazeRig.weight = Mathf.Lerp(GazeRig.weight, 1f, 0.5f);
                    var capsuleCollider = (collider1 as CapsuleCollider); //get the height of pokemon
                    Vector3 position = collider1.transform.position + Vector3.up *
                        (capsuleCollider.radius * 2) * capsuleCollider.transform.localScale.y;
                    // print(position.y);
                    GazeTransform.position = Vector3.Lerp(GazeTransform.position, position, 1f);
                    return;
                }
            }
        }

        GazeRig.weight = Mathf.Lerp(GazeRig.weight, 0f, 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        var position = transform.position;
        Gizmos.DrawLine(position, position + Vector3.down * 2f);
    }

    void CheckGrounded()
    {
        var hits = Physics.RaycastAll(new Ray(transform.position + Vector3.up * 0.8f, Vector3.down), 3f);
        // print(hitInfo.collider.name);
        bool isGround = false;

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
                isGround = true;
        }

        // var isGrounded = Physics.Raycast(new Ray(transform.position,Vector3.down),1f,LayerMask.NameToLayer("Ground"));
        animator.SetBool("IsGround", isGround);
        if (isGround)
        {
            animator.SetBool("IsInWater", !isGround);
            animator.applyRootMotion = true;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    public void AddForceToward(float force)
    {
        // print("Add force");
        if (Vector3.Angle(rigid.velocity, transform.forward) >= 45)
        {
            rigid.velocity = transform.forward;
        }

        rigid.AddForce(transform.forward * force, ForceMode.Force);
        rigid.AddForce(transform.up * force / 100f, ForceMode.Impulse);
    }

    private float rigidDrag;

    public void CheckWater(bool isInWater)
    {
        animator.SetBool("IsInWater", isInWater);
        animator.applyRootMotion = !isInWater;
        if (isInWater)
        {
            rigidDrag = rigid.drag;
            rigid.drag = 0;
        }
        else
        {
            rigid.drag = rigidDrag;
        }
    }


    void CheckInteractable()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, InteractDepth))
        {
            IInteractive interactive = raycastHit.transform.GetComponent<IInteractive>();
            interactive?.OnInteractive();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (GlobalManager.isBattling) return;
        IInteractive interactive = other.gameObject.GetComponent<IInteractive>();
        interactive?.OnInteractive(this.gameObject);
        
    }

    private void OnCollisionExit(Collision other)
    {
        // if (other.gameObject.CompareTag("Ground"))
        // {
        //     animator.SetBool("IsGround",false);
        // }
    }

    public void Movement()
    {
        if (move != Vector2.zero)
        {
            isWalking = true;
            Debug.Assert(Camera.main != null, "Camera.main != null");
            float cameraForward =
                Camera.main.transform.rotation.eulerAngles.y;
            float theta = 0;
            if (move.y > 0)
                theta = Mathf.Atan(move.x / move.y);
            else if (move.y < 0)
                theta = Mathf.Atan(move.x / move.y) + Mathf.PI;
            else
            {
                theta = move.x > 0 ? Mathf.PI / 2 : Mathf.PI * 3 / 2;
            }

            theta = Mathf.Rad2Deg * theta;
            transform.rotation = Quaternion.Euler(0, cameraForward + theta, 0);
            rigid.angularVelocity = Vector3.zero;
        }
        else
        {
            isWalking = false;
        }

        animator?.SetBool("IsWalking", isWalking);
    }

    private bool isWalking;
    private Vector2 move;


    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}