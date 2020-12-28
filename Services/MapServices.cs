using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hodogram.IServices;
using Hodogram.Models;

namespace Hodogram.Services
{
    public class MapServices : IMapService
    {
        public Field[,] createMap(int width, int heigth, List<FieldDTO> nonEmptyFields)
        {
            Field[,] map = new Field[width, heigth];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < heigth; j++)
                {
                    var field = new Field();

                    field.X_Cord = i;
                    field.Y_Cord = j;

                    map[i, j] = field;
                }
            }

            this.assignFields(map, width, heigth, nonEmptyFields);
            return map;
        }

        public Field[,] assignFields(Field[,] map, int width, int heigth, List<FieldDTO> nonEmptyFields)
        {
            int emptyIndex = 0;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < heigth; j++)
                {
                    if (i != 0)
                    {
                        map[i, j].Left = map[i - 1, j];
                    }

                    if (i != width - 1)
                    {
                        map[i, j].Right = map[i + 1, j];
                    }

                    if (j != 0)
                    {
                        map[i, j].Down = map[i, j - 1];
                    }

                    if (j != heigth - 1)
                    {
                        map[i, j].Up = map[i, j + 1];
                    }

                    map[i, j].Status = "prazno";
                }
            }

            map[1, 0].Status = "zid";
            map[1, 1].Status = "zid";
            map[1, 2].Status = "zid";
            map[1, 3].Status = "zid";
            map[3, 0].Status = "zid";
            map[3, 2].Status = "zid";
            map[3, 3].Status = "zid";
            map[4, 3].Status = "zid";

            // foreach (var nonEmptyField in nonEmptyFields)
            // {
            //     map[nonEmptyField.X_Cord, nonEmptyField.Y_Cord].Status = nonEmptyField.Status;
            // }

            return map;
        }

        public Field[] findPath(Field[] targetFields)
        {
            return sortAscYCord(sortAscYCord(targetFields));
        }

        public List<Field> findPathNoWalls(Field[] targetFields)
        {
            sortAscXCord(sortAscYCord(targetFields));

            List<Field> finalPath = new List<Field>();

            finalPath.Add(targetFields[0]);

            for (int i = 0; i < targetFields.Length - 1; i++)
            {
                int minPathCost = 9999;

                Field fieldToCompare = new Field();

                for (int j = 0; j < targetFields.Length; j++)
                {
                    int newPathCost = calculatePathCost(finalPath.Last(), targetFields[j]);

                    if (newPathCost < minPathCost && (finalPath.Last() != targetFields[j]) &&
                        !finalPath.Contains(targetFields[j]))
                    {
                        minPathCost = newPathCost;
                        fieldToCompare = targetFields[j];
                    }
                }

                if (!finalPath.Contains(fieldToCompare))
                {
                    finalPath.Add(fieldToCompare);
                }
            }

            return finalPath;
        }

        public List<FieldDTO> findPathWithWalls(Field[,] map, Field start, Field end)
        {
            List<Field> finalPath = new List<Field>();

            Field currentField = start;
            Field previousField = start;

            while (currentField != end)
            {
                int stuck = 0;

                List<Path> potentialCandidateFields = new List<Path>();

                if (CanMove(currentField.Left, previousField))
                {
                    var potentialCandidateField = new Path()
                    {
                        field = currentField.Left,
                        cost = calculatePathCost(currentField.Left, end),
                    };
                    potentialCandidateFields.Add(potentialCandidateField);
                }
                else
                {
                    stuck++;
                }

                if (CanMove(currentField.Right, previousField))
                {
                    var potentialCandidateField = new Path()
                    {
                        field = currentField.Right,
                        cost = calculatePathCost(currentField.Right, end),
                    };
                    potentialCandidateFields.Add(potentialCandidateField);
                }
                else
                {
                    stuck++;
                }

                if (CanMove(currentField.Up, previousField))
                {
                    var potentialCandidateField = new Path()
                    {
                        field = currentField.Up,
                        cost = calculatePathCost(currentField.Up, end),
                    };
                    potentialCandidateFields.Add(potentialCandidateField);
                }
                else
                {
                    stuck++;
                }


                if (CanMove(currentField.Down, previousField))
                {
                    var potentialCandidateField = new Path()
                    {
                        field = currentField.Down,
                        cost = calculatePathCost(currentField.Down, end),
                    };
                    potentialCandidateFields.Add(potentialCandidateField);
                }
                else
                {
                    stuck++;
                }

                Field nextField;

                if (stuck != 4)
                {
                    var smallestPath = potentialCandidateFields.Min(x => x.cost);

                    nextField = potentialCandidateFields.First(x => x.cost == smallestPath).field;
                    previousField = currentField;
                    currentField = nextField;
                    finalPath.Add(currentField);
                }
                else
                {
                    finalPath.Remove(currentField);
                    finalPath.Remove(previousField);
                    nextField = previousField;
                    previousField = currentField;
                    currentField = nextField;
                }
            }

            return createFinalPath(finalPath);
        }

        public int calculatePathCost(Field field1, Field field2)
        {
            int pathCost;

            if (field1.X_Cord == field2.X_Cord)
            {
                pathCost = Math.Abs(field1.Y_Cord - field2.Y_Cord);
            }
            else if (field1.Y_Cord == field2.Y_Cord)
            {
                pathCost = Math.Abs(field1.X_Cord - field2.X_Cord);
            }
            else
            {
                pathCost = Math.Abs(field1.X_Cord - field2.X_Cord) + Math.Abs(field1.Y_Cord - field2.Y_Cord);
            }

            return pathCost;
        }


        private Field[] sortAscXCord(Field[] targetFields)
        {
            for (int i = 0; i < targetFields.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (targetFields[i].X_Cord < targetFields[j].X_Cord)
                    {
                        Field temp;

                        temp = targetFields[i];
                        targetFields[i] = targetFields[j];
                        targetFields[j] = temp;
                    }
                }
            }

            return targetFields;
        }

        private Field[] sortAscYCord(Field[] targetFields)
        {
            for (int i = 0; i < targetFields.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if ((targetFields[i].X_Cord == targetFields[j].X_Cord) &&
                        (targetFields[i].Y_Cord < targetFields[j].Y_Cord))
                    {
                        Field temp;

                        temp = targetFields[i];
                        targetFields[i] = targetFields[j];
                        targetFields[j] = temp;
                    }
                }
            }

            return targetFields;
        }

        private bool CanMove(Field targetField, Field previousField)
        {
            if (targetField != null && targetField.Status == "prazno")
            {
                if (targetField != previousField)
                {
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        private List<FieldDTO> createFinalPath(List<Field> targetPath)
        {
            List<FieldDTO> finalPath = new List<FieldDTO>();
            foreach (var field in targetPath)
            {
                var protofield = new FieldDTO()
                {
                    X_Cord = field.X_Cord,
                    Y_Cord = field.Y_Cord,
                    Status = field.Status
                };
                finalPath.Add(protofield);
            }

            return finalPath;
        }
    }
}