COMMENT ON TABLE users IS 'Base identity account for every platform actor. Supports workers, employers, accommodation owners, food providers, admins, and future roles.';
COMMENT ON TABLE roles IS 'System and future custom roles used for multi-role user access.';
COMMENT ON TABLE permissions IS 'Fine-grained RBAC permissions grouped by platform module.';
COMMENT ON TABLE user_roles IS 'Many-to-many assignment between users and roles, with optional expiry.';
COMMENT ON TABLE role_permissions IS 'Many-to-many permission grants for roles.';
COMMENT ON TABLE user_profiles IS 'General user profile fields shared by all role types.';
COMMENT ON TABLE verifications IS 'Email, mobile, document, business, and address verification workflow records.';
COMMENT ON TABLE user_sessions IS 'Refresh-session records for authentication, device tracking, and revocation.';
COMMENT ON TABLE user_preferences IS 'Flexible per-user preferences for jobs, accommodation, food, notifications, and matching.';
COMMENT ON TABLE audit_logs IS 'Immutable-style activity trail for security, compliance, and admin investigations.';

COMMENT ON TABLE countries IS 'Country lookup table for normalized addresses.';
COMMENT ON TABLE states IS 'State or province lookup table under countries.';
COMMENT ON TABLE cities IS 'City lookup table under states.';
COMMENT ON TABLE areas IS 'Area, neighborhood, or postal-zone lookup table under cities.';
COMMENT ON TABLE locations IS 'Reusable address and GPS coordinate table for users, jobs, properties, food providers, and delivery.';

COMMENT ON TABLE worker_profiles IS 'Role extension for job seekers and workers, including salary expectations and AI matching metadata.';
COMMENT ON TABLE skills IS 'Normalized skill taxonomy shared by worker profiles and job requirements.';
COMMENT ON TABLE worker_skills IS 'Worker-to-skill bridge with proficiency, experience, and verification support.';
COMMENT ON TABLE worker_availability IS 'Recurring availability windows used for scheduling and matching.';
COMMENT ON TABLE worker_experience IS 'Worker employment history and practical experience records.';
COMMENT ON TABLE worker_education IS 'Worker education history.';
COMMENT ON TABLE worker_documents IS 'Worker uploaded documents, resumes, IDs, and verification artifacts.';

COMMENT ON TABLE employer_profiles IS 'Role extension for employers and business owners who post jobs.';
COMMENT ON TABLE job_categories IS 'Hierarchical category taxonomy for jobs.';
COMMENT ON TABLE jobs IS 'Canonical job posting table supporting full-time, part-time, temporary, and contract work.';
COMMENT ON TABLE job_skills IS 'Required or optional skills for a job.';
COMMENT ON TABLE job_locations IS 'One or more job work locations, including map coordinates and remote flag.';
COMMENT ON TABLE job_schedules IS 'Job shift, day, and date schedule requirements.';
COMMENT ON TABLE job_applications IS 'Worker application workflow for jobs.';
COMMENT ON TABLE shortlists IS 'Employer shortlist records linked to applications.';
COMMENT ON TABLE hiring_status_history IS 'Application status transition history for hiring workflows.';
COMMENT ON TABLE contracts IS 'Agreement created after a successful job application and hiring decision.';

COMMENT ON TABLE matching_scores IS 'AI or rules-based worker-to-job matching scores and factor breakdowns.';
COMMENT ON TABLE recommendation_history IS 'Recommendation impressions, interactions, and conversion logs.';
COMMENT ON TABLE search_history IS 'User search queries, filters, and result counts for analytics and personalization.';
COMMENT ON TABLE user_behavior_events IS 'Behavioral event stream for AI personalization and product analytics.';
COMMENT ON TABLE entity_embeddings IS 'Vector embeddings or external vector references for users, jobs, properties, food items, and future entities.';

COMMENT ON TABLE accommodation_providers IS 'Role extension for hostel, PG, room, and accommodation owners.';
COMMENT ON TABLE room_types IS 'Lookup table for room classifications such as single, shared, dorm, or PG.';
COMMENT ON TABLE properties IS 'Accommodation listings with location, coordinates, ownership, and rating aggregates.';
COMMENT ON TABLE rooms IS 'Individual rooms or bed groups inside a property with capacity and pricing.';
COMMENT ON TABLE facilities IS 'Reusable accommodation facility taxonomy.';
COMMENT ON TABLE property_facilities IS 'Property-to-facility bridge with optional details.';
COMMENT ON TABLE room_availability IS 'Vacancy and availability windows for rooms.';
COMMENT ON TABLE accommodation_bookings IS 'Worker booking workflow for rooms and accommodation.';
COMMENT ON TABLE property_images IS 'Image gallery records for accommodation properties.';

COMMENT ON TABLE food_providers IS 'Role extension for restaurants, mess services, tiffin services, and meal providers.';
COMMENT ON TABLE food_items IS 'Individual food menu items sold by a provider.';
COMMENT ON TABLE food_plans IS 'Monthly or duration-based meal subscription plans.';
COMMENT ON TABLE food_plan_items IS 'Food plan composition by item and meal slot.';
COMMENT ON TABLE food_subscriptions IS 'User subscriptions to meal plans and delivery locations.';
COMMENT ON TABLE delivery_areas IS 'Food provider delivery coverage by area or radius.';

COMMENT ON TABLE conversations IS 'Conversation container for worker-employer, worker-accommodation, food, support, and system chats.';
COMMENT ON TABLE conversation_participants IS 'Users participating in each conversation with read and mute state.';
COMMENT ON TABLE messages IS 'Conversation message records.';
COMMENT ON TABLE message_attachments IS 'Files attached to messages.';
COMMENT ON TABLE notifications IS 'User notification queue and delivery/read tracking.';

COMMENT ON TABLE billing_subscriptions IS 'Platform billing subscriptions for premium features and future paid plans.';
COMMENT ON TABLE invoices IS 'Invoice header records for payable platform charges.';
COMMENT ON TABLE payments IS 'Payment intent or payment record linked to any payable entity.';
COMMENT ON TABLE transactions IS 'Provider transaction attempts and responses for payments and refunds.';

COMMENT ON TABLE reviews IS 'Generic review system for workers, employers, hostels, food providers, and future marketplace entities.';
COMMENT ON TABLE reports IS 'Generated admin and analytics report records.';
COMMENT ON TABLE complaints IS 'Trust and safety complaints against users, jobs, properties, food services, or other entities.';
COMMENT ON TABLE analytics_events IS 'Product analytics events for dashboards, funnels, and operational reporting.';
