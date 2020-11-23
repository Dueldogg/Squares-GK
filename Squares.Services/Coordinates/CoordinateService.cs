using Squares.Models.Coordinates;
using Squares.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Squares.Services.Coordinates
{
    public class CoordinateService : ICoordinateService
    {
        private readonly SquareDbContext _squareDbContext;
        public CoordinateService(SquareDbContext squareDbContext) {
            _squareDbContext = squareDbContext;
        }

        public async Task<CoordinateList> CreateCoordinateList(List<Coordinate> coordinates)
        {
            CoordinateList createdCoordinateList = await _squareDbContext.CreateCoordinateList(coordinates).ConfigureAwait(false);
            return createdCoordinateList;
        }

        public async Task<CoordinateList> GetCoordinateList(int coordinateListId)
        {
            CoordinateList updatedCoordinateList = await _squareDbContext.GetCoordinateList(coordinateListId).ConfigureAwait(false);
            return updatedCoordinateList;
        }

        public async Task CreateCoordinateInList(int coordinateListId, Coordinate coordinate)
        {
            await _squareDbContext.AddCoordinateToList(coordinateListId, coordinate).ConfigureAwait(false);
        }

        public async Task DeleteCoordinate(int coordinateListId, int coordinateId)
        {
            await _squareDbContext.DeleteCoordinate(coordinateListId, coordinateId).ConfigureAwait(false);
        }
    }
}
