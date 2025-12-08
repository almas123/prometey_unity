using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -5);
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private bool lookAtTarget = true;
    
    [Header("Bounds (Optional)")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 50f;
    [SerializeField] private float minZ = -50f;
    [SerializeField] private float maxZ = 50f;
    
    private void Start()
    {
        // Если цель не установлена, ищем игрока по тегу
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogWarning("CameraFollow: Игрок не найден! Установите цель вручную или добавьте тег 'Player' к игроку.");
            }
        }
        
        // Устанавливаем начальную позицию камеры
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
    
    private void LateUpdate()
    {
        if (target == null)
            return;
        
        // Вычисляем желаемую позицию камеры
        Vector3 desiredPosition = target.position + offset;
        
        // Применяем ограничения, если они включены
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZ, maxZ);
        }
        
        // Плавно перемещаем камеру к желаемой позиции
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        
        // Поворачиваем камеру к цели, если включено
        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }
    
    // Метод для изменения цели во время игры
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    // Метод для изменения смещения камеры
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
}
