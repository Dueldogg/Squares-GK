using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Squares.Api;
using Squres.Api.Mapping;
using Squares.Contracts.Coordinates;
using Squares.Contracts.Squares;
using Squares.Services.Coordinates;
using Squares.Services.Squares;

namespace Squres.Api.Controllers
{
    [ApiController]
    public class CoordinatesController : ControllerBase
    {
        private readonly ICoordinateService _coordinateService;
        private readonly ISquareService _squareService;
        public CoordinatesController(ICoordinateService coordinateService, ISquareService squareService)
        {
            _coordinateService = coordinateService;
            _squareService = squareService;
        }

        /// <summary>
        /// Create a new coordinate list with coordinates.
        /// </summary>
        /// <param name="coordinates">Requried information to create coordinates.</param>
        /// <returns>The created coordinate list.</returns>
        /// <response code="201">The coordinate list was successfully created.</response>
        /// <response code="400">The coordinate list request is malformed.</response>
        [HttpPost("coordinateLists")]
        [ProducesResponseType(typeof(CoordinateList), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateCoordinateList([FromBody] List<CoordinateCreateRequest> coordinates)
        {
            List<Squares.Models.Coordinates.Coordinate> mappedCoordinates = coordinates.Select(Mapper.Map).ToList();
            Squares.Models.Coordinates.CoordinateList createdCoordinateList = await _coordinateService.CreateCoordinateList(mappedCoordinates).ConfigureAwait(false);
            CoordinateList apiCoordinateList = Mapper.Map(createdCoordinateList);

            return CreatedAtRoute(RouteNames.GetCoordinateListById, new { coordinateListId = apiCoordinateList.Id }, apiCoordinateList);
        }

        /// <summary>
        /// Get a coordinate list by unique identifier.
        /// </summary>
        /// <param name="coordinateListId">Coordinate list unique identifier.</param>
        /// <returns>The requested coordinate list.</returns>
        /// <response code="200">The coordinate list was successfully retrieved.</response>
        /// <response code="404">The coordinate list does not exist.</response>
        [HttpGet("coordinateLists/{coordinateListId}", Name = RouteNames.GetCoordinateListById)]
        [ProducesResponseType(typeof(CoordinateList), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCoordinateList(int coordinateListId)
        {
            Squares.Models.Coordinates.CoordinateList createdCoordinateList = await _coordinateService.GetCoordinateList(coordinateListId).ConfigureAwait(false);
            CoordinateList apiCoordinateList = Mapper.Map(createdCoordinateList);

            return Ok(apiCoordinateList);
        }

        /// <summary>
        /// Add a new coordinate in a coordinate list.
        /// </summary>
        /// <param name="coordinateListId">Coordinate list unique identifier.</param>
        /// <param name="coordinate">Requried information to create a new coordinate.</param>
        /// <returns>The updated coordinate list.</returns>
        /// <response code="200">The coordinate was successfully added to the coordinate list.</response>
        [HttpPost("coordinateLists/{coordinateListId}/coordinate")]
        [ProducesResponseType(typeof(CoordinateList), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddCoordinate(int coordinateListId, [FromBody] CoordinateCreateRequest coordinate)
        {
            Squares.Models.Coordinates.Coordinate mappedCoordinate = Mapper.Map(coordinate);
            await _coordinateService.AddCoordinateToList(coordinateListId, mappedCoordinate).ConfigureAwait(false);

            return Ok();
        }

        /// <summary>
        /// Delete a single coordinate from a coordinate list by unique identifier.
        /// </summary>
        /// <param name="coordinateListId">Coordinate list unique identifier.</param>
        /// <param name="coordinateId">Coordinate unique identifier.</param>
        /// <response code="204">The coordinate was successfully deleted.</response>
        /// <response code="404">The coordinate does not exist.</response>
        [HttpDelete("coordinateLists/{coordinateListId}/coordinates/{coordinateId}")]
        [ProducesResponseType(typeof(Coordinate), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCoordinate(int coordinateListId, int coordinateId)
        {
            await _coordinateService.DeleteCoordinate(coordinateListId, coordinateId).ConfigureAwait(false);

            return NoContent();
        }

        /// <summary>
        /// Get all possible squares from a coordinate list by unique identifier.
        /// </summary>
        /// <param name="coordinateListId">Coordinate list unique identifier.</param>
        /// <returns>A list of possible squares.</returns>
        /// <response code="200">The squares were successfully retrieved.</response>
        [HttpGet("coordinateLists/{coordinateListId}/squares")]
        [ProducesResponseType(typeof(Square), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSquares(int coordinateListId)
        {
            List<Squares.Models.Squares.Square> squares = await _squareService.GetSquares(coordinateListId).ConfigureAwait(false);
            List<Square> apiSquares = squares.Select(Mapper.Map).ToList();

            return Ok(apiSquares);
        }
    }
}
