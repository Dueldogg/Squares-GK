using Squares.Models.Coordinates;
using Squares.Persistence.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Squares.Persistence
{
    public class SquareDbContext : DbContext
    {
        public SquareDbContext(DbContextOptions<SquareDbContext> options) : base(options) { }
        private DbSet<CoordinateListDto> CoordinateLists { get; set; }
        private DbSet<CoordinateDto> Coordinates { get; set; }

        public async Task<CoordinateList> CreateCoordinateList(List<Coordinate> coordinates)
        {
            CoordinateListDto coordinateListDto = new CoordinateListDto();
            CoordinateLists.Add(coordinateListDto);
            await SaveChangesAsync().ConfigureAwait(false);

            coordinates.ForEach(x => x.CoordinateListId = coordinateListDto.Id);
            List<CoordinateDto> coordinateDtos = coordinates.Select(DtoMapper.Map).ToList();
            Coordinates.AddRange(coordinateDtos);
            await SaveChangesAsync().ConfigureAwait(false);

            CoordinateList createdCoordinateList = DtoMapper.Map(coordinateListDto, coordinateDtos);

            return createdCoordinateList;
        }

        public async Task<CoordinateList> GetCoordinateList(int coordinateListId)
        {
            CoordinateListDto coordinateListDto = await CoordinateLists
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == coordinateListId)
                .ConfigureAwait(false);
            List<CoordinateDto> coordinateDtos = await Coordinates
                .AsQueryable()
                .Where(x => x.CoordinateListId == coordinateListId)
                .ToListAsync()
                .ConfigureAwait(false);
            CoordinateList coordinateList = DtoMapper.Map(coordinateListDto, coordinateDtos);

            return coordinateList;
        }

        public async Task AddCoordinateToList(int coordinateListId, Coordinate coordinate)
        {
            coordinate.CoordinateListId = coordinateListId;
            CoordinateDto coordinateDto = DtoMapper.Map(coordinate);
            Coordinates.Add(coordinateDto);
            await SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteCoordinate(int coordinateListId, int coordinateId)
        {
            CoordinateDto coordinateToRemove = await Coordinates
                .FirstOrDefaultAsync(x => x.CoordinateListId == coordinateListId && x.Id == coordinateId)
                .ConfigureAwait(false);
            Coordinates.Remove(coordinateToRemove);
            await SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
