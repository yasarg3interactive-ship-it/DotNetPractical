CREATE INDEX idx_users_status ON users(status);
CREATE INDEX idx_users_last_active ON users(last_active_at DESC);
CREATE INDEX idx_user_roles_role ON user_roles(role_id);
CREATE INDEX idx_verifications_user_status ON verifications(user_id, status);
CREATE INDEX idx_sessions_user_status ON user_sessions(user_id, status);
CREATE INDEX idx_preferences_scope ON user_preferences(preference_scope);
CREATE INDEX idx_audit_logs_entity ON audit_logs(entity_type, entity_id, created_at DESC);
CREATE INDEX idx_audit_logs_actor ON audit_logs(actor_user_id, created_at DESC);

CREATE INDEX idx_states_country ON states(country_id);
CREATE INDEX idx_cities_state ON cities(state_id);
CREATE INDEX idx_areas_city ON areas(city_id);
CREATE INDEX idx_locations_geo ON locations USING GIST (geo_point);
CREATE INDEX idx_locations_place ON locations(google_place_id);

CREATE INDEX idx_worker_profiles_user ON worker_profiles(user_id);
CREATE INDEX idx_worker_profiles_location ON worker_profiles(current_location_id);
CREATE INDEX idx_worker_profiles_rating ON worker_profiles(average_rating DESC);
CREATE INDEX idx_worker_skills_skill ON worker_skills(skill_id, proficiency_level DESC);
CREATE INDEX idx_worker_availability_worker_day ON worker_availability(worker_profile_id, day_of_week, start_time, end_time);
CREATE INDEX idx_worker_documents_status ON worker_documents(verification_status);

CREATE INDEX idx_employer_profiles_user ON employer_profiles(user_id);
CREATE INDEX idx_jobs_employer_status ON jobs(employer_profile_id, status);
CREATE INDEX idx_jobs_category_status ON jobs(job_category_id, status);
CREATE INDEX idx_jobs_open_published ON jobs(published_at DESC) WHERE status = 'open';
CREATE INDEX idx_job_skills_skill ON job_skills(skill_id);
CREATE INDEX idx_job_locations_job ON job_locations(job_id);
CREATE INDEX idx_job_locations_geo ON job_locations USING GIST (geo_point);
CREATE INDEX idx_job_schedules_job_day ON job_schedules(job_id, day_of_week);
CREATE INDEX idx_applications_job_status ON job_applications(job_id, status);
CREATE INDEX idx_applications_worker_status ON job_applications(worker_profile_id, status);
CREATE INDEX idx_contracts_worker_status ON contracts(worker_profile_id, status);
CREATE INDEX idx_contracts_employer_status ON contracts(employer_profile_id, status);

CREATE INDEX idx_matching_scores_job_score ON matching_scores(job_id, overall_score DESC);
CREATE INDEX idx_matching_scores_worker_score ON matching_scores(worker_profile_id, overall_score DESC);
CREATE INDEX idx_recommendations_user_type ON recommendation_history(user_id, recommendation_type, shown_at DESC);
CREATE INDEX idx_search_history_user_scope ON search_history(user_id, search_scope, created_at DESC);
CREATE INDEX idx_behavior_user_event ON user_behavior_events(user_id, event_name, occurred_at DESC);
CREATE INDEX idx_embeddings_entity ON entity_embeddings(entity_type, entity_id);
CREATE INDEX idx_embeddings_vector ON entity_embeddings USING ivfflat (embedding vector_cosine_ops) WITH (lists = 100);

CREATE INDEX idx_accommodation_providers_user ON accommodation_providers(user_id);
CREATE INDEX idx_properties_provider_active ON properties(accommodation_provider_id, is_active);
CREATE INDEX idx_properties_geo ON properties USING GIST (geo_point);
CREATE INDEX idx_rooms_property_available ON rooms(property_id, is_available);
CREATE INDEX idx_room_availability_room_dates ON room_availability(room_id, available_from, available_to);
CREATE INDEX idx_bookings_room_status_dates ON accommodation_bookings(room_id, status, check_in_date, check_out_date);
CREATE INDEX idx_bookings_worker_status ON accommodation_bookings(worker_profile_id, status);

CREATE INDEX idx_food_providers_user ON food_providers(user_id);
CREATE INDEX idx_food_providers_location ON food_providers(location_id);
CREATE INDEX idx_food_items_provider_available ON food_items(food_provider_id, is_available);
CREATE INDEX idx_food_plans_provider_active ON food_plans(food_provider_id, is_active);
CREATE INDEX idx_food_subscriptions_user_status ON food_subscriptions(user_id, status);
CREATE INDEX idx_delivery_areas_provider ON delivery_areas(food_provider_id);

CREATE INDEX idx_conversation_related ON conversations(related_entity_type, related_entity_id);
CREATE INDEX idx_conversation_participants_user ON conversation_participants(user_id);
CREATE INDEX idx_messages_conversation_sent ON messages(conversation_id, sent_at DESC);
CREATE INDEX idx_notifications_user_status ON notifications(user_id, status, created_at DESC);
CREATE INDEX idx_notifications_unread ON notifications(user_id, created_at DESC) WHERE read_at IS NULL;

CREATE INDEX idx_billing_subscriptions_user_status ON billing_subscriptions(user_id, status);
CREATE INDEX idx_invoices_user_status ON invoices(user_id, status);
CREATE INDEX idx_payments_user_status ON payments(user_id, status);
CREATE INDEX idx_payments_entity ON payments(payable_entity_type, payable_entity_id);
CREATE INDEX idx_transactions_payment ON transactions(payment_id, created_at DESC);

CREATE INDEX idx_reviews_target ON reviews(target_entity_type, target_entity_id, status);
CREATE INDEX idx_reviews_reviewer ON reviews(reviewer_user_id, created_at DESC);
CREATE INDEX idx_reports_type_status ON reports(report_type, status);
CREATE INDEX idx_complaints_target_status ON complaints(target_entity_type, target_entity_id, status);
CREATE INDEX idx_complaints_assigned_status ON complaints(assigned_to, status);
CREATE INDEX idx_analytics_event_name_time ON analytics_events(event_name, occurred_at DESC);
CREATE INDEX idx_analytics_user_time ON analytics_events(user_id, occurred_at DESC);

CREATE INDEX idx_preferences_json ON user_preferences USING GIN (preferences);
CREATE INDEX idx_jobs_metadata_json ON jobs USING GIN (metadata);
CREATE INDEX idx_behavior_properties_json ON user_behavior_events USING GIN (event_properties);
CREATE INDEX idx_analytics_properties_json ON analytics_events USING GIN (properties);
