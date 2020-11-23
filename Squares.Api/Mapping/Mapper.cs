using Squares.Contracts.Coordinates;
using Squares.Contracts.Squares;
using System.Linq;

namespace Squres.Api.Mapping
{
    public static class Mapper
    {
        public static Coordinate Map(Squares.Models.Coordinates.Coordinate coordinate)
        {
            return new Coordinate
            {
                Id = coordinate.Id,
                X = coordinate.X,
                Y = coordinate.Y
            };
        }

        public static Squares.Models.Coordinates.Coordinate Map(CoordinateCreateRequest coordinate)
        {
            return new Squares.Models.Coordinates.Coordinate
            {
                X = coordinate.X,
                Y = coordinate.Y
            };
        }

        public static CoordinateList Map(Squares.Models.Coordinates.CoordinateList coordinateList)
        {
            return new CoordinateList
            {
                Id = coordinateList.Id,
                Coordinates = coordinateList.Coordinates.Select(Map).ToList()
            };
        }

        public static Square Map(Squares.Models.Squares.Square square)
        {
            return new Square
            {
                Coordinates = square.Coordinates.Select(Map).ToList()
            };
        }
    }
}
