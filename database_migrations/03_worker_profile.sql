CREATE TABLE worker_profiles (
    worker_profile_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL UNIQUE REFERENCES users(user_id) ON DELETE CASCADE,
    headline VARCHAR(180),
    expected_salary_min NUMERIC(12, 2),
    expected_salary_max NUMERIC(12, 2),
    expected_salary_model salary_model,
    total_experience_months INTEGER NOT NULL DEFAULT 0,
    current_location_id UUID REFERENCES locations(location_id) ON DELETE SET NULL,
    resume_url TEXT,
    profile_strength_score NUMERIC(5, 2),
    average_rating NUMERIC(3, 2) NOT NULL DEFAULT 0,
    rating_count INTEGER NOT NULL DEFAULT 0,
    matching_metadata JSONB NOT NULL DEFAULT '{}'::jsonb,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT worker_salary_range_chk CHECK (expected_salary_min IS NULL OR expected_salary_max IS NULL OR expected_salary_min <= expected_salary_max)
);

CREATE TABLE skills (
    skill_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    skill_name VARCHAR(120) NOT NULL UNIQUE,
    skill_category VARCHAR(100),
    normalized_name VARCHAR(120) NOT NULL UNIQUE,
    is_verified BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE worker_skills (
    worker_profile_id UUID NOT NULL REFERENCES worker_profiles(worker_profile_id) ON DELETE CASCADE,
    skill_id UUID NOT NULL REFERENCES skills(skill_id) ON DELETE RESTRICT,
    proficiency_level SMALLINT NOT NULL DEFAULT 1,
    years_experience NUMERIC(4, 1),
    is_primary BOOLEAN NOT NULL DEFAULT FALSE,
    verified_at TIMESTAMPTZ,
    PRIMARY KEY (worker_profile_id, skill_id),
    CONSTRAINT worker_skill_proficiency_chk CHECK (proficiency_level BETWEEN 1 AND 5)
);

CREATE TABLE worker_availability (
    availability_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    worker_profile_id UUID NOT NULL REFERENCES worker_profiles(worker_profile_id) ON DELETE CASCADE,
    day_of_week SMALLINT NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    effective_from DATE,
    effective_to DATE,
    is_available BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT worker_availability_day_chk CHECK (day_of_week BETWEEN 0 AND 6),
    CONSTRAINT worker_availability_time_chk CHECK (start_time < end_time)
);

CREATE TABLE worker_experience (
    experience_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    worker_profile_id UUID NOT NULL REFERENCES worker_profiles(worker_profile_id) ON DELETE CASCADE,
    company_name VARCHAR(180),
    job_title VARCHAR(160) NOT NULL,
    employment_type employment_type,
    start_date DATE,
    end_date DATE,
    description TEXT,
    location_id UUID REFERENCES locations(location_id) ON DELETE SET NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT worker_experience_dates_chk CHECK (end_date IS NULL OR start_date IS NULL OR start_date <= end_date)
);

CREATE TABLE worker_education (
    education_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    worker_profile_id UUID NOT NULL REFERENCES worker_profiles(worker_profile_id) ON DELETE CASCADE,
    institution_name VARCHAR(180) NOT NULL,
    degree VARCHAR(160),
    field_of_study VARCHAR(160),
    start_year SMALLINT,
    end_year SMALLINT,
    is_current BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE worker_documents (
    document_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    worker_profile_id UUID NOT NULL REFERENCES worker_profiles(worker_profile_id) ON DELETE CASCADE,
    document_type VARCHAR(80) NOT NULL,
    document_url TEXT NOT NULL,
    file_name VARCHAR(240),
    mime_type VARCHAR(120),
    verification_status verification_status NOT NULL DEFAULT 'pending',
    verified_by UUID REFERENCES users(user_id) ON DELETE SET NULL,
    verified_at TIMESTAMPTZ,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);
