using RadarBolsa.Application.Signals;
using RadarBolsa.Domain.Signals;

namespace RadarBolsa.Application.Abstractions.Persistence;

public interface ISignalRepository
{
    Task<ManualSignal> AddAsync(
        CreateManualSignalInput input,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ManualSignal>> ListAsync(
        CancellationToken cancellationToken);
}
