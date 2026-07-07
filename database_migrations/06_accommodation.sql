CREATE TABLE accommodation_providers (
    accommodation_provider_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
    business_name VARCHAR(180) NOT NULL,
    verification_status verification_status NOT NULL DEFAULT 'pending',
    contact_number VARCHAR(20),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE room_types (
    room_type_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    type_name VARCHAR(100) NOT NULL UNIQUE,
    description TEXT
);

CREATE TABLE properties (
    property_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    accommodation_provider_id UUID NOT NULL REFERENCES accommodation_providers(accommodation_provider_id) ON DELETE CASCADE,
    property_name VARCHAR(180) NOT NULL,
    property_type VARCHAR(80) NOT NULL,
    description TEXT,
    location_id UUID REFERENCES locations(location_id) ON DELETE SET NULL,
    latitude NUMERIC(10, 7),
    longitude NUMERIC(10, 7),
    geo_point GEOGRAPHY(Point, 4326),
    address_text TEXT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    average_rating NUMERIC(3, 2) NOT NULL DEFAULT 0,
    rating_count INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE rooms (
    room_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    property_id UUID NOT NULL REFERENCES properties(property_id) ON DELETE CASCADE,
    room_type_id UUID REFERENCES room_types(room_type_id) ON DELETE SET NULL,
    room_number VARCHAR(80),
    floor_number VARCHAR(40),
    capacity INTEGER NOT NULL DEFAULT 1,
    occupied_count INTEGER NOT NULL DEFAULT 0,
    monthly_price NUMERIC(12, 2) NOT NULL,
    security_deposit NUMERIC(12, 2),
    is_available BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT rooms_capacity_chk CHECK (capacity > 0 AND occupied_count >= 0 AND occupied_count <= capacity)
);

CREATE TABLE facilities (
    facility_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    facility_name VARCHAR(120) NOT NULL UNIQUE,
    facility_category VARCHAR(80)
);

CREATE TABLE property_facilities (
    property_id UUID NOT NULL REFERENCES properties(property_id) ON DELETE CASCADE,
    facility_id UUID NOT NULL REFERENCES facilities(facility_id) ON DELETE RESTRICT,
    details TEXT,
    PRIMARY KEY (property_id, facility_id)
);

CREATE TABLE room_availability (
    room_availability_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    room_id UUID NOT NULL REFERENCES rooms(room_id) ON DELETE CASCADE,
    available_from DATE NOT NULL,
    available_to DATE,
    available_beds INTEGER NOT NULL,
    price_override NUMERIC(12, 2),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE accommodation_bookings (
    booking_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    room_id UUID NOT NULL REFERENCES rooms(room_id) ON DELETE RESTRICT,
    worker_profile_id UUID NOT NULL REFERENCES worker_profiles(worker_profile_id) ON DELETE RESTRICT,
    status booking_status NOT NULL DEFAULT 'requested',
    check_in_date DATE NOT NULL,
    check_out_date DATE,
    total_amount NUMERIC(12, 2),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE property_images (
    property_image_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    property_id UUID NOT NULL REFERENCES properties(property_id) ON DELETE CASCADE,
    image_url TEXT NOT NULL,
    sort_order INTEGER NOT NULL DEFAULT 0,
    is_primary BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);
