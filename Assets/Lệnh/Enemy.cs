using UnityEngine;
using UnityEngine.AI;// Sử dụng thư viện AI của Unity để làm việc với NavMeshAgent
using System.Collections;

//Lệnh này cho AI kẻ địch cũng như các chức năng của nó (Ở trong enemy)
public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent; // Component điều khiển AI di chuyển trên NavMesh

    public GameObject healObject; // Đối tượng hồi máu mà kẻ địch có thể tạo ra khi chết

    public Transform fireSpot; // Vị trí mà từ đó kẻ địch bắn đạn

    public GameObject quái_con; // Prefab của quái con mà kẻ địch có thể tạo ra

    public Transform player; // Tham chiếu đến người chơi

    public GameObject effect; // Hiệu ứng được kích hoạt khi kẻ địch chết

    public LayerMask whatIsGround, whatIsFlying, whatIsPlayer; // Các LayerMask để xác định loại đối tượng

    public float maxHealth; // Mức máu của kẻ địch
    private float health;//mức máu hiện tại của kẻ địch

    public RaycastHit ray; // Thông tin về tia raycast

    private Vector3 direction; // Hướng mà kẻ địch sẽ bắn hoặc nhìn về

    private readonly Collider[] hitColliders = new Collider[10]; // Mảng lưu trữ các Collider tìm được

    public bool isBoss; // Biến kiểm tra xem kẻ địch có phải là boss không
    private bool isDead; // Biến kiểm tra xem kẻ địch đã chết chưa
    public bool isExplore = false; // Có nổ tung khi chết không
    public bool isDuplicate = false; // Có nhân bản ra quái con khi chết không
    public bool isFly = false; // Có bay được không

    // Patroling
    public Vector3 walkPoint; // Điểm đến khi tuần tra
    bool walkPointSet; // Biến kiểm tra xem đã có điểm tuần tra chưa
    public float walkPointRange; // Khoảng cách tối đa cho điểm tuần tra

    // Attacking
    public float timeBetweenAttacks; // Thời gian giữa các lần tấn công
    bool alreadyAttacked; // Biến kiểm tra xem kẻ địch đã tấn công chưa
    public GameObject projectile; // Đối tượng đạn để bắn

    // States
    public float sightRange, attackRange; // Tầm nhìn và tầm tấn công
    public bool playerInSightRange, playerInAttackRange; // Biến kiểm tra xem người chơi có trong tầm nhìn và tầm tấn công không

    private void Awake()
    {
        health = maxHealth;
        isDead = false;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        if (isBoss)
        {
            PlayerUI.instance.healthBarBoss.SetActive(true);// Nếu là boss, hiển thị thanh máu của boss trên UI
        }
    }

    private void Update()
    {
        // Kiểm tra xem người chơi có trong tầm nhìn và tầm tấn công không
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Xử lý các trạng thái của kẻ địch dựa trên việc phát hiện người chơi
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
            if (isBoss)//giúp boss vừa đi vừa bắn ;)
            {
                Patroling();
            }
        }
    }

    private void Patroling()
    {
        // Nếu điểm đi dạo (walk point) chưa được thiết lập, gọi phương thức tìm kiếm điểm đi dạo mới.
        if (!walkPointSet) SearchWalkPoint();

        // Nếu điểm đi dạo đã được thiết lập, chỉ định điểm đó làm điểm đến cho NavMeshAgent,
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        // Tính toán khoảng cách từ vị trí hiện tại của kẻ địch đến điểm đi dạo.
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Kiểm tra xem kẻ địch đã đến gần điểm đi dạo chưa.
        // và cần phải tìm một điểm đi dạo mới.
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        // Tính toán một điểm ngẫu nhiên trong phạm vi cho trước.
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        // Tạo một Vector3 mới cho điểm đi dạo dựa trên vị trí hiện tại của kẻ địch và x z ngẫu nhiên
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (!isFly)
        {
            // Nếu Raycast từ điểm đi dạo xuống dưới và chạm vào mặt đất, 
            // điều này có nghĩa là điểm đi dạo hợp lệ và kẻ địch có thể di chuyển đến đó.
            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
                walkPointSet = true;
        }
        else
        {
            // Nếu kẻ địch có thể bay, chúng ta sẽ sử dụng Raycast tương tự nhưng với LayerMask khác
            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsFlying))
                walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        Vector3 Position = new Vector3(player.position.x, transform.position.y, player.position.z);
        agent.SetDestination(Position);//xóa điểm đi đến hiện tại và đi theo người chơi
    }

    private void AttackPlayer()
    {
        //khiến kẻ địch đứng im
        agent.SetDestination(transform.position);
        //nếu không bay thì nhìn người chơi và lấy giá trị bắn
        if (!isFly)
        {
            transform.LookAt(player);
            direction = transform.forward;
        }
        else
        {
            transform.LookAt(player, Vector3.up);
            direction = player.position - transform.position;
        }

        if (!alreadyAttacked)//nếu chưa bắn thì gọi lệnh bắn
        {
            StartCoroutine(HandleAttack());
        }
    }

    private IEnumerator HandleAttack()
    {

        var bullet = Instantiate(projectile, fireSpot.position, fireSpot.rotation);//tạo viên đạn ở fireSpot

        //RayCast
        if (Physics.Raycast(transform.position, direction, out ray, 100, whatIsPlayer))//kiểm tra va chạm raycast từ hướng kẻ địch đến người chơi
        {
            Vector3 directionbullet = (ray.point - transform.position).normalized;//định hướng viên đạn dựa theo giá trị nhận được
            bullet.GetComponent<Rigidbody>().velocity = directionbullet * 60;//đẩy viên đạn đi
        }

        alreadyAttacked = true;//tránh lặp lại việc bắn
        yield return new WaitForSeconds(timeBetweenAttacks);//đợi một thời gian rồi cho phép bắn tiếp
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        //kiểm tra để tránh máu giảm xuống dưới 0
        if (health - damage > 0)
            health -= damage;
        else
        {
            health = 0;
        }

        if (isBoss)
        {
            PlayerUI.instance.BossHealth(health,maxHealth);//Tạo hiệu ứng giảm máu cho boss
        }

        if (health <= 0) StartCoroutine(DestroyEnemy());//gọi lệnh hủy Enemy
    }
    private IEnumerator DestroyEnemy()
    {
        if (isDead) yield break;//đã chết thì không gọi lại
        isDead = true;
        if (isExplore)
        {
            Explosion();
        }
        if (isDuplicate)
        {
            Duplicate();
        }
        if (Player.instance.currentEnergy <= 80)//kiểm tra để năng lượng người chơi không vượt quá 100
        {
            Player.instance.currentEnergy += 20;
        }
        if (isBoss)
        {
            PlayerUI.instance.WINNER();//Gọi lệnh chiến thắng
        }
        Instantiate(healObject, transform.position, transform.rotation);//tạo viên hồi máu ở chỗ quái chết
        sinh_quái.instance.số_lượng_địch--;//giảm giá trị số lượng địch ở SummonEnemy
        Destroy(gameObject);//hủy kẻ địch
    }

    void Explosion()
    {
        GameObject boom = Instantiate(effect, transform.position, transform.rotation);//tạo hiệu ứng nổ
        Destroy(boom, 2f); // hủy hiệu ứng sau 2 giây
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, 8f, hitColliders);//kiểm tra va chạm 8m xung quanh kẻ địch

        for (int i = 0; i < numColliders; i++)//kiểm tra thành phần va chạm có phải player không
        {
            // Process each Collider found.
            if (hitColliders[i].CompareTag("Player"))
            {
                Player.instance.TakeDamage(20);//trừ người chơi 20 máu nếu va chạm
            }
        }
    }
    void Duplicate()
    {
        for (int i = 0; i < 2; i++)
        {
            Instantiate(quái_con, transform.position, transform.rotation);//gọi 2 quái con tại vị trí kẻ địch
        }
        sinh_quái.instance.số_lượng_địch += 2;//cộng thêm 2 giá trị kẻ địch ở EnemySummon
    }

    private void OnDrawGizmosSelected()//hiển thị phạm vi tấn công và tầm nhìn kẻ địch khi chọn ở scene
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
