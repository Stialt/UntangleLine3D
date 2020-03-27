using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UntangleLines;

public class GameAreaPuzzle : MonoBehaviour
{
    //Prefabs

    //[0.5.0] Lines removed since now more than one gem is used
    //public GameObject someGem;
    //public float height = 10.04f;

    public GameObject[] PrefabGems;
    public GameObject line;
    private float[] gemHeights = { 10.21f, 10.04f, 10.04f, 10.3f, 10.2f };
    private float averageHeight = 10.152f;


    public static int N = 5;
    public static float midX = 143, midY = 211;
    public static float size = 16;


    private List<GameObject> lines = new List<GameObject>();
    private List<GameObject> gems = new List<GameObject>();

    private static Puzzle puzzle = new Puzzle();

    public GameObject victoryScreen;

    private static bool isVictory = false;
    private static bool isRestart = false;

    System.Random random = new System.Random();

    public static void setRestart(bool val)
    {
        isRestart = val;
    }

    // [0.5.0] Removed since now game is started from UI
    /*void Start()
    {
        createPuzzle();
    }*/

    // Update is called once per frame
    void Update()
    {
        //Updating Lines Positions
        int count = 0;
        for (int i = 0; i < puzzle.N; i++)
        {
            for (int j = i + 1; j < puzzle.N; j++)
            {
                if (puzzle.Edges[i, j] == 1)
                {

                    //Update line
                    //if (lines.Count <= count) break;
                    GameObject newLine = lines[count++];
                    LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
                    Vector3[] positions = new Vector3[2];

                    //TODO: [0.4.0] Adjust height of lines for carried items
                    float adjustedHeight = averageHeight + 0.2f;
                    if (CarryItem.carryID == i) adjustedHeight = averageHeight + 1f;
                    positions[0] = new Vector3(puzzle.randomPoints[i].X, adjustedHeight, puzzle.randomPoints[i].Y);

                    if (CarryItem.carryID == j) adjustedHeight = averageHeight + 1f;
                    else adjustedHeight = averageHeight + 0.2f;
                    positions[1] = new Vector3(puzzle.randomPoints[j].X, adjustedHeight, puzzle.randomPoints[j].Y);

                    //TODO: [0.4.0] Set red for intersecting lines
                    puzzle.getIntersectMap();
                    if (puzzle.EdgesIntersectionMap[i, j] == 1)
                        lineRenderer.SetColors(Color.red, Color.red);
                    else
                        lineRenderer.SetColors(Color.blue, Color.blue);
                    
                    lineRenderer.SetPositions(positions);
                }
            }
        }
        //~~~ Updating Lines Positions

        //Checking Victory flag
        if (isVictory)
        {
            if (victoryScreen.activeSelf == true)
                isVictory = false;
            else
                StartCoroutine(showVictory());
        }

        //Checking for restart
        if (isRestart)
        {
            isRestart = false;
            createPuzzle();
            victoryScreen.SetActive(false);
        }

    }
    
    public static void checkVictory()
    {
        if (!IntersectUtil.globalIntersect(puzzle.Edges, puzzle.randomPoints, puzzle.N))
        {
            //---------VICTORY----------
            isVictory = true;
        }
    }

    public IEnumerator showVictory()
    {
        isVictory = false;
        victoryScreen.SetActive(true);
        ButtonHandler.victoryActive = true;
        yield return new WaitForSeconds(5f);
        //victoryScreen.SetActive(false);
    }

    public static void updatePoint(string ID, float X, float Z)
    {
        int id = Int32.Parse(ID);
        if (id < 0) return;
        puzzle.randomPoints[id].X = X;
        puzzle.randomPoints[id].Y = Z;
    }

    public void createPuzzle()
    {

        //Destroy all previously created gems
        foreach (GameObject gameObject in gems)
            Destroy(gameObject);
        gems.Clear();

        //Destroy all previously created lines
        foreach (GameObject line in lines)
            Destroy(line);
        lines.Clear();
        

        puzzle.createPuzzle(N, (int) midX, (int) midY, (int) size);

        for (int i = 0; i < puzzle.N; i++)
        {
            myPoint p = puzzle.randomPoints[i];

            //[0.5.0] Get Random Gem from prefabs
            int index = random.Next(5);
            float gemHeight = gemHeights[index];
            GameObject someGem = PrefabGems[index];

            GameObject newGem = Instantiate(someGem);
            newGem.transform.localPosition = new Vector3(p.X, gemHeight, p.Y);
            newGem.GetComponent<MultipleTags>().Rename(0, "" + p.ID);
            gems.Add(newGem);
        }

        //Debug
        /*Debug.Log("Some point is " + puzzle.points[0].X + ", " + puzzle.points[0].Y);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < puzzle.N; i++)
        {
            for (int j = 0; j < puzzle.N; j++)
            {
                sb.Append(" " + puzzle.Edges[i,j]);
            }
            Debug.Log(sb);
            sb.Clear();
        }*/
        //~~ Debug


        //Creating Lines
        for (int i = 0; i < puzzle.N; i++)
        {
            for (int j = i + 1; j < puzzle.N; j++)
            {
                if (puzzle.Edges[i, j] == 1)
                {
                    //Create line
                    GameObject newLine = Instantiate(line);
                    LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
                    Vector3[] positions = new Vector3[2];
                    positions[0] = new Vector3(puzzle.randomPoints[i].X, averageHeight + 0.2f, puzzle.randomPoints[i].Y);
                    positions[1] = new Vector3(puzzle.randomPoints[j].X, averageHeight + 0.2f, puzzle.randomPoints[j].Y);
                    lineRenderer.SetPositions(positions);

                    //TODO: [0.4.0] Set red for intersecting lines
                    puzzle.getIntersectMap();
                    if (puzzle.EdgesIntersectionMap[i, j] == 1)
                        lineRenderer.SetColors(Color.red, Color.red);
                    else
                        lineRenderer.SetColors(Color.blue, Color.blue);

                    lines.Add(newLine);
                }
            }
        }
        //~~~ Creating Lines
    }
}
