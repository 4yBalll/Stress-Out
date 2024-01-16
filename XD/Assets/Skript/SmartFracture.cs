using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartFracture : MonoBehaviour
{
    public float breakRadius = .2f;
    public float breakForce = 100;

    private List<SubFracture> cells;

    void Start()
    {
        InitSubFractures();
    }

    void InitSubFractures() // устанавливаем соеденение между обьектами
    {
        cells = new List<SubFracture>();
        cells.AddRange(transform.GetComponentsInChildren<SubFracture>());

        // Find all adjacent cells for every cell
        foreach (SubFracture cell in cells)
        {
            BoxCollider tempCollider = cell.gameObject.AddComponent<BoxCollider>();

            // Увеличьте размер BoxCollider в 1.5 раза
            tempCollider.size *= 2.4f;

            // Test if other cells are within the bounding box, then add to subfracture connections list
            Collider[] hitColliders = Physics.OverlapBox(cell.transform.position, tempCollider.size / 2, cell.transform.rotation);
            int i = 0;

            while (i < hitColliders.Length)
            {
                if (hitColliders[i].GetComponent<SubFracture>() && hitColliders[i].transform.root == cell.transform.root && hitColliders[i].gameObject != cell.gameObject)
                {
                    cell.connections.Add(hitColliders[i].GetComponent<SubFracture>());
                    hitColliders[i].GetComponent<SubFracture>().connections.Add(cell);
                    Debug.Log(cell.name + "_" + hitColliders[i].name);
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
            if (Vector3.Distance(cell.transform.position, point) < breakRadius)
            {
                // If a given cell is close to the collision, free it.
                cell.connections = new List<SubFracture>();
                cell.grounded = false;
                cell.GetComponent<Rigidbody>().isKinematic = false;
                cell.GetComponent<Rigidbody>().AddForceAtPosition(force, point, ForceMode.Force);
            }
        }
    }
}

