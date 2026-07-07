CREATE TABLE food_providers (
    food_provider_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
    business_name VARCHAR(180) NOT NULL,
    provider_type VARCHAR(80) NOT NULL,
    verification_status verification_status NOT NULL DEFAULT 'pending',
    location_id UUID REFERENCES locations(location_id) ON DELETE SET NULL,
    average_rating NUMERIC(3, 2) NOT NULL DEFAULT 0,
    rating_count INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE food_items (
    food_item_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    food_provider_id UUID NOT NULL REFERENCES food_providers(food_provider_id) ON DELETE CASCADE,
    item_name VARCHAR(160) NOT NULL,
    description TEXT,
    food_type VARCHAR(60),
    price NUMERIC(12, 2) NOT NULL,
    is_available BOOLEAN NOT NULL DEFAULT TRUE,
    metadata JSONB NOT NULL DEFAULT '{}'::jsonb,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE food_plans (
    food_plan_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    food_provider_id UUID NOT NULL REFERENCES food_providers(food_provider_id) ON DELETE CASCADE,
    plan_name VARCHAR(160) NOT NULL,
    description TEXT,
    duration_days INTEGER NOT NULL,
    price NUMERIC(12, 2) NOT NULL,
    meals_per_day INTEGER NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE food_plan_items (
    food_plan_id UUID NOT NULL REFERENCES food_plans(food_plan_id) ON DELETE CASCADE,
    food_item_id UUID NOT NULL REFERENCES food_items(food_item_id) ON DELETE RESTRICT,
    meal_slot VARCHAR(40) NOT NULL,
    PRIMARY KEY (food_plan_id, food_item_id, meal_slot)
);

CREATE TABLE food_subscriptions (
    food_subscription_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    food_plan_id UUID NOT NULL REFERENCES food_plans(food_plan_id) ON DELETE RESTRICT,
    user_id UUID NOT NULL REFERENCES users(user_id) ON DELETE RESTRICT,
    status subscription_status NOT NULL DEFAULT 'active',
    start_date DATE NOT NULL,
    end_date DATE,
    delivery_location_id UUID REFERENCES locations(location_id) ON DELETE SET NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE delivery_areas (
    delivery_area_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    food_provider_id UUID NOT NULL REFERENCES food_providers(food_provider_id) ON DELETE CASCADE,
    area_id UUID REFERENCES areas(area_id) ON DELETE CASCADE,
    radius_km NUMERIC(6, 2),
    delivery_fee NUMERIC(12, 2) NOT NULL DEFAULT 0,
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);
