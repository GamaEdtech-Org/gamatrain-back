using CSharpFunctionalExtensions;
using GamaEdtech.DataSource.Utils;
using GamaEdtech.Domain.Base;
using GamaEdtech.Domain.Cities;
using GamaEdtech.Domain.Schools;
using Microsoft.AspNetCore.Mvc;

namespace GamaEdtech.Gateway.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SchoolsController : ControllerBase
{
	private readonly GamaEdtechDbContext _dbContext;
	private readonly ISchoolRepository _schoolRepository;

	public SchoolsController(
		GamaEdtechDbContext dbContext, 
		ISchoolRepository schoolRepository)
	{
		_dbContext = dbContext;
		_schoolRepository = schoolRepository;
	}

	///<summary>
	/// Search for schools by name
	///</summary>
	/// 
	///<remarks>
	/// Sample request:
	/// 
	///     GET /Schools
	///     
	///		Query params:
	///		{
	///			"name": "Required"
	///		}
	///</remarks>
	///
	///<response code="200">Returns list of schools 
	///						(returns empty list if no school is found based on search queries)
	///</response>
	///<response code="500">Server error</response>
	[HttpGet]
	public async Task<IActionResult> Search([FromQuery] string? name)
	{
		var schools = await _schoolRepository.Find(name);

		return Ok(schools.Select(x => new SchoolInListDto
		{
			Id = x.Id.Value,
			Name = new SchoolNameDto
			{
				InEnglish = x.Name.InEnglish,
				InLocalLanguage = x.Name.InLocalLanguage,
			},
			Type = x.Type == SchoolType.Public ? 0 : 1
		}));
	}

	///<summary>
	/// Search for schools by location and radius
	///</summary>
	/// 
	///<remarks>
	/// Sample request:
	/// 
	///     GET /Schools/geo-search
	///     
	///		Query params:
	///		{
	///			"latitude": double - Range [-90, 90],
	///			"longitude": double - Range [-180, 180], 
	///			"radiusInKm": double - Default(0)
	///		}
	///</remarks>
	///
	///<response code="200">Returns list of schools 
	///						(returns empty list if no school is found based on search queries)
	///</response>
	///<response code="400"></response>
	///<response code="500">Server error</response>
	[HttpGet("geo-search")]
	public async Task<IActionResult> SearchByLocation(
		[FromQuery] SearchByLocationDto dto)
	{
		var location = new Location(
				latitude: dto.Latitude,
				longitude: dto.Longitude);

		var schools = await _schoolRepository.FindByLocation(
			location: location,
			radiusInKm: dto.RadiusInKm);

		return Ok(schools.Select(x => new SchoolInListDto
		{
			Id = x.Id.Value,
			Name = new SchoolNameDto
			{
				InEnglish = x.Name.InEnglish,
				InLocalLanguage = x.Name.InLocalLanguage,
			},
			Type = x.Type == SchoolType.Public ? 0 : 1
		}));
	}


	///<summary>
	/// Register school
	///</summary>
	/// 
	///<remarks>
	/// Sample request:
	///
	///     POST /schools
	///     
	///     Request body:
	///     {
	///		  "name": {
	///			"inEnglish": "Required, MaxLength(100)",
	///			"inLocalLanguage": "Optional, MaxLenght(100)"
	///		  },
	///		  "type": 0,
	///		  "address": {
	///			"description": "Required, MaxLenght(500)",
	///			"location": {
	///			  "latitude": double - Range [-90, 90],
	///			  "longitude": double - Range [-180, 180],
	///			},
	///			"state": "Required, MaxLenght(50)",
	///			"city": "Required, MaxLenght(50)",
	///			"zipCode": "Required, MaxLenght(10)"
	///		  }
	///		}
	///</remarks>
	///
	///<response code="201">Returns newly registered school's Id at header</response>
	///<response code="400"></response>
	///<response code="500">Server error</response>
	[HttpPost]
	public async Task<IActionResult> RegisterSchool(
		[FromBody] RegisterSchoolDto dto)
	{
		var name = new SchoolName(
				inEnglish: dto.Name.InEnglish,
				inLocalLanguage: dto.Name.InLocalLanguage);

		var location = new Location(
					latitude: dto.Address.Location.Latitude,
					longitude: dto.Address.Location.Longitude);

		var address = new Address(
				description: dto.Address.Description,
				location: location,
				stateId: dto.Address.StateId.HasValue ? new Id(dto.Address.StateId.Value) : null,
				cityId: new Id(dto.Address.CityId),
				zipCode: dto.Address.ZipCode);


		var school = new School(
			name: name,
			type: dto.Type == 0 ? SchoolType.Public : SchoolType.Private,
			address: address);

		await _schoolRepository.Add(school);
		await _dbContext.SaveChangesAsync();

		return CreatedAtAction(nameof(RegisterSchool), new { id = school.Id }, null); ;
	}

	///<summary>
	/// Remove School
	///</summary>
	/// 
	///<remarks>
	/// Sample request:
	///
	///     DELETE /Schools/{id:int}
	///</remarks>
	///
	///<response code="204"></response>
	///<response code="404"></response>
	///<response code="500">Server error</response>
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Remove([FromRoute] int id)
	{
		var city = await _schoolRepository.GetBy(new Id(id));

		if (city is null)
			return NotFound();

		await _schoolRepository.Remove(city);
		await _dbContext.SaveChangesAsync();

		return NoContent();
	}
}

