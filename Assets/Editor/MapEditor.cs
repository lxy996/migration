using UnityEngine;
using UnityEditor;

public class MapEditor : EditorWindow
{
    private GameObject selectedPrefab;
    private HexGrid hexGrid;
    private string[] types = new string[] { "Terrain", "Object", "Reward" }; // ʾ������
    private int selectedTypeIndex;

    [MenuItem("Window/Map Editor")]
    public static void ShowWindow()
    {
        GetWindow<MapEditor>("Map Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Select Object to Place", EditorStyles.boldLabel);

        // �����˵�ѡ������
        selectedTypeIndex = EditorGUILayout.Popup("Type", selectedTypeIndex, types);

        // �ṩһ����ťѡ��Ԥ����
        if (GUILayout.Button("Select Object"))
        {
            string path = EditorUtility.OpenFilePanel("Select Prefab", "Assets/", "prefab");
            if (!string.IsNullOrEmpty(path))
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
                selectedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }
        }

        if (selectedPrefab != null)
        {
            GUILayout.Label("Selected: " + selectedPrefab.name);
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (selectedPrefab != null && Event.current.type == UnityEngine.EventType.MouseDown)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                HexCell cell = hexGrid.GetCellAt(hit.point);
                if (cell != null)
                {
                    GameObject newObject = Instantiate(selectedPrefab, cell.transform.position, Quaternion.identity);
                    cell.AddObject(types[selectedTypeIndex], newObject);
                    Event.current.Use();
                }
            }
        }
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        hexGrid = FindObjectOfType<HexGrid>();
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}