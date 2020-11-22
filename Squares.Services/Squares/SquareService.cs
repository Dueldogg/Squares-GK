using System.Collections.Generic;
using System.Linq;
using Squares.Models.Squares;
using Squares.Models.Coordinates;
using Squares.Persistence;
using System;
using System.Threading.Tasks;

namespace Squares.Services.Squares
{
    public class SquareService : ISquareService
    {
        private readonly SquareDbContext _squareDbContext;
        public SquareService(SquareDbContext squareDbContext)
        {
            _squareDbContext = squareDbContext;
        }

        public async Task<List<Square>> GetSquares(int coordinateListId)
        {
            CoordinateList coordinateList = await _squareDbContext.GetCoordinateList(coordinateListId).ConfigureAwait(false);
            List<Square> squares = GetSquares(coordinateList.Coordinates);

            return squares;
        }

        private List<Square> GetSquares(List<Coordinate> coordinates)
        {
            List<Square> squares = new List<Square>();

            HashSet<Coordinate> set = new HashSet<Coordinate>();
            foreach (var coordinate in coordinates)
            {
                set.Add(coordinate);
            }

            for (int i = 0; i < coordinates.Count; i++)
            {
                for (int j = 0; j < coordinates.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    Coordinate x1 = coordinates[i];
                    Coordinate x2 = coordinates[j];

                    Coordinate[] neededCoordinates = GetNeededDiagonalCoordinates(x1, x2);
                    try
                    {
                        Coordinate x3 = coordinates.FirstOrDefault(x => x.X == neededCoordinates[0].X && x.Y == neededCoordinates[0].Y);
                        if (x3 != null)
                        {
                            Coordinate x4 = coordinates.FirstOrDefault(x => x.X == neededCoordinates[1].X && x.Y == neededCoordinates[1].Y);
                            if (x4 != null)
                            {
                                Square square = new Square
                                {
                                    Coordinates = new List<Coordinate> { x1, x2, x3, x4 }
                                };

                                bool squaresListContainsGivenSquare = SquaresListContainsGivenSquare(squares, square);

                                if (!squaresListContainsGivenSquare)
                                {
                                    squares.Add(square);
                                }
                            }
                        }
                    }
                    catch(Exception)
                    {
                        continue;
                    }                                     
                }
            }

            return squares;
        }

        private bool SquaresListContainsGivenSquare(List<Square> squares, Square square)
        {
            foreach(Square existingSquare in squares)
            {
                List<Coordinate> firstNotSecond = existingSquare.Coordinates.Except(square.Coordinates).ToList();
                List<Coordinate> secondNotFirst = square.Coordinates.Except(existingSquare.Coordinates).ToList();

                if(!firstNotSecond.Any() && !secondNotFirst.Any())
                {
                    return true;
                }
            }

            return false;
        }

        private Coordinate[] GetNeededDiagonalCoordinates(Coordinate x1, Coordinate x2)
        {
            Coordinate[] res = new Coordinate[2];

            if(((x1.X + x2.X) % 2) > 0 || ((x1.Y + x2.Y) % 2) > 0)
            {
                return res;
            }

            int midX = (x1.X + x2.X) / 2;
            int midY = (x1.Y + x2.Y) / 2;

            int Ax = x1.X - midX;
            int Ay = x1.Y - midY;
            int bX = midX - Ay;
            int bY = midY + Ax;

            Coordinate b = new Coordinate {
                X = bX,
                Y = bY
            };

            int cX = (x2.X - midX);
            int cY = (x2.Y - midY);
            int dX = midX - cY;
            int dY = midY + cX;

            Coordinate d = new Coordinate {
                X = dX,
                Y = dY
            };

            res[0] = b;
            res[1] = d;

            return res;
        }
    }
}
