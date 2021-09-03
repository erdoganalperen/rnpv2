using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private Transform PlayerSprite;
    private Animator anim;
    public float speed;
    private bool _movement = false;
    [SerializeField] private GameObject _camera;
    public Vector3 cameraOffset;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _camera.transform.position = transform.position + cameraOffset;
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        if (joystick.Horizontal > 0 || joystick.Horizontal < 0 || joystick.Vertical > 0 || joystick.Vertical > 0)
        {
            if (anim.GetBool("playerMove") != true)
            {
                anim.SetBool("playerMove", true);
            }
            PlayerSprite.position = new Vector3(joystick.Horizontal + transform.position.x,
                0, joystick.Vertical + transform.position.z);
            transform.LookAt(new Vector3(PlayerSprite.position.x, 0,
                PlayerSprite.position.z));
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            _movement = true;
            //camera
            var pos = transform.position + cameraOffset;
            _camera.transform.position = pos;
        }
        else if (_movement == true)
        {
            anim.SetBool("playerMove", false);
            _movement = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            GameManager.Instance.CollectedItem();
            Destroy(other.gameObject);
        }
    }
}