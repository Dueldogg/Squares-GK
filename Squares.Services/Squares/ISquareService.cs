using System.Collections.Generic;
using System.Threading.Tasks;
using Squares.Models.Squares;

namespace Squares.Services.Squares
{
    public interface ISquareService
    {
        Task<List<Square>> GetSquares(int coordinateListId);
    }
}
