using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rb;
    private Camera _mainCamera;

    [Header("Gun Data")] 
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Bullet bulletPrefab;
    
    [Header("Movement Data")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private  float rotationSpeed;
    
    private float _verticalInput;
    private float _horizontalInput;

    [Header("Tower Data")] 
    [SerializeField] private  Transform towerTransform;
    [SerializeField] private  float towerRotationSpeed;
    
    [Header("Aim Data")]
    [SerializeField] private  Transform aimTransform;
    [SerializeField] private  LayerMask whatIsAimMask;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        UpdateAim();
        CheckInputs();
    }
    
    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyBodyRotation(); 
        ApplyTowersRotation();
    }
    
    private void CheckInputs()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)) Shoot();
        
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
        
        if(_verticalInput < 0) 
            _horizontalInput = -Input.GetAxis("Horizontal");
    }
    
    private Bullet SpawnBullet()
    {
        var newBullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        return newBullet;
    }
 
    private void Shoot()
    {
        var bullet = SpawnBullet();
        bullet.rb.velocity = gunPoint.forward * bulletSpeed;
        Destroy(bullet.gameObject, 7f);
    }

    private void ApplyTowersRotation()
    {
        Vector3 direction = aimTransform.position - towerTransform.position;
        direction.y = 0;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        towerTransform.rotation = Quaternion.RotateTowards(towerTransform.rotation, targetRotation, towerRotationSpeed);
    }

    private void ApplyBodyRotation()
    {
        transform.Rotate(0, _horizontalInput * rotationSpeed, 0);
    }

    private void ApplyMovement()
    {
        Vector3 currentVelocity = _rb.velocity;
        Vector3 movement = transform.forward * (moveSpeed * _verticalInput);
        Vector3 force = transform.forward * (moveSpeed * _verticalInput);
        _rb.AddForce(force, ForceMode.Acceleration);
        
        movement.y = currentVelocity.y;

        _rb.velocity = movement;
    }

    private void UpdateAim()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsAimMask ))
        {
            var fixedY = aimTransform.position.y;
            aimTransform.position = new Vector3(hit.point.x, fixedY, hit.point.z);
        }
    }
}
