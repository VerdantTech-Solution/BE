-- SEEDER DATA FOR VERDANTTECH DATABASE
-- All passwords are: $2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS
-- Adjusted to match schema v6: Removed green_certifications from products, added inserts for product_sustainability_credentials, fixed column mismatches, added missing inserts for new tables
-- Dates adjusted to be recent as of 2025-09-09, ensured foreign key consistency

-- Insert Users (with gmail.com emails and consistent password)
INSERT INTO `users` (`id`, `email`, `password_hash`, `role`, `full_name`, `phone_number`, `is_verified`, `verification_token`, `verification_sent_at`, `avatar_url`, `status`, `last_login_at`, `created_at`, `updated_at`, `deleted_at`) VALUES
(1, 'admin@gmail.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'admin', 'Quản trị viên hệ thống', '0901234567', 1, NULL, NULL, NULL, 'active', '2025-09-09 08:00:00', '2025-09-08 07:00:00', '2025-09-09 08:00:00', NULL),
(2, 'staff1@gmail.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'staff', 'Nguyễn Văn Nhân Viên 1', '0901234568', 1, NULL, NULL, NULL, 'active', '2025-09-09 07:30:00', '2025-09-08 07:00:00', '2025-09-09 07:30:00', NULL),
(3, 'staff2@gmail.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'staff', 'Trần Thị Nhân Viên 2', '0901234569', 1, NULL, NULL, NULL, 'active', '2025-09-09 07:00:00', '2025-09-08 08:00:00', '2025-09-09 07:00:00', NULL),
(4, 'staff3@gmail.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'staff', 'Lê Văn Nhân Viên 3', '0901234570', 1, NULL, NULL, NULL, 'active', '2025-09-09 06:30:00', '2025-09-08 08:30:00', '2025-09-09 06:30:00', NULL),
(5, 'customer1@gmail.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'customer', 'Phạm Văn Khách Hàng 1', '0901234571', 1, NULL, NULL, NULL, 'active', '2025-09-09 08:15:00', '2025-09-08 09:00:00', '2025-09-09 08:15:00', NULL),
(6, 'customer2@gmail.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'customer', 'Hoàng Thị Khách Hàng 2', '0901234572', 1, NULL, NULL, NULL, 'active', '2025-09-09 08:10:00', '2025-09-08 09:30:00', '2025-09-09 08:10:00', NULL),
(7, 'farmer1@gmail.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'customer', 'Nguyễn Văn Nông Dân 1', '0901234573', 1, NULL, NULL, NULL, 'active', '2025-09-09 06:00:00', '2025-09-08 10:00:00', '2025-09-09 06:00:00', NULL),
(8, 'farmer2@gmail.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'customer', 'Trần Thị Nông Dân 2', '0901234574', 1, NULL, NULL, NULL, 'active', '2025-09-09 05:30:00', '2025-09-08 10:30:00', '2025-09-09 05:30:00', NULL),
(9, 'testuser@gmail.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'customer', 'Người Dùng Test', '0901234575', 0, 'test-token-123', '2025-09-09 07:00:00', NULL, 'active', NULL, '2025-09-09 07:00:00', '2025-09-09 07:00:00', NULL),
(10, 'inactive@gmail.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'customer', 'Người Dùng Không Hoạt Động', '0901234576', 1, NULL, NULL, NULL, 'inactive', '2025-09-08 15:00:00', '2025-09-08 11:00:00', '2025-09-08 15:00:00', NULL);

-- Insert Farm Profiles
INSERT INTO `farm_profiles` (`id`, `user_id`, `farm_name`, `farm_size_hectares`, `location_address`, `province`, `district`, `commune`, `latitude`, `longitude`, `primary_crops`, `created_at`, `updated_at`) VALUES
(1, 7, 'Trang trại Xanh Sạch Đồng Nai', 5.50, 'Số 123 Đường Nông Nghiệp, Tân Phong', 'Đồng Nai', 'Biên Hòa', 'Tân Phong', 10.9545, 106.8441, 'Lúa, Rau xanh, Cà chua', '2025-09-08 10:00:00', '2025-09-09 06:00:00'),
(2, 8, 'Trang trại Hữu Cơ Long An', 8.25, 'Số 456 Đường Nông Thôn, Đức Hòa Thượng', 'Long An', 'Đức Hòa', 'Đức Hòa Thượng', 10.8838, 106.4226, 'Rau củ, Trái cây, Thảo dược', '2025-09-08 10:30:00', '2025-09-09 05:30:00');

-- Insert Sustainability Certifications
INSERT INTO `sustainability_certifications` (`id`, `code`, `name`, `category`, `issuing_body`, `description`, `is_active`, `created_at`, `updated_at`) VALUES
-- 🌱 Các chứng chỉ nông nghiệp bền vững
(1, 'GLOBALGAP', 'GlobalG.A.P. – Good Agricultural Practices', 'organic', 'GlobalG.A.P. c/o FoodPLUS GmbH', 'Thực hành nông nghiệp tốt toàn cầu', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(2, 'VIETGAP', 'VietGAP – Thực hành nông nghiệp tốt tại Việt Nam', 'organic', 'Bộ Nông nghiệp và Phát triển nông thôn Việt Nam', 'Thực hành nông nghiệp tốt tại Việt Nam', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(3, 'ASEANGAP', 'ASEAN GAP – ASEAN Good Agricultural Practice', 'organic', 'ASEAN Secretariat', 'Chuẩn GAP trong khu vực ASEAN', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(4, 'USDA_ORGANIC', 'USDA Organic Certification', 'organic', 'United States Department of Agriculture', 'Tiêu chuẩn hữu cơ Hoa Kỳ', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(5, 'EU_ORGANIC', 'EU Organic Farming Certification', 'organic', 'European Commission', 'Tiêu chuẩn hữu cơ châu Âu', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(6, 'IFOAM_ORGANIC', 'IFOAM Organic International', 'organic', 'IFOAM - Organics International', 'Liên đoàn quốc tế các phong trào nông nghiệp hữu cơ', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(7, 'JAS_ORGANIC', 'JAS Organic (Japan Agricultural Standard)', 'organic', 'Ministry of Agriculture, Forestry and Fisheries of Japan', 'Tiêu chuẩn hữu cơ Nhật Bản', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(8, 'CANADA_ORGANIC', 'Canada Organic Certification', 'organic', 'Canadian Food Inspection Agency', 'Tiêu chuẩn hữu cơ Canada', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(9, 'RAINFOREST_ALLIANCE', 'Rainforest Alliance Certified', 'environmental', 'Rainforest Alliance', 'Nông nghiệp bền vững gắn với bảo vệ rừng', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(10, 'UTZ_CERTIFIED', 'UTZ Certified (merged with Rainforest Alliance)', 'environmental', 'UTZ Certified (now Rainforest Alliance)', 'Thực hành nông nghiệp bền vững', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(11, 'DEMETER_BIODYNAMIC', 'Demeter Biodynamic Certification', 'organic', 'Demeter International', 'Nông nghiệp sinh học – động lực', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),

-- 🌿 Các chứng chỉ môi trường & carbon
(12, 'ISO_14001', 'ISO 14001 Environmental Management', 'environmental', 'International Organization for Standardization', 'Quản lý môi trường', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(13, 'ISO_50001', 'ISO 50001 Energy Management', 'energy', 'International Organization for Standardization', 'Quản lý năng lượng', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(14, 'CARBON_NEUTRAL', 'Carbon Neutral Certification', 'environmental', 'Various organizations (Carbon Trust, Climate Active, etc.)', 'Chứng nhận trung tính carbon', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(15, 'SBTI', 'Science Based Targets initiative (SBTi)', 'environmental', 'Science Based Targets initiative', 'Cam kết giảm phát thải theo khoa học', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(16, 'FAIR_CARBON', 'Fair Carbon Standard', 'environmental', 'Fair Carbon Foundation', 'Tiêu chuẩn tín chỉ carbon', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),

-- 🤝 Các chứng chỉ thương mại công bằng & xã hội
(17, 'FAIRTRADE', 'Fairtrade International Certification', 'fair_trade', 'Fairtrade International', 'Thương mại công bằng', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(18, 'SA8000', 'SA8000 Social Accountability', 'social', 'Social Accountability International', 'Trách nhiệm xã hội', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(19, 'SEDEX_SMETA', 'Sedex / SMETA Audit', 'social', 'Sedex', 'Đạo đức trong chuỗi cung ứng', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(20, 'BCORP', 'B Corp Certification', 'social', 'B Lab', 'Doanh nghiệp vì cộng đồng và môi trường', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),

-- 🍃 Các chứng chỉ ngành thực phẩm & an toàn
(21, 'HACCP', 'HACCP - Hazard Analysis Critical Control Points', 'food_safety', 'Various certification bodies', 'Phân tích mối nguy và kiểm soát điểm tới hạn', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(22, 'ISO_22000', 'ISO 22000 / FSSC 22000 Food Safety Management', 'food_safety', 'International Organization for Standardization', 'Quản lý an toàn thực phẩm', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(23, 'HALAL', 'Halal Certification', 'food_safety', 'Various Halal certification bodies', 'Chứng nhận Halal', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(24, 'KOSHER', 'Kosher Certification', 'food_safety', 'Various Kosher certification agencies', 'Chứng nhận Kosher', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(25, 'NON_GMO', 'Non-GMO Project Verified', 'food_safety', 'Non-GMO Project', 'Không biến đổi gen', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00');

-- Insert Vendor Profiles
INSERT INTO `vendor_profiles` (`id`, `user_id`, `company_name`, `slug`, `business_registration_number`, `tax_code`, `company_address`, `verified_at`, `verified_by`, `commission_rate`, `rating_average`, `total_reviews`, `created_at`, `updated_at`) VALUES
(1, 3, 'Công Ty Thiết Bị Nông Nghiệp Xanh', 'cong-ty-thiet-bi-nong-nghiep-xanh', 'BRN123456789', 'TC001234567', 'Số 789 Đường Công Nghiệp, Quận 7, TP.HCM', '2025-09-09 07:00:00', 1, 10.00, 4.5, 25, '2025-09-08 08:00:00', '2025-09-09 07:00:00'),
(2, 4, 'Cửa Hàng Nông Sản Sạch VerdantTech', 'cua-hang-nong-san-sach-verdanttech', 'BRN987654321', 'TC009876543', 'Số 321 Đường Nông Sản, Quận Tân Bình, TP.HCM', '2025-09-09 06:30:00', 1, 8.00, 4.7, 42, '2025-09-08 08:30:00', '2025-09-09 06:30:00');

-- Insert Supported Banks
INSERT INTO `supported_banks` (`id`, `bank_code`, `bank_name`, `image_url`, `is_active`, `created_at`, `updated_at`) VALUES
(1, 'VCB', 'Vietcombank', 'https://example.com/banks/vcb.png', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(2, 'TCB', 'Techcombank', 'https://example.com/banks/tcb.png', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(3, 'ACB', 'Asia Commercial Bank', 'https://example.com/banks/acb.png', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00');

-- Insert Vendor Bank Accounts
INSERT INTO `vendor_bank_accounts` (`id`, `vendor_id`, `bank_id`, `account_number`, `account_holder`, `is_default`, `created_at`, `updated_at`) VALUES
(1, 1, 1, '1234567890', 'Công Ty Thiết Bị Nông Nghiệp Xanh', 1, '2025-09-09 07:05:00', '2025-09-09 07:05:00'),
(2, 2, 3, '0987654321', 'Cửa Hàng Nông Sản Sạch VerdantTech', 1, '2025-09-09 06:35:00', '2025-09-09 06:35:00');

-- Insert Wallets
INSERT INTO `wallets` (`id`, `vendor_id`, `balance`, `pending_withdraw`, `created_at`, `updated_at`) VALUES
(1, 1, 10000000.00, 0.00, '2025-09-09 08:00:00', '2025-09-09 08:00:00'),
(2, 2, 2500000.00, 0.00, '2025-09-09 08:00:00', '2025-09-09 08:00:00');

-- Insert Vendor Sustainability Credentials
INSERT INTO `vendor_sustainability_credentials` (`id`, `vendor_id`, `certification_id`, `certificate_url`, `status`, `rejection_reason`, `uploaded_at`, `verified_at`, `verified_by`, `created_at`, `updated_at`) VALUES
-- Vendor 1 (Công Ty Thiết Bị Nông Nghiệp Xanh) credentials
(1, 1, 12, 'https://example.com/certificates/vendor1_iso14001.pdf', 'verified', NULL, '2025-09-08 09:00:00', '2025-09-09 07:00:00', 1, '2025-09-08 09:00:00', '2025-09-09 07:00:00'),
(2, 1, 13, 'https://example.com/certificates/vendor1_iso50001.pdf', 'verified', NULL, '2025-09-08 09:15:00', '2025-09-09 07:00:00', 1, '2025-09-08 09:15:00', '2025-09-09 07:00:00'),
(3, 1, 14, 'https://example.com/certificates/vendor1_carbon_neutral.pdf', 'verified', NULL, '2025-09-08 09:30:00', '2025-09-09 07:00:00', 1, '2025-09-08 09:30:00', '2025-09-09 07:00:00'),
(4, 1, 21, 'https://example.com/certificates/vendor1_haccp.pdf', 'pending', NULL, '2025-09-09 08:00:00', NULL, NULL, '2025-09-09 08:00:00', '2025-09-09 08:00:00'),

-- Vendor 2 (Cửa Hàng Nông Sản Sạch VerdantTech) credentials  
(5, 2, 4, 'https://example.com/certificates/vendor2_usda_organic.pdf', 'verified', NULL, '2025-09-08 10:00:00', '2025-09-09 06:30:00', 1, '2025-09-08 10:00:00', '2025-09-09 06:30:00'),
(6, 2, 2, 'https://example.com/certificates/vendor2_vietgap.pdf', 'verified', NULL, '2025-09-08 10:15:00', '2025-09-09 06:30:00', 1, '2025-09-08 10:15:00', '2025-09-09 06:30:00'),
(7, 2, 17, 'https://example.com/certificates/vendor2_fairtrade.pdf', 'verified', NULL, '2025-09-08 10:30:00', '2025-09-09 06:30:00', 1, '2025-09-08 10:30:00', '2025-09-09 06:30:00'),
(8, 2, 25, 'https://example.com/certificates/vendor2_non_gmo.pdf', 'rejected', 'Chứng chỉ không rõ ràng, cần upload lại bản gốc', '2025-09-09 09:00:00', '2025-09-09 10:00:00', 1, '2025-09-09 09:00:00', '2025-09-09 10:00:00');

-- Insert Product Categories
INSERT INTO `product_categories` (`id`, `parent_id`, `name`, `name_en`, `slug`, `description`, `icon_url`, `sort_order`, `is_active`, `created_at`, `updated_at`) VALUES
(1, NULL, 'Thiết Bị Nông Nghiệp', 'Agricultural Equipment', 'thiet-bi-nong-nghiep', 'Các loại máy móc và thiết bị phục vụ sản xuất nông nghiệp', NULL, 1, 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(2, 1, 'Máy Cày', 'Tractors', 'may-cay', 'Máy cày và thiết bị làm đất', NULL, 1, 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(3, 1, 'Máy Gặt', 'Harvesters', 'may-gat', 'Máy gặt và thu hoạch', NULL, 2, 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(4, NULL, 'Hạt Giống', 'Seeds', 'hat-giong', 'Hạt giống chất lượng cao', NULL, 2, 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(5, 4, 'Hạt Giống Rau', 'Vegetable Seeds', 'hat-giong-rau', 'Hạt giống rau củ hữu cơ', NULL, 1, 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(6, NULL, 'Phân Bón', 'Fertilizers', 'phan-bon', 'Phân bón hữu cơ và hóa học', NULL, 3, 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00');

-- Insert Products (removed green_certifications column and values)
INSERT INTO `products` (`id`, `category_id`, `product_code`, `name`, `name_en`, `slug`, `description`, `description_en`, `price`, `cost_price`, `commission_rate`, `discount_percentage`, `energy_efficiency_rating`, `specifications`, `manual_urls`, `images`, `warranty_months`, `stock_quantity`, `weight_kg`, `dimensions_cm`, `is_featured`, `is_active`, `view_count`, `sold_count`, `rating_average`, `total_reviews`, `created_at`, `updated_at`) VALUES
(1, 2, 'TC001', 'Máy Cày Mini Điện VerdantTech V1', 'VerdantTech V1 Mini Electric Tractor', 'may-cay-mini-dien-verdanttech-v1', 'Máy cày mini sử dụng năng lượng điện, thân thiện với môi trường, phù hợp cho nông trại nhỏ.', 'Mini electric tractor, eco-friendly, suitable for small farms.', 25000000.00, 20000000.00, 10.00, 10.00, 'A++', '{"power": "10kW", "battery": "48V 100Ah"}', 'manual_tc001.pdf', 'tc001_1.jpg,tc001_2.jpg', 24, 50, 500.000, '{"length": 250, "width": 120, "height": 150}', 1, 1, 120, 5, 4.60, 25, '2025-09-08 07:00:00', '2025-09-09 07:00:00'),
(2, 3, 'HV002', 'Máy Gặt Lúa Tự Động VerdantTech H2', 'VerdantTech H2 Automatic Rice Harvester', 'may-gat-lua-tu-dong-verdanttech-h2', 'Máy gặt lúa tự động với công nghệ AI, tiết kiệm thời gian và công sức.', 'Automatic rice harvester with AI technology, time and labor saving.', 150000000.00, 120000000.00, 8.00, 5.00, 'A+', '{"engine": "Diesel 50HP", "capacity": "2 tons/hour"}', 'manual_hv002.pdf', 'hv002_1.jpg,hv002_2.jpg', 36, 10, 2500.000, '{"length": 450, "width": 200, "height": 250}', 1, 1, 85, 3, 4.80, 15, '2025-09-08 07:15:00', '2025-09-09 07:15:00'),
(3, 5, 'SD003', 'Hạt Giống Rau Cải Xanh Hữu Cơ', 'Organic Green Mustard Seeds', 'hat-giong-rau-cai-xanh-huu-co', 'Hạt giống rau cải xanh hữu cơ, tỷ lệ nảy mầm cao, kháng bệnh tốt.', 'Organic green mustard seeds, high germination rate, good disease resistance.', 50000.00, 30000.00, 5.00, 0.00, NULL, '{"germination_rate": "95%", "pack_size": "100g"}', 'manual_sd003.pdf', 'sd003_1.jpg', 0, 200, 0.100, '{"length": 10, "width": 5, "height": 2}', 0, 1, 200, 50, 4.50, 30, '2025-09-08 07:30:00', '2025-09-09 07:30:00'),
(4, 6, 'FT004', 'Phân Bón Hữu Cơ Compost Premium', 'Premium Organic Compost Fertilizer', 'phan-bon-huu-co-compost-premium', 'Phân bón hữu cơ từ compost, giàu dinh dưỡng, an toàn cho đất và cây trồng.', 'Premium organic compost fertilizer, nutrient-rich, safe for soil and plants.', 100000.00, 70000.00, 7.00, 10.00, NULL, '{"npk": "5-5-5", "weight": "25kg"}', 'manual_ft004.pdf', 'ft004_1.jpg,ft004_2.jpg', 0, 100, 25.000, '{"length": 50, "width": 30, "height": 10}', 1, 1, 150, 20, 4.70, 18, '2025-09-08 07:45:00', '2025-09-09 07:45:00'),
(5, 1, 'DR005', 'Drone Phun Thuốc Thông Minh VerdantTech D3', 'VerdantTech D3 Smart Spraying Drone', 'drone-phun-thuoc-thong-minh-verdanttech-d3', 'Drone phun thuốc tự động với AI, chính xác cao, tiết kiệm thuốc.', 'Smart spraying drone with AI, high precision, pesticide saving.', 30000000.00, 25000000.00, 12.00, 15.00, 'A', '{"flight_time": "30min", "capacity": "10L"}', 'manual_dr005.pdf', 'dr005_1.jpg,dr005_2.jpg', 12, 15, 5.000, '{"length": 100, "width": 100, "height": 50}', 0, 1, 90, 7, 4.40, 12, '2025-09-08 08:00:00', '2025-09-09 08:00:00');

-- Insert Product Sustainability Credentials (new for v6)
INSERT INTO `product_sustainability_credentials` (`id`, `product_id`, `certification_id`, `certificate_url`, `status`, `rejection_reason`, `uploaded_at`, `verified_at`, `verified_by`, `created_at`, `updated_at`) VALUES
-- Product 1 (Máy Cày Mini Điện VerdantTech V1) credentials
(1, 1, 13, 'https://example.com/certificates/product1_iso50001.pdf', 'verified', NULL, '2025-09-08 09:00:00', '2025-09-09 07:00:00', 1, '2025-09-08 09:00:00', '2025-09-09 07:00:00'),
(2, 1, 14, 'https://example.com/certificates/product1_carbon_neutral.pdf', 'verified', NULL, '2025-09-08 09:15:00', '2025-09-09 07:00:00', 1, '2025-09-08 09:15:00', '2025-09-09 07:00:00'),
-- Product 2 (Máy Gặt Lúa Tự Động VerdantTech H2) credentials
(3, 2, 12, 'https://example.com/certificates/product2_iso14001.pdf', 'verified', NULL, '2025-09-08 09:30:00', '2025-09-09 07:00:00', 1, '2025-09-08 09:30:00', '2025-09-09 07:00:00'),
(4, 2, 15, 'https://example.com/certificates/product2_sbti.pdf', 'pending', NULL, '2025-09-09 08:00:00', NULL, NULL, '2025-09-09 08:00:00', '2025-09-09 08:00:00'),
-- Product 3 (Hạt Giống Rau Cải Xanh Hữu Cơ) credentials
(5, 3, 4, 'https://example.com/certificates/product3_usda_organic.pdf', 'verified', NULL, '2025-09-08 10:00:00', '2025-09-09 06:30:00', 1, '2025-09-08 10:00:00', '2025-09-09 06:30:00'),
(6, 3, 2, 'https://example.com/certificates/product3_vietgap.pdf', 'verified', NULL, '2025-09-08 10:15:00', '2025-09-09 06:30:00', 1, '2025-09-08 10:15:00', '2025-09-09 06:30:00'),
-- Product 4 (Phân Bón Hữu Cơ Compost Premium) credentials
(7, 4, 1, 'https://example.com/certificates/product4_globalgap.pdf', 'verified', NULL, '2025-09-08 10:30:00', '2025-09-09 06:30:00', 1, '2025-09-08 10:30:00', '2025-09-09 06:30:00'),
(8, 4, 25, 'https://example.com/certificates/product4_non_gmo.pdf', 'rejected', 'Chứng chỉ không rõ ràng, cần upload lại bản gốc', '2025-09-09 09:00:00', '2025-09-09 10:00:00', 1, '2025-09-09 09:00:00', '2025-09-09 10:00:00'),
-- Product 5 (Drone Phun Thuốc Thông Minh VerdantTech D3) credentials
(9, 5, 9, 'https://example.com/certificates/product5_rainforest_alliance.pdf', 'verified', NULL, '2025-09-08 11:00:00', '2025-09-09 07:00:00', 1, '2025-09-08 11:00:00', '2025-09-09 07:00:00'),
(10, 5, 14, 'https://example.com/certificates/product5_carbon_neutral.pdf', 'pending', NULL, '2025-09-09 08:30:00', NULL, NULL, '2025-09-09 08:30:00', '2025-09-09 08:30:00');

-- Insert Forum Categories
INSERT INTO `forum_categories` (`id`, `name`, `description`, `is_active`, `created_at`, `updated_at`) VALUES
(1, 'Kỹ Thuật Canh Tác', 'Thảo luận về các phương pháp canh tác bền vững và hữu cơ', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(2, 'Thiết Bị Nông Nghiệp', 'Chia sẻ kinh nghiệm sử dụng máy móc và thiết bị nông nghiệp xanh', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00'),
(3, 'Phòng Trừ Sâu Bệnh', 'Các biện pháp phòng trừ sâu bệnh thân thiện với môi trường', 1, '2025-09-08 07:00:00', '2025-09-08 07:00:00');

-- Insert Forum Posts
INSERT INTO `forum_posts` (`id`, `category_id`, `user_id`, `title`, `slug`, `content`, `tags`, `view_count`, `reply_count`, `like_count`, `dislike_count`, `is_pinned`, `is_locked`, `status`, `moderated_reason`, `moderated_by`, `last_activity_at`, `created_at`, `updated_at`) VALUES
(1, 1, 7, 'Kinh nghiệm trồng lúa hữu cơ tại Đồng Nai', 'kinh-nghiem-trong-lua-huu-co-tai-dong-nai', '[{"order": 1, "type": "text", "content": "Chào mọi người, mình đang trồng lúa hữu cơ tại Đồng Nai. Ai có kinh nghiệm chia sẻ nhé!"}, {"order": 2, "type": "image", "content": "https://example.com/images/lua-huu-co.jpg"}]', 'lúa, hữu cơ, đồng nai', 150, 5, 20, 2, 1, 0, 'published', NULL, NULL, '2025-09-09 10:00:00', '2025-09-08 14:00:00', '2025-09-09 10:00:00'),
(2, 2, 5, 'Review máy cày mini điện VerdantTech V1', 'review-may-cay-mini-dien-verdanttech-v1', '[{"order": 1, "type": "text", "content": "Mình mới mua máy cày mini điện V1, chạy rất êm và tiết kiệm. Có ai dùng chưa?"}]', 'máy cày, điện, verdanttech', 80, 3, 15, 1, 0, 0, 'published', NULL, NULL, '2025-09-09 06:00:00', '2025-09-08 15:00:00', '2025-09-09 06:00:00'),
(3, 3, 8, 'Biện pháp phòng sâu bệnh tự nhiên cho rau củ', 'bien-phap-phong-sau-benh-tu-nhien-cho-rau-cu', '[{"order": 1, "type": "text", "content": "Mọi người thường dùng gì để phòng sâu bệnh cho rau mà không dùng thuốc hóa học?"}]', 'sâu bệnh, rau củ, tự nhiên', 120, 4, 18, 0, 0, 0, 'published', NULL, NULL, '2025-09-09 10:00:00', '2025-09-08 16:00:00', '2025-09-09 10:00:00');

-- Insert Forum Comments
INSERT INTO `forum_comments` (`id`, `post_id`, `user_id`, `parent_id`, `content`, `like_count`, `dislike_count`, `status`, `moderated_reason`, `moderated_by`, `created_at`, `updated_at`) VALUES
(1, 1, 8, NULL, 'Mình ở Long An cũng trồng lúa hữu cơ. Quan trọng là chọn giống lúa phù hợp không?', 3, 0, 'visible', NULL, NULL, '2025-09-08 15:00:00', '2025-09-08 15:00:00'),
(2, 1, 7, 1, 'Mình thường chọn giống lúa ST24 hoặc ST25 vì phù hợp với đất phù sa và có chất lượng gạo tốt. Bạn nên tham khảo thêm ý kiến kỹ thuật viên địa phương nhé!', 5, 0, 'visible', NULL, NULL, '2025-09-08 16:30:00', '2025-09-08 16:30:00'),
(3, 1, 5, NULL, 'Bài viết rất hữu ích! Mình đang cân nhắc chuyển từ canh tác truyền thống sang hữu cơ.', 2, 0, 'visible', NULL, NULL, '2025-09-09 07:00:00', '2025-09-09 07:00:00'),
(4, 2, 3, NULL, 'Cảm ơn bạn đã đánh giá sản phẩm của chúng tôi! Nếu có bất kỳ thắc mắc nào về sử dụng, hãy liên hệ với bộ phận hỗ trợ kỹ thuật nhé.', 4, 0, 'visible', NULL, NULL, '2025-09-08 17:00:00', '2025-09-08 17:00:00'),
(5, 2, 8, 4, 'Máy chạy rất ổn, chỉ có điều pin hơi nhanh hết khi làm đất cứng. Các bạn có kế hoạch nâng cấp dung lượng pin không?', 1, 0, 'visible', NULL, NULL, '2025-09-09 06:00:00', '2025-09-09 06:00:00'),
(6, 3, 7, NULL, 'Bạn có thể thử dùng dung dịch tỏi ớt để xịt phòng trừ sâu bệnh. Mình dùng hiệu quả lắm!', 6, 0, 'visible', NULL, NULL, '2025-09-09 10:00:00', '2025-09-09 10:00:00');

-- Insert Chatbot Conversations
INSERT INTO `chatbot_conversations` (`id`, `user_id`, `session_id`, `title`, `context`, `is_active`, `started_at`, `ended_at`) VALUES
(1, 5, 'session_20250908_001', 'Tư vấn chọn máy cày', '{"topic": "equipment_consultation", "products_discussed": ["TC001"], "user_preferences": {"budget": "under_30m", "farm_size": "small"}}', 0, '2025-09-08 14:00:00', '2025-09-08 14:30:00'),
(2, 7, 'session_20250909_001', 'Hỗ trợ kỹ thuật canh tác', '{"topic": "farming_techniques", "crop_type": "rice", "farming_method": "organic"}', 1, '2025-09-09 06:00:00', NULL),
(3, 6, 'session_20250909_002', 'Tư vấn phân bón hữu cơ', '{"topic": "fertilizer_consultation", "crop_type": "vegetables", "soil_type": "sandy"}', 0, '2025-09-09 08:00:00', '2025-09-09 08:45:00');

-- Insert Chatbot Messages
INSERT INTO `chatbot_messages` (`id`, `conversation_id`, `message_type`, `message_text`, `attachments`, `created_at`) VALUES
(1, 1, 'user', 'Tôi cần tư vấn chọn máy cày cho ruộng nhỏ khoảng 2ha', NULL, '2025-09-08 14:00:00'),
(2, 1, 'bot', 'Với diện tích 2ha, tôi khuyên bạn nên chọn máy cày mini điện VerdantTech V1. Máy này có những ưu điểm: tiết kiệm năng lượng, thân thiện môi trường, phù hợp với ruộng nhỏ.', NULL, '2025-09-08 14:00:30'),
(3, 1, 'user', 'Giá của máy này là bao nhiêu? Có khuyến mãi không?', NULL, '2025-09-08 14:02:00'),
(4, 1, 'bot', 'Máy cày mini điện VerdantTech V1 có giá 25.000.000 VNĐ. Hiện tại đang có khuyến mãi 10%, giá sau khuyến mãi là 22.500.000 VNĐ. Bạn có muốn xem thêm thông tin chi tiết không?', NULL, '2025-09-08 14:02:30'),
(5, 2, 'user', 'Tôi muốn hỏi về kỹ thuật trồng lúa hữu cơ', NULL, '2025-09-09 06:00:00'),
(6, 2, 'bot', 'Trồng lúa hữu cơ cần chú ý những điểm sau: 1) Chuẩn bị đất: Sử dụng phân compost thay vì phân hóa học 2) Chọn giống: Nên chọn giống lúa địa phương thích nghi 3) Quản lý nước: Tưới tiêu đúng thời điểm 4) Phòng trừ sâu bệnh: Sử dụng biện pháp sinh học', NULL, '2025-09-09 06:01:00'),
(7, 3, 'user', 'Tôi trồng rau, đất cát, nên dùng loại phân nào?', NULL, '2025-09-09 08:00:00'),
(8, 3, 'bot', 'Với đất cát trồng rau, tôi khuyên bạn sử dụng Phân Compost Hữu Cơ Premium. Loại phân này giúp cải thiện cấu trúc đất cát, tăng khả năng giữ nước và cung cấp dinh dưỡng lâu dài cho cây trồng.', NULL, '2025-09-09 08:00:30'),
(9, 3, 'user', 'Cảm ơn bạn! Tôi sẽ đặt mua ngay.', NULL, '2025-09-09 08:44:00'),
(10, 3, 'bot', 'Rất vui được hỗ trợ bạn! Chúc bạn canh tác thành công. Nếu có thắc mắc gì khác, đừng ngần ngại liên hệ nhé!', NULL, '2025-09-09 08:44:30');

-- Insert Weather Data Cache
INSERT INTO `weather_data_cache` (`id`, `farm_profile_id`, `api_source`, `weather_date`, `temperature_min`, `temperature_max`, `temperature_avg`, `humidity_percentage`, `precipitation_mm`, `wind_speed_kmh`, `wind_direction`, `uv_index`, `weather_condition`, `weather_icon`, `sunrise_time`, `sunset_time`, `raw_api_response`, `fetched_at`) VALUES
(1, 1, 'openweathermap', '2025-09-09', 25.00, 32.00, 28.50, 78.20, 0.00, 12.30, 'NE', 7.0, 'sunny', '01d', '06:15:00', '18:30:00', '{"coord":{"lon":106.8441,"lat":10.9545},"weather":[{"id":800,"main":"Clear","description":"clear sky","icon":"01d"}],"main":{"temp":28.5,"feels_like":32.1,"temp_min":25,"temp_max":32,"pressure":1013,"humidity":78}}', '2025-09-09 06:00:00'),
(2, 2, 'openweathermap', '2025-09-09', 26.00, 33.00, 29.10, 82.50, 2.50, 8.70, 'SE', 6.0, 'partly_cloudy', '02d', '06:10:00', '18:25:00', '{"coord":{"lon":106.4226,"lat":10.8838},"weather":[{"id":801,"main":"Clouds","description":"few clouds","icon":"02d"}],"main":{"temp":29.1,"feels_like":33.8,"temp_min":26,"temp_max":33,"pressure":1012,"humidity":82}}', '2025-09-09 05:30:00');

-- Insert Environmental Data (added for completeness)
INSERT INTO `environmental_data` (`id`, `farm_profile_id`, `user_id`, `measurement_date`, `soil_ph`, `co2_footprint`, `soil_moisture_percentage`, `soil_type`, `notes`, `created_at`, `updated_at`) VALUES
(1, 1, 7, '2025-09-09', 6.5, 120.50, 45.20, 'Đất phù sa', 'Đo lường sau mưa', '2025-09-09 06:00:00', '2025-09-09 06:00:00'),
(2, 2, 8, '2025-09-09', 7.0, 85.30, 38.50, 'Đất đỏ Bazan', 'Kiểm tra hàng tuần', '2025-09-09 05:30:00', '2025-09-09 05:30:00');

-- Insert Fertilizers (added for completeness)
INSERT INTO `fertilizers` (`id`, `environmental_data_id`, `organic_fertilizer`, `npk_fertilizer`, `urea_fertilizer`, `phosphate_fertilizer`, `created_at`, `updated_at`) VALUES
(1, 1, 50.00, 10.00, 5.00, 15.00, '2025-09-09 06:00:00', '2025-09-09 06:00:00'),
(2, 2, 40.00, 8.00, 4.00, 12.00, '2025-09-09 05:30:00', '2025-09-09 05:30:00');

-- Insert Energy Usage (added for completeness)
INSERT INTO `energy_usage` (`id`, `environmental_data_id`, `electricity_kwh`, `gasoline_liters`, `diesel_liters`, `created_at`, `updated_at`) VALUES
(1, 1, 100.00, 20.00, 30.00, '2025-09-09 06:00:00', '2025-09-09 06:00:00'),
(2, 2, 80.00, 15.00, 25.00, '2025-09-09 05:30:00', '2025-09-09 05:30:00');

-- Insert Requests (added for completeness)
INSERT INTO `requests` (`id`, `requester_id`, `request_type`, `title`, `description`, `status`, `priority`, `reference_type`, `reference_id`, `amount`, `admin_notes`, `rejection_reason`, `assigned_to`, `processed_by`, `processed_at`, `created_at`, `updated_at`) VALUES
(1, 3, 'payout_request', 'Yêu cầu thanh toán hoa hồng tháng 9', 'Yêu cầu thanh toán hoa hồng từ bán hàng tháng 9', 'pending', 'medium', 'vendor', 1, 2000000.00, NULL, NULL, 2, NULL, NULL, '2025-09-09 07:00:00', '2025-09-09 07:00:00'),
(2, 5, 'refund_request', 'Yêu cầu hoàn tiền đơn hàng #1', 'Sản phẩm bị hỏng', 'in_review', 'high', 'order', 1, 22500000.00, 'Kiểm tra sản phẩm', NULL, 2, NULL, NULL, '2025-09-09 08:15:00', '2025-09-09 08:15:00');

-- Insert Orders
INSERT INTO `orders` (`id`, `customer_id`, `status`, `subtotal`, `tax_amount`, `shipping_fee`, `discount_amount`, `total_amount`, `shipping_address`, `shipping_method`, `tracking_number`, `notes`, `cancelled_reason`, `cancelled_at`, `confirmed_at`, `shipped_at`, `delivered_at`, `created_at`, `updated_at`) VALUES
(1, 5, 'delivered', 25000000.00, 500000.00, 300000.00, 2500000.00, 23000000.00, '{"street": "123 Đường ABC", "district": "Quận 1", "city": "TP.HCM", "country": "Vietnam"}', 'express', 'EXP20250908001', NULL, NULL, NULL, '2025-09-08 12:00:00', '2025-09-08 15:00:00', '2025-09-09 10:00:00', '2025-09-08 10:00:00', '2025-09-09 10:00:00'),
(2, 6, 'shipped', 1716750.00, 0.00, 50000.00, 383250.00, 1483500.00, '{"street": "456 Đường DEF", "district": "Quận 2", "city": "TP.HCM", "country": "Vietnam"}', 'standard', 'STD20250909001', NULL, NULL, NULL, '2025-09-09 10:00:00', NULL, NULL, '2025-09-09 09:00:00', '2025-09-09 10:00:00'),
(3, 7, 'processing', 12000000.00, 800000.00, 200000.00, 960000.00, 11240000.00, '{"street": "789 Đường GHI", "district": "Quận 3", "city": "TP.HCM", "country": "Vietnam"}', 'express', NULL, 'Cần hỗ trợ lắp đặt', NULL, NULL, NULL, NULL, NULL, '2025-09-09 11:00:00', '2025-09-09 11:30:00');

-- Insert Order Details
INSERT INTO `order_details` (`id`, `order_id`, `product_id`, `quantity`, `unit_price`, `discount_amount`, `subtotal`, `created_at`) VALUES
(1, 1, 1, 1, 25000000.00, 2500000.00, 22500000.00, '2025-09-08 10:00:00'),
(2, 2, 3, 10, 150000.00, 0.00, 1500000.00, '2025-09-09 09:00:00'),
(3, 2, 4, 3, 85000.00, 38250.00, 216750.00, '2025-09-09 09:00:00'),
(4, 3, 5, 1, 12000000.00, 960000.00, 11040000.00, '2025-09-09 11:00:00');

-- Insert Transactions (adjusted to match schema v6)
INSERT INTO `transactions` (`id`, `transaction_type`, `amount`, `currency`, `order_id`, `customer_id`, `vendor_id`, `wallet_id`, `balance_before`, `balance_after`, `status`, `description`, `metadata`, `reference_type`, `reference_id`, `created_by`, `processed_by`, `created_at`, `completed_at`, `updated_at`) VALUES
(1, 'payment_in', 23000000.00, 'VND', 1, 5, NULL, NULL, NULL, NULL, 'completed', 'Payment for order #1 - Máy cày', '{"gateway": "vnpay", "reference": "VNP20250908001"}', 'order', 1, 5, 1, '2025-09-08 11:30:00', '2025-09-08 11:30:00', '2025-09-08 11:30:00'),
(2, 'payment_in', 1483500.00, 'VND', 2, 6, NULL, NULL, NULL, NULL, 'completed', 'Payment for order #2 - Hạt giống và phân bón', '{"gateway": "momo", "reference": "MOMO20250909001"}', 'order', 2, 6, 1, '2025-09-09 09:15:00', '2025-09-09 09:15:00', '2025-09-09 09:15:00'),
(3, 'payment_in', 11240000.00, 'VND', 3, 7, NULL, NULL, NULL, NULL, 'pending', 'Payment for order #3 - Drone phun thuốc', '{"gateway": "bank", "account": "VCB123456789"}', 'order', 3, 7, NULL, '2025-09-09 11:00:00', NULL, '2025-09-09 11:00:00'),
(4, 'commission', 2000000.00, 'VND', 1, NULL, 1, 1, 8000000.00, 10000000.00, 'completed', 'Commission from sale of product #1', '{"commission_rate": 8, "product_id": 1, "sale_amount": 25000000}', 'product', 1, 1, 2, '2025-09-09 15:00:00', '2025-09-09 15:00:00', '2025-09-09 15:00:00'),
(5, 'commission', 147000.00, 'VND', 2, NULL, 2, 2, 2353000.00, 2500000.00, 'completed', 'Commission from sale of products #3 and #4', '{"commission_rate": 5.5, "products": [3,4], "sale_amount": 2490000}', 'product', 3, 1, 2, '2025-09-09 16:00:00', '2025-09-09 16:00:00', '2025-09-09 16:00:00');

-- Insert Payments (adjusted to match schema v6)
INSERT INTO `payments` (`id`, `order_id`, `transaction_id`, `payment_method`, `payment_gateway`, `gateway_transaction_id`, `amount`, `status`, `gateway_response`, `refund_amount`, `refund_reason`, `refunded_at`, `paid_at`, `failed_at`, `created_at`, `updated_at`) VALUES
(1, 1, 1, 'bank_transfer', 'vnpay', 'VNP2025090801234567', 23000000.00, 'completed', '{"code": "00", "message": "Success", "bank": "VCB"}', 0.00, NULL, NULL, '2025-09-08 11:30:00', NULL, '2025-09-08 10:00:00', '2025-09-08 11:30:00'),
(2, 2, 2, 'credit_card', 'stripe', 'STR_2025090909876543', 1483500.00, 'completed', '{"id": "ch_abc123", "status": "succeeded"}', 0.00, NULL, NULL, '2025-09-09 09:15:00', NULL, '2025-09-09 09:00:00', '2025-09-09 09:15:00'),
(3, 3, 3, 'cod', 'manual', 'COD2025090911001', 11240000.00, 'pending', '{}', 0.00, NULL, NULL, NULL, NULL, '2025-09-09 11:00:00', '2025-09-09 11:00:00');

-- Insert Cashouts (adjusted to match schema v6)
INSERT INTO `cashouts` (`id`, `vendor_id`, `transaction_id`, `amount`, `bank_code`, `bank_account_number`, `bank_account_holder`, `status`, `cashout_type`, `gateway_transaction_id`, `reference_type`, `reference_id`, `notes`, `processed_by`, `created_at`, `processed_at`, `updated_at`) VALUES
(1, 1, 4, 2000000.00, 'VCB', '1234567890', 'Công Ty Thiết Bị Nông Nghiệp Xanh', 'pending', 'commission_payout', NULL, 'order', 1, 'Hoa hồng từ đơn hàng #1', NULL, '2025-09-09 15:30:00', NULL, '2025-09-09 15:30:00'),
(2, 2, 5, 147000.00, 'TCB', '0987654321', 'Cửa Hàng Nông Sản Sạch VerdantTech', 'completed', 'commission_payout', 'CASHOUT2025090916001', 'order', 2, 'Hoa hồng từ đơn hàng #2', 2, '2025-09-09 16:30:00', '2025-09-09 16:30:00', '2025-09-09 16:30:00');

-- Insert Purchase Inventory (adjusted to match schema v6, added default values for new columns)
INSERT INTO `purchase_inventory` (`id`, `product_id`, `sku`, `vendor_profile_id`, `quantity`, `unit_cost_price`, `total_cost`, `commission_rate`, `batch_number`, `lot_number`, `serial_numbers`, `supplier_invoice`, `purchase_order_number`, `warehouse_location`, `expiry_date`, `manufacturing_date`, `quality_check_status`, `quality_check_notes`, `quality_checked_by`, `quality_checked_at`, `condition_on_arrival`, `damage_notes`, `notes`, `balance_after`, `created_by`, `purchased_at`, `received_at`, `created_at`, `updated_at`) VALUES
(1, 1, 'SKU_TC001_001', 1, 5, 18000000.00, 90000000.00, 0.00, 'BATCH001', 'LOT001', 'SN001,SN002,SN003,SN004,SN005', 'INV001', 'PO001', 'WH_A1', NULL, '2025-08-01', 'passed', 'No issues', 2, '2025-09-08 10:00:00', 'new', NULL, 'Máy cày đầu tiên nhập kho', 5, 1, '2025-09-08 09:00:00', '2025-09-08 10:00:00', '2025-09-08 09:00:00', '2025-09-08 10:00:00'),
(2, 2, 'SKU_HV002_001', 2, 10, 70000000.00, 700000000.00, 0.00, 'BATCH002', 'LOT002', NULL, 'INV002', 'PO002', 'WH_B2', NULL, '2025-07-15', 'passed', 'All units functional', 2, '2025-09-08 15:00:00', 'new', NULL, 'Hệ thống tưới nhập kho', 10, 1, '2025-09-08 14:00:00', '2025-09-08 15:00:00', '2025-09-08 14:00:00', '2025-09-09 16:00:00'),
(3, 3, 'SKU_SD003_001', 1, 20, 80000.00, 1600000.00, 0.00, 'BATCH003', 'LOT003', NULL, 'INV003', 'PO003', 'WH_C3', '2026-09-08', '2025-06-01', 'passed', 'High quality seeds', 2, '2025-09-08 12:00:00', 'new', NULL, 'Hạt giống nhập kho', 20, 1, '2025-09-08 11:00:00', '2025-09-08 12:00:00', '2025-09-08 11:00:00', '2025-09-09 15:00:00'),
(4, 4, 'SKU_FT004_001', 2, 8, 45000.00, 360000.00, 0.00, 'BATCH004', 'LOT004', NULL, 'INV004', 'PO004', 'WH_D4', '2026-03-01', '2025-05-01', 'passed', 'Organic certified', 2, '2025-09-08 09:00:00', 'new', NULL, 'Phân bón nhập kho', 8, 1, '2025-09-08 08:00:00', '2025-09-08 09:00:00', '2025-09-08 08:00:00', '2025-09-08 08:00:00'),
(5, 5, 'SKU_DR005_001', 1, 3, 8500000.00, 25500000.00, 0.00, 'BATCH005', 'LOT005', 'SN101,SN102,SN103', 'INV005', 'PO005', 'WH_E5', NULL, '2025-08-15', 'passed', 'Drones tested', 2, '2025-09-08 11:00:00', 'new', NULL, 'Drone phun thuốc nhập kho', 3, 1, '2025-09-08 10:00:00', '2025-09-08 11:00:00', '2025-09-08 10:00:00', '2025-09-09 09:00:00');

-- Insert Sales Inventory (adjusted to match schema v6)
INSERT INTO `sales_inventory` (`id`, `product_id`, `order_id`, `quantity`, `unit_sale_price`, `total_revenue`, `commission_amount`, `balance_after`, `movement_type`, `notes`, `created_by`, `sold_at`, `created_at`) VALUES
(1, 1, 1, 1, 25000000.00, 25000000.00, 2000000.00, 4, 'sale', 'Máy cày bán cho khách hàng 1', 3, '2025-09-08 10:00:00', '2025-09-08 10:00:00'),
(2, 3, 2, 3, 80000.00, 240000.00, 12000.00, 17, 'sale', 'Hạt giống corn bán cho khách hàng 2', 4, '2025-09-09 09:00:00', '2025-09-09 09:00:00'),
(3, 4, 2, 50, 45000.00, 2250000.00, 135000.00, -42, 'sale', 'Phân bón bán kèm hạt giống', 4, '2025-09-09 09:00:00', '2025-09-09 09:00:00'),
(4, 5, 3, 1, 12000000.00, 12000000.00, 1200000.00, 2, 'sale', 'Drone phun thuốc cho nông dân 1', 3, '2025-09-09 11:00:00', '2025-09-09 11:00:00');

-- Insert Product Reviews
INSERT INTO `product_reviews` (`id`, `product_id`, `order_id`, `customer_id`, `rating`, `title`, `comment`, `images`, `helpful_count`, `unhelpful_count`, `created_at`, `updated_at`) VALUES
(1, 1, 1, 5, 5, 'Máy cày tuyệt vời!', 'Máy chạy êm, tiết kiệm điện và rất phù hợp với ruộng nhỏ của tôi. Chất lượng tốt, đóng gói cẩn thận.', 'review1-1.jpg, review1-2.jpg', 12, 1, '2025-09-09 16:00:00', '2025-09-09 16:00:00'),
(2, 3, 2, 6, 4, 'Phân compost chất lượng', 'Phân rất tốt, cây trồng phát triển nhanh sau khi bón. Mùi không quá nặng.', 'review2-1.jpg', 8, 0, '2025-09-09 18:00:00', '2025-09-09 18:00:00'),
(3, 4, 2, 6, 5, 'Hạt giống nảy mầm tốt', 'Tỷ lệ nảy mầm cao như quảng cáo, cà chua to và ngon. Sẽ mua lại lần sau.', NULL, 5, 0, '2025-09-09 19:00:00', '2025-09-09 19:00:00');