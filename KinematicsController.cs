using UnityEngine;

public class ChairController : MonoBehaviour
{
    public Collider collisionChecker;
    private Rigidbody[] childRigidbodies;
    private bool isKinematicEnabled = true;

    void Start()
    {
        // �������� ��� �������� ������� � ����������� Rigidbody
        childRigidbodies = GetComponentsInChildren<Rigidbody>();

        // ������������� ��������� ����������
        SetKinematic(isKinematicEnabled);
    }

    void Update()
    {
        // ���������, ������������� �� Collider � �������� ��������
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, Mathf.Infinity) && hit.collider == collisionChecker)
        {
            // ���� ��������������� ���������, ��������� ����������
            SetKinematic(false);
        }
        else
        {
            // ���� ��� ���������������, �������� ����������
            SetKinematic(true);
        }
    }

    void SetKinematic(bool isEnabled)
    {
        // �������� ��� ��������� ���������� ��� ���� �������� Rigidbody
        foreach (Rigidbody childRigidbody in childRigidbodies)
        {
            childRigidbody.isKinematic = isEnabled;
        }

        // ��������� ������� ���������
        isKinematicEnabled = isEnabled;
    }
}
