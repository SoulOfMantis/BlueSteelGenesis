using System.Collections.Generic;
using UnityEngine;




    /// <summary>
    /// класс модуля
    /// </summary>
    public abstract class GameModule

    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int range = 0;
       protected List<Vector3Int> getAvailableCells(int n, Vector3Int start)
        {
            List<Vector3Int> res = new List<Vector3Int>();
            HashSet<Vector3Int> toAdd = new HashSet<Vector3Int>();
            res.Add(start);
            for (int i = 1; i <= n; i++)
            {
                foreach (var cell in res)
                {
                    toAdd.Add(new Vector3Int(cell.x + 1, cell.y));
                    toAdd.Add(new Vector3Int(cell.x - 1, cell.y));
                    toAdd.Add(new Vector3Int(cell.x, cell.y + 1));
                    toAdd.Add(new Vector3Int(cell.x, cell.y - 1));

                }
                foreach (var cell in toAdd)
                {
                    if (!res.Contains(cell))
                        res.Add(cell);
                }
                toAdd.Clear();
            }
            return res;
        }
        public virtual List<Vector3Int> getCellsInRange(Vector3Int start)
        {
            return getAvailableCells(range, start);
        }

        public void changeName(string newName) => Name = newName;
        public abstract void Effect(Character user, Vector3Int pos);

        public virtual void Initialize()
        {
            Debug.Log($"Module {GetType().Name} initialized");
        }

    }


