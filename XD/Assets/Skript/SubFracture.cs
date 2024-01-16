using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SubFracture : MonoBehaviour
{
    public bool grounded;
    public bool connected;

    public List<SubFracture> connections;

    private SmartFracture parent;

    private void Start()
    {
        parent = transform.root.GetComponent<SmartFracture>();
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        for (int i = 0; i < connections.Count; i++) // Make sure it is not isolated
        {
            if (!connections[i].grounded && !connections[i].connected)
            {
                connections.Remove(connections[i]);
            }
        }

        bool somehowGrounded = false;

        for (int i = 0; i < connections.Count; i++) // Make sure it is connect to a ground some way or another
        {
            if (connections[i].grounded)
            {
                somehowGrounded = true;
                break;
            }

            for (int i2 = 0; i2 < connections[i].connections.Count; i2++) // Check one more iteration through
            {
                if (connections[i].connections[i2].grounded)
                {
                    somehowGrounded = true;
                    break;
                }
                for (int i3 = 0; i3 < connections[i2].connections[i].connections.Count; i3++)
                {
                    if (connections[i].connections[i2].connections[i3].grounded)
                    {
                        somehowGrounded = true;
                        break;
                    }
                    for (int i4= 0; i4 < connections[i3].connections[i2].connections[i].connections.Count; i4++)
                    {
                        if (connections[i].connections[i2].connections[i3].connections[i4].grounded)
                        {
                            somehowGrounded = true;
                            break;
                        }
                    }
                }
            }
        }

        connected = somehowGrounded && connections.Count >= 1 || grounded;

        GetComponent<Rigidbody>().isKinematic = connected;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проверка наличия ссылки на SmartFracture в родительском объекте
        if (parent == null)
        {
            UnityEngine.Debug.Log("Your debug message here.");
            return;
        }

        // Проверка наличия коллайдера в публичной переменной и столкновения с ним
        if (parent.collisionObject != null && collision.collider == parent.collisionObject.GetComponent<Collider>())
        {
            // Вызываем метод Fracture у родительского объекта SmartFracture
            parent.Fracture(collision.contacts[0].point, collision.impulse);
        }
        else if (collision.impulse.magnitude > parent.breakForce)
        {
            // Если условие по величине импульса выполняется, обнуляем соединения и вызываем Fracture
            connections = new List<SubFracture>();
            grounded = false;
            parent.Fracture(collision.contacts[0].point, collision.impulse);
        }
    }


    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < connections.Count; i++)
        {
            Gizmos.DrawLine(transform.position, connections[i].transform.position);
        }
    }
}

