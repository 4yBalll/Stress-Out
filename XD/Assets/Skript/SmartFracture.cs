using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SmartFracture : MonoBehaviour
{
    public float breakRadius = 0.2f;
    public float breakForce = 100;
    public GameObject collisionObject;  // Новое публичное поле для объекта

    private List<SubFracture> cells;

    void Start()
    {
        InitSubFractures();
    }

    void InitSubFractures()
    {
        cells = new List<SubFracture>();
        cells.AddRange(transform.GetComponentsInChildren<SubFracture>());

        foreach (SubFracture cell in cells)
        {
            BoxCollider tempCollider = cell.gameObject.AddComponent<BoxCollider>();
            tempCollider.size *= 2.4f;

            Collider[] hitColliders = Physics.OverlapBox(cell.transform.position, tempCollider.size / 2, cell.transform.rotation);
            int i = 0;

            while (i < hitColliders.Length)
            {
                if (hitColliders[i].GetComponent<SubFracture>() && hitColliders[i].transform.root == cell.transform.root && hitColliders[i].gameObject != cell.gameObject)
                {
                    cell.connections.Add(hitColliders[i].GetComponent<SubFracture>());
                    hitColliders[i].GetComponent<SubFracture>().connections.Add(cell);
                    UnityEngine.Debug.Log(cell.name + "_" + hitColliders[i].name);

                }
                i++;
            }

            Destroy(tempCollider);
        }
    }

    public void Fracture(Vector3 point, Vector3 force)
    {
        foreach (SubFracture cell in cells)
        {
            // Добавьте проверку на столкновение с объектом
            if (collisionObject != null && cell.gameObject == collisionObject)
            {
                // Если подобъект столкнулся с collisionObject, обнулите его соединения и установите grounded в false
                cell.connections = new List<SubFracture>();
                cell.grounded = false;
                cell.GetComponent<Rigidbody>().isKinematic = false;
                cell.GetComponent<Rigidbody>().AddForceAtPosition(force, point, ForceMode.Force);
            }
        }
    }
}
