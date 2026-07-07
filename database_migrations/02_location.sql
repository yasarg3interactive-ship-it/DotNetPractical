CREATE TABLE countries (
    country_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    iso2 CHAR(2) NOT NULL UNIQUE,
    iso3 CHAR(3) NOT NULL UNIQUE,
    country_name VARCHAR(120) NOT NULL,
    phone_code VARCHAR(10),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE states (
    state_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    country_id UUID NOT NULL REFERENCES countries(country_id) ON DELETE RESTRICT,
    state_name VARCHAR(140) NOT NULL,
    state_code VARCHAR(30),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    UNIQUE (country_id, state_name)
);

CREATE TABLE cities (
    city_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    state_id UUID NOT NULL REFERENCES states(state_id) ON DELETE RESTRICT,
    city_name VARCHAR(140) NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    UNIQUE (state_id, city_name)
);

CREATE TABLE areas (
    area_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    city_id UUID NOT NULL REFERENCES cities(city_id) ON DELETE RESTRICT,
    area_name VARCHAR(160) NOT NULL,
    postal_code VARCHAR(20),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    UNIQUE (city_id, area_name, postal_code)
);

CREATE TABLE locations (
    location_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    country_id UUID REFERENCES countries(country_id) ON DELETE RESTRICT,
    state_id UUID REFERENCES states(state_id) ON DELETE RESTRICT,
    city_id UUID REFERENCES cities(city_id) ON DELETE RESTRICT,
    area_id UUID REFERENCES areas(area_id) ON DELETE RESTRICT,
    address_line1 TEXT,
    address_line2 TEXT,
    landmark TEXT,
    latitude NUMERIC(10, 7),
    longitude NUMERIC(10, 7),
    geo_point GEOGRAPHY(Point, 4326),
    google_place_id VARCHAR(160),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT locations_lat_lng_chk CHECK (
        (latitude IS NULL AND longitude IS NULL)
        OR (latitude BETWEEN -90 AND 90 AND longitude BETWEEN -180 AND 180)
    )
);

ALTER TABLE user_profiles
ADD CONSTRAINT user_profiles_default_location_fk
FOREIGN KEY (default_location_id) REFERENCES locations(location_id) ON DELETE SET NULL;
