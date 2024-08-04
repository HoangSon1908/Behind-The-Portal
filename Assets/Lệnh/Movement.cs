using System.Collections;
using UnityEngine;

// Lệnh để điều khiển di chuyển của nhân vật (Bên trong Player)
public class Movement : MonoBehaviour
{
    // Biến để điều chỉnh tốc độ di chuyển của nhân vật
    [Header("Movement")]
    public float moveSpeed;
    public float desireMoveSpeed;
    public float speedMultiplier;

    // Biến để điều chỉnh sức cản khi nhân vật đang đứng trên mặt đất
    public float groundDrag;

    // Biến để điều chỉnh lực nhảy của nhân vật
    public float jumpForce;

    // Hệ số tốc độ di chuyển trong không gian
    public float airMultiplier;

    // Biến kiểm tra xem nhân vật có sẵn sàng nhảy lần 2 hay không
    bool doubleJump;

    // Tốc độ di chuyển khi đi bộ (ẩn để không hiển thị trong Inspector)
    [HideInInspector] public float walkSpeed;

    // Keybind để nhảy
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    // Biến kiểm tra mặt đất dưới nhân vật
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    // Hướng mặt của nhân vật
    public Transform orientation;

    // Biến đầu vào từ bàn phím
    float horizontalInput;
    float verticalInput;

    // Hướng di chuyển của nhân vật
    Vector3 moveDirection;

    // Component Rigidbody để điều khiển vật lý của nhân vật
    Rigidbody rb;

    // Khởi tạo các giá trị ban đầu
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        speedMultiplier = 1f;
    }

    // Cập nhật trạng thái của nhân vật
    private void Update()
    {
        if (PlayerUI.isPaused)//nếu đăng tạm dừng thì dừng lệnh
        { return; }

        // Kiểm tra xem nhân vật có đang đứng trên mặt đất hay không
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        // Xử lý đầu vào từ người chơi
        MyInput();

        // Điều chỉnh tốc độ di chuyển
        SpeedControl();

        // Xử lý sự ma sát
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    // Cập nhật vật lý của nhân vật
    private void FixedUpdate()
    {
        bool speedchange = desireMoveSpeed != moveSpeed;//nếu khác nhau thì là true
        if (speedchange)
        {
            StartCoroutine(SmoothChangeSpeed());
        }
        MovePlayer();
    }

    // Xử lý đầu vào từ người chơi
    private void MyInput()
    {
        // Lấy giá trị đầu vào từ trục ngang và trục dọc
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(grounded && !Input.GetKey(jumpKey))
        {
            doubleJump = false;
        }
        
        // Xử lý việc nhảy
        if (Input.GetKeyDown(jumpKey))
        {
            if (grounded || doubleJump)
            {
                Jump();
                doubleJump = !doubleJump;
            }
        }
    }

    // Di chuyển nhân vật
    private void MovePlayer()
    {
        // Tính toán hướng di chuyển
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Nếu đang ở trên mặt đất
        if (grounded)
            // Áp dụng lực di chuyển
            rb.AddForce(10f * moveSpeed *speedMultiplier * moveDirection.normalized, ForceMode.Force);
        // Nếu đang trong không gian
        else if (!grounded)
            // Áp dụng lực di chuyển với tốc độ tăng thêm khi trong không gian
            rb.AddForce(10f * airMultiplier * moveSpeed * speedMultiplier * moveDirection.normalized, ForceMode.Force);
    }

    // Điều chỉnh tốc độ di chuyển của nhân vật
    private void SpeedControl()
    {
        Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z);

        // Giới hạn tốc độ di chuyển
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    // Hàm nhảy của nhân vật
    private void Jump()
    {
        // Đặt lại vận tốc theo trục y
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Áp dụng lực nhảy
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    IEnumerator SmoothChangeSpeed()//thay đổi vận tốc từ từ đến vận tốc mong muốn
    {
        float time = 0;
        float delay =0.25f;
        float startValue = moveSpeed;

        while (time < delay)
        {
            moveSpeed = Mathf.Lerp(startValue, desireMoveSpeed, time / delay);
            time += Time.deltaTime;

            yield return null;
        }

        moveSpeed = desireMoveSpeed;
    }
}
