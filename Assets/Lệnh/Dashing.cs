using UnityEngine;
using System.Collections; // Thư viện hỗ trợ coroutine

//Tập lệnh này sử dụng cho chức năng lướt và đồ họa của nó (Ở trong player)
public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation; // Tham chiếu đến hướng của nhân vật.
    public Transform playerCam; // Camera của người chơi.
    private Rigidbody rb; // Rigidbody của nhân vật.
    private Movement pm; // Script di chuyển của nhân vật.
    public GameObject Effect; // Hiệu ứng được kích hoạt khi Dash.

    [Header("Dashing")]
    public float dashForce; // Lực đẩy khi Dash.
    public float maxDashYSpeed; // Tốc độ Y tối đa khi Dash.
    public float dashDuration; // Thời gian Dash kéo dài.

    [Header("Settings")]
    public bool useCameraForward = true; // Sử dụng hướng của camera để xác định hướng Dash.
    public bool allowAllDirections = true; // Cho phép Dash theo mọi hướng.
    public bool disableGravity = false; // Tạm thời vô hiệu hóa trọng lực khi Dash.
    public bool resetVel = true; // Đặt lại vận tốc khi bắt đầu Dash.

    [Header("Cooldown")]
    public float dashCd; // Thời gian hồi chiêu Dash.
    private bool isDashing = false; // Biến kiểm tra xem Dash có đang được kích hoạt không.

    [Header("Input")]
    public KeyCode dashKey = KeyCode.LeftShift; // Phím nhấn để Dash.

    private Vector3 inputDirection; // Hướng nhập từ người chơi.

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Lấy component Rigidbody.
        pm = GetComponent<Movement>(); // Lấy script di chuyển.
    }

    private void Update()
    {
        // Lưu đầu vào từ người chơi.
        inputDirection = GetInputDirection();

        // Kiểm tra nếu người chơi nhấn phím Dash và không phải đang trong Dash.
        if ((Input.GetKeyDown(dashKey) || Input.GetMouseButtonDown(1)) && !isDashing)
            StartCoroutine(PerformDash()); // Bắt đầu coroutine Dash.
    }

    private IEnumerator PerformDash()
    {
        isDashing = true; // Đánh dấu là đang Dash.
        Effect.SetActive(true); // Kích hoạt hiệu ứng Dash.
        pm.desireMoveSpeed = maxDashYSpeed; // Đặt tốc độ mong muốn khi Dash.
        pm.groundDrag = 0f; // Giảm ma sát với mặt đất.

        if (resetVel)
            rb.velocity = Vector3.zero; // Đặt lại vận tốc.

        if (disableGravity)
            rb.useGravity = false; // Tạm thời vô hiệu hóa trọng lực.

        rb.AddForce(inputDirection * dashForce, ForceMode.Impulse); // Áp dụng lực Dash.

        yield return new WaitForSeconds(dashDuration); // Chờ thời gian Dash.

        pm.desireMoveSpeed = 9; // Đặt lại tốc độ mong muốn sau Dash.
        Effect.SetActive(false); // Tắt hiệu ứng Dash.
        pm.groundDrag = 4f; // Đặt lại ma sát với mặt đất.

        if (disableGravity)
            rb.useGravity = true; // Kích hoạt lại trọng lực.

        yield return new WaitForSeconds(dashCd); // Chờ đợi thời gian hồi chiêu Dash.

        isDashing = false; // Đánh dấu là Dash đã sẵn sàng để sử dụng lại.
    }

    private Vector3 GetInputDirection()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Lấy đầu vào ngang.
        float verticalInput = Input.GetAxisRaw("Vertical"); // Lấy đầu vào dọc.

        // Chọn giữa hướng của camera hoặc hướng của nhân vật để xác định hướng Dash.
        Transform forwardT = useCameraForward ? playerCam : orientation; // kiểm tra bool useCameraForward là true thì dùng playerCam và ngược lại
        // Tính toán hướng dựa trên đầu vào.
        Vector3 direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;

        // Nếu không có đầu vào, Dash theo hướng phía trước.
        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized; // Trả về hướng đã chuẩn hóa.
    }
}
