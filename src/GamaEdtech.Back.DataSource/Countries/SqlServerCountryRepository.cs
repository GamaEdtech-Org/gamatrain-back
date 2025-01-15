

using GamaEdtech.Back.DataSource.Utils;
using GamaEdtech.Back.Domain.Base;
using GamaEdtech.Back.Domain.Countries;
using Microsoft.EntityFrameworkCore;

namespace GamaEdtech.Back.DataSource.Contries;

public class SqlServerCountryRepository : ICountryRepository
{
	private readonly GamaEdtechDbContext _dbContext;

	public SqlServerCountryRepository(GamaEdtechDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Country?> GetBy(Id id)
	{
		return await _dbContext.Countries.FindAsync(id);
	}

	public async Task<bool> ContainsCountrywithName(string name)
	{
		return await _dbContext.Countries
			.SingleOrDefaultAsync(x => x.Name == name) is not null;
	}

	public async Task<bool> ContainsCountrywithCode(string code)
	{
		return await _dbContext.Countries
			.SingleOrDefaultAsync(x => x.Code == code) is not null;
	}

	public async Task Add(Country country)
	{
		await _dbContext.Countries.AddAsync(country);
	}

	public async Task Remove(Country country)
	{
		_dbContext.Countries.Remove(country);
	}
}
