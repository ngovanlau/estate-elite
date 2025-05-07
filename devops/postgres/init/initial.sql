DO $$
BEGIN
-- Create user if not exists
IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'lau') THEN
    CREATE USER lau WITH PASSWORD 'lau2962003';
END IF;

-- Create database if not exists
IF NOT EXISTS (SELECT FROM pg_database WHERE datname = 'estate_elite') THEN
    CREATE DATABASE estate_elite WITH OWNER lau;
END IF;
END
$$;

-- Connect to the estate_elite database
\c estate_elite
-- Additional initialization scripts can go here