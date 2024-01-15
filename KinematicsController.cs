using UnityEngine;

public class ChairController : MonoBehaviour
{
    public Collider collisionChecker;
    private Rigidbody[] childRigidbodies;
    private bool isKinematicEnabled = true;

    void Start()
    {
        // Получаем все дочерние объекты с компонентом Rigidbody
        childRigidbodies = GetComponentsInChildren<Rigidbody>();

        // Устанавливаем начальную кинематику
        SetKinematic(isKinematicEnabled);
    }

    void Update()
    {
        // Проверяем, соприкасается ли Collider с заданным объектом
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, Mathf.Infinity) && hit.collider == collisionChecker)
        {
            // Если соприкосновение произошло, выключаем кинематику
            SetKinematic(false);
        }
        else
        {
            // Если нет соприкосновения, включаем кинематику
            SetKinematic(true);
        }
    }

    void SetKinematic(bool isEnabled)
    {
        // Включаем или выключаем кинематику для всех дочерних Rigidbody
        foreach (Rigidbody childRigidbody in childRigidbodies)
        {
            childRigidbody.isKinematic = isEnabled;
        }

        // Обновляем текущее состояние
        isKinematicEnabled = isEnabled;
    }
}
