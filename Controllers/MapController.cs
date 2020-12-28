using System.Collections;
using System.Collections.Generic;
using Hodogram.IServices;
using Hodogram.Models;
using Hodogram.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hodogram.Controllers
{
    [Route("map")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IMapService _mapServices;

        public MapController(IMapService mapServices)
        {
            _mapServices = mapServices;
        }

        // [HttpPost]
        // public void GetMap(int width, int heigth)
        // {
        //     _mapServices.createMap(width, heigth);
        // }

        [HttpPost("findpath")]
        public Field[] FindPath([FromBody] Field[] targetFields)
        {
            return _mapServices.findPath(targetFields);
        }

        [HttpPost("findpathnowalls")]
        public List<Field> FindPathNoWalls([FromBody] Field[] targetFields)
        {
            return _mapServices.findPathNoWalls(targetFields);
        }

        [HttpPost("calculatepathcost")]
        public int CalculatePathCost([FromBody] Field[] targetFields)
        {
            return _mapServices.calculatePathCost(targetFields[0], targetFields[1]);
        }

        [HttpPost("findpathwithwalls")]
        public List<FieldDTO> FindPathWithWalls(int width, int heigth, int endXCord, int endYCord, List<FieldDTO> nonEmptyFields)
        {
            var map = _mapServices.createMap(width, heigth, nonEmptyFields);
            var x = _mapServices.findPathWithWalls(map, map[0, 0], map[endXCord, endYCord]);

            return x;
        }
    }
}