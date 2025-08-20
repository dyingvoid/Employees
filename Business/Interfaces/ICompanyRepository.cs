using Data.Entities;

namespace Business.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetById(int id, CancellationToken ct = default);
}