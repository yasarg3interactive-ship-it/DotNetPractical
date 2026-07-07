CREATE TABLE matching_scores (
    matching_score_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    worker_profile_id UUID NOT NULL REFERENCES worker_profiles(worker_profile_id) ON DELETE CASCADE,
    job_id UUID NOT NULL REFERENCES jobs(job_id) ON DELETE CASCADE,
    model_version VARCHAR(80) NOT NULL,
    overall_score NUMERIC(6, 3) NOT NULL,
    skill_score NUMERIC(6, 3),
    distance_score NUMERIC(6, 3),
    availability_score NUMERIC(6, 3),
    experience_score NUMERIC(6, 3),
    salary_score NUMERIC(6, 3),
    rating_score NUMERIC(6, 3),
    explanation JSONB NOT NULL DEFAULT '{}'::jsonb,
    calculated_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    UNIQUE (worker_profile_id, job_id, model_version)
);

CREATE TABLE recommendation_history (
    recommendation_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
    recommendation_type VARCHAR(80) NOT NULL,
    target_entity_type VARCHAR(80) NOT NULL,
    target_entity_id UUID NOT NULL,
    score NUMERIC(6, 3),
    model_version VARCHAR(80),
    reason JSONB NOT NULL DEFAULT '{}'::jsonb,
    shown_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    clicked_at TIMESTAMPTZ,
    dismissed_at TIMESTAMPTZ,
    converted_at TIMESTAMPTZ
);

CREATE TABLE search_history (
    search_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES users(user_id) ON DELETE SET NULL,
    search_scope VARCHAR(80) NOT NULL,
    query_text TEXT,
    filters JSONB NOT NULL DEFAULT '{}'::jsonb,
    result_count INTEGER,
    location_id UUID REFERENCES locations(location_id) ON DELETE SET NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE user_behavior_events (
    behavior_event_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES users(user_id) ON DELETE SET NULL,
    event_name VARCHAR(120) NOT NULL,
    entity_type VARCHAR(80),
    entity_id UUID,
    event_properties JSONB NOT NULL DEFAULT '{}'::jsonb,
    occurred_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE entity_embeddings (
    embedding_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    entity_type VARCHAR(80) NOT NULL,
    entity_id UUID NOT NULL,
    embedding_model VARCHAR(120) NOT NULL,
    embedding VECTOR(1536),
    embedding_ref TEXT,
    metadata JSONB NOT NULL DEFAULT '{}'::jsonb,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    UNIQUE (entity_type, entity_id, embedding_model)
);
