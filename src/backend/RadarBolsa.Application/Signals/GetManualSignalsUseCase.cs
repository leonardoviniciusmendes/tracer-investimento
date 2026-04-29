using RadarBolsa.Application.Abstractions.Persistence;
using RadarBolsa.Domain.Signals;

namespace RadarBolsa.Application.Signals;

public sealed class GetManualSignalsUseCase(
    ISignalRepository signalRepository)
{
    public Task<IReadOnlyList<ManualSignal>> ExecuteAsync(
        CancellationToken cancellationToken) =>
        signalRepository.ListAsync(cancellationToken);
}
