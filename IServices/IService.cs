using System.Collections;
using System.Collections.Generic;
using Hodogram.Models;

namespace Hodogram.IServices
{
    public interface IMapService
    {
        Field[,] createMap(int width, int heigth, List<FieldDTO> nonEmptyFields);
        Field[,] assignFields(Field[,] map, int width, int heigth, List<FieldDTO> nonEmptyFields);
        Field[] findPath(Field[] targetFields);
        List<Field> findPathNoWalls(Field[] targetFields);
        int calculatePathCost(Field field1, Field field2);
        List<FieldDTO> findPathWithWalls(Field[,] map, Field start, Field end);
    }
}