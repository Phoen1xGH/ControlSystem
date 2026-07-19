CREATE DATABASE IF NOT EXISTS logs;

-- Структурные логи приложения (распарсенный CLEF-JSON от Serilog).
CREATE TABLE IF NOT EXISTS logs.app_logs
(
    timestamp      DateTime64(3),
    level          LowCardinality(String),
    message        String,
    template       String,
    source_context LowCardinality(String),
    operation      LowCardinality(String),
    request_id     String,
    request_path   String,
    exception      String,
    namespace      LowCardinality(String),
    pod            String,
    container      LowCardinality(String)
)
ENGINE = MergeTree
ORDER BY (timestamp)
TTL toDateTime(timestamp) + INTERVAL 3 DAY;

-- Сырые логи инфраструктуры (Postgres/Redis/MinIO — свой текстовый формат).
CREATE TABLE IF NOT EXISTS logs.infra_logs
(
    timestamp DateTime64(3) DEFAULT now64(3),
    namespace LowCardinality(String),
    pod       String,
    container LowCardinality(String),
    message   String
)
ENGINE = MergeTree
ORDER BY (timestamp)
TTL toDateTime(timestamp) + INTERVAL 3 DAY;
