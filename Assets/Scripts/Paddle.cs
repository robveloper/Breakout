using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Joystick joystick; //mobile
    [SerializeField] float paddleSpeed = 5;
    [SerializeField] float xLimit = 5;
    [SerializeField] float bigSizeTime = 10;
    //[SerializeField] GameManager gameManager; 
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate = 1;
    [SerializeField] float bulletsTime = 10;
    [SerializeField] Vector3 bulletOffset;
    bool bulletsActive;
    public bool BulletsActive{
        get => bulletsActive;
        set{
            bulletsActive = value;
            StartCoroutine(ShootBullets());
            Invoke("ResetBulletsActive", bulletsTime);
        }
    }
    void ResetBulletsActive()
    {
        bulletsActive = false;
        GameManager.Instance.powerUpIsActive = false;
    }

    IEnumerator ShootBullets()
    {
        while (BulletsActive)
        {
           Instantiate(bulletPrefab, transform.position + bulletOffset, Quaternion.identity);
            yield return new WaitForSeconds(fireRate);     
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(0, -4, 0);
        transform.position = new Vector3(0f, -2.99f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        MoveJoystick();
      
        MoveLimits();
    }

    void MoveJoystick() {

        //Vector2 direction = Vector2.right * joystick.Horizontal;

        float movement = joystick.Horizontal;
        transform.position += Vector3.right * movement * Time.deltaTime * paddleSpeed;
    }

    void Move()
    {
        float movement = Input.GetAxisRaw("Horizontal");
        transform.position += Vector3.right * movement * Time.deltaTime * paddleSpeed;
    }

    void MoveLimits()
    {
        if(transform.position.x > 7.44f)
        {
            transform.position = new Vector3(7.44f, transform.position.y, 0);
        }
        else if(transform.position.x < -7.44f)
        {
            transform.position = new Vector3(-7.44f, transform.position.y, 0);
        }

    }

    public void IncreaseSize()
    {
        if(!GameManager.Instance.ballIsOnPlay)
            return;
        Vector3 newSize = transform.localScale;
        newSize.x = 1.2f;
        transform.localScale = newSize; 
        StartCoroutine(BackToOriginalSize());
    }

    IEnumerator BackToOriginalSize()
    {
        yield return new WaitForSeconds(bigSizeTime);
        transform.localScale = new Vector3(0.8f, 0.5f, 1);
        GameManager.Instance.powerUpIsActive = false;
    }

}
