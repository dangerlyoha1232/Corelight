using System.Collections.Generic;
using UnityEngine;
using Services;

namespace Plates
{
    public class PlatePath : MonoBehaviour, IService
    {
        public static PlatePath Instance;

        public void Init()
        {
            Instance = this;
        }

        public List<Plate> GeneratePath(Plate start, Plate end)
        {
            List<Plate> openSet = new List<Plate>();

            foreach (var plate in Plate.AllPlates)
            {
                plate.GScore = float.MaxValue;
            }

            start.GScore = 0;
            start.HScore = Vector3.Distance(start.transform.position, end.transform.position);
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                int lowestF = 0;

                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].FScore() < openSet[lowestF].FScore())
                    {
                        lowestF = i;
                    }
                }
                
                Plate currentPlate = openSet[lowestF];
                openSet.Remove(currentPlate);

                if (currentPlate == end)
                {
                    List<Plate> path = new List<Plate>();
                    
                    path.Insert(0, end);

                    while (currentPlate != start)
                    {
                        currentPlate = currentPlate.CameFrom;
                        path.Add(currentPlate);
                    }
                    
                    path.Reverse();
                    return path;
                }

                foreach (var connectedPlate in currentPlate.Connections)
                {
                    float heldGScore = currentPlate.GScore + Vector3.Distance(currentPlate.transform.position, connectedPlate.transform.position);

                    if (heldGScore < connectedPlate.GScore)
                    {
                        connectedPlate.CameFrom = currentPlate;
                        connectedPlate.GScore = heldGScore;
                        connectedPlate.HScore = Vector3.Distance(connectedPlate.transform.position, end.transform.position);

                        if (!openSet.Contains(connectedPlate))
                        {
                            openSet.Add(connectedPlate);
                        }
                    }
                }
            }

            return null;
        }
    }
}