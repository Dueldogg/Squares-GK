using Squares.Models.Coordinates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Squares.Services.Coordinates
{
    public interface ICoordinateService
    {
        Task<CoordinateList> CreateCoordinateList(List<Coordinate> coordinates);
        Task<CoordinateList> GetCoordinateList(int coordinateListId);
        Task AddCoordinateToList(int coordinateListId, Coordinate coodrinate);
        Task DeleteCoordinate(int coordinateListId, int coodrinateId);
    }
}
