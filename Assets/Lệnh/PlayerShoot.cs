using UnityEngine;
using UnityEngine.UI;

//Tập lệnh xử lý việc bắn của người chơi (Bên trong mỗi khẩu súng)
public class PlayerShoot : MonoBehaviour
{
    //chỉ số súng
    public float timeBetweenShooting, spread, range, reloadTime;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    private int bulletsLeft;
    int bulletShoot;//Số đạn còn lại chưa bắn mỗi lần gọi lệnh bắn(dùng cho súng shotgun)

    //số liệu của đạn
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

    //các hàm kiểm tra
    bool shooting, readyToShoot, reloading;

    //tham chiếu các đối tượng 
    public Camera fpsCam;

    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Đồ họa
    public Text ammo;
    public Animator nạp_đạn;

    //Âm thanh
    public AudioSource shoot;
    public AudioSource reload;

    private void Awake()
    {
        bulletsLeft = magazineSize;//đặt lại số đạn hiện tại
        readyToShoot = true;
    }
    private void Update()
    {
        if(PlayerUI.isPaused)//Đăng tạm dừng thì không chạy lệnh
        { return; }
        
        MyInput();

        if(bulletsLeft==0 && !reloading)//nếu hết đạn thì nạp lại nếu đăng chưa nạp
        {
            Reload();
        }

        if (!Player.instance.isGodmode)//tạo hiệu ứng băng đạn vô tận khi vào Godmmode
        {
            ammo.text = bulletsLeft + " / " + magazineSize;
        }
        else
        {
            ammo.text = "99999";
        }
    }
    private void MyInput()//hàm để xử lý Input người chơi
    {
        //xử lý khẩu súng có chức năng sấy không :)
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) 
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0){
            bulletShoot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        shoot.Play();//phát tiếng súng

        //tạo chức năng đạn tẽ cho shotgun
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        var bullet = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);

        //tạo raycast để định hướng viên đạn
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Vector3 directionbullet = (rayHit.point - transform.position).normalized;
            bullet.GetComponent<Rigidbody>().velocity = directionbullet * bulletSpeed;
        }

        //nếu đăng god mode thì không trừ đạn
        if (!Player.instance.isGodmode)
        {
            bulletsLeft--;
        }
        bulletShoot--;

        if (bulletShoot > 0 && bulletsLeft > 0)//chức năng cho shotgun bắn nhiều viên đạn
        {
            Invoke(nameof(Shoot),0f);
        }
        else
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
        }
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        reload.Play();
        Invoke(nameof(ReloadFinished), reloadTime);
        nạp_đạn.Play("Reload");
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
