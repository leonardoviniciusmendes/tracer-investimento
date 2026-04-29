USE radarbolsa;

INSERT INTO manual_signals (
    tracked_asset_id,
    signal_type,
    confidence,
    note,
    captured_at)
SELECT
    ta.id,
    'buy',
    88,
    'Fluxo operacional consistente e tese de dividendos reforcada pelo acompanhamento manual.',
    '2026-04-29 13:00:00'
FROM tracked_assets ta
WHERE ta.ticker = 'PETR4'
  AND NOT EXISTS (
      SELECT 1
      FROM manual_signals ms
      WHERE ms.tracked_asset_id = ta.id
        AND ms.signal_type = 'buy'
        AND ms.captured_at = '2026-04-29 13:00:00'
  );

INSERT INTO manual_signals (
    tracked_asset_id,
    signal_type,
    confidence,
    note,
    captured_at)
SELECT
    ta.id,
    'watch',
    74,
    'Ativo permanece monitorado pela resiliencia operacional e liquidez elevada.',
    '2026-04-29 13:05:00'
FROM tracked_assets ta
WHERE ta.ticker = 'ITUB4'
  AND NOT EXISTS (
      SELECT 1
      FROM manual_signals ms
      WHERE ms.tracked_asset_id = ta.id
        AND ms.signal_type = 'watch'
        AND ms.captured_at = '2026-04-29 13:05:00'
  );
