USE radarbolsa;

INSERT INTO tracked_assets (ticker, company_name, sector, is_active)
VALUES
    ('PETR4', 'Petrobras PN', 'Energia', b'1'),
    ('ITUB4', 'Itau Unibanco PN', 'Financeiro', b'1'),
    ('WEGE3', 'WEG ON', 'Industria', b'1'),
    ('TAEE11', 'Taesa Unit', 'Utilidade Publica', b'1')
ON DUPLICATE KEY UPDATE
    company_name = VALUES(company_name),
    sector = VALUES(sector),
    is_active = VALUES(is_active);

INSERT INTO opportunities (
    tracked_asset_id,
    current_price,
    target_price,
    score,
    thesis,
    captured_at)
SELECT
    ta.id,
    31.42,
    37.80,
    84,
    'Geracao de caixa consistente, dividendos relevantes e desconto frente aos pares.',
    '2026-04-29 12:00:00'
FROM tracked_assets ta
WHERE ta.ticker = 'PETR4'
  AND NOT EXISTS (
      SELECT 1
      FROM opportunities o
      WHERE o.tracked_asset_id = ta.id
        AND o.captured_at = '2026-04-29 12:00:00'
  );

INSERT INTO opportunities (
    tracked_asset_id,
    current_price,
    target_price,
    score,
    thesis,
    captured_at)
SELECT
    ta.id,
    34.15,
    39.60,
    81,
    'Rentabilidade elevada, qualidade operacional e resiliencia em ciclos distintos.',
    '2026-04-29 12:05:00'
FROM tracked_assets ta
WHERE ta.ticker = 'ITUB4'
  AND NOT EXISTS (
      SELECT 1
      FROM opportunities o
      WHERE o.tracked_asset_id = ta.id
        AND o.captured_at = '2026-04-29 12:05:00'
  );

INSERT INTO opportunities (
    tracked_asset_id,
    current_price,
    target_price,
    score,
    thesis,
    captured_at)
SELECT
    ta.id,
    48.90,
    55.00,
    78,
    'Execucao recorrente e exposicao a tendencias de eletrificacao e eficiencia.',
    '2026-04-29 12:10:00'
FROM tracked_assets ta
WHERE ta.ticker = 'WEGE3'
  AND NOT EXISTS (
      SELECT 1
      FROM opportunities o
      WHERE o.tracked_asset_id = ta.id
        AND o.captured_at = '2026-04-29 12:10:00'
  );

INSERT INTO opportunities (
    tracked_asset_id,
    current_price,
    target_price,
    score,
    thesis,
    captured_at)
SELECT
    ta.id,
    34.70,
    38.10,
    74,
    'Receita previsivel, foco em transmissao e perfil defensivo para carteira.',
    '2026-04-29 12:15:00'
FROM tracked_assets ta
WHERE ta.ticker = 'TAEE11'
  AND NOT EXISTS (
      SELECT 1
      FROM opportunities o
      WHERE o.tracked_asset_id = ta.id
        AND o.captured_at = '2026-04-29 12:15:00'
  );
