-- SEEDER DATA FOR VERDANTTECH DATABASE v7.0.1
-- All passwords are: $2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS
-- Created for schema v7.0.1: Supports multiple vendor certificates, bank accounts, and products
-- Dates adjusted to be recent as of 2025-09-18, ensured foreign key consistency

-- Insert Users (admin, staff, vendors, and customers)
INSERT INTO `users` (`id`, `email`, `password_hash`, `role`, `full_name`, `phone_number`, `address`, `tax_code`, `is_verified`, `verification_token`, `verification_sent_at`, `avatar_url`, `status`, `last_login_at`, `RefreshToken`, `RefreshTokenExpiresAt`, `created_at`, `updated_at`, `deleted_at`) VALUES
(1, 'admin@verdanttech.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'admin', 'Quản Trị Viên Hệ Thống', '0901234567', '123 Đường Lê Lợi, Quận 1, TP.HCM', 'TAX001', 1, NULL, NULL, NULL, 'active', '2025-09-17 08:00:00', NULL, NULL, '2025-09-16 07:00:00', '2025-09-17 08:00:00', NULL),
(2, 'staff1@verdanttech.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'staff', 'Nguyễn Văn An', '0901234568', '456 Đường Nguyễn Huệ, Quận 3, TP.HCM', 'TAX002', 1, NULL, NULL, NULL, 'active', '2025-09-17 07:30:00', NULL, NULL, '2025-09-16 07:30:00', '2025-09-17 07:30:00', NULL),
(3, 'staff2@verdanttech.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'staff', 'Trần Thị Bình', '0901234569', '789 Đường Trần Hưng Đạo, Quận 5, TP.HCM', 'TAX003', 1, NULL, NULL, NULL, 'active', '2025-09-17 07:00:00', NULL, NULL, '2025-09-16 08:00:00', '2025-09-17 07:00:00', NULL),
(4, 'vendor1@verdanttech.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'vendor', 'Công Ty Nông Nghiệp Xanh Việt', '0901234570', '101 Đường Công Nghiệp, Quận 7, TP.HCM', 'TAX004', 1, NULL, NULL, NULL, 'active', '2025-09-17 08:15:00', NULL, NULL, '2025-09-16 09:00:00', '2025-09-17 08:15:00', NULL),
(5, 'vendor2@verdanttech.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'vendor', 'Cửa Hàng Hữu Cơ Sạch', '0901234571', '202 Đường Nông Sản, Quận Tân Bình, TP.HCM', 'TAX005', 1, NULL, NULL, NULL, 'active', '2025-09-17 08:10:00', NULL, NULL, '2025-09-16 09:30:00', '2025-09-17 08:10:00', NULL),
(6, 'vendor3@verdanttech.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'vendor', 'Công Ty Thiết Bị Nông Nghiệp Tân Tiến', '0901234572', '303 Đường Lê Văn Sỹ, Quận 3, TP.HCM', 'TAX006', 1, NULL, NULL, NULL, 'active', '2025-09-17 08:05:00', NULL, NULL, '2025-09-16 10:00:00', '2025-09-17 08:05:00', NULL),
(7, 'customer1@verdanttech.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'customer', 'Lê Văn Hùng', '0901234573', '404 Đường Nguyễn Trãi, Quận 5, TP.HCM', 'TAX007', 1, NULL, NULL, NULL, 'active', '2025-09-17 08:20:00', NULL, NULL, '2025-09-16 09:15:00', '2025-09-17 08:20:00', NULL),
(8, 'customer2@verdanttech.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'customer', 'Nguyễn Thị Mai', '0901234574', '505 Đường Cách Mạng Tháng 8, Quận 10, TP.HCM', 'TAX008', 1, NULL, NULL, NULL, 'active', '2025-09-17 08:25:00', NULL, NULL, '2025-09-16 09:45:00', '2025-09-17 08:25:00', NULL),
(9, 'farmer1@verdanttech.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'customer', 'Trần Văn Nam', '0901234575', '606 Đường Nông Nghiệp, Biên Hòa, Đồng Nai', 'TAX009', 1, NULL, NULL, NULL, 'active', '2025-09-17 06:00:00', NULL, NULL, '2025-09-16 10:15:00', '2025-09-17 06:00:00', NULL),
(10, 'farmer2@verdanttech.com', '$2a$11$eebvzn7Au.D1ILICdBn4zeE8kMjPcMwg2CkbCUOiVsWFURxS6JriS', 'customer', 'Phạm Thị Lan', '0901234576', '707 Đường Nông Thôn, Đức Hòa, Long An', 'TAX010', 1, NULL, NULL, NULL, 'active', '2025-09-17 05:30:00', NULL, NULL, '2025-09-16 10:30:00', '2025-09-17 05:30:00', NULL);

-- Insert Vendor Profiles (for vendor users)
INSERT INTO `vendor_profiles` (`id`, `vendor_id`, `company_name`, `slug`, `business_registration_number`, `company_address`, `province`, `district`, `commune`, `verified_at`, `verified_by`, `created_at`, `updated_at`) VALUES
(1, 4, 'Công Ty Nông Nghiệp Xanh Việt', 'cong-ty-nong-nghiep-xanh-viet', 'BRN123456789', '101 Đường Công Nghiệp, Quận 7, TP.HCM', 'TP.HCM', 'Quận 7', 'Phường Tân Phong', '2025-09-17 07:00:00', 1, '2025-09-16 08:00:00', '2025-09-17 07:00:00'),
(2, 5, 'Cửa Hàng Hữu Cơ Sạch', 'cua-hang-huu-co-sach', 'BRN987654321', '202 Đường Nông Sản, Quận Tân Bình, TP.HCM', 'TP.HCM', 'Quận Tân Bình', 'Phường 15', '2025-09-17 06:30:00', 1, '2025-09-16 08:30:00', '2025-09-17 06:30:00'),
(3, 6, 'Công Ty Thiết Bị Nông Nghiệp Tân Tiến', 'cong-ty-thiet-bi-nong-nghiep-tan-tien', 'BRN456789123', '303 Đường Lê Văn Sỹ, Quận 3, TP.HCM', 'TP.HCM', 'Quận 3', 'Phường 13', '2025-09-17 06:45:00', 1, '2025-09-16 09:00:00', '2025-09-17 06:45:00');

-- Insert Vendor Certificates (each vendor has multiple certificates)
INSERT INTO `vendor_certificates` (`id`, `vendor_profile_id`, `certification_code`, `certification_name`, `certificate_url`, `status`, `rejection_reason`, `uploaded_at`, `verified_at`, `verified_by`, `created_at`, `updated_at`) VALUES
-- Vendor 1 (Công Ty Nông Nghiệp Xanh Việt)
(1, 1, 'ISO14001', 'ISO 14001 Environmental Management', 'https://example.com/certificates/vendor1_iso14001.pdf', 'verified', NULL, '2025-09-16 09:00:00', '2025-09-17 07:00:00', 1, '2025-09-16 09:00:00', '2025-09-17 07:00:00'),
(2, 1, 'ISO50001', 'ISO 50001 Energy Management', 'https://example.com/certificates/vendor1_iso50001.pdf', 'verified', NULL, '2025-09-16 09:15:00', '2025-09-17 07:00:00', 1, '2025-09-16 09:15:00', '2025-09-17 07:00:00'),
(3, 1, 'CARBON_NEUTRAL', 'Carbon Neutral Certification', 'https://example.com/certificates/vendor1_carbon_neutral.pdf', 'pending', NULL, '2025-09-17 08:00:00', NULL, NULL, '2025-09-17 08:00:00', '2025-09-17 08:00:00'),
-- Vendor 2 (Cửa Hàng Hữu Cơ Sạch)
(4, 2, 'USDA_ORGANIC', 'USDA Organic Certification', 'https://example.com/certificates/vendor2_usda_organic.pdf', 'verified', NULL, '2025-09-16 10:00:00', '2025-09-17 06:30:00', 1, '2025-09-16 10:00:00', '2025-09-17 06:30:00'),
(5, 2, 'VIETGAP', 'VietGAP – Thực hành nông nghiệp tốt tại Việt Nam', 'https://example.com/certificates/vendor2_vietgap.pdf', 'verified', NULL, '2025-09-16 10:15:00', '2025-09-17 06:30:00', 1, '2025-09-16 10:15:00', '2025-09-17 06:30:00'),
(6, 2, 'FAIRTRADE', 'Fairtrade International Certification', 'https://example.com/certificates/vendor2_fairtrade.pdf', 'rejected', 'Chứng chỉ không rõ nguồn gốc', '2025-09-17 09:00:00', '2025-09-17 10:00:00', 1, '2025-09-17 09:00:00', '2025-09-17 10:00:00'),
-- Vendor 3 (Công Ty Thiết Bị Nông Nghiệp Tân Tiến)
(7, 3, 'ISO9001', 'ISO 9001 Quality Management', 'https://example.com/certificates/vendor3_iso9001.pdf', 'verified', NULL, '2025-09-16 11:00:00', '2025-09-17 06:45:00', 1, '2025-09-16 11:00:00', '2025-09-17 06:45:00'),
(8, 3, 'GLOBALGAP', 'GlobalGAP Certification', 'https://example.com/certificates/vendor3_globalgap.pdf', 'pending', NULL, '2025-09-17 08:30:00', NULL, NULL, '2025-09-17 08:30:00', '2025-09-17 08:30:00'),
(9, 3, 'HACCP', 'HACCP - Hazard Analysis Critical Control Points', 'https://example.com/certificates/vendor3_haccp.pdf', 'verified', NULL, '2025-09-16 11:30:00', '2025-09-17 06:45:00', 1, '2025-09-16 11:30:00', '2025-09-17 06:45:00');

-- Insert Vendor Bank Accounts (each vendor has multiple bank accounts)
INSERT INTO `vendor_bank_accounts` (`id`, `vendor_id`, `bank_code`, `account_number`, `account_holder`, `is_default`, `created_at`, `updated_at`) VALUES
-- Vendor 1 (Công Ty Nông Nghiệp Xanh Việt)
(1, 4, 'VCB', '1234567890', 'Công Ty Nông Nghiệp Xanh Việt', 1, '2025-09-17 07:05:00', '2025-09-17 07:05:00'),
(2, 4, 'TPB', '0987654321', 'Công Ty Nông Nghiệp Xanh Việt', 0, '2025-09-17 07:10:00', '2025-09-17 07:10:00'),
-- Vendor 2 (Cửa Hàng Hữu Cơ Sạch)
(3, 5, 'ACB', '1122334455', 'Cửa Hàng Hữu Cơ Sạch', 1, '2025-09-17 06:35:00', '2025-09-17 06:35:00'),
(4, 5, 'MBB', '2233445566', 'Cửa Hàng Hữu Cơ Sạch', 0, '2025-09-17 06:40:00', '2025-09-17 06:40:00'),
-- Vendor 3 (Công Ty Thiết Bị Nông Nghiệp Tân Tiến)
(5, 6, 'SCB', '3344556677', 'Công Ty Thiết Bị Nông Nghiệp Tân Tiến', 1, '2025-09-17 06:50:00', '2025-09-17 06:50:00'),
(6, 6, 'VIB', '4455667788', 'Công Ty Thiết Bị Nông Nghiệp Tân Tiến', 0, '2025-09-17 06:55:00', '2025-09-17 06:55:00');

-- Insert Wallets (one per vendor)
INSERT INTO `wallets` (`id`, `vendor_id`, `balance`, `last_transaction_id`, `last_updated_by`, `created_at`, `updated_at`) VALUES
(1, 4, 15000000.00, NULL, NULL, '2025-09-17 08:00:00', '2025-09-17 08:00:00'),
(2, 5, 3000000.00, NULL, NULL, '2025-09-17 08:00:00', '2025-09-17 08:00:00'),
(3, 6, 5000000.00, NULL, NULL, '2025-09-17 08:00:00', '2025-09-17 08:00:00');

-- Insert Farm Profiles (for farmer customers)
INSERT INTO `farm_profiles` (`id`, `customer_id`, `farm_name`, `farm_size_hectares`, `location_address`, `province`, `district`, `commune`, `primary_crops`, `is_active`, `created_at`, `updated_at`) VALUES
(1, 9, 'Trang Trại Xanh Đồng Nai', 6.00, '123 Đường Nông Nghiệp, Biên Hòa', 'Đồng Nai', 'Biên Hòa', 'Tân Phong', 'Lúa, Rau xanh, Cà chua', 1, '2025-09-16 10:00:00', '2025-09-17 06:00:00'),
(2, 10, 'Trang Trại Hữu Cơ Long An', 9.50, '456 Đường Nông Thôn, Đức Hòa', 'Long An', 'Đức Hòa', 'Đức Hòa Thượng', 'Rau củ, Trái cây, Thảo dược', 1, '2025-09-16 10:30:00', '2025-09-17 05:30:00');

-- Insert Product Categories
INSERT INTO `product_categories` (`id`, `parent_id`, `name`, `slug`, `description`, `icon_url`, `is_active`, `created_at`, `updated_at`) VALUES
(1, NULL, 'Thiết Bị Nông Nghiệp', 'thiet-bi-nong-nghiep', 'Máy móc và thiết bị phục vụ sản xuất nông nghiệp', NULL, 1, '2025-09-16 07:00:00', '2025-09-16 07:00:00'),
(2, 1, 'Máy Cày', 'may-cay', 'Máy cày và thiết bị làm đất', NULL, 1, '2025-09-16 07:00:00', '2025-09-16 07:00:00'),
(3, 1, 'Máy Gặt', 'may-gat', 'Máy gặt và thu hoạch', NULL, 1, '2025-09-16 07:00:00', '2025-09-16 07:00:00'),
(4, NULL, 'Hạt Giống', 'hat-giong', 'Hạt giống chất lượng cao', NULL, 1, '2025-09-16 07:00:00', '2025-09-16 07:00:00'),
(5, 4, 'Hạt Giống Rau', 'hat-giong-rau', 'Hạt giống rau củ hữu cơ', NULL, 1, '2025-09-16 07:00:00', '2025-09-16 07:00:00'),
(6, NULL, 'Phân Bón', 'phan-bon', 'Phân bón hữu cơ và hóa học', NULL, 1, '2025-09-16 07:00:00', '2025-09-16 07:00:00'),
(7, NULL, 'Hệ Thống Tưới', 'he-thong-tuoi', 'Hệ thống tưới tiêu thông minh', NULL, 1, '2025-09-16 07:00:00', '2025-09-16 07:00:00');

-- Insert Products (each vendor has multiple products)
INSERT INTO `products` (`id`, `category_id`, `vendor_id`, `product_code`, `product_name`, `slug`, `description`, `unit_price`, `commission_rate`, `discount_percentage`, `energy_efficiency_rating`, `specifications`, `manual_urls`, `images`, `warranty_months`, `stock_quantity`, `weight_kg`, `dimensions_cm`, `is_active`, `view_count`, `sold_count`, `rating_average`, `created_at`, `updated_at`) VALUES
-- Vendor 1 (Công Ty Nông Nghiệp Xanh Việt)
(1, 2, 4, 'TC001', 'Máy Cày Điện Eco V1', 'may-cay-dien-eco-v1', 'Máy cày điện thân thiện môi trường cho nông trại nhỏ.', 27000000.00, 10.00, 5.00, 'A++', '{"power": "12kW", "battery": "48V 120Ah"}', 'manual_tc001.pdf', 'tc001_1.jpg,tc001_2.jpg', 24, 50, 500.000, '{"length": 250, "width": 120, "height": 150}', 1, 150, 10, 4.70, '2025-09-16 07:00:00', '2025-09-17 07:00:00'),
(2, 3, 4, 'HV002', 'Máy Gặt Lúa Tự Động H3', 'may-gat-lua-tu-dong-h3', 'Máy gặt lúa tự động với công nghệ AI hiện đại.', 160000000.00, 8.00, 0.00, 'A+', '{"engine": "Diesel 60HP", "capacity": "2.5 tons/hour"}', 'manual_hv002.pdf', 'hv002_1.jpg,hv002_2.jpg', 36, 15, 2600.000, '{"length": 460, "width": 210, "height": 260}', 1, 100, 5, 4.80, '2025-09-16 07:15:00', '2025-09-17 07:15:00'),
(3, 7, 4, 'IR001', 'Hệ Thống Tưới AI SmartFlow', 'he-thong-tuoi-ai-smartflow', 'Hệ thống tưới tự động sử dụng AI để tối ưu hóa nước.', 18000000.00, 12.00, 10.00, 'A', '{"coverage": "1 hectare", "sensors": "soil moisture, weather"}', 'manual_ir001.pdf', 'ir001_1.jpg,ir001_2.jpg', 24, 20, 50.000, '{"length": 200, "width": 150, "height": 100}', 1, 80, 3, 4.60, '2025-09-16 07:30:00', '2025-09-17 07:30:00'),
-- Vendor 2 (Cửa Hàng Hữu Cơ Sạch)
(4, 5, 5, 'SD001', 'Hạt Giống Rau Mầm Hữu Cơ', 'hat-giong-rau-mam-huu-co', 'Hạt giống rau mầm hữu cơ, tỷ lệ nảy mầm cao.', 45000.00, 5.00, 0.00, NULL, '{"germination_rate": "95%", "pack_size": "50g"}', 'manual_sd001.pdf', 'sd001_1.jpg', 0, 300, 0.050, '{"length": 8, "width": 4, "height": 2}', 1, 250, 60, 4.50, '2025-09-16 07:45:00', '2025-09-17 07:45:00'),
(5, 6, 5, 'FT001', 'Phân Bón Hữu Cơ BioGrow', 'phan-bon-huu-co-biogrow', 'Phân bón hữu cơ từ compost, an toàn cho cây trồng.', 120000.00, 7.00, 5.00, NULL, '{"npk": "5-5-5", "weight": "20kg"}', 'manual_ft001.pdf', 'ft001_1.jpg,ft001_2.jpg', 0, 150, 20.000, '{"length": 50, "width": 30, "height": 10}', 1, 180, 25, 4.70, '2025-09-16 08:00:00', '2025-09-17 08:00:00'),
-- Vendor 3 (Công Ty Thiết Bị Nông Nghiệp Tân Tiến)
(6, 2, 6, 'TC002', 'Máy Cày Mini Hybrid V2', 'may-cay-mini-hybrid-v2', 'Máy cày hybrid kết hợp điện và xăng, đa năng.', 32000000.00, 10.00, 10.00, 'A+', '{"power": "15kW", "fuel": "Hybrid"}', 'manual_tc002.pdf', 'tc002_1.jpg,tc002_2.jpg', 24, 40, 600.000, '{"length": 260, "width": 130, "height": 160}', 1, 120, 8, 4.60, '2025-09-16 08:15:00', '2025-09-17 08:15:00'),
(7, 7, 6, 'DR001', 'Drone Phun Thuốc Pro V3', 'drone-phun-thuoc-pro-v3', 'Drone phun thuốc tự động với công nghệ AI.', 35000000.00, 12.00, 15.00, 'A', '{"flight_time": "35min", "capacity": "12L"}', 'manual_dr001.pdf', 'dr001_1.jpg,dr001_2.jpg', 12, 20, 5.000, '{"length": 110, "width": 110, "height": 50}', 1, 95, 6, 4.50, '2025-09-16 08:30:00', '2025-09-17 08:30:00');

-- Insert Product Certificates (each product has multiple certificates)
INSERT INTO `product_certificates` (`id`, `product_id`, `certification_code`, `certification_name`, `certificate_url`, `status`, `rejection_reason`, `uploaded_at`, `verified_at`, `verified_by`, `created_at`, `updated_at`) VALUES
-- Product 1 (Máy Cày Điện Eco V1)
(1, 1, 'ISO50001', 'ISO 50001 Energy Management', 'https://example.com/certificates/product1_iso50001.pdf', 'verified', NULL, '2025-09-16 09:00:00', '2025-09-17 07:00:00', 1, '2025-09-16 09:00:00', '2025-09-17 07:00:00'),
(2, 1, 'CARBON_NEUTRAL', 'Carbon Neutral Certification', 'https://example.com/certificates/product1_carbon_neutral.pdf', 'verified', NULL, '2025-09-16 09:15:00', '2025-09-17 07:00:00', 1, '2025-09-16 09:15:00', '2025-09-17 07:00:00'),
-- Product 2 (Máy Gặt Lúa Tự Động H3)
(3, 2, 'ISO14001', 'ISO 14001 Environmental Management', 'https://example.com/certificates/product2_iso14001.pdf', 'verified', NULL, '2025-09-16 09:30:00', '2025-09-17 07:00:00', 1, '2025-09-16 09:30:00', '2025-09-17 07:00:00'),
(4, 2, 'SBTI', 'SBTi - Science Based Targets Initiative', 'https://example.com/certificates/product2_sbti.pdf', 'pending', NULL, '2025-09-17 08:00:00', NULL, NULL, '2025-09-17 08:00:00', '2025-09-17 08:00:00'),
-- Product 3 (Hệ Thống Tưới AI SmartFlow)
(5, 3, 'RAINFOREST_ALLIANCE', 'Rainforest Alliance Certification', 'https://example.com/certificates/product3_rainforest_alliance.pdf', 'verified', NULL, '2025-09-16 10:00:00', '2025-09-17 07:00:00', 1, '2025-09-16 10:00:00', '2025-09-17 07:00:00'),
(6, 3, 'ISO9001', 'ISO 9001 Quality Management', 'https://example.com/certificates/product3_iso9001.pdf', 'rejected', 'Cần bổ sung thông tin chi tiết', '2025-09-17 08:15:00', '2025-09-17 09:00:00', 1, '2025-09-17 08:15:00', '2025-09-17 09:00:00'),
-- Product 4 (Hạt Giống Rau Mầm Hữu Cơ)
(7, 4, 'USDA_ORGANIC', 'USDA Organic Certification', 'https://example.com/certificates/product4_usda_organic.pdf', 'verified', NULL, '2025-09-16 10:30:00', '2025-09-17 06:30:00', 1, '2025-09-16 10:30:00', '2025-09-17 06:30:00'),
(8, 4, 'VIETGAP', 'VietGAP – Thực hành nông nghiệp tốt tại Việt Nam', 'https://example.com/certificates/product4_vietgap.pdf', 'verified', NULL, '2025-09-16 10:45:00', '2025-09-17 06:30:00', 1, '2025-09-16 10:45:00', '2025-09-17 06:30:00'),
-- Product 5 (Phân Bón Hữu Cơ BioGrow)
(9, 5, 'GLOBALGAP', 'GlobalGAP Certification', 'https://example.com/certificates/product5_globalgap.pdf', 'verified', NULL, '2025-09-16 11:00:00', '2025-09-17 06:30:00', 1, '2025-09-16 11:00:00', '2025-09-17 06:30:00'),
(10, 5, 'NON_GMO', 'Non-GMO Project Verified', 'https://example.com/certificates/product5_non_gmo.pdf', 'rejected', 'Chứng chỉ không rõ ràng', '2025-09-17 09:00:00', '2025-09-17 10:00:00', 1, '2025-09-17 09:00:00', '2025-09-17 10:00:00'),
-- Product 6 (Máy Cày Mini Hybrid V2)
(11, 6, 'ISO50001', 'ISO 50001 Energy Management', 'https://example.com/certificates/product6_iso50001.pdf', 'verified', NULL, '2025-09-16 11:15:00', '2025-09-17 06:45:00', 1, '2025-09-16 11:15:00', '2025-09-17 06:45:00'),
(12, 6, 'CARBON_NEUTRAL', 'Carbon Neutral Certification', 'https://example.com/certificates/product6_carbon_neutral.pdf', 'pending', NULL, '2025-09-17 08:30:00', NULL, NULL, '2025-09-17 08:30:00', '2025-09-17 08:30:00'),
-- Product 7 (Drone Phun Thuốc Pro V3)
(13, 7, 'RAINFOREST_ALLIANCE', 'Rainforest Alliance Certification', 'https://example.com/certificates/product7_rainforest_alliance.pdf', 'verified', NULL, '2025-09-16 11:30:00', '2025-09-17 06:45:00', 1, '2025-09-16 11:30:00', '2025-09-17 06:45:00'),
(14, 7, 'ISO14001', 'ISO 14001 Environmental Management', 'https://example.com/certificates/product7_iso14001.pdf', 'verified', NULL, '2025-09-16 11:45:00', '2025-09-17 06:45:00', 1, '2025-09-16 11:45:00', '2025-09-17 06:45:00');

-- Insert Product Registrations
INSERT INTO `product_registrations` (`id`, `vendor_id`, `category_id`, `proposed_product_code`, `proposed_product_name`, `description`, `unit_price`, `energy_efficiency_rating`, `specifications`, `manual_urls`, `images`, `warranty_months`, `weight_kg`, `dimensions_cm`, `status`, `rejection_reason`, `approved_by`, `created_at`, `updated_at`) VALUES
(1, 4, 2, 'TC003', 'Máy Cày Điện Eco V3', 'Phiên bản nâng cấp của máy cày điện với công suất cao hơn.', 30000000.00, 'A++', '{"power": "15kW", "battery": "48V 150Ah"}', 'manual_tc003.pdf', 'tc003_1.jpg,tc003_2.jpg', 24, 520.000, '{"length": 260, "width": 125, "height": 155}', 'pending', NULL, NULL, '2025-09-17 07:00:00', '2025-09-17 07:00:00'),
(2, 5, 5, 'SD002', 'Hạt Giống Cà Chua Hữu Cơ', 'Hạt giống cà chua hữu cơ nhập khẩu từ Hà Lan.', 60000.00, NULL, '{"germination_rate": "98%", "pack_size": "50g"}', 'manual_sd002.pdf', 'sd002_1.jpg', 0, 0.050, '{"length": 8, "width": 4, "height": 2}', 'approved', NULL, 1, '2025-09-16 14:00:00', '2025-09-17 08:00:00'),
(3, 6, 7, 'IR002', 'Hệ Thống Tưới Tiết Kiệm Nước', 'Hệ thống tưới nhỏ giọt thông minh.', 12000000.00, 'A', '{"coverage": "0.5 hectare", "sensors": "soil moisture"}', 'manual_ir002.pdf', 'ir002_1.jpg,ir002_2.jpg', 12, 40.000, '{"length": 180, "width": 120, "height": 80}', 'rejected', 'Thiếu thông tin kỹ thuật', 2, '2025-09-16 10:00:00', '2025-09-17 16:00:00');

-- Insert Forum Categories
INSERT INTO `forum_categories` (`id`, `name`, `description`, `is_active`, `created_at`, `updated_at`) VALUES
(1, 'Kỹ Thuật Canh Tác', 'Thảo luận về các phương pháp canh tác bền vững và hữu cơ', 1, '2025-09-16 07:00:00', '2025-09-16 07:00:00'),
(2, 'Thiết Bị Nông Nghiệp', 'Chia sẻ kinh nghiệm sử dụng máy móc và thiết bị nông nghiệp', 1, '2025-09-16 07:00:00', '2025-09-16 07:00:00'),
(3, 'Phòng Trừ Sâu Bệnh', 'Các biện pháp phòng trừ sâu bệnh thân thiện với môi trường', 1, '2025-09-16 07:00:00', '2025-09-16 07:00:00');

-- Insert Forum Posts
INSERT INTO `forum_posts` (`id`, `forum_category_id`, `customer_id`, `title`, `slug`, `content`, `tags`, `view_count`, `like_count`, `dislike_count`, `is_pinned`, `status`, `created_at`, `updated_at`) VALUES
(1, 1, 9, 'Chia sẻ kinh nghiệm trồng rau hữu cơ', 'chia-se-kinh-nghiem-trong-rau-huu-co', '[{"order": 1, "type": "text", "content": "Mình đang thử trồng rau hữu cơ tại Đồng Nai. Có ai có kinh nghiệm chọn giống tốt không?"}, {"order": 2, "type": "image", "content": "https://example.com/images/rau-huu-co.jpg"}]', 'rau hữu cơ, đồng nai, canh tác', 200, 25, 1, 1, 'visible', '2025-09-16 14:00:00', '2025-09-17 10:00:00'),
(2, 2, 7, 'Đánh giá máy cày điện Eco V1', 'danh-gia-may-cay-dien-eco-v1', '[{"order": 1, "type": "text", "content": "Máy cày điện Eco V1 chạy rất ổn, tiết kiệm năng lượng. Ai đã dùng chưa?"}]', 'máy cày, điện, eco v1', 100, 20, 0, 0, 'visible', '2025-09-16 15:00:00', '2025-09-17 06:00:00'),
(3, 3, 10, 'Cách phòng trừ sâu bệnh tự nhiên', 'cach-phong-tru-sau-benh-tu-nhien', '[{"order": 1, "type": "text", "content": "Mình muốn dùng biện pháp tự nhiên để phòng sâu bệnh. Có gợi ý nào không?"}]', 'sâu bệnh, tự nhiên, rau củ', 150, 22, 0, 0, 'visible', '2025-09-16 16:00:00', '2025-09-17 10:00:00');

-- Insert Forum Comments
INSERT INTO `forum_comments` (`id`, `forum_post_id`, `customer_id`, `parent_id`, `content`, `like_count`, `dislike_count`, `status`, `created_at`, `updated_at`) VALUES
(1, 1, 10, NULL, 'Mình dùng giống rau mầm của Cửa Hàng Hữu Cơ Sạch, rất tốt!', 4, 0, 'visible', '2025-09-16 15:00:00', '2025-09-16 15:00:00'),
(2, 1, 9, 1, 'Cảm ơn bạn! Mình sẽ thử giống đó. Bạn có dùng phân bón gì không?', 5, 0, 'visible', '2025-09-16 16:30:00', '2025-09-16 16:30:00'),
(3, 2, 4, NULL, 'Máy cày Eco V1 của chúng tôi có pin nâng cấp trong phiên bản mới. Liên hệ để được tư vấn nhé!', 3, 0, 'visible', '2025-09-16 17:00:00', '2025-09-16 17:00:00'),
(4, 3, 9, NULL, 'Mình dùng dung dịch tỏi ớt phun lên rau, rất hiệu quả.', 6, 0, 'visible', '2025-09-17 10:00:00', '2025-09-17 10:00:00');

-- Insert Chatbot Conversations
INSERT INTO `chatbot_conversations` (`id`, `customer_id`, `session_id`, `title`, `context`, `is_active`, `started_at`, `ended_at`) VALUES
(1, 7, 'session_20250917_001', 'Tư vấn máy cày', '{"topic": "equipment_consultation", "products_discussed": ["TC001"], "user_preferences": {"budget": "under_30m"}}', 0, '2025-09-17 08:00:00', '2025-09-17 08:30:00'),
(2, 9, 'session_20250917_002', 'Hỗ trợ canh tác hữu cơ', '{"topic": "farming_techniques", "crop_type": "vegetables", "farming_method": "organic"}', 1, '2025-09-17 06:00:00', NULL),
(3, 10, 'session_20250917_003', 'Tư vấn phân bón', '{"topic": "fertilizer_consultation", "crop_type": "vegetables", "soil_type": "sandy"}', 0, '2025-09-17 08:00:00', '2025-09-17 08:45:00');

-- Insert Chatbot Messages
INSERT INTO `chatbot_messages` (`id`, `conversation_id`, `message_type`, `message_text`, `attachments`, `created_at`) VALUES
(1, 1, 'user', 'Tôi cần máy cày cho ruộng 2ha, ngân sách dưới 30 triệu.', NULL, '2025-09-17 08:00:00'),
(2, 1, 'bot', 'Máy Cày Điện Eco V1 (27 triệu VNĐ) rất phù hợp với ruộng 2ha. Máy tiết kiệm năng lượng và thân thiện môi trường.', NULL, '2025-09-17 08:01:00'),
(3, 2, 'user', 'Làm sao để trồng rau hữu cơ hiệu quả?', NULL, '2025-09-17 06:00:00'),
(4, 2, 'bot', 'Để trồng rau hữu cơ hiệu quả, bạn nên: 1) Chọn giống hữu cơ như rau mầm từ Cửa Hàng Hữu Cơ Sạch. 2) Sử dụng phân compost. 3) Quản lý sâu bệnh bằng biện pháp sinh học.', NULL, '2025-09-17 06:01:00'),
(5, 3, 'user', 'Đất cát nên dùng phân bón nào cho rau?', NULL, '2025-09-17 08:00:00'),
(6, 3, 'bot', 'Với đất cát, Phân Bón Hữu Cơ BioGrow (120.000 VNĐ/20kg) sẽ cải thiện độ giữ nước và dinh dưỡng cho đất.', NULL, '2025-09-17 08:01:00');

-- Insert Environmental Data
INSERT INTO `environmental_data` (`id`, `farm_profile_id`, `customer_id`, `measurement_date`, `soil_ph`, `co2_footprint`, `soil_moisture_percentage`, `soil_type`, `notes`, `created_at`, `updated_at`) VALUES
(1, 1, 9, '2025-09-17', 6.8, 110.50, 42.30, 'DatPhuSa', 'Đo sau khi tưới', '2025-09-17 06:00:00', '2025-09-17 06:00:00'),
(2, 2, 10, '2025-09-17', 7.2, 90.20, 40.10, 'DatDoBazan', 'Đo định kỳ', '2025-09-17 05:30:00', '2025-09-17 05:30:00');

-- Insert Fertilizers
INSERT INTO `fertilizers` (`id`, `environmental_data_id`, `organic_fertilizer`, `npk_fertilizer`, `urea_fertilizer`, `phosphate_fertilizer`, `created_at`, `updated_at`) VALUES
(1, 1, 60.00, 15.00, 5.00, 20.00, '2025-09-17 06:00:00', '2025-09-17 06:00:00'),
(2, 2, 50.00, 10.00, 4.00, 15.00, '2025-09-17 05:30:00', '2025-09-17 05:30:00');

-- Insert Energy Usage
INSERT INTO `energy_usage` (`id`, `environmental_data_id`, `electricity_kwh`, `gasoline_liters`, `diesel_liters`, `created_at`, `updated_at`) VALUES
(1, 1, 120.00, 25.00, 35.00, '2025-09-17 06:00:00', '2025-09-17 06:00:00'),
(2, 2, 90.00, 20.00, 30.00, '2025-09-17 05:30:00', '2025-09-17 05:30:00');

-- Insert Cart
INSERT INTO `cart` (`id`, `customer_id`, `created_at`, `updated_at`) VALUES
(1, 7, '2025-09-17 08:00:00', '2025-09-17 08:30:00'),
(2, 8, '2025-09-17 09:00:00', '2025-09-17 09:15:00'),
(3, 9, '2025-09-17 10:00:00', '2025-09-17 10:00:00');

-- Insert Cart Items
INSERT INTO `cart_items` (`id`, `cart_id`, `product_id`, `quantity`, `unit_price`, `created_at`, `updated_at`) VALUES
(1, 1, 4, 10, 45000.00, '2025-09-17 08:00:00', '2025-09-17 08:00:00'),
(2, 1, 5, 2, 120000.00, '2025-09-17 08:15:00', '2025-09-17 08:15:00'),
(3, 2, 1, 1, 27000000.00, '2025-09-17 09:00:00', '2025-09-17 09:00:00'),
(4, 3, 4, 15, 45000.00, '2025-09-17 10:00:00', '2025-09-17 10:00:00');

-- Insert Orders
INSERT INTO `orders` (`id`, `customer_id`, `status`, `subtotal`, `tax_amount`, `shipping_fee`, `discount_amount`, `total_amount`, `shipping_address`, `shipping_method`, `tracking_number`, `notes`, `cancelled_reason`, `cancelled_at`, `confirmed_at`, `shipped_at`, `delivered_at`, `created_at`, `updated_at`) VALUES
(1, 7, 'delivered', 27000000.00, 540000.00, 300000.00, 1350000.00, 26490000.00, '{"street": "404 Đường Nguyễn Trãi", "district": "Quận 5", "city": "TP.HCM", "country": "Vietnam"}', 'express', 'EXP20250917001', NULL, NULL, NULL, '2025-09-16 12:00:00', '2025-09-16 15:00:00', '2025-09-17 10:00:00', '2025-09-16 10:00:00', '2025-09-17 10:00:00'),
(2, 8, 'shipped', 1650000.00, 0.00, 50000.00, 82500.00, 1617500.00, '{"street": "505 Đường Cách Mạng Tháng 8", "district": "Quận 10", "city": "TP.HCM", "country": "Vietnam"}', 'standard', 'STD20250917001', NULL, NULL, NULL, '2025-09-17 10:00:00', NULL, NULL, '2025-09-17 09:00:00', '2025-09-17 10:00:00'),
(3, 9, 'processing', 35000000.00, 700000.00, 200000.00, 5250000.00, 30450000.00, '{"street": "606 Đường Nông Nghiệp", "district": "Biên Hòa", "city": "Đồng Nai", "country": "Vietnam"}', 'express', NULL, 'Cần hỗ trợ lắp đặt', NULL, NULL, NULL, NULL, NULL, '2025-09-17 11:00:00', '2025-09-17 11:30:00');

-- Insert Order Details
INSERT INTO `order_details` (`id`, `order_id`, `product_id`, `quantity`, `unit_price`, `discount_amount`, `subtotal`, `created_at`) VALUES
(1, 1, 1, 1, 27000000.00, 1350000.00, 25650000.00, '2025-09-16 10:00:00'),
(2, 2, 4, 10, 45000.00, 2250.00, 427500.00, '2025-09-17 09:00:00'),
(3, 2, 5, 10, 120000.00, 6000.00, 1140000.00, '2025-09-17 09:00:00'),
(4, 3, 7, 1, 35000000.00, 5250000.00, 29750000.00, '2025-09-17 11:00:00');

-- Insert Transactions
INSERT INTO `transactions` (`id`, `transaction_type`, `amount`, `currency`, `order_id`, `user_id`, `status`, `note`, `gateway_payment_id`, `created_by`, `processed_by`, `created_at`, `completed_at`, `updated_at`) VALUES
(1, 'payment_in', 26490000.00, 'VND', 1, 7, 'completed', 'Thanh toán đơn hàng #1 - Máy cày', 'VNP20250916001', 7, 1, '2025-09-16 11:30:00', '2025-09-16 11:30:00', '2025-09-16 11:30:00'),
(2, 'payment_in', 1617500.00, 'VND', 2, 8, 'completed', 'Thanh toán đơn hàng #2 - Hạt giống và phân bón', 'MOMO20250917001', 8, 1, '2025-09-17 09:15:00', '2025-09-17 09:15:00', '2025-09-17 09:15:00'),
(3, 'payment_in', 30450000.00, 'VND', 3, 9, 'pending', 'Thanh toán đơn hàng #3 - Drone phun thuốc', 'BANK20250917001', 9, NULL, '2025-09-17 11:00:00', NULL, '2025-09-17 11:00:00'),
(4, 'commission', 2565000.00, 'VND', 1, 4, 'completed', 'Hoa hồng từ bán sản phẩm #1', NULL, 1, 2, '2025-09-17 15:00:00', '2025-09-17 15:00:00', '2025-09-17 15:00:00'),
(5, 'commission', 156750.00, 'VND', 2, 5, 'completed', 'Hoa hồng từ bán sản phẩm #4 và #5', NULL, 1, 2, '2025-09-17 16:00:00', '2025-09-17 16:00:00', '2025-09-17 16:00:00');

-- Insert Payments
INSERT INTO `payments` (`id`, `order_id`, `payment_method`, `payment_gateway`, `gateway_payment_id`, `amount`, `status`, `gateway_response`, `created_at`, `updated_at`) VALUES
(1, 1, 'bank_transfer', 'vnpay', 'VNP2025091601234567', 26490000.00, 'completed', '{"code": "00", "message": "Success", "bank": "VCB"}', '2025-09-16 10:00:00', '2025-09-16 11:30:00'),
(2, 2, 'credit_card', 'stripe', 'STR_2025091709876543', 1617500.00, 'completed', '{"id": "ch_abc123", "status": "succeeded"}', '2025-09-17 09:00:00', '2025-09-17 09:15:00'),
(3, 3, 'cod', 'manual', 'COD20250917001', 30450000.00, 'pending', '{}', '2025-09-17 11:00:00', '2025-09-17 11:00:00');

-- Insert Cashouts
INSERT INTO `cashouts` (`id`, `vendor_id`, `transaction_id`, `bank_account_id`, `amount`, `status`, `reason`, `gateway_transaction_id`, `reference_type`, `reference_id`, `notes`, `processed_by`, `created_at`, `processed_at`, `updated_at`) VALUES
(1, 4, 4, 1, 2565000.00, 'pending', 'Thanh toán hoa hồng', NULL, 'order', 1, 'Hoa hồng từ đơn hàng #1', NULL, '2025-09-17 15:30:00', NULL, '2025-09-17 15:30:00'),
(2, 5, 5, 3, 156750.00, 'completed', 'Thanh toán hoa hồng', 'CASHOUT20250917001', 'order', 2, 'Hoa hồng từ đơn hàng #2', 2, '2025-09-17 16:30:00', '2025-09-17 16:30:00', '2025-09-17 16:30:00');

-- Insert Batch Inventory
INSERT INTO `batch_inventory` (`id`, `product_id`, `sku`, `vendor_id`, `batch_number`, `lot_number`, `quantity`, `unit_cost_price`, `expiry_date`, `manufacturing_date`, `quality_check_status`, `quality_checked_by`, `quality_checked_at`, `notes`, `created_at`, `updated_at`) VALUES
(1, 1, 'SKU_TC001_001', 4, 'BATCH001', 'LOT001', 50, 20000000.00, NULL, '2025-08-01', 'passed', 2, '2025-09-16 10:00:00', 'Lô máy cày đầu tiên', '2025-09-16 09:00:00', '2025-09-16 10:00:00'),
(2, 2, 'SKU_HV002_001', 4, 'BATCH002', 'LOT002', 15, 120000000.00, NULL, '2025-07-15', 'passed', 2, '2025-09-16 15:00:00', 'Lô máy gặt nhập kho', '2025-09-16 14:00:00', '2025-09-17 15:00:00'),
(3, 3, 'SKU_IR001_001', 4, 'BATCH003', 'LOT003', 20, 15000000.00, NULL, '2025-08-10', 'passed', 2, '2025-09-16 12:00:00', 'Lô hệ thống tưới nhập kho', '2025-09-16 11:00:00', '2025-09-17 12:00:00'),
(4, 4, 'SKU_SD001_001', 5, 'BATCH004', 'LOT004', 300, 30000.00, '2026-09-16', '2025-06-01', 'passed', 2, '2025-09-16 09:00:00', 'Lô hạt giống nhập kho', '2025-09-16 08:00:00', '2025-09-17 08:00:00'),
(5, 5, 'SKU_FT001_001', 5, 'BATCH005', 'LOT005', 150, 80000.00, '2026-03-01', '2025-05-01', 'passed', 2, '2025-09-16 10:00:00', 'Lô phân bón nhập kho', '2025-09-16 09:00:00', '2025-09-17 09:00:00'),
(6, 6, 'SKU_TC002_001', 6, 'BATCH006', 'LOT006', 40, 24000000.00, NULL, '2025-08-15', 'passed', 2, '2025-09-16 11:00:00', 'Lô máy cày hybrid nhập kho', '2025-09-16 10:00:00', '2025-09-17 10:00:00'),
(7, 7, 'SKU_DR001_001', 6, 'BATCH007', 'LOT007', 20, 28000000.00, NULL, '2025-08-20', 'passed', 2, '2025-09-16 12:00:00', 'Lô drone nhập kho', '2025-09-16 11:00:00', '2025-09-17 11:00:00');

-- Insert Export Inventory
INSERT INTO `export_inventory` (`id`, `product_id`, `order_id`, `quantity`, `balance_after`, `movement_type`, `notes`, `created_by`, `created_at`, `updated_at`) VALUES
(1, 1, 1, 1, 49, 'sale', 'Máy cày bán cho khách hàng 1', 4, '2025-09-16 10:00:00', '2025-09-16 10:00:00'),
(2, 4, 2, 10, 290, 'sale', 'Hạt giống bán cho khách hàng 2', 5, '2025-09-17 09:00:00', '2025-09-17 09:00:00'),
(3, 5, 2, 10, 140, 'sale', 'Phân bón bán kèm hạt giống', 5, '2025-09-17 09:00:00', '2025-09-17 09:00:00'),
(4, 7, 3, 1, 19, 'sale', 'Drone bán cho nông dân 1', 6, '2025-09-17 11:00:00', '2025-09-17 11:00:00');

-- Insert Product Reviews
INSERT INTO `product_reviews` (`id`, `product_id`, `order_id`, `customer_id`, `rating`, `title`, `comment`, `images`, `created_at`, `updated_at`) VALUES
(1, 1, 1, 7, 5, 'Máy cày tuyệt vời', 'Máy cày điện Eco V1 hoạt động ổn định, tiết kiệm năng lượng.', 'review1_1.jpg,review1_2.jpg', '2025-09-17 16:00:00', '2025-09-17 16:00:00'),
(2, 4, 2, 8, 4, 'Hạt giống chất lượng', 'Hạt giống nảy mầm tốt, nhưng cần hướng dẫn chi tiết hơn.', 'review2_1.jpg', '2025-09-17 18:00:00', '2025-09-17 18:00:00'),
(3, 5, 2, 8, 5, 'Phân bón tốt', 'Phân bón BioGrow giúp cây phát triển nhanh, rất hài lòng.', NULL, '2025-09-17 19:00:00', '2025-09-17 19:00:00');

-- Insert Requests
INSERT INTO `requests` (`id`, `user_id`, `request_type`, `reference_type`, `reference_id`, `title`, `description`, `status`, `reply_notes`, `processed_by`, `processed_at`, `created_at`, `updated_at`) VALUES
(1, 4, 'payout_request', 'order', 1, 'Yêu cầu thanh toán hoa hồng tháng 9', 'Yêu cầu thanh toán hoa hồng từ đơn hàng #1', 'pending', NULL, NULL, NULL, '2025-09-17 15:00:00', '2025-09-17 15:00:00'),
(2, 7, 'refund_request', 'order', 1, 'Yêu cầu hoàn tiền đơn hàng #1', 'Máy cày bị lỗi kỹ thuật', 'in_review', 'Kiểm tra lỗi kỹ thuật', NULL, NULL, '2025-09-17 16:00:00', '2025-09-17 16:00:00');