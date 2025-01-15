using GamaEdtech.Back.DataSource.Utils;
using GamaEdtech.Back.Domain.Base;
using GamaEdtech.Back.Domain.States;
using Microsoft.EntityFrameworkCore;

namespace GamaEdtech.Back.DataSource.States;

public class SqlServerStateRepository : IStateRepository
{
	private readonly GamaEdtechDbContext _dbContext;

	public SqlServerStateRepository(GamaEdtechDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<State?> GetBy(Id id)
	{
		return await _dbContext.States.FindAsync(id);
	}

	public async Task<bool> ContainsStateWithNameInCountry(string name, Id countryId)
	{
		return await _dbContext.States
			.Where(x => x.Name == name && x.CountryId == countryId)
			.AnyAsync();
	}

	public async Task<bool> ContainsStateWithCodeInCountry(string code, Id countryId)
	{
		return await _dbContext.States
			.Where(x => x.Code == code && x.CountryId == countryId)
			.AnyAsync();
	}

	public async Task<bool> ContainsStateInCountryWith(Id id)
	{
		return await _dbContext.States
			.Where(x => x.CountryId == id)
			.AnyAsync();
	}

	public async Task Add(State state)
	{
		await _dbContext.States.AddAsync(state);
	}

	public async Task Remove(State state)
	{
		_dbContext.States.Remove(state);
	}
}
