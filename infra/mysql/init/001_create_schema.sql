CREATE DATABASE IF NOT EXISTS radarbolsa;
USE radarbolsa;

CREATE TABLE IF NOT EXISTS tracked_assets (
    id BIGINT NOT NULL AUTO_INCREMENT,
    ticker VARCHAR(12) NOT NULL,
    company_name VARCHAR(120) NOT NULL,
    sector VARCHAR(80) NOT NULL,
    is_active BIT NOT NULL DEFAULT b'1',
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    UNIQUE KEY uq_tracked_assets_ticker (ticker)
);

CREATE TABLE IF NOT EXISTS opportunities (
    id BIGINT NOT NULL AUTO_INCREMENT,
    tracked_asset_id BIGINT NOT NULL,
    current_price DECIMAL(10, 2) NOT NULL,
    target_price DECIMAL(10, 2) NOT NULL,
    score INT NOT NULL,
    thesis VARCHAR(500) NOT NULL,
    captured_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    CONSTRAINT fk_opportunities_tracked_assets
        FOREIGN KEY (tracked_asset_id) REFERENCES tracked_assets (id)
);

CREATE TABLE IF NOT EXISTS manual_signals (
    id BIGINT NOT NULL AUTO_INCREMENT,
    tracked_asset_id BIGINT NOT NULL,
    signal_type VARCHAR(16) NOT NULL,
    confidence INT NOT NULL,
    note VARCHAR(500) NOT NULL,
    captured_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    CONSTRAINT fk_manual_signals_tracked_assets
        FOREIGN KEY (tracked_asset_id) REFERENCES tracked_assets (id)
);
