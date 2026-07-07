CREATE TABLE employer_profiles (
    employer_profile_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
    company_name VARCHAR(180) NOT NULL,
    business_type VARCHAR(120),
    registration_number VARCHAR(120),
    verification_status verification_status NOT NULL DEFAULT 'pending',
    location_id UUID REFERENCES locations(location_id) ON DELETE SET NULL,
    average_rating NUMERIC(3, 2) NOT NULL DEFAULT 0,
    rating_count INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE job_categories (
    job_category_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    parent_category_id UUID REFERENCES job_categories(job_category_id) ON DELETE SET NULL,
    category_name VARCHAR(140) NOT NULL,
    category_slug VARCHAR(160) NOT NULL UNIQUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE jobs (
    job_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    employer_profile_id UUID NOT NULL REFERENCES employer_profiles(employer_profile_id) ON DELETE CASCADE,
    job_category_id UUID REFERENCES job_categories(job_category_id) ON DELETE SET NULL,
    title VARCHAR(180) NOT NULL,
    description TEXT NOT NULL,
    employment_type employment_type NOT NULL,
    salary_model salary_model NOT NULL,
    salary_min NUMERIC(12, 2),
    salary_max NUMERIC(12, 2),
    openings_count INTEGER NOT NULL DEFAULT 1,
    min_experience_months INTEGER NOT NULL DEFAULT 0,
    status job_status NOT NULL DEFAULT 'draft',
    application_deadline TIMESTAMPTZ,
    published_at TIMESTAMPTZ,
    metadata JSONB NOT NULL DEFAULT '{}'::jsonb,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT jobs_salary_range_chk CHECK (salary_min IS NULL OR salary_max IS NULL OR salary_min <= salary_max),
    CONSTRAINT jobs_openings_chk CHECK (openings_count > 0)
);

CREATE TABLE job_skills (
    job_id UUID NOT NULL REFERENCES jobs(job_id) ON DELETE CASCADE,
    skill_id UUID NOT NULL REFERENCES skills(skill_id) ON DELETE RESTRICT,
    required_level SMALLINT NOT NULL DEFAULT 1,
    is_mandatory BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (job_id, skill_id),
    CONSTRAINT job_skill_level_chk CHECK (required_level BETWEEN 1 AND 5)
);

CREATE TABLE job_locations (
    job_location_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    job_id UUID NOT NULL REFERENCES jobs(job_id) ON DELETE CASCADE,
    location_id UUID REFERENCES locations(location_id) ON DELETE SET NULL,
    latitude NUMERIC(10, 7),
    longitude NUMERIC(10, 7),
    geo_point GEOGRAPHY(Point, 4326),
    is_remote_allowed BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE job_schedules (
    job_schedule_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    job_id UUID NOT NULL REFERENCES jobs(job_id) ON DELETE CASCADE,
    day_of_week SMALLINT,
    start_time TIME,
    end_time TIME,
    start_date DATE,
    end_date DATE,
    shift_label VARCHAR(80),
    required_workers INTEGER NOT NULL DEFAULT 1,
    CONSTRAINT job_schedule_day_chk CHECK (day_of_week IS NULL OR day_of_week BETWEEN 0 AND 6),
    CONSTRAINT job_schedule_time_chk CHECK (start_time IS NULL OR end_time IS NULL OR start_time < end_time)
);

CREATE TABLE job_applications (
    application_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    job_id UUID NOT NULL REFERENCES jobs(job_id) ON DELETE CASCADE,
    worker_profile_id UUID NOT NULL REFERENCES worker_profiles(worker_profile_id) ON DELETE CASCADE,
    status application_status NOT NULL DEFAULT 'submitted',
    cover_note TEXT,
    expected_salary NUMERIC(12, 2),
    applied_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    UNIQUE (job_id, worker_profile_id)
);

CREATE TABLE shortlists (
    shortlist_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    application_id UUID NOT NULL UNIQUE REFERENCES job_applications(application_id) ON DELETE CASCADE,
    shortlisted_by UUID REFERENCES users(user_id) ON DELETE SET NULL,
    notes TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE hiring_status_history (
    hiring_status_history_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    application_id UUID NOT NULL REFERENCES job_applications(application_id) ON DELETE CASCADE,
    old_status application_status,
    new_status application_status NOT NULL,
    changed_by UUID REFERENCES users(user_id) ON DELETE SET NULL,
    reason TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE contracts (
    contract_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    job_id UUID NOT NULL REFERENCES jobs(job_id) ON DELETE RESTRICT,
    application_id UUID UNIQUE REFERENCES job_applications(application_id) ON DELETE SET NULL,
    worker_profile_id UUID NOT NULL REFERENCES worker_profiles(worker_profile_id) ON DELETE RESTRICT,
    employer_profile_id UUID NOT NULL REFERENCES employer_profiles(employer_profile_id) ON DELETE RESTRICT,
    status contract_status NOT NULL DEFAULT 'draft',
    start_date DATE NOT NULL,
    end_date DATE,
    agreed_salary NUMERIC(12, 2),
    salary_model salary_model,
    terms_url TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now()
);
