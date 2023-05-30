using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBoxManager : MonoBehaviour
{
    [SerializeField] private Mesh moveFlagSkin;

    SelectedDictionary selectedUnits;
    RaycastHit hitData;
    LayerMask mask;

    bool dragSelect;

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 startPos;
    Vector3 endPos;

    //Stores the corners of the UI selection box
    Vector2[] corners;

    //Stores the 3D vertices of the projected bounding box
    Vector3[] verts;

    private void Awake()
    {
        selectedUnits = GetComponent<SelectedDictionary>();
    }

    // Start is called before the first frame update
    void Start()
    {
        dragSelect = false;
        mask = LayerMask.GetMask("Buildings", "Units");
    }

    // Update is called once per frame
    void Update()
    {
        //Left mouse button (LMB) was pressed (not yet realeased)
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        //While LMB is pressed down
        if (Input.GetMouseButton(0))
        {
            float dist = (startPos - Input.mousePosition).magnitude;

            //If the current mouse position is a certain distance away from the
            //initial position then we consider a dragging movement
            if (dist > 40)
            {
                dragSelect = true;
            }
        }

        //LMB is released
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Here!!");
            Debug.Log("Dragsel: " + dragSelect);
            if (!dragSelect)
            {
                //Single unit select
                Ray ray = Camera.main.ScreenPointToRay(startPos);

                if(Physics.Raycast(ray, out hitData, 10000.0f, mask))
                {
                    //Something was hit
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //Inclusive select
                        Debug.Log("Here2!!");
                        selectedUnits.AddSelected(hitData.transform.gameObject);
                    }
                    else
                    {
                        Debug.Log("Here3!");
                        //Exclusive select
                        selectedUnits.deselectAll();
                        selectedUnits.AddSelected(hitData.transform.gameObject);
                    }
                }
                else
                {
                    //Nothing was hit, deselect everything
                    selectedUnits.deselectAll();
                }
            }
            else
            {
                //Multiple unit select
                verts = new Vector3[4];
                int i = 0;
                endPos = Input.mousePosition;
                corners = GetBoundingBox(startPos, endPos);

                foreach (Vector2 corner in corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);

                    LayerMask groundMask = LayerMask.GetMask("Ground");

                    if (Physics.Raycast(ray, out hitData, 10000.0f, groundMask))
                    {
                        verts[i] = new Vector3(hitData.point.x, 0, hitData.point.z);
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hitData.point, Color.red, 1.0f);
                    }

                    i++;
                }

                //Generate the Mesh
                selectionMesh = GenerateSelectionMesh(verts);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    //Exclusive Select
                    selectedUnits.deselectAll();
                }

                //Delete the bounding box after a short time, so it doesn't keep selecting units
                Destroy(selectionBox, 0.02f);
            }

            dragSelect = false;
        }
    }

    private void OnGUI()
    {
        if (dragSelect)
        {
            //Draw a Rectangle on screen
        }
    }

    //Returns a rectangle's corners in a specific order
    Vector2[] GetBoundingBox(Vector2 start, Vector2 end)
    {
        Vector2 p1;
        Vector2 p2;
        Vector2 p3;
        Vector2 p4;

        if (start.x < end.x)
        {
            //If start is to the left of end
            if (start.y > end.y)
            {
                //If start is above end
                p1 = start;
                p2 = new Vector2(end.x, start.y);
                p3 = new Vector2(start.x, end.y);
                p4 = end;
            }
            else
            {
                //If start is below end
                p1 = new Vector2(start.x, end.y);
                p2 = end;
                p3 = start;
                p4 = new Vector2(end.x, start.y);
            }
        }
        else
        {
            //If start is to the right of end
            if (start.y > end.y)
            {
                //If start is above end
                p1 = new Vector2(end.x, start.y);
                p2 = start;
                p3 = end;
                p4 = new Vector2(start.x, end.y);
            }
            else
            {
                //If start is below end
                p1 = end;
                p2 = new Vector2(start.x, end.y);
                p3 = new Vector2(end.x, start.y);
                p4 = start;
            }
        }

        //Correct ordering for easy triangle generation
        Vector2[] corners = { p1, p2, p3, p4 };
        return corners;
    }

    //generate a mesh from a rectangle
    Mesh GenerateSelectionMesh(Vector3[] corners)
    {
        Vector3[] verts = new Vector3[8];
        //Correct order for triangle generation for the mesh cube
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

        for (int i = 0; i < 4; i++)
        {
            //Create a rectangle using the 4 provided corners
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            //Create another rectangle on top of the initial one
            verts[j] = corners[j - 4] + Vector3.up * 100.0f;
        }

        //Create a mesh based on the 2 rectangles created before
        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Units"))
        {
            selectedUnits.AddSelected(other.gameObject);
        }
    }
}
