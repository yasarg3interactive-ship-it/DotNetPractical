CREATE TABLE reviews (
    review_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    reviewer_user_id UUID NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
    target_entity_type VARCHAR(80) NOT NULL,
    target_entity_id UUID NOT NULL,
    related_entity_type VARCHAR(80),
    related_entity_id UUID,
    rating SMALLINT NOT NULL,
    review_text TEXT,
    status review_status NOT NULL DEFAULT 'published',
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT reviews_rating_chk CHECK (rating BETWEEN 1 AND 5),
    UNIQUE (reviewer_user_id, target_entity_type, target_entity_id, related_entity_type, related_entity_id)
);

CREATE TABLE reports (
    report_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    report_type VARCHAR(100) NOT NULL,
    generated_by UUID REFERENCES users(user_id) ON DELETE SET NULL,
    parameters JSONB NOT NULL DEFAULT '{}'::jsonb,
    report_url TEXT,
    status VARCHAR(40) NOT NULL DEFAULT 'queued',
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    completed_at TIMESTAMPTZ
);

CREATE TABLE complaints (
    complaint_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    complainant_user_id UUID NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
    target_entity_type VARCHAR(80) NOT NULL,
    target_entity_id UUID NOT NULL,
    complaint_category VARCHAR(100) NOT NULL,
    description TEXT NOT NULL,
    status complaint_status NOT NULL DEFAULT 'open',
    assigned_to UUID REFERENCES users(user_id) ON DELETE SET NULL,
    resolution_notes TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    resolved_at TIMESTAMPTZ
);

CREATE TABLE analytics_events (
    analytics_event_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES users(user_id) ON DELETE SET NULL,
    anonymous_id VARCHAR(120),
    event_name VARCHAR(120) NOT NULL,
    source VARCHAR(80),
    session_id UUID,
    entity_type VARCHAR(80),
    entity_id UUID,
    properties JSONB NOT NULL DEFAULT '{}'::jsonb,
    occurred_at TIMESTAMPTZ NOT NULL DEFAULT now()
);
