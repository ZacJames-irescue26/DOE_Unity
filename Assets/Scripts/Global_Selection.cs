using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_Selection : MonoBehaviour
{
    Selected_Dictionary selected_table;
    List<Transform> selection_list = new List<Transform>();
    RaycastHit hit;
    LayerMask IgnoreLayer;
    bool dragSelect;
    public GameObject cursorObject;
    public int Owner;

    //Collider variables
    //=======================================================//

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 p1;
    Vector3 p2;

    //the corners of our 2d selection box
    Vector2[] corners;

    //the vertices of our meshcollider
    Vector3[] verts;
    Vector3[] vecs;

    // Start is called before the first frame update
    void Start()
    {
        IgnoreLayer = LayerMask.GetMask("Ground");
        selected_table = GetComponent<Selected_Dictionary>();
        dragSelect = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        //1. when left mouse button clicked (but not released)
        if (Input.GetMouseButtonDown(0))
        {
            p1 = Input.mousePosition;
        }

        //2. while left mouse button held
        if (Input.GetMouseButton(0))
        {
            if ((p1 - Input.mousePosition).magnitude > 40)
            {
                dragSelect = true;
            }
        }

        //3. when mouse button comes up
        if (Input.GetMouseButtonUp(0))
        {
            if (dragSelect == false) //single select
            {
                Ray ray = Camera.main.ScreenPointToRay(p1);

                if (Physics.Raycast(ray, out hit, 50000.0f, ~IgnoreLayer))
                {
                    if (hit.transform.gameObject.GetComponent<UnitStats>() != null)
                    {
                        if (Owner == hit.transform.gameObject.GetComponent<UnitStats>().Owner)
                        {
                            if (Input.GetKey(KeyCode.LeftShift)) //inclusive select
                            {
                                /*if(!selection_list.Contains(hit.transform))
                                {
                                    selection_list.Add(hit.transform);
                                }*/
                                selected_table.AddSelected(hit.transform.gameObject);
                            }
                            else //exclusive selected
                            {
                                selected_table.DeselectAll();
                                selected_table.AddSelected(hit.transform.gameObject);
                                /*selection_list.Clear();
                                selection_list.Add(hit.transform);*/
                            }

                        }

                    }
                    else if (GetComponent<BuildingStats>())
                    {
                        if (Owner == hit.transform.gameObject.GetComponent<BuildingStats>().Owner)
                        {
                            // change ui to show building
                        }
                    }

                }
                else //if we didnt hit something
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //do nothing
                    }
                    else
                    {
                        selected_table.DeselectAll();
                        //selection_list.Clear();
                    }
                }
            }
            else //marquee select
            {
                verts = new Vector3[4];
                vecs = new Vector3[4];
                int i = 0;
                p2 = Input.mousePosition;
                corners = getBoundingBox(p1, p2);

                foreach (Vector2 corner in corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);

                    if (Physics.Raycast(ray, out hit, 50000.0f, IgnoreLayer))
                    {
                        verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        vecs[i] = ray.origin - hit.point;
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
                    }
                    i++;
                }

                //generate the mesh
                selectionMesh = generateSelectionMesh(verts, vecs);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;
                //selectionBox.size = new Vector3()

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    //selection_list.Clear();
                    selected_table.DeselectAll();
                }

                Destroy(selectionBox, 0.02f);

            }//end marquee select

            dragSelect = false;

        }

        else if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift) && selected_table.HaveSelectedUnits())
        {
            //create a ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //check if we hit our unit
            if (Physics.Raycast(ray, out hit))
            {

                foreach (KeyValuePair<int, GameObject> unit in selected_table.selectedTable)
                {
                    /*PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                    pU.MoveUnit(hit.point);*/

                    cursorObject.transform.position = hit.point;
                    Base_Behavior bb = unit.Value.GetComponent<Base_Behavior>();
                    bb.target = cursorObject;
                    bb.changeState(Base_Behavior.UnitFSM.MoveAttack);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && selected_table.HaveSelectedUnits())
        {
            //create a ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //check if we hit our unit
            if (Physics.Raycast(ray, out hit))
            {

                LayerMask layerHit = hit.transform.gameObject.layer;
                switch (layerHit.value)
                {
                    case 11: // UnBuiltBuildings
                        foreach (KeyValuePair<int, GameObject> unit in selected_table.selectedTable)
                        {
                            /*PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                            pU.MoveUnit(hit.point);*/
                            if (unit.Value.GetComponent<UnitStats>().Class == UnitStats.UnitClass.Worker)
                            {
                                GameObject BuildingToBuild = hit.transform.gameObject;
                                Base_Behavior bb = unit.Value.GetComponent<Base_Behavior>();
                                bb.target = BuildingToBuild;
                                bb.changeState(Base_Behavior.UnitFSM.Build);
                            }
                            else
                            {
                                cursorObject.transform.position = hit.point;
                                Base_Behavior bb = unit.Value.GetComponent<Base_Behavior>();
                                bb.target = cursorObject;
                                bb.changeState(Base_Behavior.UnitFSM.Seek);
                            }


                        }

                        break;
                    //case 10:
                    //attack or set as target
                    // break;
                    default: //if none of the above happens 
                        foreach (KeyValuePair<int, GameObject> unit in selected_table.selectedTable)
                        {
                            /*PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                            pU.MoveUnit(hit.point);*/

                            cursorObject.transform.position = hit.point;
                            Base_Behavior bb = unit.Value.GetComponent<Base_Behavior>();
                            bb.target = cursorObject;
                            bb.changeState(Base_Behavior.UnitFSM.Seek);


                        }
                        break;
                }
            }
        
        }
        
    }
    private void OnGUI()
    {
        if (dragSelect == true)
        {
            var rect = Utils.GetScreenRect(p1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    //create a bounding box (4 corners in order) from the start and end mouse position
    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        // Min and Max to get 2 corners of rectangle regardless of drag direction.
        Vector2 a = new Vector2(p1.x, p1.y);
        Vector2 b = new Vector2(p2.x, p2.y);
        Vector2 bottomLeft = new Vector2();
        bottomLeft = Vector2.Min(a, b);
        Vector3 topRight = Vector3.Max(p1, p2);
        Debug.Log(bottomLeft + ", " + topRight);
        // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
        Vector2[] corners =
        {
            new Vector2(bottomLeft.x, topRight.y),
            new Vector2(topRight.x, topRight.y),
            new Vector2(bottomLeft.x, bottomLeft.y),
            new Vector2(topRight.x, bottomLeft.y)
        };
        return corners;

    }

    //generate a mesh from the 4 bottom points
    Mesh generateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j-4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Owner == other.GetComponent<UnitStats>().Owner)
        {
            selected_table.AddSelected(other.gameObject);
        }
        //selection_list.Add(other.transform);
    }

    private bool HaveSelectedUnits()
    {
        if (selection_list.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
