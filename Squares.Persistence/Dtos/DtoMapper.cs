using Squares.Models.Coordinates;
using System.Collections.Generic;
using System.Linq;

namespace Squares.Persistence.Dtos
{
    public static class DtoMapper
    {
        static public CoordinateDto Map(Coordinate coordinate)
        {
            return new CoordinateDto
            {
                Id = coordinate.Id,
                CoordinateListId = coordinate.CoordinateListId,
                X = coordinate.X,
                Y = coordinate.Y
            };
        }

        static public CoordinateListDto Map(CoordinateList coordinateList)
        {
            return new CoordinateListDto
            {
                Id = coordinateList.Id
            };
        }

        static public Coordinate Map(CoordinateDto coordinateDto)
        {
            return new Coordinate
            {
                Id = coordinateDto.Id,
                CoordinateListId = coordinateDto.CoordinateListId,
                X = coordinateDto.X,
                Y = coordinateDto.Y
            };
        }

        static public CoordinateList Map(CoordinateListDto coordinateListDto, List<CoordinateDto> coordinateDtos)
        {
            return new CoordinateList
            {
                Id = coordinateListDto.Id,
                Coordinates = coordinateDtos.Select(Map).ToList()
            };
        }
    }
}
